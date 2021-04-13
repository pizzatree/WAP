using UnityEngine;
using System.Collections;

// namespace Player
// {
    public class EventManager : MonoBehaviour
    {
        public delegate void OnHealthUpdate();
        public static event OnHealthUpdate onHealthUpdate;

        public void HealthUpdate() {
            if (onHealthUpdate != null)
                onHealthUpdate();
        }

        // private void OnCollisionEnter(Collision other)
        // {
        //     if (other.gameObject.tag == "Rocket") {
        //         if (HealthChanged != null)
        //             HealthChanged();
        //     }
        // }

        // public static event HealthUpdate HealthChanged;
        // private float health;

        // public void StartProcess()
        // {
        //     Console.WriteLine("Process Started!");
        //     // some code here..
        //     OnHealthChanged();
        // }

        // protected virtual void OnHealthChanged() //protected virtual method
        // {
        //     //if ProcessCompleted is not null then call delegate
        //     HealthChanged?.Invoke();
        // }
    }
// }