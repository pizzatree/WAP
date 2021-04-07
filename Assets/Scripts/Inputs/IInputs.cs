using UnityEngine;

namespace Inputs
{
    public interface IInputs
    {
        Vector2 MoveDirection();
        Vector2 AimDirection();
        bool    PressedJump();
        bool    HoldingJump();
        bool    PressedSlide();
        bool    HoldingSlide();
        bool    PressedFire();
        bool    PressedReload();
        bool    PressedPause();
        bool    PressedSlap();
    }
}