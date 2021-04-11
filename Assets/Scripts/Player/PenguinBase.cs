using Inputs;
using Managers;
using Mirror;
using UnityEngine;

namespace Player
{
    [SelectionBase]
    public class PenguinBase : NetworkBehaviour
    {
        [SerializeField]
        private Transform cameraTransform; // for the love of all that's holy we must do this differently
        // when it's more built up

        public IInputs InputHandler { get; private set; }

        public override void OnStartLocalPlayer()
        {
            base.OnStartLocalPlayer();

            gameObject.AddComponent<PlayerCursor>();
            CameraManager.Instance?.HandleNewCharacter(cameraTransform);

            InputHandler = new KBM();
            SendMessage("AssignInputs", InputHandler);
        }

        private void OnDisable()
        {
            if(!isLocalPlayer)
                return;

            CameraManager.Instance?.HandleLostCharacter();
        }

        private void Update()
        {
            //this.GetComponent<NavMeshAgent>().enabled = isAI;
        }

        private void OnCollisionEnter(Collision other)
        {
            if(other.gameObject.layer == LayerMask.NameToLayer("Surface"))
                SendMessage("HandleGrounded", true);

            if(other.gameObject.tag == "Finish")
            {
                CmdSetFlagOwner(other.gameObject);
            }
        }

        private void OnCollisionExit(Collision other)
        {
            if(other.gameObject.layer == LayerMask.NameToLayer("Surface"))
                SendMessage("HandleGrounded", false);
        }

        [Command]
        private void CmdSetFlagOwner(GameObject flag)
        {
            RpcSetFlagHolder(flag);
        }

        [ClientRpc]
        private void RpcSetFlagHolder(GameObject flag)
        {
            Flag flagComp = flag.GetComponent<Flag>();
            flagComp.playerHolding = this.gameObject;
            flagComp.isHeld        = true;
        }
    }
}