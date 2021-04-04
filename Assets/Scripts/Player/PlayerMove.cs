using Mirror;
using UnityEngine;

namespace Player
{
    public class PlayerMove : NetworkBehaviour
    {
        [SyncVar] public Vector3 mvmt;      // for syncing walking motion across the network
        [SyncVar] public Vector3 mvmtSlide; // for syncing sliding motion across the network

        [SerializeField] private float moveSpeed   = 1.0f,
                                       camRotSpeed = 100.0f,
                                       jumpForce   = 10f,
                                       slideSpeed  = 1.0f;
        private bool doJump;
        private bool    grounded;
        private Vector3 newRotOffset;

        private Rigidbody rb;
        private Animator ani;

        private void Start()
        {
            rb = GetComponent<Rigidbody>();
            ani = GetComponentInChildren<Animator>();
        }

        private void Update()
        {
            if(!isLocalPlayer)
                return;

            // basic motion & rotation
            mvmt =  (transform.forward * (moveSpeed * (int)Input.GetAxisRaw("Vertical"))) +
                    (transform.right   * (moveSpeed * (int)Input.GetAxisRaw("Horizontal")));

            mvmtSlide = (transform.forward * (slideSpeed * Input.GetAxisRaw("Vertical"))) +
                        (transform.right   * (slideSpeed * Input.GetAxisRaw("Horizontal")));

            newRotOffset = new Vector3(0,
                                       Input.GetAxis("Mouse X") * camRotSpeed,
                                       0); // TODO: rotate head independently ahead of the camera

            // I know this should be in FixedUpdate()
            // but otherwise the game is unplayable, turning the camera is VERY stuttery
            // TODO: unlock the camera from the back of the penguin
            rb.MoveRotation(rb.rotation * Quaternion.Euler(newRotOffset * Time.deltaTime)); 

            ani.SetBool("isWalking", mvmt.magnitude >= 0.01f); // should probably move this somewhere else, but this works for now

            if(Input.GetKeyDown(KeyCode.Space) && grounded)
                doJump = true;
                // Jump();
        }

        private void FixedUpdate()
        {
            if(!isLocalPlayer)
                return;
            
            CmdMove(this.GetComponent<PlayerSlide>().GetSliding(), doJump); // ask the server to move the way you want to
            doJump = false;

            // rb.AddForce(mvmt, ForceMode.Acceleration); // old way (clientside)
            // rb.velocity = mvmt;
            // rb.MovePosition(rb.position + (mvmt * Time.fixedDeltaTime));
            // rb.MoveRotation(rb.rotation * Quaternion.Euler(newRotOffset * Time.fixedDeltaTime));
        }


        [Command]
        private void CmdMove(bool sliding, bool jumping) {
            // tell the server to update the movement of the players
            if (sliding) {
                RpcMoveSlide();
            }
            else {
                RpcMove();
            }

            if (jumping) {
                RpcJump();
            }
        }

        [ClientRpc]
        private void RpcMove() {
            rb.velocity = new Vector3(mvmt.x, rb.velocity.y, mvmt.z);
            // rb.AddForce(mvmt, ForceMode.VelocityChange);
        }

        [ClientRpc]
        private void RpcMoveSlide() {
            rb.AddForce(mvmtSlide, ForceMode.Acceleration);
        }

        [ClientRpc]
        private void RpcJump()
        {
            var force = new Vector3(0f, jumpForce, 0f);
            
            if(grounded)
                rb.AddForce(force, ForceMode.Impulse);
        }


        
        // Called by PlayerBase via Message
        private void HandleGrounded(bool newValue) => grounded = newValue;
    }
}