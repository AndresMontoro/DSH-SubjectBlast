using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class ResultsController : MonoBehaviour
{
    public Text TotalPointsLevel;
    public Text Week;

    void Start()
    {
        int selectedWeek = PlayerPrefs.GetInt("SelectedWeek", 0);
        TotalPointsLevel.text = "0000";
        Week.text = "Semana " + selectedWeek;
        //GuardarResultadoNivel(1, 3, 1000);
        //GuardarResultadoNivel(5, 6, 1500);
        //GuardarResultadosEnArchivo();
        //CargarResultadosDesdeArchivo();
        //MostrarDatosGuardados();
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

    // Llamado cuando el objeto está a punto de ser destruido
    void OnDestroy()
    {
        GuardarResultadosEnArchivo();
    }
}
