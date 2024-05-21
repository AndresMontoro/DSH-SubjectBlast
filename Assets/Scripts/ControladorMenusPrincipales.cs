using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.IO;
using System.Collections.Generic;
using System;

public class ControladorMenusPrincipales : MonoBehaviour
{
    public Button playButton;
    public Button modoHistoria;
    public Button undo;
    public Button modoMultijugador;
    public Button crearPartida;
    public Button unirsePartida;
    public Button jugarMejoresTiempos;
    public Button modoContrarreloj;

    public GameObject mainMenu;
    public GameObject selectGameModes;
    public GameObject selectYears;
    public GameObject multiplayer;
    public GameObject InfoHistoria;
    public Button InfoHistoriaButton;
    public GameObject InfoHistoriaTexto;
    public Button InfoHistoriaTextoButton;
    public GameObject ModoHistoriaTerminado;
    public GameObject MejoresTiempos;
    public GameObject InputNameContrarreloj;
    public Button ModoHistoriaTerminadoOkButton;
    public Button ModoHistoriaTerminadoRejugarButton;
    public Button[] optionButtons; // Array de botones para las opciones
    private float[] resultadosCursos = new float[5]; // Array para almacenar los tiempos de cada curso
    private List<string> cursoKeys = new List<string>();
    private float[] tiemposCursos = new float[5]; // Array para almacenar los tiempos de cada curso

    // Start is called before the first frame update
    void Start()
    {
        playButton.onClick.AddListener(PlayGame);
        modoHistoria.onClick.AddListener(ShowLevels);
        undo.onClick.AddListener(GoBack);
        modoMultijugador.onClick.AddListener(ShowMultiplayer);
        InfoHistoriaButton.onClick.AddListener(ActivarDesactivarInfoHistoria);
        InfoHistoriaTextoButton.onClick.AddListener(ActivarDesactivarInfoHistoria);
        ModoHistoriaTerminadoOkButton.onClick.AddListener(GoBAckAndDisableInfo);
        ModoHistoriaTerminadoRejugarButton.onClick.AddListener(RejugarModoHistoria);
        modoContrarreloj.onClick.AddListener(ShowMejoresTiempos);
        jugarMejoresTiempos.onClick.AddListener(ShowInputNameContrarreloj);

        mainMenu.SetActive(true);
        selectGameModes.SetActive(false);
        selectYears.SetActive(false);
        undo.gameObject.SetActive(false);
        multiplayer.SetActive(false);
        MejoresTiempos.SetActive(false);
        InputNameContrarreloj.SetActive(false);

        // Desactivar los botones de opciones
        foreach (var button in optionButtons)
        {
            button.interactable = false;
        }
        optionButtons[0].interactable = true;

        CopiarArchivosARutaPersistente();
        CargarResultadosDesdeArchivo();
        CargarTiemposDesdeArchivo();

        for (int i = 1; i <= 5; i++)
        {
            resultadosCursos[i - 1] = SumarResultadosCurso(i);
            if (resultadosCursos[i - 1] >= 3500
                && ComprobarMinimoNivel(i)
                && i != 5) optionButtons[i].interactable = true;
        }

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
        if (resultadosCursos[4] >= 3500 && selectYears.activeSelf)
        {
            foreach (var button in optionButtons)
            {
                button.interactable = false;
            }
            ModoHistoriaTerminado.SetActive(true);
        }
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
        InfoHistoria.SetActive(true);
    }

    void ShowMultiplayer()
    {
        Debug.Log("Show Multiplayer");
        SceneManager.LoadScene("Multijugador");
        /*selectGameModes.SetActive(false);
        multiplayer.SetActive(true);*/
    }

    public void ShowMejoresTiempos()
    {
        Debug.Log("Show Mejores Tiempos");
        selectGameModes.SetActive(false);
        MejoresTiempos.SetActive(true);
    }

    public void ShowInputNameContrarreloj()
    {
        MejoresTiempos.SetActive(false);
        InputNameContrarreloj.SetActive(true);
    }

    void ActivarDesactivarInfoHistoria()
    {
        if (InfoHistoriaTexto.activeSelf) InfoHistoriaTexto.SetActive(false);
        else InfoHistoriaTexto.SetActive(true);
    }

    public void LoadNextSceneWithOption(int option)
    {
        // Guardar la opción seleccionada en PlayerPrefs
        PlayerPrefs.SetInt("SelectedOption", option);

        // Cargar la siguiente escena
        SceneManager.LoadScene("SeleccionarNiveles" + PlayerPrefs.GetInt("SelectedOption", 1));
    }

    // Función para cargar los resultados desde un archivo de texto
    public void CargarResultadosDesdeArchivo()
    {
        string filePath = Path.Combine(Application.persistentDataPath, "resultados.txt");

        // Verificar si el archivo existe
        if (File.Exists(filePath))
        {
            // Limpiar la lista de claves
            cursoKeys.Clear();

            // Leer todas las líneas del archivo
            string[] lines = File.ReadAllLines(filePath);

            // Recorrer cada línea del archivo
            foreach (string line in lines)
            {
                // Dividir la línea en la clave y el valor
                string[] parts = line.Split(':');
                string key = parts[0].Trim();
                int value;
                if (int.TryParse(parts[1].Trim(), out value))
                {
                    // Agregar la clave a la lista de claves
                    cursoKeys.Add(key);

                    // Guardar el valor en PlayerPrefs
                    PlayerPrefs.SetInt(key, value);
                }
            }

            // Guardar los cambios en PlayerPrefs
            PlayerPrefs.Save();
        }
        else
        {
            Debug.LogError("El archivo de resultados no existe.");
        }
    }

    // Función para sumar los resultados de un curso específico
    public int SumarResultadosCurso(int curso)
    {
        int resultadoTotal = 0;

        // Iterar sobre las claves asociadas a los resultados
        foreach (var key in cursoKeys)
        {
            // Verificar si la clave corresponde al curso específico
            if (key.StartsWith("Curso" + curso + "Nivel"))
            {
                // Sumar el valor asociado a la clave al resultado total
                resultadoTotal += PlayerPrefs.GetInt(key);
            }
        }

        return resultadoTotal;
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
            InfoHistoria.SetActive(false);
        }
        else if (MejoresTiempos.activeSelf)
        {
            selectGameModes.SetActive(true);
            MejoresTiempos.SetActive(false);
        }
        else if (InputNameContrarreloj.activeSelf)
        {
            MejoresTiempos.SetActive(true);
            InputNameContrarreloj.SetActive(false);
        }
    }

    public void GoBAckAndDisableInfo()
    {
        ModoHistoriaTerminado.SetActive(false);
        GoBack();
    }

    void RejugarModoHistoria()
    {
        MatriculaNueva();
        ModoHistoriaTerminado.SetActive(false);
        SceneManager.LoadScene("Menu principal");
    }

    public void MatriculaNueva()
    {
        string filePath = Path.Combine(Application.persistentDataPath, "resultados.txt");
        using (StreamWriter writer = new StreamWriter(filePath))
        {
            for (int curso = 1; curso <= 5; curso++)
            {
                for (int nivel = 1; nivel <= 7; nivel++)
                {
                    string key = "Curso" + curso + "Nivel" + nivel;
                    writer.WriteLine(key + ": " + 0);
                }
            }
        }
        GuardarTiemposEnArchivoMatriculaNueva();
    }

    // Función para guardar los tiempos de todos los cursos en un archivo de texto
    public void GuardarTiemposEnArchivoMatriculaNueva()
    {
        string filePath = Path.Combine(Application.persistentDataPath, "tiempos.txt");
        using (StreamWriter writer = new StreamWriter(filePath, false)) // Sobrescribe el archivo
        {
            for (int i = 0; i < tiemposCursos.Length; i++)
            {
                string key = "Curso" + (i + 1);
                writer.WriteLine(key + ": " + 1200);
            }
        }
    }

    // Función para cargar los tiempos desde un archivo de texto
    public void CargarTiemposDesdeArchivo()
    {
        string filePath = Path.Combine(Application.persistentDataPath, "tiempos.txt");
        if (File.Exists(filePath))
        {
            string[] lines = File.ReadAllLines(filePath);
            foreach (string line in lines)
            {
                string[] parts = line.Split(':');
                if (parts.Length == 2)
                {
                    string curso = parts[0].Trim();
                    float tiempo;
                    if (float.TryParse(parts[1].Trim(), out tiempo))
                    {
                        for (int i = 0; i < tiemposCursos.Length; i++)
                        {
                            if (curso == "Curso" + (i + 1))
                            {
                                tiemposCursos[i] = tiempo;
                            }
                        }
                    }
                }
            }
        }
        Debug.Log("Se han cargado los tiempos");
    }

    public void LoadContrarrelojScene()
    {
        Debug.Log("Cargando escena contrarreloj...");
    }

    bool ComprobarMinimoNivel(int curso)
    {
        for (int i = 1; i <= 7; i++)
        {
            string key = "Curso" + curso + "Nivel" + i;
            int resultadoNivel = PlayerPrefs.GetInt(key, 0);
            if (resultadoNivel < 500) return false;
        }
        return true;
    }

    void CopiarArchivosARutaPersistente()
    {
        string[] archivos = { "resultados.txt", "tiempos.txt" };
        foreach (string archivo in archivos)
        {
            string rutaDestino = Path.Combine(Application.persistentDataPath, archivo);
            if (!File.Exists(rutaDestino))
            {
                TextAsset archivoOriginal = Resources.Load<TextAsset>("Resultados/" + Path.GetFileNameWithoutExtension(archivo));
                if (archivoOriginal != null)
                {
                    File.WriteAllBytes(rutaDestino, archivoOriginal.bytes);
                }
                else
                {
                    Debug.LogError("No se pudo encontrar el archivo: " + archivo);
                }
            }
        }
    }
}