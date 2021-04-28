using Mirror;
using UnityEngine;

namespace Player
{
    public class PlayerSlide : ReceivesInputs
    {
        [SerializeField] private PhysicMaterial regPMat, slidePMat;

        private bool grounded;
        private bool sliding;

        private Collider[] colliders;
        private Animator ani;

        private bool isBot;

        private void Start()
        {
            isBot = isServer && !isLocalPlayer;

            colliders = GetComponentsInChildren<Collider>();
            ani = GetComponentInChildren<Animator>();
        }

        private void Update()
        {
            if(!isLocalPlayer && !isBot)
                return;

            var newSlide = Input.GetKey(KeyCode.LeftControl);
            if(newSlide == sliding) 
                return;
            
            sliding = newSlide;

            if(sliding)
                SetCollidersToSlide();
            else
                ResetColliders();

            ani.SetBool("isSliding", sliding); // should probably move this somewhere else, but this works for now
        }

        private void ResetColliders()
        {
            SetPhysicMaterial(regPMat);
        }

        private void SetCollidersToSlide()
        {
            SetPhysicMaterial(slidePMat);
        }

        private void SetPhysicMaterial(PhysicMaterial newMaterial)
        {
            foreach(var collider in colliders)
                collider.material = newMaterial;
        }

        public bool GetSliding() {
            return sliding;
        }

        // Called by PenguinBase via Message
        private void HandleGrounded(bool newValue) => grounded = newValue;
    }
}