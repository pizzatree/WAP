using System;
using Managers;
using Mirror;
using UnityEngine;

namespace Player
{
    [RequireComponent(typeof(PlayerMove),
                         typeof(PlayerCursor),
                         typeof(Rigidbody))]
    [SelectionBase]
    public class PlayerBase : NetworkBehaviour
    {
        [SerializeField]
        private Transform cameraTransform; // for the love of all that's holy we must do this differently
                                           // when it's more built up

        public override void OnStartLocalPlayer()
        {
            base.OnStartLocalPlayer();

            CameraManager.Instance?.HandleNewCharacter(cameraTransform);
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