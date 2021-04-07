using System;
using Inputs;
using Managers;
using Mirror;
using UnityEngine;

namespace Player
{
    [RequireComponent(typeof(PlayerMove),
                         typeof(Rigidbody))]
    [SelectionBase]
    public class PlayerBase : NetworkBehaviour
    {
        [SerializeField]
        private Transform cameraTransform; // for the love of all that's holy we must do this differently
        // when it's more built up

        private IInputs inputHandler;

        public override void OnStartLocalPlayer()
        {
            base.OnStartLocalPlayer();

            gameObject.AddComponent<PlayerCursor>();
            CameraManager.Instance?.HandleNewCharacter(cameraTransform);

            inputHandler = new KBM();
        }

        private void OnDisable()
        {
            if(!isLocalPlayer)
                return;

            CameraManager.Instance?.HandleLostCharacter();
        }

        private void OnCollisionEnter(Collision other)
        {
            if(other.gameObject.layer == LayerMask.NameToLayer("Surface"))
                SendMessage("HandleGrounded", true);
        }

        private void OnCollisionExit(Collision other)
        {
            if(other.gameObject.layer == LayerMask.NameToLayer("Surface"))
                SendMessage("HandleGrounded", false);
        }
    }
}