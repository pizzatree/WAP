using Mirror;
using UnityEngine;

namespace Player
{
    public class Jetpack : ReceivesInputs
    {
        [SerializeField] private float      verticalThrust = 15f;
        [SerializeField] private ParticleSystem[] particles;

        private bool startedJetpack, usingJetpack;
        private bool grounded;
        
        private float jetpackTimeLimit = 2.0f;
        private float timeSinceJetpack = 0;

        private Rigidbody rb;

        private bool isBot;

        private void Start()
        {
            isBot = isServer && !isLocalPlayer;

            Debug.Log("Add server thrust limit to jetpack");
            Debug.Log("Stop hardcoding jetpack, use inputs class when made");

            grounded = true;
            rb       = GetComponent<Rigidbody>();
        }

        private void Update()
        {
            if(!isLocalPlayer && !isBot)
                return;

            startedJetpack = !grounded      && (startedJetpack || inputs.PressedJump());
            usingJetpack   = startedJetpack && inputs.HoldingJump() && timeSinceJetpack <= jetpackTimeLimit;

            if (usingJetpack)
                timeSinceJetpack += Time.deltaTime;
            if (grounded)
                timeSinceJetpack = 0;

            foreach(var particle in particles)
            {
                if(!usingJetpack)
                    particle.Stop(true, ParticleSystemStopBehavior.StopEmitting);
                else
                    particle.Play(true);
            }
        }

        private void FixedUpdate()
        {
            if((!isLocalPlayer && !isBot) || !usingJetpack)
                return;

            var force = new Vector3(0f, verticalThrust, 0f);
            rb.AddForce(force, ForceMode.Acceleration);
        }

        // Called by PenguinBase via Message
        private void HandleGrounded(bool newValue) => grounded = newValue;
    }
}