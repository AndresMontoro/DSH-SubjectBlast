using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ControladorMenusPrincipales : MonoBehaviour
{
    public Button playButton;
    public Button modoHistoria;
    public GameObject mainMenu;
    public GameObject selectGameModes;
    // Start is called before the first frame update
    void Start()
    {
        playButton.onClick.AddListener(PlayGame);
        modoHistoria.onClick.AddListener(ShowLevels);
        mainMenu.SetActive(true);
        selectGameModes.SetActive(false);
        Debug.Log("Start");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void PlayGame()
    {
        Debug.Log("Play Game");
        mainMenu.SetActive(false);
        selectGameModes.SetActive(true);
    }

    void ShowLevels()
    {
        Debug.Log("Show Levels");
        selectGameModes.SetActive(false);
    }
}
