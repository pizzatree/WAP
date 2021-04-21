using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Credits : MonoBehaviour
{
    public GameObject creditsObject;

    public void ToggleCredits() {
        creditsObject.SetActive(!creditsObject.activeInHierarchy);
    }

    public void ExitCredits() {
        creditsObject.SetActive(false);
    }
}
