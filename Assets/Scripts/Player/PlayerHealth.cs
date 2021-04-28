using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

// this is not using EventHandlers, but it should work for now I guess
namespace Player
{
    public class PlayerHealth : NetworkBehaviour
    {
        [SyncVar] public int health = 100;
        private double timeSinceDeath;

        private bool isBot;
        
        private void Start()
        {
            isBot = isBot = isServer && !isLocalPlayer;
        }

        void Update() {
            if (!isLocalPlayer && !isBot)
                return;

            // foreach(GameObject rocket in GameObject.FindGameObjectsWithTag("Rocket")) {
            //     if (Vector3.Distance(rocket.transform.position, this.transform.position) < 2) {
            //         CmdSetHealth(0);
            //         rocket.GetComponent<Rocket>().BlowUp();
            //     }
            // }

            if (health <= 0) {
                timeSinceDeath+=Time.deltaTime;

                if (timeSinceDeath >= 0.5f)
                    CmdSetHealth(100);
            }
        }

        void OnCollisionEnter(Collision other) {
            if (!isLocalPlayer && !isBot)
                return;

            if (other.gameObject.tag == "Rocket") {
                if (other.gameObject.GetComponent<Rocket>().greenTeam != this.GetComponent<PenguinBase>().greenTeam) {
                    CmdSetHealth(0);
                    other.gameObject.GetComponent<Rocket>().CmdBlowUp();
                    
                    if(isBot)
                        RpcSetHealth(0);
                }
            }
            else if (other.gameObject.tag == "Instakill") {
                CmdSetHealth(0);
                
                if(isBot)
                    RpcSetHealth(0);
            }
        }

        [Command]
        private void CmdDecrementHealth(int amt) {
            RpcDecrementHealth(amt);
        }
        
        [Command]
        private void CmdSetHealth(int amt) {
            RpcSetHealth(amt);
        }

        [ClientRpc]
        private void RpcDecrementHealth(int amt) {
            health -= amt;
        }

        [ClientRpc]
        private void RpcSetHealth(int amt) {
            health = amt;
        }

    }
}
