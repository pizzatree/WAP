using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using UnityEngine.UI;

public enum GameState {
    TeamSelection,
    GameReady,
    Game,
    GameEnd
}

public class ServerGameManager : NetworkBehaviour
{
    public Transform[] teamSpawns;
    [SyncVar] public GameState gameState = GameState.TeamSelection;
    public GameObject[] teamFlags;
    public Transform[] flagSpawns;
    public Material[] teamColors;
    public AudioClip[] announcerClips;

    [SyncVar] public int playersConnected;
    double time;
    [SyncVar] int timeToDisplay;
    [SyncVar] int winnerIndex;

    void Start() {
        ResetVariables();
    }

    void OnDisable() {
        ResetVariables(true);
    }

    void Update()
    {
        if (isServer) {
            // RpcUpdatePlayers(gameState);
            // RpcChangeGameState(gameState);

            playersConnected = FindObjectOfType<NetworkManager>().numPlayers;
            Debug.Log("Players Connected: " + playersConnected);

            if (gameState == GameState.TeamSelection) { // basically do nothing, wait until host is ready
                Debug.Log("Game State: Team Selection");

                if (Input.GetKeyDown(KeyCode.Return)) { // this is what I'm using to "ready up" the server
                    RpcCloseDoors();
                    RpcChangeGameState(GameState.GameReady);
                }
            }
            else if (gameState == GameState.GameReady) { // spawn in flags, do countdown, Open Doors
                Debug.Log("Game State: Game Ready");

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
            else if (gameState == GameState.Game) { // Check Flag Positions(?), Respawn Players(?), determine win condition
                Debug.Log("Game State: Playing Game");
                if (Vector3.Distance(teamSpawns[0].transform.position, GameObject.Find("PurpleFlag(Clone)").transform.position) < 8) {
                    RpcSetWinner(0);
                    RpcChangeGameState(GameState.GameEnd);
                }
                else if (Vector3.Distance(teamSpawns[1].transform.position, GameObject.Find("GreenFlag(Clone)").transform.position) < 8) {
                    RpcSetWinner(1);
                    RpcChangeGameState(GameState.GameEnd);
                }
            }
            else if (gameState == GameState.GameEnd) { // Reset penguin positions, wipe flags, close doors, reset win conditions
                Debug.Log("Game State: Game Finished");

                // play win sound

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

        if (!local && isServer) {
            RpcChangeGameState(GameState.TeamSelection);
            RpcSetTimer(-2);
            RpcCloseDoors();
            RpcDestroyFlags();
            RpcSetWinner(-1);
        }
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

}
