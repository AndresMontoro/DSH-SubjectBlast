using UnityEngine;
using UnityEngine.UI;
using System.IO;

public class TimerController : MonoBehaviour
{
    public Text timerText; // Referencia al Text donde se mostrará el temporizador
    private float[] tiemposCursos = new float[5]; // Array para almacenar los tiempos de cada curso
    private bool isTimerRunning = false; // Estado del temporizador
    private int cursoActualIndex; // Índice del curso actual
    public Button undo;
    public GameObject TiempoAgotado;
    public Button TiempoAgotadoButton;
    public GameObject MaximoPuntosConseguido;

    void Start()
    {
        // Copiar archivos necesarios a la ruta persistente
        CopiarArchivosARutaPersistente();
        
        // Cargar los tiempos desde el archivo al iniciar
        CargarTiemposDesdeArchivo();
        
        // Establecer el curso actual basado en la opción seleccionada
        EstablecerCursoActual(PlayerPrefs.GetInt("SelectedOption", 0));
        ActualizarTextoTemporizador(cursoActualIndex);
        ComenzarTemporizador();

        // Configurar listeners para botones
        undo.onClick.AddListener(GoBack);
        TiempoAgotadoButton.onClick.AddListener(GoBack);
    }

    void Update()
    {
        // Si el temporizador está corriendo, actualizar el tiempo restante
        if (isTimerRunning)
        {
            // Restar el tiempo transcurrido desde el último frame para el curso actual
            tiemposCursos[cursoActualIndex] -= Time.deltaTime;

            if (MaximoPuntosConseguido.activeSelf)
            {
                DetenerTemporizador();
            }

            // Si el temporizador llega a cero, detenerlo
            if (tiemposCursos[cursoActualIndex] <= 0)
            {
                DetenerTemporizador();
                timerText.text = "00:00";
                TiempoAgotado.SetActive(true);
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
        string filePath = Path.Combine(Application.persistentDataPath, "tiempos.txt");
        using (StreamWriter writer = new StreamWriter(filePath, false)) // Sobrescribe el archivo
        {
            for (int i = 0; i < tiemposCursos.Length; i++)
            {
                string key = "Curso" + (i + 1);
                if (tiemposCursos[i] < 0) tiemposCursos[i] = 0;
                writer.WriteLine(key + ": " + tiemposCursos[i]);
            }
        }
    }

    // Función para cargar los tiempos desde un archivo de texto
    public void CargarTiemposDesdeArchivo()
    {
        string filePath = Path.Combine(Application.persistentDataPath, "tiempos.txt");
        if (File.Exists(filePath))
        {
            string[] lines = File.ReadAllLines(filePath);
            foreach (string line in lines)
            {
                // Dividir la línea en dos partes usando el primer ':' encontrado
                int separatorIndex = line.IndexOf(':');
                if (separatorIndex != -1)
                {
                    string curso = line.Substring(0, separatorIndex).Trim();
                    if (float.TryParse(line.Substring(separatorIndex + 1).Trim(), out float tiempo))
                    {
                        for (int i = 0; i < tiemposCursos.Length; i++)
                        {
                            if (curso == "Curso" + (i + 1))
                            {
                                tiemposCursos[i] = tiempo;
                                Debug.Log("Tiempo del curso " + (i+1) + " : " + tiemposCursos[i]);
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
        int minutos = Mathf.FloorToInt(tiemposCursos[curso] / 60);
        int segundos = Mathf.FloorToInt(tiemposCursos[curso] % 60);
        if (minutos < 0) minutos = 0;
        if (segundos < 0) segundos = 0;
        timerText.text = minutos.ToString("00") + ":" + segundos.ToString("00");
    }

    // Llamado cuando el objeto está a punto de ser destruido
    void OnDestroy()
    {
        GuardarTiemposEnArchivo();
    }

    void GoBack()
    {
        GuardarTiemposEnArchivo();
        UnityEngine.SceneManagement.SceneManager.LoadScene("SeleccionarNiveles" + PlayerPrefs.GetInt("SelectedOption", 1));
    }

    void CopiarArchivosARutaPersistente()
    {
        string[] archivos = { "tiempos.txt" };
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