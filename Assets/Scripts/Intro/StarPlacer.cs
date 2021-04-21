using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

namespace Intro
{
    public class StarPlacer : MonoBehaviour
    {
        [SerializeField] private GameObject star;
        [SerializeField] private int        numStars = 350;

        private void Start()
        {
            var size = GetComponentInParent<CanvasScaler>().referenceResolution;
            for(int i = 0; i < numStars; i++)
            {
                var pos     = new Vector3(Random.Range(0f, size.x), Random.Range(0f, size.y));
                var newStar = Instantiate(star, Vector3.zero, Quaternion.identity, transform);
                newStar.transform.localPosition = pos;
            }
        }
    }
}