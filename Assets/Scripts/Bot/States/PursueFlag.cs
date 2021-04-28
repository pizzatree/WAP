using Player;
using UnityEngine;
using UnityEngine.AI;

namespace Bot.States
{
    public class PursueFlag : IState
    {
        private NavMeshAgent agent;
        private bool         pursueGreen;

        public PursueFlag(NavMeshAgent agent, bool pursueGreen)
        {
            this.agent       = agent;
            this.pursueGreen = pursueGreen;
        }

        public void Tick()
        {
            // basically update
            if (agent != null) {
                var findFlagName = (pursueGreen) ? "GreenFlag" : "PurpleFlag";
                var flagObj      = GameObject.Find(findFlagName   + "(Clone)");
                
                if(flagObj)
                    agent.SetDestination(flagObj.transform.position);
            }
        }

        public void OnEnter()
        {
            // probably find target flag here
            agent.enabled = true;
        }

        public void OnExit()
        {
            // clean up?
            agent.enabled = false;
        }

    }
}