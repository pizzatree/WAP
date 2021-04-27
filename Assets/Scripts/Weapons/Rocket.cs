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
    [SyncVar] public bool greenTeam;

    void Update()
    {
        if (!isServer)
            return;

        timeSinceInstantiated += Time.deltaTime;

        this.transform.position += travelDir*rocketSpeed;

        if (timeSinceInstantiated > travelTime)
        {
            RpcBlowUp();
        }
    }

    void OnCollisionEnter(Collision col)
    {
        if (!isServer)
            return;

        if (timeSinceInstantiated > 0.1f && col.gameObject.tag != "Player")
            RpcBlowUp();
    }

    [Command]
    public void CmdBlowUp(){
        RpcBlowUp();
    }

    [ClientRpc]
    private void RpcBlowUp()
    {
        Instantiate(explosionParticles, transform.position, Quaternion.identity, null);
        Destroy(gameObject);
    }

}
