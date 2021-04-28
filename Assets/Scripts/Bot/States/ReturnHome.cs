using Player;
using UnityEngine;
using UnityEngine.AI;

namespace Bot.States
{
    public class ReturnHome : IState
    {
        private NavMeshAgent agent;
        private bool         pursueGreen;

        public ReturnHome(NavMeshAgent agent, bool pursueGreen)
        {
            this.agent       = agent;
            this.pursueGreen = pursueGreen;
        }

        public void Tick()
        {
            // basically update
            if (agent != null) {
                var findIglooName = (pursueGreen) ? "Green Igloo" : "Purple Igloo";
                var iglooObj      = GameObject.Find(findIglooName);
                
                if(iglooObj)
                    agent.SetDestination(iglooObj.transform.position);
            }
        }

        public void OnEnter()
        {
            // probably find target igloo here
            agent.enabled = true;
        }

        public void OnExit()
        {
            // clean up?
            agent.enabled = false;
        }

    }
}