using System;
using System.Collections;
using System.Collections.Generic;
using Mirror;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuNetworkButtons : MonoBehaviour
{
    [SerializeField] private TMP_Text       statusText;
    [SerializeField] private TMP_InputField serverAddress;

    [SerializeField] private GameObject[] uiInteractables;

    private NetworkManager networkManager;
    
    private void Start()
    {
        SceneManager.LoadScene(1, LoadSceneMode.Additive);
        networkManager  = FindObjectOfType<NetworkManager>();
        statusText.text = "";
    }

    public void HandleConnect()
    {
        SetInteractiveElementsTo(false);
        if(serverAddress.text.Equals(""))
            serverAddress.text = "localhost";
        
        networkManager.networkAddress = serverAddress.text;
        networkManager.StartClient();
    }

    public void HandleListenServer()
    {
        SetInteractiveElementsTo(false);
        networkManager.StartHost();
    }

    public void HandleHost()
    {
        SetInteractiveElementsTo(false);
        networkManager.StartServer();
    }

    private void SetInteractiveElementsTo(bool active)
    {
        foreach(var interactable in uiInteractables)
            interactable.SetActive(active);
    }

    private void Update()
    {
        if(!networkManager)
            networkManager = FindObjectOfType<NetworkManager>();

        if(!NetworkClient.isConnected && NetworkClient.active)
            HandleConnecting();
        else
            statusText.text = ""; // string changes in update are bad, remove sometime

        HandleClientReady();
    }

    private static void HandleClientReady()
    {
        if(NetworkClient.isConnected && !ClientScene.ready)
        {
            ClientScene.Ready(NetworkClient.connection);

            if(ClientScene.localPlayer == null)
            {
                ClientScene.AddPlayer();
                SceneManager.UnloadScene(0);
            }
        }
    }

    private void HandleConnecting()
    {
        statusText.text = $"Connecting to {networkManager.networkAddress}.. \n" +
                          $"Press escape to exit";

        if(Input.GetKeyDown(KeyCode.Escape))
        {
            networkManager.StopClient();
            SetInteractiveElementsTo(true);
        }
    }
}