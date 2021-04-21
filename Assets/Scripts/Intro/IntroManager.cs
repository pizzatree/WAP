using UnityEngine;
using UnityEngine.SceneManagement;

namespace Intro
{
    public class IntroManager : MonoBehaviour
    {
        [SerializeField] private RectTransform textCrawl;
        [SerializeField] private float         crawlEnd = 1250;
        [SerializeField] private float         crawlSpeed = 1f;

        private float curY;

        private AudioSource audioSource;
        private float       startVolume;

        private void Start()
        {
            audioSource = GetComponent<AudioSource>();
            startVolume = audioSource.volume;
        }

        // Update is called once per frame
        private void Update()
        {
            textCrawl.Translate(0f, Time.deltaTime * crawlSpeed, 0f);

            curY = textCrawl.localPosition.y;
            if(curY >= crawlEnd)
                StartMenuScene();

            var distFromEnd = crawlEnd - curY;
            if(distFromEnd <= 400)
                audioSource.volume = startVolume * ((distFromEnd - 25f) / 400);
        }

        public void StartMenuScene()
        {
            SceneManager.LoadScene(1);
        }
    }
}