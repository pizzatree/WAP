using UnityEngine;
using UnityEngine.UI;

namespace Intro
{
    public class Star : MonoBehaviour
    {
        private Image image;

        private float offset, scale, speed;

        private void Start()
        {
            image = GetComponent<Image>();

            var curSize = image.rectTransform.sizeDelta;
            image.rectTransform.sizeDelta = new Vector2(Random.Range(curSize.x * .75f, curSize.x * 1.25f),
                                                        Random.Range(curSize.y * .75f, curSize.y * 1.25f));
            image.rectTransform.Rotate(0f, 0f, Random.Range(0f, 360f));

            offset = Random.Range(0f,   0.2f);
            scale  = Random.Range(0.1f, 0.2f);
            speed  = 2f;
        }

        private void Update()
        {
            var color = image.color;
            color.a     = (Mathf.Sin(Time.time * speed) * scale) + offset;
            image.color = color;
        }
    }
}