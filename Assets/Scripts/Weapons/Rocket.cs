using Mirror;
using Unity.Mathematics;
using UnityEngine;

public class Rocket : NetworkBehaviour
{
    private float timeSinceInstantiated = 0;
    public Vector3 travelDir = Vector3.zero;
    public float rocketSpeed = 0.2f;
    public float travelTime = 2.0f;

    [SerializeField] private GameObject explosionParticles;

    void Update()
    {
        timeSinceInstantiated += Time.deltaTime;

        this.transform.position += travelDir*rocketSpeed;

        if (timeSinceInstantiated > travelTime)
        {
            BlowUp();
        }
    }

    void OnCollisionEnter(Collision col)
    {
        if (timeSinceInstantiated > 0.2f)
            BlowUp();
    }

    private void BlowUp()
    {
        Instantiate(explosionParticles, transform.position, Quaternion.identity, null);
        Destroy(gameObject);
    }

}
