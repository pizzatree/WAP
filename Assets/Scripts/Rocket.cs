using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rocket : MonoBehaviour
{
    private float timeSinceInstantiated = 0;
    public Vector3 travelDir = Vector3.zero;

    void Update()
    {
        timeSinceInstantiated += Time.deltaTime;

        this.transform.position += travelDir*0.1f;

        if (timeSinceInstantiated > 1.5f) {
            Destroy(gameObject);
        }
    }

    void OnCollisionEnter(Collision col)
    {
        Destroy(gameObject);
    }

}
