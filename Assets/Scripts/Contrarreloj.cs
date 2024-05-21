using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

[Serializable]
public class Resultado
{
    public string nombre;
    public int puntuacion;

    public Resultado(string nombre, int puntuacion)
    {
        this.nombre = nombre;
        this.puntuacion = puntuacion;
    }
}

public class Contrarreloj : MonoBehaviour
{
    private const int MaxTiempos = 10;
    private List<Resultado> mejoresTiempos = new List<Resultado>();

    public List<Text> textosNombres; // Lista de Texts para mostrar los nombres en la escena
    public List<Text> textosPuntuaciones; // Lista de Texts para mostrar las puntuaciones en la escena

    public Button jugarButton;
    public Text nombreJugador;

    void Start()
    {
        InicializarArchivoDeResultados();
        CargarResultadosDesdeArchivo();
        MostrarResultados();
        jugarButton.onClick.AddListener(LoadScene);
    }

    void Update()
    {
        // Aquí puedes actualizar la lógica de tu juego contrarreloj
    }

    // Función para inicializar el archivo de resultados con datos predeterminados
    private void InicializarArchivoDeResultados()
    {
        string filePath = Path.Combine(Application.persistentDataPath, "tiemposContrarreloj.txt");
        if (!File.Exists(filePath))
        {
            using (StreamWriter writer = new StreamWriter(filePath))
            {
                // Datos predeterminados
                List<Resultado> datosIniciales = new List<Resultado>
                {
                    new Resultado("pacodisc", 9999),
                    new Resultado("CHARLY", 1500),
                    new Resultado("PEDROSANC", 745),
                    new Resultado("ANDRES", 666),
                    new Resultado("JOSEMOT", 425),
                    new Resultado("JOHN", 422),
                    new Resultado("GUERRERO", 421),
                    new Resultado("SALITAS", 420),
                    new Resultado("DSH", 356),
                    new Resultado("JOSEALONSO", 342)
                };

                foreach (Resultado resultado in datosIniciales)
                {
                    writer.WriteLine($"{resultado.nombre}: {resultado.puntuacion}");
                }
            }
        }
    }

    // Función para cargar los resultados desde el archivo
    public void CargarResultadosDesdeArchivo()
    {
        string filePath = Path.Combine(Application.persistentDataPath, "tiemposContrarreloj.txt");
        
        if (File.Exists(filePath))
        {
            string[] lines = File.ReadAllLines(filePath);
            mejoresTiempos.Clear();

            foreach (string line in lines)
            {
                string[] parts = line.Split(':');
                if (parts.Length == 2)
                {
                    string nombre = parts[0].Trim();
                    int puntuacion;
                    if (int.TryParse(parts[1].Trim(), out puntuacion))
                    {
                        mejoresTiempos.Add(new Resultado(nombre, puntuacion));
                    }
                }
            }

            mejoresTiempos.Sort((a, b) => b.puntuacion.CompareTo(a.puntuacion)); // Ordenar de mayor a menor
        }
        else
        {
            Debug.LogError("El archivo de resultados no existe.");
        }
    }

    // Función para guardar los resultados en el archivo
    public void GuardarResultadosEnArchivo()
    {
        string filePath = Path.Combine(Application.persistentDataPath, "tiemposContrarreloj.txt");
        using (StreamWriter writer = new StreamWriter(filePath))
        {
            foreach (Resultado resultado in mejoresTiempos)
            {
                writer.WriteLine($"{resultado.nombre}: {resultado.puntuacion}");
            }
        }
    }

    // Función para añadir un nuevo tiempo y reordenar la lista
    public void GuardarNuevoTiempo(string nombre, int nuevoTiempo)
    {
        mejoresTiempos.Add(new Resultado(nombre, nuevoTiempo));
        mejoresTiempos.Sort((a, b) => b.puntuacion.CompareTo(a.puntuacion)); // Ordenar de mayor a menor

        // Limitar la lista a los 10 mejores tiempos
        if (mejoresTiempos.Count > MaxTiempos)
        {
            mejoresTiempos.RemoveAt(mejoresTiempos.Count - 1);
        }

        GuardarResultadosEnArchivo();
        MostrarResultados();
    }

    // Función para mostrar los resultados en la UI
    public void MostrarResultados()
    {
        for (int i = 0; i < MaxTiempos; i++)
        {
            if (i < mejoresTiempos.Count)
            {
                if (i < textosNombres.Count && i < textosPuntuaciones.Count)
                {
                    textosNombres[i].text = mejoresTiempos[i].nombre;
                    textosPuntuaciones[i].text = mejoresTiempos[i].puntuacion.ToString();
                }
            }
            else
            {
                if (i < textosNombres.Count && i < textosPuntuaciones.Count)
                {
                    textosNombres[i].text = "";
                    textosPuntuaciones[i].text = "";
                }
            }
        }
    }

    public int MejorResultado()
    {
        return mejoresTiempos[1].puntuacion;
    }

    // Función para cargar la escena de contrarreloj

    void LoadScene()
    {
        PlayerPrefs.SetString("NombreJugador", nombreJugador.text);
        SceneManager.LoadScene("Contrarreloj");
    }
}