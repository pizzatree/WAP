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

        void OnCollisionEnter(Collision other) {
            if (other.gameObject.tag == "Rocket") {
                CmdDecrementHealth((int)(10*Vector3.Distance(other.transform.position, this.transform.position))/2);
            }
        }

        [Command]
        private void CmdDecrementHealth(int amt) {
            RpcDecrementHealth(amt);
        }

        [ClientRpc]
        private void RpcDecrementHealth(int amt) {
            health -= amt;
        }


    }
}
