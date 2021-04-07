using UnityEngine;

public interface IInputs
{
    public Vector2 MoveDirection();
    public Vector2 AimDirection();
    public bool    PressedJump();
    public bool    HoldingJump();
    public bool    PressedSlide();
    public bool    HoldingSlide();
    public bool PressedSlap();
}
