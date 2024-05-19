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
    public Text TotalPointsCourse;
    public GameObject AvisoCursoNoSuperado;
    public Button OkyVolverAlMenu;
    public CourseResultsController courseResultsController;

    void Update()
    {
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);

            // Verificar si el toque es sobre un elemento UI
            if (EventSystem.current.IsPointerOverGameObject(touch.fingerId))
            {
                Debug.Log("Toque sobre un elemento UI");
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

    void Start()
    {
        // Recuperar la opción seleccionada desde PlayerPrefs
        int selectedOption = PlayerPrefs.GetInt("SelectedOption", 0);

        // Actualizar el título de la escena
        updateCourseTitle(selectedOption);
        TotalPointsCourse.text = CargarResultadoTotalCurso("Curso" + selectedOption).ToString("0000");

        AvisoCursoNoSuperado.SetActive(false);
        undo.onClick.AddListener(GoBack);
        OkyVolverAlMenu.onClick.AddListener(CallResultsControllerFunction);
    }

    void CallResultsControllerFunction()
    {
        // Asegúrate de que resultsController sea una referencia válida
        if (courseResultsController != null)
        {
            // Llama a la función deseada en ResultsController
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

    public int CargarResultadoTotalCurso(string curso)
    {
        Debug.Log("Cargando resultado del Curso" + curso);
        string filePath = "Assets/Resources/Resultados/resultados.txt";
        int resultadoTotal = 0;

        // Verificar si el archivo existe
        if (File.Exists(filePath))
        {
            // Leer todas las líneas del archivo
            string[] lines = File.ReadAllLines(filePath);

            // Recorrer cada línea del archivo
            foreach (string line in lines)
            {
                Debug.Log(line);
                // Verificar si la línea comienza con el nombre del curso especificado
                if (line.StartsWith("" + curso))
                {
                    // Extraer el resultado de la línea y sumarlo al resultado total
                    string[] parts = line.Split(':');
                    int resultadoNivel;
                    if (int.TryParse(parts[1].Trim(), out resultadoNivel))
                    {
                        resultadoTotal += resultadoNivel;
                    }
                }
            }
        }
        else
        {
            Debug.LogError("El archivo de resultados no existe.");
        }
        Debug.Log("Resultado del " + curso + " es " + resultadoTotal);
        return resultadoTotal;
    }

    public int getWeek(string week){

        int numero = 0;
        // Utilizamos una expresión regular para extraer el número
        Match match = Regex.Match(week, @"\d+");

        // Verificamos si se encontró un número en la cadena
        if (match.Success)
        {
            // Convertimos el valor de la coincidencia a un número entero
            numero = int.Parse(match.Value);
        }

        return numero;
    }

    void LoadScene()
    {
        // Guardar la semana seleccionada en PlayerPrefs
        int semana = getWeek(gameObject.name);
        PlayerPrefs.SetInt("SelectedWeek", semana);

        SceneManager.LoadScene("VisorNiveles");
    }

    public void updateCourseTitle(int selectedOption)
    {
        switch(selectedOption) {
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

    // Función para cargar los tiempos desde un archivo y devolver el tiempo de un curso específico
    public float ObtenerTiempoDeCurso(int numeroDeCurso)
    {
        string filePath = "Assets/Resources/Resultados/tiempos.txt";
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

    public void GoBack()
    {
        SceneManager.LoadScene("Menu principal");
    }
}