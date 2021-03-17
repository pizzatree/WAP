using Mirror;
using UnityEngine;

namespace Player
{
    public class PlayerCursor : NetworkBehaviour // probably doesn't need to be a netbehaviour
    {
        public override void OnStartLocalPlayer()
        {
            base.OnStartLocalPlayer();

            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible   = false;
        }

        private void OnDisable()
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible   = true;
        }

        private void Update()
        {
            if(!isLocalPlayer || !Input.GetKeyDown(KeyCode.Escape))
                return;

            Cursor.lockState = (Cursor.lockState != CursorLockMode.None)
                ? CursorLockMode.None
                : CursorLockMode.Confined;
            Cursor.visible = !Cursor.visible;

            Debug.Log("Stop hardcoding the cursor lock when inputs class is made.");
        }
    }
}