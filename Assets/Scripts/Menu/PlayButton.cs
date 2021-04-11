using UnityEngine;
using UnityEngine.SceneManagement;

namespace Menu
{
    public class PlayButton : MonoBehaviour
    {
        public void ChangeScene(string sceneName)
            => SceneManager.LoadScene(sceneName);

        public void ChangeScene(int sceneNumber)
            => SceneManager.LoadScene(sceneNumber);
    }
}