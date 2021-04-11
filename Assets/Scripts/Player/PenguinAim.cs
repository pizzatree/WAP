using UnityEngine;

namespace Player
{
    public class PenguinAim : ReceivesInputs
    {
        [SerializeField] private float verticalClamp = 60f;

        [SerializeField] private Transform camera;

        private float yRot = 0f;

        private void Update()
        {
            var mouse = inputs.AimDirection();
            
            var newRot = camera.localEulerAngles;
            yRot += mouse.y * Time.deltaTime;
            yRot =  Mathf.Clamp(yRot, -verticalClamp, verticalClamp);
            newRot.x                = yRot;
            
            camera.localEulerAngles = newRot;
            transform.Rotate(Vector3.up * (mouse.x * Time.deltaTime));
        }
    }
}