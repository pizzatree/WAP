using System;
using System.Linq;
using Bot.States;
using Mirror;
using Player;
using UnityEngine;
using UnityEngine.AI;

namespace Health
{
    public delegate void Notify();

    public class Health
    {
        private float health;
        public event Notify HealthChanged;

        public void StartProcess()
        {
            Console.WriteLine("Process Started!");
            // some code here..
            OnHealthChanged();
        }

        protected virtual void OnHealthChanged() //protected virtual method
        {
            //if ProcessCompleted is not null then call delegate
            HealthChanged?.Invoke();
        }
    }
}