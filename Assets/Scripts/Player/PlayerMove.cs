using Mirror;
using UnityEngine;

namespace Player
{
    public class PlayerMove : NetworkBehaviour
    {
        [SyncVar]
        public Vector3 motion; // will be used for syncing motion across the network, for now just using networktransform

        [SerializeField]
        private float moveSpeed   = 3f,
                      camRotSpeed = 100.0f;

        private Vector3 newRotOffset;

        private Rigidbody rb;

        private void Start()
        {
            rb = GetComponent<Rigidbody>();
        }

        private void Update()
        {
            if(!isLocalPlayer)
                return;

            // basic motion & rotation
            motion = (transform.forward * (moveSpeed * Input.GetAxis("Vertical"))) +
                     (transform.right   * (moveSpeed * Input.GetAxis("Horizontal")));
            newRotOffset = new Vector3(0,
                                       Input.GetAxis("Mouse X") * camRotSpeed,
                                       0); // no Y rotation on the model, no head animations yet
        }

        private void FixedUpdate()
        {
            if(!isLocalPlayer)
                return;

            rb.MovePosition(rb.position + (motion * Time.fixedDeltaTime));
            rb.MoveRotation(rb.rotation * Quaternion.Euler(newRotOffset * Time.fixedDeltaTime));
        }
    }
}