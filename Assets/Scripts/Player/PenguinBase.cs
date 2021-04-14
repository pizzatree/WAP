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
        private ServerGameManager gameManager;

        public IInputs InputHandler { get; private set; }
        // public ITeam TeamHandler {get; private set; }
        [SyncVar] public bool greenTeam = false; // oh dear, it appears like I'm doing this one quick and dirty
        [SyncVar] public bool isHoldingFlag = false;

        public override void OnStartLocalPlayer()
        {
            base.OnStartLocalPlayer();

            gameManager = GameObject.Find("[ Game Manager ]").GetComponent<ServerGameManager>();

            gameObject.AddComponent<PlayerCursor>();
            CameraManager.Instance?.HandleNewCharacter(cameraTransform);

            // absolutely garbo team picking, can implement with ITeam later though 
            if (gameManager.playersConnected %2 == 1) {
                greenTeam = true;
                this.transform.position = gameManager.teamSpawns[0].position;
            }
            else {
                greenTeam = false;
                this.transform.position = gameManager.teamSpawns[1].position;
            }
            CmdSetTeam(greenTeam);

            InputHandler = new KBM();
            SendMessage("AssignInputs", InputHandler);
        }

        private void OnDisable()
        {
            if(!isLocalPlayer)
                return;

            CameraManager.Instance?.HandleLostCharacter();
        }

        private void Update() {}

        private void OnCollisionEnter(Collision other)
        {
            if(other.gameObject.layer == LayerMask.NameToLayer("Surface"))
                SendMessage("HandleGrounded", true);

        }

        private void OnTriggerEnter(Collider other)
        {
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
            if (flagComp.greenTeam == this.greenTeam) {
                if (greenTeam)
                    flag.transform.position = gameManager.flagSpawns[0].position;
                else
                    flag.transform.position = gameManager.flagSpawns[1].position;
            }
            else {
                flagComp.playerHolding = this.gameObject;
                flagComp.isHeld        = true;
                isHoldingFlag          = true;
            }
        }

        [Command]
        private void CmdSetTeam(bool greenTeam) {
            RpcSetTeam(greenTeam);
        }

        [ClientRpc]
        private void RpcSetTeam(bool greenTeam) {
            this.greenTeam = greenTeam;
            // ATTEMPTED TO CHANGE HELMETS, BUT DID NOT WORK!!
            // if (greenTeam)
            //     this.GetComponentInChildren<Helmet_DO_NOT_REMOVE_AWFUL_CODE_INVOLVED>().gameObject.GetComponent<MeshRenderer>().material = gameManager.teamColors[0];
            // else
            //     this.GetComponentInChildren<Helmet_DO_NOT_REMOVE_AWFUL_CODE_INVOLVED>().gameObject.GetComponent<MeshRenderer>().material = gameManager.teamColors[1];

        }
    }
}