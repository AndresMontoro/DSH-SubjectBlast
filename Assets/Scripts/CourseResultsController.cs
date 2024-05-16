using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class CourseResultsController : MonoBehaviour
{
    private List<string> cursoKeys = new List<string>();
    public Text TotalPointsCourse;

    // Start is called before the first frame update
    void Start()
    {
        CargarResultadosDesdeArchivo();
        Debug.Log("" + SumarResultadosCurso(1));
        MostrarDatosGuardados();
        TotalPointsCourse.text = SumarResultadosCurso(2).ToString("0000");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // Función para cargar los resultados desde un archivo de texto
    public void CargarResultadosDesdeArchivo()
    {
        string filePath = "Assets/Resources/Resultados/resultados.txt";
        
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
}
