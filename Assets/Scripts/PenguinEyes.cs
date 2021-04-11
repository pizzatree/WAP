using Player;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class PenguinEyes : MonoBehaviour
{
    [SerializeField] private float timeBetweenBlinks = 3f;

    private Animator animator;

    private static readonly int AngerTrigger  = Animator.StringToHash("Angry");
    private static readonly int ScaredTrigger = Animator.StringToHash("Scared");
    private static readonly int BlinkTrigger = Animator.StringToHash("Blink");

    private void Start()
    {
        animator = GetComponent<Animator>();

        GetComponentInParent<RocketLauncher>().OnFire += () => animator.SetTrigger(AngerTrigger);
        // OnHit += () => animator.SetTrigger(ScaredTrigger);
        Invoke(nameof(Blink), Random.Range(0.75f * timeBetweenBlinks, 1.25f * timeBetweenBlinks));
    }

    private void Blink()
    {
        animator.SetTrigger(BlinkTrigger);
        Invoke(nameof(Blink), Random.Range(0.75f * timeBetweenBlinks, 1.25f * timeBetweenBlinks));
    }
}