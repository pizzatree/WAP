using Player;
using UnityEngine;

namespace Inputs
{
    public class PlayerCursor : MonoBehaviour
    {
        private IInputs inputs;
        
        private void OnEnable()
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible   = false;

            inputs = GetComponent<PlayerBase>().InputHandler;
        }

        private void OnDisable()
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible   = true;
        }

        private void Update()
        {
            if(inputs != null && !inputs.PressedPause() )
                return;
            
            Cursor.lockState = (Cursor.lockState != CursorLockMode.None)
                ? CursorLockMode.None
                : CursorLockMode.Confined;
            Cursor.visible = !Cursor.visible;
        }
    }
}