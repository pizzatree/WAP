using UnityEngine;
using Mirror;

public class Flag : NetworkBehaviour
{
    [SyncVar] public bool isHeld;
    [SyncVar] public GameObject playerHolding;
    [SyncVar] public bool greenTeam;

    void Update()
    {
        if (isHeld) {
            this.transform.position = playerHolding.transform.position + 4*Vector3.up;

            if (playerHolding.gameObject.GetComponent<Player.PlayerHealth>().health <= 0) {
                // this.isHeld = false;
                // playerHolding = null;
                RpcRemoveHolder();
            }

            // to prevent people from BOOKING IT with the case 
            if (Vector3.Distance(Vector3.zero, this.transform.position) > 500) {
                RpcRemoveHolder();
            }
        }
    }


    [ClientRpc]
    private void RpcRemoveHolder() {
        this.isHeld = false;
        playerHolding = null;

        if (greenTeam)
            this.transform.position = GameObject.Find("[ Game Manager ]").GetComponent<ServerGameManager>().flagSpawns[0].position;
        else
            this.transform.position = GameObject.Find("[ Game Manager ]").GetComponent<ServerGameManager>().flagSpawns[1].position;
    }
}
