using UnityEngine;

namespace Inputs
{
    public class Bot : IInputs
    {
        public bool pressedJump,
                     holdingJump,
                     pressedSlide,
                     holdingSlide,
                     pressedFire,
                     pressedReload,
                     pressedPause,
                     pressedSlap;
        
        public Vector2 MoveDirection()
            => Vector2.zero;

        public Vector2 AimDirection()
            => Vector2.zero;

        public bool PressedJump()
            => pressedJump;
        
        public bool HoldingJump()
            => holdingJump;

        public bool PressedSlide()
            => pressedSlide;
        
        public bool HoldingSlide()
            => holdingSlide;
        
        public bool PressedFire()
            => pressedFire;
        
        public bool PressedReload()
            => pressedReload;
        
        public bool PressedPause()
            => pressedPause;
        
        public bool PressedSlap()
            => pressedSlap;
        
        
    }
}