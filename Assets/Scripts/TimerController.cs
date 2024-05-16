using UnityEngine;
using UnityEngine.UI;
using System.IO;

public class TimerController : MonoBehaviour
{
    public Text timerText; // Referencia al Text donde se mostrará el temporizador
    private float[] tiemposCursos = new float[5]; // Array para almacenar los tiempos de cada curso
    private bool isTimerRunning = false; // Estado del temporizador
    private int cursoActualIndex; // Índice del curso actual

    void Start()
    {
        // Cargar los tiempos desde el archivo al iniciar
        CargarTiemposDesdeArchivo();
        EstablecerCursoActual(/*PlayerPrefs.GetInt("SelectedOption", 0);*/1);
        ActualizarTextoTemporizador(cursoActualIndex);
        ComenzarTemporizador();
    }

    void Update()
    {
        // Si el temporizador está corriendo, actualizar el tiempo restante
        if (isTimerRunning)
        {
            // Restar el tiempo transcurrido desde el último frame para el curso actual
            tiemposCursos[cursoActualIndex] -= Time.deltaTime;
            // Si el temporizador llega a cero, detenerlo
            if (tiemposCursos[cursoActualIndex] <= 0)
            {
                DetenerTemporizador();
                timerText.text = "0:00";

            }
            // Actualizar el texto del temporizador
            ActualizarTextoTemporizador(cursoActualIndex);
        }
    }

    // Método para establecer el curso actual
    public void EstablecerCursoActual(int index)
    {
        cursoActualIndex = index - 1;
    }

    // Función para comenzar el temporizador
    public void ComenzarTemporizador()
    {
        isTimerRunning = true;
    }

    // Función para detener el temporizador
    public void DetenerTemporizador()
    {
        isTimerRunning = false;
    }

    // Función para continuar el temporizador
    public void ContinuarTemporizador()
    {
        isTimerRunning = true;
    }

    // Función para guardar los tiempos de todos los cursos en un archivo de texto
    public void GuardarTiemposEnArchivo()
    {
        string filePath = "Assets/Resources/Resultados/tiempos.txt";
        using (StreamWriter writer = new StreamWriter(filePath, false)) // Sobrescribe el archivo
        {
            for (int i = 0; i < tiemposCursos.Length; i++)
            {
                string key = "Curso" + (i + 1);
                if(tiemposCursos[i] < 0) tiemposCursos[i] = 0;
                writer.WriteLine(key + ": " + tiemposCursos[i]);
            }
        }
    }

    // Función para cargar los tiempos desde un archivo de texto
    public void CargarTiemposDesdeArchivo()
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

    // Función para actualizar el texto del temporizador
    private void ActualizarTextoTemporizador(int curso)
    {
        // Se asume que se mostrará el tiempo del primer curso
        int minutos = Mathf.FloorToInt(tiemposCursos[curso ] / 60);
        int segundos = Mathf.FloorToInt(tiemposCursos[curso ] % 60);
        if(minutos < 0) minutos = 0;
        if(segundos < 0) segundos = 0;
        timerText.text = minutos.ToString("0") + ":" + segundos.ToString("00");
        Debug.Log("Se ha actualizado el tiempo a " + timerText.text);
    }

    // Llamado cuando el objeto está a punto de ser destruido
    void OnDestroy()
    {
        GuardarTiemposEnArchivo();
    }
}
