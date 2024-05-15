using System.IO;
using UnityEngine;

public class ResultsController : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        // Recuperar la opción seleccionada desde PlayerPrefs
        int selectedWeek = PlayerPrefs.GetInt("SelectedWeek", 0);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void GuardarResultadoNivel(string curso, int nivel, int resultado)
    {
        PlayerPrefs.SetInt("Curso" + curso + "Nivel" + nivel, resultado);
        PlayerPrefs.Save();
    }

    public void GuardarResultadosEnArchivo()
    {
        string filePath = Application.persistentDataPath + "/resultados.txt";
        using (StreamWriter writer = new StreamWriter(filePath))
        {
            for (int i = 1; i <= 7; i++)
            {
                int resultadoNivel = PlayerPrefs.GetInt("ResultadoNivel" + i, 0);
                writer.WriteLine("Nivel " + i + ": " + resultadoNivel);
            }
        }
    }
}
