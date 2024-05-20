using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ResultsController : MonoBehaviour
{
    public Text TotalPointsLevel;
    public Text Week;
    public GameObject MaximoPuntosConseguido;
    public Button MaximoPuntosConseguidoButton;
    public GameObject TiempoAgotado;
    public Button undo;
    public int ValorFicha = 8;
    int Points;

    void Start()
    {
        int selectedWeek = PlayerPrefs.GetInt("SelectedWeek", 0);
        TotalPointsLevel.text = "0000";
        Week.text = "Semana " + selectedWeek;
        MaximoPuntosConseguidoButton.onClick.AddListener(GoBack);
        undo.onClick.AddListener(GuardarYSalir);
        //GuardarResultadoNivel(1, 1, 950);
        //GuardarResultadoNivel(5, 6, 1500);
        //GuardarResultadosEnArchivo();
        //CargarResultadosDesdeArchivo();
        //MostrarDatosGuardados();
    }

    void Update()
    {
        Points = int.Parse(TotalPointsLevel.text);
        if(Points >= 1000)
        {
            TotalPointsLevel.text = "1000";
            GuardarResultadoNivel(PlayerPrefs.GetInt("SelectedOption", 0), PlayerPrefs.GetInt("SelectedWeek", 0), 1000);
            MaximoPuntosConseguido.SetActive(true);
        }
        if(TiempoAgotado.activeSelf)
        {
            GuardarResultadoNivel(PlayerPrefs.GetInt("SelectedOption", 0), PlayerPrefs.GetInt("SelectedWeek", 0), Points);
        }
    }

    public void GuardarResultadoNivel(int curso, int nivel, int resultado)
    {
        string key = "Curso" + curso + "Nivel" + nivel;
        int existingResult = PlayerPrefs.GetInt(key, int.MaxValue); // Obtener el resultado existente o un valor máximo si no existe
        if (resultado > existingResult)
        {
            PlayerPrefs.SetInt(key, resultado);
        }
        PlayerPrefs.Save();
    }

    public void GuardarResultadosEnArchivo()
    {
        string filePath = "Assets/Resources/Resultados/resultados.txt";
        using (StreamWriter writer = new StreamWriter(filePath))
        {
            for (int curso = 1; curso <= 5; curso++)
            {
                for (int nivel = 1; nivel <= 7; nivel++)
                {
                    string key = "Curso" + curso + "Nivel" + nivel;
                    int resultado = PlayerPrefs.GetInt(key, 0);
                    writer.WriteLine(key + ": " + resultado);
                }
            }
        }
    }

    public void CargarResultadosDesdeArchivo()
    {
        string filePath = "Assets/Resources/Resultados/resultados.txt";
        
        // Verificar si el archivo existe
        if (File.Exists(filePath))
        {
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

    public void MostrarDatosGuardados()
    {
        Debug.Log("Datos guardados en PlayerPrefs:");
        for (int curso = 1; curso <= 5; curso++)
        {
            for (int nivel = 1; nivel <= 7; nivel++)
            {
                string key = "Curso" + curso + "Nivel" + nivel;
                int resultado = PlayerPrefs.GetInt(key, 0);
                Debug.Log(key + ": " + resultado);
            }
        }
    }

    public void SumarPuntos()
    {
        int Puntos = int.Parse(TotalPointsLevel.text);
        Puntos += ValorFicha;
        TotalPointsLevel.text = Puntos.ToString("0000");
        GuardarResultadoNivel(PlayerPrefs.GetInt("SelectedOption", 0), PlayerPrefs.GetInt("SelectedWeek"), Puntos);
    }

    void GuardarYSalir()
    {
        GuardarResultadosEnArchivo();
        SceneManager.LoadScene("SeleccionarNiveles" + PlayerPrefs.GetInt("SelectedOption", 0));
    }

    // Llamado cuando el objeto está a punto de ser destruido
    void OnDestroy()
    {
        GuardarResultadosEnArchivo();
    }

    public void GoBack()
    {
        SceneManager.LoadScene("SeleccionarNiveles" + PlayerPrefs.GetInt("SelectedOption", 0));
    }
}
