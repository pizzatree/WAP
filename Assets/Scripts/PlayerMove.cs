using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class PlayerMove : NetworkBehaviour
{
    [SyncVar] public Vector3 motion; // will be used for syncing motion across the network, for now just using networktransform
    private Rigidbody rb; // player's rigidbody
    private float moveSpeed = 0.05f; // motion speed
    private float camSpeed = 100.0f; // camera rotation speed

    // Start is called before the first frame update
    void Start()
    {
        rb = this.GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!isLocalPlayer)
            return;
            
        // makes the cursor invisible, should probably move this somewhere else later
        Cursor.visible = false;

        // basic motion & rotation
        motion = (Vector3.forward*moveSpeed*Input.GetAxis("Vertical")) + (Vector3.right*moveSpeed*Input.GetAxis("Horizontal"));
        transform.Translate(motion);
        transform.Rotate(new Vector3(0, Input.GetAxis("Mouse X") * camSpeed * Time.deltaTime, 0)); // no Y rotation on the model, no head animations yet
    }
}
