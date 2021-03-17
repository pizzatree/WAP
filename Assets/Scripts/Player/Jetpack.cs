using Mirror;
using UnityEngine;

namespace Player
{
    public class Jetpack : NetworkBehaviour
    {
        [SerializeField] private float verticalThrust = 15f;

        private bool startedJetpack, usingJetpack;
        private bool grounded;
    
        private Rigidbody rb;

        private void Start()
        {
            Debug.Log("Add server thrust limit to jetpack");
            Debug.Log("Stop hardcoding jetpack, use inputs class when made");

            grounded = true;
            rb       = GetComponent<Rigidbody>();
        }

        private void Update()
        {
            if(!isLocalPlayer)
                return;

            startedJetpack = !grounded      && (startedJetpack || Input.GetKeyDown(KeyCode.Space));
            usingJetpack   = startedJetpack && Input.GetKey(KeyCode.Space);
        }

        private void FixedUpdate()
        {
            if(!isLocalPlayer || !usingJetpack)
                return;

            var force = new Vector3(0f, verticalThrust, 0f);
            rb.AddForce(force, ForceMode.Acceleration);
        }

        // Called by PlayerBase via Message
        private void HandleGrounded(bool newValue) => grounded = newValue;
    }
}
