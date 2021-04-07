using Player;
using UnityEngine;
using UnityEngine.AI;

namespace Bot.States
{
    public class PursueFlag : IState
    {
        private NavMeshAgent agent;

        // pro tip: when adding new fields in Rider
        // "ctorf" auto populates constructor with said fields
        public PursueFlag(NavMeshAgent agent)
        {
            this.agent = agent;
        }
    
        public void Tick()
        {
            // basically update
        }

        public void OnEnter()
        {
            // probably find target flag here
            Debug.Log("PursueFlag OnEnter()");
            agent.enabled = true;
            if (agent != null) {
                Debug.Log("Set Agent Destination");
                agent.SetDestination(GameObject.Find("Flag").transform.position);
            }
        }

        public void OnExit()
        {
            // clean up?
            agent.enabled = false;
        }

        // find target()
        // move()
        // etc. 
    }
}