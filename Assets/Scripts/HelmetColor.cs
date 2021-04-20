using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEngine;

namespace Player {
    public class HelmetColor : MonoBehaviour
    {
        private ServerGameManager gameManager;

        void Start()
        {
            gameManager = GameObject.Find("[ Game Manager ]").GetComponent<ServerGameManager>();
        }

        void Update()
        {
            // don't mind this, I'm climbing up the hierarchy to get the associated penguin object
            bool green = transform.parent.parent.parent.parent.parent.parent.parent.parent.parent.parent.GetComponent<PenguinBase>().greenTeam;

            if (green)
                this.GetComponent<Renderer>().material = gameManager.teamColors[0];
            else
                this.GetComponent<Renderer>().material = gameManager.teamColors[1];
        }
    }
}