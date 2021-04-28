using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using Player;
using UnityEngine.UI;

public enum GameState {
    TeamSelection,
    GameReady,
    Game,
    GameEnd
}

public enum AnnouncerClip {
    countdown, // ready
    greenHasBriefcase, // ready
    purpleHasBriefcase, // ready
    greenBriefcaseReturn, // ready
    purpleBriefcaseReturn, // ready
    greenWins, // ready 
    purpleWins, // ready
    maniacalLaugh
}

public class ServerGameManager : NetworkBehaviour
{
    public Transform[] teamSpawns;
    [SyncVar] public GameState gameState = GameState.TeamSelection;
    public GameObject[] teamFlags;
    public Transform[] flagSpawns;
    public Material[] teamColors;

    [SyncVar] public int playersConnected;
    private double time;
    [SyncVar] private int timeToDisplay;
    [SyncVar] private int winnerIndex;
    private GameObject greenFlag;
    private GameObject purpleFlag;
    [SyncVar] private bool greenFlagHolder = false;
    [SyncVar] private bool purpleFlagHolder = false;
    private bool greenCaseSoundPlayed = false;
    private bool purpleCaseSoundPlayed = false;

    [Header("Announcer")]
    // announcer (see enum for which indices are which clips)
    private AudioSource audioSource;
    public AudioClip[] announcerClips;

    [Header("AI")] 
    [SerializeField] private GameObject aiPenguin;


    void Start() {
        ResetVariables();
        audioSource = this.GetComponent<AudioSource>();
    }

    void OnDisable() {
        GameObject.Find("CountdownText").GetComponent<Text>().text = "";
        ResetVariables(true);
    }

    void Update()
    {
        if (isServer) {
            // RpcUpdatePlayers(gameState);
            // RpcChangeGameState(gameState);

            playersConnected = FindObjectOfType<NetworkManager>().numPlayers;
            //Debug.Log("Players Connected: " + playersConnected);

            if (gameState == GameState.TeamSelection) { // basically do nothing, wait until host is ready
                //Debug.Log("Game State: Team Selection");
                GameObject.Find("CountdownText").GetComponent<Text>().text = "Press enter to start!";

                if (Input.GetKeyDown(KeyCode.Return)) { // this is what I'm using to "ready up" the server
                    RpcCloseDoors();
                    RpcChangeGameState(GameState.GameReady);
                    RpcPlaySound(AnnouncerClip.countdown);

                    for (int i = playersConnected; i < 12; i++) {
                        SpawnAI(i%2 == 1);
                    }
                }
            }
            else if (gameState == GameState.GameReady) { // spawn in flags, do countdown, Open Doors
                //Debug.Log("Game State: Game Ready");


                time += Time.deltaTime;
                if ((int)time <= 6)
                    RpcSetTimer(5-(int)time);

                if (timeToDisplay == -1) { // the timer is over, start the game
                    time = 0;
                    SpawnFlags(true, true);
                    RpcOpenDoors();
                    RpcChangeGameState(GameState.Game);
                }
            }
            else if (gameState == GameState.Game) { // Check Flag Positions(?), determine win condition
                //Debug.Log("Game State: Playing Game");
                greenFlag = GameObject.Find("GreenFlag(Clone)");
                purpleFlag = GameObject.Find("PurpleFlag(Clone)");

                // sounds
                if (greenFlagHolder == true) {
                    // Debug.Log("[GREEN FLAG IS HELD]");
                    if (!greenCaseSoundPlayed) {
                        greenCaseSoundPlayed = true;
                        RpcPlaySound(AnnouncerClip.purpleHasBriefcase);
                        // Debug.Log("[PURPLE HAS GREEN BRIEFCASE]");
                    }
                }
                else {
                    if (greenCaseSoundPlayed) {
                        greenCaseSoundPlayed = false;
                        RpcPlaySound(AnnouncerClip.greenBriefcaseReturn);
                    }
                }

                if (purpleFlagHolder == true) {
                    // Debug.Log("[PURPLE FLAG IS HELD]");
                    if (!purpleCaseSoundPlayed) {
                        purpleCaseSoundPlayed = true;
                        RpcPlaySound(AnnouncerClip.greenHasBriefcase);
                        // Debug.Log("[GREEN HAS PURPLE BRIEFCASE]");
                    }
                }
                else {
                    if (purpleCaseSoundPlayed) {
                        purpleCaseSoundPlayed = false;
                        RpcPlaySound(AnnouncerClip.purpleBriefcaseReturn);
                    }
                }

                // RpcSetFlagHolder(0, greenFlag.GetComponent<Flag>().isHeld);
                greenFlagHolder = greenFlag.GetComponent<Flag>().isHeld;
                // RpcSetFlagHolder(1, purpleFlag.GetComponent<Flag>().isHeld);
                purpleFlagHolder = purpleFlag.GetComponent<Flag>().isHeld;

                // win conditions
                if (Vector3.Distance(teamSpawns[0].transform.position, purpleFlag.transform.position) < 8) {
                    RpcSetWinner(0);
                    RpcPlaySound(AnnouncerClip.greenWins);
                    RpcChangeGameState(GameState.GameEnd);
                }
                else if (Vector3.Distance(teamSpawns[1].transform.position, greenFlag.transform.position) < 8) {
                    RpcSetWinner(1);
                    RpcPlaySound(AnnouncerClip.purpleWins);
                    RpcChangeGameState(GameState.GameEnd);
                }
            }
            else if (gameState == GameState.GameEnd) { // Reset penguin positions, wipe flags, close doors, reset win conditions
                //Debug.Log("Game State: Game Finished");

                time += Time.deltaTime;
                if (time > 5)
                    ResetVariables();
            }

        }

        // Displaying Text For Countdown (Clientside)
        if (winnerIndex == 0)
            GameObject.Find("CountdownText").GetComponent<Text>().text = "GREEN WINS!";
        else if (winnerIndex == 1)
            GameObject.Find("CountdownText").GetComponent<Text>().text = "PURPLE WINS!";
        else if (timeToDisplay > 0)
            GameObject.Find("CountdownText").GetComponent<Text>().text = timeToDisplay.ToString();
        else if (timeToDisplay == 0)
            GameObject.Find("CountdownText").GetComponent<Text>().text = "GO!";
        else if(gameState == GameState.TeamSelection)
            GameObject.Find("CountdownText").GetComponent<Text>().text = "Press enter to start!";
        else
            GameObject.Find("CountdownText").GetComponent<Text>().text = "";
    }

    private void ResetVariables(bool local = false) {
        time = 0;
        playersConnected = 0;
        timeToDisplay = -2;
        gameState = GameState.TeamSelection;
        winnerIndex = -1;

        foreach (GameObject door in GameObject.FindGameObjectsWithTag("Door"))
            door.transform.position = new Vector3(door.transform.position.x, 58, door.transform.position.z); // hardcoded value for these doors specifically

        foreach (GameObject flag in GameObject.FindGameObjectsWithTag("Finish"))
            GameObject.Destroy(flag);
        
        foreach (GameObject player in GameObject.FindGameObjectsWithTag("Player")) {
            if (player.GetComponent<PenguinMove>().isBot) {
                GameObject.Destroy(player);                
            }
        }

        if (!local && isServer) {
            RpcChangeGameState(GameState.TeamSelection);
            RpcSetTimer(-2);
            RpcCloseDoors();
            RpcDestroyFlags();
            RpcSetWinner(-1);
        }
    }

    private void SpawnAI(bool greenTeam)
    {
        var desiredTransform = greenTeam ? teamSpawns[0] : teamSpawns[1];
        var pos              = desiredTransform.position;
        pos += new Vector3(Random.Range(-2f, 2f), 0f, Random.Range(-2f, 2f));
        
        GameObject bot = GameObject.Instantiate(aiPenguin, pos, desiredTransform.rotation) as GameObject;
        NetworkServer.Spawn(bot);
        bot.GetComponent<PenguinBase>().StartBot(greenTeam);
    }

    private void SpawnFlags(bool green, bool purple) {
        GameObject flag;
        if (green) {
            flag = GameObject.Instantiate(teamFlags[0], flagSpawns[0].position, flagSpawns[0].rotation) as GameObject;
            NetworkServer.Spawn(flag);
        }
        if (purple) {
            flag = GameObject.Instantiate(teamFlags[1], flagSpawns[1].position, flagSpawns[1].rotation) as GameObject;
            NetworkServer.Spawn(flag);
        }
    }

    [ClientRpc]
    private void RpcChangeGameState(GameState state) {
        gameState = state;
    }
    
    [ClientRpc]
    private void RpcSetTimer(int time) {
        timeToDisplay = time;
    }

    [ClientRpc]
    private void RpcOpenDoors() {
        foreach (GameObject door in GameObject.FindGameObjectsWithTag("Door"))
            door.transform.position -= Vector3.up*10;
    }

    [ClientRpc]
    private void RpcCloseDoors() {
        foreach (GameObject door in GameObject.FindGameObjectsWithTag("Door"))
            door.transform.position = new Vector3(door.transform.position.x, 58, door.transform.position.z); // hardcoded value for these doors specifically
    }

    [ClientRpc]
    private void RpcDestroyFlags() {
        foreach (GameObject flag in GameObject.FindGameObjectsWithTag("Finish"))
            GameObject.Destroy(flag);
    }

    [ClientRpc]
    private void RpcSetWinner(int teamIndex) {
        winnerIndex = teamIndex;
    }

    [ClientRpc]
    public void RpcPlaySound(AnnouncerClip clipID) {
        if (announcerClips.Length <= (int)clipID) {
            Debug.LogWarning("No AudioClip: " + clipID.ToString() + " (announcerClips[" + clipID + "])");
            return;
        }

        audioSource.PlayOneShot(announcerClips[(int)clipID]);
    }

    [ClientRpc]
    private void RpcSetFlagHolder(int teamIndex, bool held) {
        if (teamIndex == 0) {
            this.greenFlagHolder = held;
        }
        else if (teamIndex == 1) {
            this.greenFlagHolder = held;
        }
        else {
            Debug.LogWarning("Flag Holder: Invalid Team");
        }
    }

}
