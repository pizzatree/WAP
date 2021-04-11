using UnityEngine;

namespace Inputs
{
    public class KBM : IInputs
    {
        public Vector2 MoveDirection()
        {
            var x = Input.GetAxisRaw("Horizontal");
            var y = Input.GetAxisRaw("Vertical");

            return new Vector2(x, y);
        }

        public Vector2 AimDirection()
        {
            var x = Input.GetAxisRaw("Mouse X");
            var y = Input.GetAxisRaw("Mouse Y") * Preferences.AimInverted;

            return new Vector2(x, y) * Preferences.AimSensitivity;
        }

        public bool PressedJump()
            => Input.GetKeyDown(KeyCode.Space);

        public bool HoldingJump()
            => Input.GetKey(KeyCode.Space);

        public bool PressedSlide()
            => Input.GetKeyDown(KeyCode.LeftControl);

        public bool HoldingSlide()
            => Input.GetKey(KeyCode.LeftControl);

        public bool PressedFire()
            => Input.GetMouseButton(0);
    
        public bool PressedReload()
            => Input.GetKey(KeyCode.R);
    
        public bool PressedPause()
            => Input.GetKey(KeyCode.Escape);

        public bool PressedSlap()
            => Input.GetMouseButton(1);
    }
}