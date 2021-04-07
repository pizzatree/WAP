using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayButton : MonoBehaviour
{
public Button playButton;

    // Start is called before the first frame update
    void Start()
    {
        Button btn = playButton.GetComponent<Button>();
        btn.onClick.AddListener(ChangeScene);
    }

    // ChangeScene is called after Play Button is clicked
    void ChangeScene()
    {
        SceneManager.LoadScene("NetworkTest");
    }
}
