using System;
using Mirror;
using UnityEngine;

namespace Player
{
    public class RocketLauncher : NetworkBehaviour
    {
        public GameObject rocketPrefab;
        public Transform rocketLaunchPos;
        
        public event Action OnFire;

        private void Start()
        {
            Debug.Log("Add server thrust limit to jetpack");
        }

        void Update()
        {
            if (!isLocalPlayer)
                return;

            if (Input.GetMouseButtonDown(0)) {
                Debug.Log("Launch Rocket");
                CmdSpawnRocket();
            }
        }

        [Command]
        void CmdSpawnRocket() {
            
            OnFire?.Invoke();

            GameObject rocket = GameObject.Instantiate(rocketPrefab, rocketLaunchPos.position, rocketLaunchPos.rotation) as GameObject;
            rocket.GetComponent<Rocket>().travelDir = this.transform.forward;
            NetworkServer.Spawn(rocket);
        }

    }
}
