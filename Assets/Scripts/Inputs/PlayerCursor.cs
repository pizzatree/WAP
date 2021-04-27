using Player;
using UnityEngine;

namespace Inputs
{
    public class PlayerCursor : MonoBehaviour
    {
        private IInputs inputs;
        private bool pressedEscape = false;

        private void OnEnable()
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible   = false;

            inputs = GetComponent<PenguinBase>().InputHandler;
        }

        private void OnDisable()
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible   = true;
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Escape)) {
                pressedEscape = !pressedEscape;
            }

            // if(inputs == null)s
            //     return;
            
            Cursor.lockState = (pressedEscape)
                ? CursorLockMode.None
                : CursorLockMode.Confined;
            Cursor.visible = (pressedEscape);
        }
    }
}