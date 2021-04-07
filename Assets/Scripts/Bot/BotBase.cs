using System;
using System.Linq;
using Bot.States;
using Mirror;
using Player;
using UnityEngine;

namespace Bot
{
    public class BotBase : NetworkBehaviour
    {
        private StateMachine sm;

        private PlayerMove movement; // get component or assemble in start?
        // or use something else, leaving as example of how to use the states
        
        private void Start()
        {
            if(!isServer)
                return;
            CreateStates();
        }

        private void Update()
        {
            if(!isServer)
                return;
            
            sm?.Tick();
        }

        private void CreateStates()
        {
            sm = new StateMachine();

            var findFlag = new FindFlag(movement);
            var aggro    = new Aggro(movement);

            sm.AddTransition(aggro, findFlag, EnemyIsWithinRange());
            sm.AddTransition(aggro, findFlag, EnemyIsNotWithinRange());

            Func<bool> EnemyIsWithinRange() => () => 15f >= Vector3.Distance(transform.position, (FindObjectsOfType<Purple>())
                .OrderBy(t=> Vector3.Distance(transform.position, t.transform.position))
                .FirstOrDefault().GetComponent<Transform>().position);
            
            Func<bool> EnemyIsNotWithinRange() => () => 15f < Vector3.Distance(transform.position, (FindObjectsOfType<Purple>())
                .OrderBy(t=> Vector3.Distance(transform.position, t.transform.position))
                .FirstOrDefault().GetComponent<Transform>().position);
        }
    }
}