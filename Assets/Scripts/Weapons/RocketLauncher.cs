using System;
using Mirror;
using UnityEngine;

namespace Player
{
    public class RocketLauncher : ReceivesInputs
    {
        public GameObject rocketPrefab;
        public Transform rocketLaunchPos;
        
        public event Action OnFire;
        private ServerGameManager gameManager;

        private bool isBot;

        private void Start()
        {
            isBot = isServer && !isLocalPlayer;

            Debug.Log("Add server thrust limit to jetpack");
            gameManager = GameObject.Find("[ Game Manager ]").GetComponent<ServerGameManager>();
        }

        void Update()
        {
            if (!isLocalPlayer && !isBot)
                return;

            if (inputs.PressedFire() && gameManager.gameState == GameState.Game && !this.GetComponent<PlayerSlide>().GetSliding()) {
                Debug.Log("Launch Rocket");
                CmdSpawnRocket(this.GetComponent<PenguinBase>().greenTeam);
                
                if(isBot)
                    RpcSpawnRocketBot(GetComponent<PenguinBase>().greenTeam);
            }
        }

        [Command]
        void CmdSpawnRocket(bool greenTeam) {
            
            OnFire?.Invoke();

            GameObject rocket = GameObject.Instantiate(rocketPrefab, rocketLaunchPos.position, rocketLaunchPos.rotation) as GameObject;
            rocket.GetComponent<Rocket>().travelDir = this.transform.forward;
            rocket.GetComponent<Rocket>().greenTeam = greenTeam;
            NetworkServer.Spawn(rocket);
        }

        [ClientRpc]
        private void RpcSpawnRocketBot(bool isGreenTeam)
        {
            OnFire?.Invoke();

            GameObject rocket = GameObject.Instantiate(rocketPrefab, rocketLaunchPos.position, rocketLaunchPos.rotation) as GameObject;
            rocket.GetComponent<Rocket>().travelDir = this.transform.forward;
            rocket.GetComponent<Rocket>().greenTeam = isGreenTeam;
            NetworkServer.Spawn(rocket);
        }

    }
}
