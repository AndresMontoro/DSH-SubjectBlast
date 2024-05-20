using System.IO;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SeleccionarNiveles : MonoBehaviour
{
    public Button undo;
    public Text TitleText;
    public GameObject AvisoCursoNoSuperado;
    public Button OkyVolverAlMenu;
    public GameObject UltimoCursoTerminado;
    public Button UltimoCursoTerminadoButton;
    public CourseResultsController courseResultsController;

    void Start()
    {
        // Recuperar la opción seleccionada desde PlayerPrefs
        int selectedOption = PlayerPrefs.GetInt("SelectedOption", 0);

        // Actualizar el título de la escena
        updateCourseTitle(selectedOption);

        AvisoCursoNoSuperado.SetActive(false);
        undo.onClick.AddListener(GoBack);
        if (selectedOption == 5) UltimoCursoTerminadoButton.onClick.AddListener(GoBack);
        OkyVolverAlMenu.onClick.AddListener(CallResultsControllerFunction);

        // Verificaciones y lógica de inicialización
        if ((selectedOption == 5
            && courseResultsController.SumarResultadosCurso(selectedOption) >= 3500
            && ObtenerTiempoDeCurso(selectedOption) <= 0)
            || selectedOption == 5 && courseResultsController.SumarResultadosCurso(selectedOption) >= 7000)
        {
            UltimoCursoTerminado.SetActive(true);
        }

        if (courseResultsController.SumarResultadosCurso(selectedOption) >= 3500 
            && ObtenerTiempoDeCurso(selectedOption) <= 0 
            && !ComprobarMinimoNiveles())
        {
            AvisoCursoNoSuperado.SetActive(true);
            Debug.Log("Minimo de puntos en cada nivel no alcanzado");
        }

        // Copiar archivos necesarios a la ruta persistente
        CopiarArchivosARutaPersistente();
        CargarResultadosDesdeArchivo();
    }

    void Update()
    {
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);

            // Verificar si el toque es sobre un elemento UI
            if (EventSystem.current.IsPointerOverGameObject(touch.fingerId))
            {
                return; // Si el toque es sobre un elemento UI, no hacemos nada más
            }

            if (touch.phase == TouchPhase.Began)
            {
                RaycastHit hit;
                if (Physics.Raycast(Camera.main.ScreenPointToRay(touch.position), out hit))
                {
                    if (hit.transform == transform)
                    {
                        int selectedOption = PlayerPrefs.GetInt("SelectedOption", 0);
                        float tiempoCurso = ObtenerTiempoDeCurso(selectedOption);

                        if (tiempoCurso > 0)
                        {
                            LoadScene();
                        }
                        else
                        {
                            AvisoCursoNoSuperado.SetActive(true);
                            Debug.Log("No se puede cargar el nivel porque se ha terminado el tiempo para completar el curso.");
                        }
                    }
                }
            }
        }
    }

    void CallResultsControllerFunction()
    {
        if (courseResultsController != null)
        {
            courseResultsController.MatriculaNueva(PlayerPrefs.GetInt("SelectedOption", 0));
            GoBack();
        }
        else
        {
            Debug.LogError("No se ha asignado una referencia a ResultsController en el inspector.");
        }
    }

    public void LoadNextSceneWithOption(int option)
    {
        // Guardar la opción seleccionada en PlayerPrefs
        PlayerPrefs.SetInt("SelectedOption", option);
        
        // Cargar la siguiente escena
        SceneManager.LoadScene("SeleccionarNiveles");
    }

    public int getWeek(string week)
    {
        int numero = 0;
        Match match = Regex.Match(week, @"\d+");
        if (match.Success)
        {
            numero = int.Parse(match.Value);
        }
        return numero;
    }

    void LoadScene()
    {
        int semana = getWeek(gameObject.name);
        PlayerPrefs.SetInt("SelectedWeek", semana);
        SceneManager.LoadScene("VisorNiveles");
    }

    public void updateCourseTitle(int selectedOption)
    {
        switch (selectedOption)
        {
            case 1:
                TitleText.text = "1ᵉʳ curso";
                break;
            case 2:
                TitleText.text = "2º curso";
                break;
            case 3:
                TitleText.text = "3ᵉʳ curso";
                break;
            case 4:
                TitleText.text = "4º curso";
                break;
            case 5:
                TitleText.text = "TFG";
                break;
            default:
                TitleText.text = "Curso";
                break;
        }
    }

    public float ObtenerTiempoDeCurso(int numeroDeCurso)
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
                    if (float.TryParse(parts[1].Trim(), out float tiempo))
                    {
                        if (curso == "Curso" + numeroDeCurso)
                        {
                            Debug.Log("Al " + curso + " le queda " + tiempo + "s");
                            return tiempo;
                        }
                    }
                }
            }
        }
        else
        {
            Debug.LogError("El archivo no existe: " + filePath);
        }

        Debug.LogError("No se encontró el tiempo para el Curso" + numeroDeCurso);
        return -1f; // o algún valor por defecto o de error
    }

    bool ComprobarMinimoNiveles()
    {
        for (int i = 1; i <= 7; i++)
        {
            string key = "Curso" + PlayerPrefs.GetInt("SelectedOption", 0) + "Nivel" + i;
            int resultadoNivel = PlayerPrefs.GetInt(key, 0);
            if (resultadoNivel < 500) return false;
        }
        return true;
    }

    public void CargarResultadosDesdeArchivo()
    {
        string filePath = Path.Combine(Application.persistentDataPath, "resultados.txt");
        if (File.Exists(filePath))
        {
            string[] lines = File.ReadAllLines(filePath);
            foreach (string line in lines)
            {
                string[] parts = line.Split(':');
                string key = parts[0].Trim();
                if (int.TryParse(parts[1].Trim(), out int value))
                {
                    PlayerPrefs.SetInt(key, value);
                }
            }
            PlayerPrefs.Save();
        }
        else
        {
            Debug.LogError("El archivo de resultados no existe.");
        }
    }

    public void GoBack()
    {
        SceneManager.LoadScene("Menu principal");
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