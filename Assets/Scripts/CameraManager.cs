using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// should make this way more robust honestly
// instead of singleton, have player spawner have on OnSpawn event with a player to track
// and basically completely redo this. it's bad.
public class CameraManager : MonoBehaviour
{
    public static CameraManager Instance;

    private Vector3    menuPos;
    private Quaternion menuRot;

    private Transform mainCamTransform;

    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            return;
        }    
        
        Destroy(gameObject);
    }

    private void Start()
    {
        mainCamTransform = Camera.main.transform;

        menuPos = mainCamTransform.position;
        menuRot = mainCamTransform.rotation;
    }

    public void HandleNewCharacter(Transform newParent)
    {
        mainCamTransform.parent        = newParent;
        mainCamTransform.localPosition = Vector3.zero;
    }

    public void HandleLostCharacter()
    {
        mainCamTransform.parent   = null;
        mainCamTransform.position = menuPos;
        mainCamTransform.rotation = menuRot;
    }
}
