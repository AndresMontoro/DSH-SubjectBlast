using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ControladorMenusPrincipales : MonoBehaviour
{
    public Button playButton;
    public Button modoHistoria;
    public GameObject mainMenu;
    public GameObject selectGameModes;
    public GameObject selectYears;
    public GameObject undo;
    public Button[] optionButtons; // Array de botones para las opciones

    // Start is called before the first frame update
    void Start()
    {
        playButton.onClick.AddListener(PlayGame);
        modoHistoria.onClick.AddListener(ShowLevels);
        mainMenu.SetActive(true);
        selectGameModes.SetActive(false);
        selectYears.SetActive(false);
        undo.SetActive(false);

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
        undo.SetActive(true);
    }

    void ShowLevels()
    {
        Debug.Log("Show Levels");
        selectGameModes.SetActive(false);
        selectYears.SetActive(true);
    }

    public void LoadNextSceneWithOption(int option)
    {
        // Guardar la opción seleccionada en PlayerPrefs
        PlayerPrefs.SetInt("SelectedOption", option);
        
        // Cargar la siguiente escena
        SceneManager.LoadScene("SeleccionarNiveles");
    }
}
