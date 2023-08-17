using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameStartMenu : MonoBehaviour
{
    [Header("UI Pages")]
    public GameObject mainMenu;
    public GameObject options;
    public GameObject clientMenu;
    public GameObject about;

    [Header("Main Menu Buttons")]
    public Button hostButton;
    public Button clientButton;
    public Button aboutButton;
    public Button quitButton;

    public List<Button> returnButtons;

    // Start is called before the first frame update
    void Start()
    {
        EnableMainMenu();

        //Hook events
        hostButton.onClick.AddListener(StartHost);
        clientButton.onClick.AddListener(StartClient);
        aboutButton.onClick.AddListener(EnableAbout);
        quitButton.onClick.AddListener(QuitGame);

        foreach (var item in returnButtons)
        {
            item.onClick.AddListener(EnableMainMenu);
        }
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void StartHost()
    {
        HideAll();
        SceneTransitionManager.singleton.InitiatlizeAsHost = true;
        SceneTransitionManager.singleton.GoToSceneAsync(1);
    }

    public void StartClient()
    {
        HideAll();
        EnableClientMenu();
    }

    public void HideAll()
    {
        mainMenu.SetActive(false);
        options.SetActive(false);
        about.SetActive(false);
        clientMenu.SetActive(false);
    }

    public void EnableMainMenu()
    {
        mainMenu.SetActive(true);
        options.SetActive(false);
        about.SetActive(false);
        clientMenu.SetActive(false);
    }
    public void EnableClientMenu()
    {
        mainMenu.SetActive(false);
        options.SetActive(false);
        about.SetActive(false);
        clientMenu.SetActive(true);
    }
    public void EnableAbout()
    {
        mainMenu.SetActive(false);
        options.SetActive(false);
        about.SetActive(true);
        clientMenu.SetActive(false);
    }
}
