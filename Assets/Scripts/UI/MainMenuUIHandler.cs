using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class MainMenuUIHandler : MonoBehaviour
{
    [Header("Panels")]
    public GameObject playerDetailsPanel;
    public GameObject sessionBrowserPanel;
    public GameObject createSessionPanel;
    public GameObject statusPanel;

    // [Header("Player settings")]
    // public TMP_InputField playerNameInputField;

    [Header("Room Name")]
    public TMP_InputField roomNameInputField;

    [Header("Region")]
    public TMP_InputField regionNameInputField;

    // Start is called before the first frame update
    void Start()
    {
        if (PlayerPrefs.HasKey("RegionName"))
            regionNameInputField.text = PlayerPrefs.GetString("RegionName");
        if (PlayerPrefs.HasKey("SessionName"))
            roomNameInputField.text = PlayerPrefs.GetString("SessionName");

    }

    void HideAllPanels()
    {
        playerDetailsPanel.SetActive(false);
        sessionBrowserPanel.SetActive(false);
        statusPanel.SetActive(false);
        createSessionPanel.SetActive(false);
    }

    public void OnStartGameClicked()
    {
        // PlayerPrefs.SetString("PlayerNickname", playerNameInputField.text);
        PlayerPrefs.SetString("SessionName", roomNameInputField.text);
        PlayerPrefs.SetString("RegionName", regionNameInputField.text);
        PlayerPrefs.Save();
        SceneManager.LoadScene("Scenes/Hackathon",LoadSceneMode.Single);
        // GameManager.instance.playerNickName = playerNameInputField.text;

        // NetworkRunnerHandler networkRunnerHandler = FindObjectOfType<NetworkRunnerHandler>();

        // networkRunnerHandler.CreateGame(roomNameInputField.text,regionNameInputField.text);


    }

    // public void OnCreateNewGameClicked()
    // {
    //     HideAllPanels();

    //     createSessionPanel.SetActive(true);
    // }

    // s

    // public void OnJoiningServer()
    // {
    //     HideAllPanels();

    //     statusPanel.gameObject.SetActive(true);
    // }
}
