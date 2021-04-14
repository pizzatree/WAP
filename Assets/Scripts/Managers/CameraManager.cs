using Cinemachine;
using UnityEngine;

// should make this way more robust honestly
// instead of singleton, have player spawner have on OnSpawn event with a player to track
// and basically completely redo this. it's bad.
namespace Managers
{
    public class CameraManager : MonoBehaviour
    {
        public static CameraManager Instance;

        [SerializeField]
        private CinemachineVirtualCamera playerCam, birdsEyeCam;

        private void Awake()
        {
            if(Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);
                return;
            }    
        
            Destroy(gameObject);
        }

        public void HandleNewCharacter(Transform newFollow)
        {
            playerCam.gameObject.SetActive(true);
            birdsEyeCam.gameObject.SetActive(false);
            playerCam.Follow = newFollow;
        }

        public void HandleLostCharacter()
        {
            playerCam.gameObject.SetActive(false);
            birdsEyeCam.gameObject.SetActive(true);
        }
    }
}
