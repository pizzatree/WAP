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

            inputs = GetComponent<PenguinBase>().InputHandler;
        }

        private void OnDisable()
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible   = true;
        }

        private void Update()
        {
            if(inputs == null)
                return;
            
            Cursor.lockState = (inputs.PressedPause())
                ? CursorLockMode.None
                : CursorLockMode.Confined;
            Cursor.visible = (Cursor.lockState != CursorLockMode.Confined);
        }
    }
}