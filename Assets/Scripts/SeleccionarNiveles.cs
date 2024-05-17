using System.IO;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SeleccionarNiveles : MonoBehaviour
{
    public Button undo;
    public Text TitleText;

    void Update()
    {
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            
            if (touch.phase == TouchPhase.Began)
            {
                RaycastHit hit;
                if (Physics.Raycast(Camera.main.ScreenPointToRay(touch.position), out hit))
                {
                    if (hit.transform == transform)
                    {
                        LoadScene();
                    }
                }
            }
        }
    }

    void Start()
    {
        undo.onClick.AddListener(GoBack);

        // Recuperar la opción seleccionada desde PlayerPrefs
        int selectedOption = PlayerPrefs.GetInt("SelectedOption", 0);

        // Actualizar el título de la escena
        updateCourseTitle(selectedOption);
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
        string filePath = Application.persistentDataPath + "/resultados.txt";
        int resultadoTotal = 0;

        // Verificar si el archivo existe
        if (File.Exists(filePath))
        {
            // Leer todas las líneas del archivo
            string[] lines = File.ReadAllLines(filePath);

            // Recorrer cada línea del archivo
            foreach (string line in lines)
            {
                // Verificar si la línea comienza con el nombre del curso especificado
                if (line.StartsWith("Curso" + curso))
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

    public void GoBack()
    {
        SceneManager.LoadScene("Menu principal");
    }
}