using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SeleccionarNiveles : MonoBehaviour
{
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
        // Recuperar la opción seleccionada desde PlayerPrefs
        int selectedOption = PlayerPrefs.GetInt("SelectedOption", 0);
        
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

    void LoadScene()
    {
        Debug.Log("Cargando nivel: " + gameObject.name);
    }
}