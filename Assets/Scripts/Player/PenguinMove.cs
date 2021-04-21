using System;
using Inputs;
using Mirror;
using UnityEngine;

// TODO: REFACTOR THIS NOT TO INCLUDE PLAYER REFERENCES
namespace Player
{
    public class PenguinMove : ReceivesInputs
    {
        [SyncVar] public Vector3 moveValue; // for syncing motion across the network

        [SerializeField] private float moveSpeed   = 1.8f,
                                       jumpForce   = 5f,
                                       slideSpeed  = 3f;

        private bool    doJump;
        private bool    grounded;
        private Vector3 newRotOffset;

        private Rigidbody rb;
        private Animator  ani;
        private ServerGameManager gameManager;
        
        private void Start()
        {
            rb  = GetComponent<Rigidbody>();
            ani = GetComponentInChildren<Animator>();
            gameManager = GameObject.Find("[ Game Manager ]").GetComponent<ServerGameManager>();
        }

        private void Update()
        {
            if(!isLocalPlayer || inputs == null || gameManager.gameState == GameState.GameEnd)
                return;

            if (gameManager.gameState == GameState.TeamSelection || this.GetComponent<PlayerHealth>().health <= 0) {
                if (GetComponent<PenguinBase>().greenTeam)
                    this.transform.position = gameManager.teamSpawns[0].position;
                else
                    this.transform.position = gameManager.teamSpawns[1].position;
            }

            if (gameManager.gameState == GameState.TeamSelection)
                return;
            
            rb.isKinematic = false;

            // basic motion & rotation
            var moveInput = inputs.MoveDirection();
            moveValue = transform.forward * (moveSpeed * (int) moveInput.y) +
                        transform.right   * (moveSpeed * (int) moveInput.x);


            ani.SetBool("isWalking",
                        moveValue.magnitude >=
                        0.01f); // should probably move this somewhere else, but this works for now
            


            if(inputs.PressedJump() && grounded)
                doJump = true;
        }

        private void FixedUpdate()
        {
            if(!isLocalPlayer)
                return;

            CmdMove(this.GetComponent<PlayerSlide>().GetSliding(),
                    doJump); // ask the server to move the way you want to
            doJump = false;

        }


        [Command]
        private void CmdMove(bool sliding, bool jumping)
        {
            // tell the server to update the movement of the players
            if(sliding)
                RpcMoveSlide();
            else
                RpcMove();

            if(jumping)
                RpcJump();
        }

        [ClientRpc]
        private void RpcMove()
        {
            var mvmt = moveSpeed * moveValue;
            rb.velocity = new Vector3(mvmt.x, rb.velocity.y, mvmt.z);
            // rb.AddForce(mvmt, ForceMode.VelocityChange);
        }

        [ClientRpc]
        private void RpcMoveSlide()
        {
            var mvmtSlide = slideSpeed * moveValue;
            rb.AddForce(mvmtSlide, ForceMode.Acceleration);
        }

        [ClientRpc]
        private void RpcJump()
        {
            var force = new Vector3(0f, jumpForce, 0f);

            if(grounded)
                rb.AddForce(force, ForceMode.Impulse);
        }


        // Called by PenguinBase via Message
        private void HandleGrounded(bool newValue) => grounded = newValue;
    }
}