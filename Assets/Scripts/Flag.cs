using UnityEngine;
using Mirror;

public class Flag : NetworkBehaviour
{
    [SyncVar] public bool isHeld;
    [SyncVar] public GameObject playerHolding;

    void Update()
    {
        if (isHeld) {
            this.transform.position = playerHolding.transform.position + 4*Vector3.up;
        }
    }
}
