using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ControladorMenusPrincipales : MonoBehaviour
{
    public Button playButton;
    public Button modoHistoria;
    public Button undo;
    public Button modoMultijugador;
    public Button crearPartida;
    public Button unirsePartida;

    public GameObject mainMenu;
    public GameObject selectGameModes;
    public GameObject selectYears;
    public GameObject multiplayer;
    public Button[] optionButtons; // Array de botones para las opciones

    // Start is called before the first frame update
    void Start()
    {
        playButton.onClick.AddListener(PlayGame);
        modoHistoria.onClick.AddListener(ShowLevels);
        undo.onClick.AddListener(GoBack);
        modoMultijugador.onClick.AddListener(ShowMultiplayer);
        mainMenu.SetActive(true);
        selectGameModes.SetActive(false);
        selectYears.SetActive(false);
        undo.gameObject.SetActive(false);
        multiplayer.SetActive(false);

        // Asignar eventos a los botones
        for (int i = 0; i < optionButtons.Length; i++)
        {
            int optionIndex = i; // Guardar el valor de i para su uso en el evento
            optionButtons[i].onClick.AddListener(() => LoadNextSceneWithOption(optionIndex + 1));
        }

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
        undo.gameObject.SetActive(true);
    }

    void ShowLevels()
    {
        Debug.Log("Show Levels");
        selectGameModes.SetActive(false);
        selectYears.SetActive(true);
    }

    void ShowMultiplayer()
    {
        Debug.Log("Show Multiplayer");
        selectGameModes.SetActive(false);
        multiplayer.SetActive(true);
    }

    public void LoadNextSceneWithOption(int option)
    {
        // Guardar la opción seleccionada en PlayerPrefs
        PlayerPrefs.SetInt("SelectedOption", option);
        
        // Cargar la siguiente escena
        SceneManager.LoadScene("SeleccionarNiveles" + PlayerPrefs.GetInt("SelectedOption", 1));
    }

    public void GoBack()
    {
        if (selectGameModes.activeSelf)
        {
            mainMenu.SetActive(true);
            selectGameModes.SetActive(false);
            undo.gameObject.SetActive(false);
        }
        else if (selectYears.activeSelf)
        {
            selectGameModes.SetActive(true);
            selectYears.SetActive(false);
        }
    }
}
