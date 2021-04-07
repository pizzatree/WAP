using Mirror;
using UnityEngine;

public class Rocket : NetworkBehaviour
{
    private float timeSinceInstantiated = 0;
    public Vector3 travelDir = Vector3.zero;
    public float rocketSpeed = 0.2f;

    void Update()
    {
        timeSinceInstantiated += Time.deltaTime;

        this.transform.position += travelDir*rocketSpeed;

        if (timeSinceInstantiated > 1.5f) {
            Destroy(gameObject);
        }
    }

    void OnCollisionEnter(Collision col)
    {
        if (timeSinceInstantiated > 0.2f)
            Destroy(gameObject);
    }

}
