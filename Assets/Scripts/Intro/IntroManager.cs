using UnityEngine;
using UnityEngine.SceneManagement;

namespace Intro
{
    public class IntroManager : MonoBehaviour
    {
        [SerializeField] private RectTransform textCrawl;
        [SerializeField] private float         crawlStart = -800, crawlEnd = 1250;
        [SerializeField] private float         crawlSpeed = 1f;

        private float curY;

        private AudioSource audioSource;
        private float       startVolume;

        private void Start()
        {
            audioSource = GetComponent<AudioSource>();
            startVolume = audioSource.volume;
        
            curY = crawlStart;
        }

        // Update is called once per frame
        private void Update()
        {
            textCrawl.localPosition = new Vector3(0, curY, 0);
        
            curY += crawlSpeed * Time.deltaTime;
            if(curY >= crawlEnd)
                StartMenuScene();
        
            var distFromEnd = crawlEnd - curY;
            if(distFromEnd <= 225f)
                audioSource.volume = startVolume * ((distFromEnd - 25f) / 225f);
        }

        public void StartMenuScene()
        {
            SceneManager.LoadScene(1);
        }
    }
}
