using Mirror;
using UnityEngine;

namespace Player
{
    public class PlayerSlide : NetworkBehaviour
    {
        [SerializeField] private PhysicMaterial regPMat, slidePMat;

        private bool grounded;
        private bool sliding;

        private Collider[] colliders;

        private void Start()
        {
            colliders = GetComponentsInChildren<Collider>();
        }

        private void Update()
        {
            if(!isLocalPlayer)
                return;

            var newSlide = Input.GetKey(KeyCode.LeftControl);
            if(newSlide == sliding) 
                return;
            
            sliding = newSlide;

            if(sliding)
                SetCollidersToSlide();
            else
                ResetColliders();
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

        // Called by PlayerBase via Message
        private void HandleGrounded(bool newValue) => grounded = newValue;
    }
}