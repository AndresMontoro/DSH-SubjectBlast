using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ResultsControllerContrarreloj : MonoBehaviour
{
    public Text totalPointsText; // Texto para mostrar los puntos
    public Text resultText; // Texto para mostrar el resultado concreto
    public Text timerText; // Texto para mostrar el temporizador
    public float timerDuration = 180f; // Duración del temporizador en segundos (3 minutos)
    private float timer; // Tiempo restante del temporizador
    public Button undo; 
    public int ValorFicha = 6;
    int Points;
    public Contrarreloj contrarreloj;
    string nombreJugador;
    public GameObject resultadoFinal;
    public Button resultadoFinalButton;
    public Text TextoFinal;

    [Header("Reset Tablero")]
    public Grid grid;
    public Button reset;

    void Start()
    {
        // Inicializar el temporizador
        timer = timerDuration;
        undo.onClick.AddListener(GoBack);
        resultadoFinalButton.onClick.AddListener(GoBack);
        reset.onClick.AddListener(ResetBoard);
        nombreJugador = PlayerPrefs.GetString("NombreJugador", "");
        Debug.Log("El nombre del jugador es " + nombreJugador);
        resultText.text = contrarreloj.MejorResultado().ToString("0000");

        // Asignar el evento al botón de retroceso
        // Agrega la función GoBack al evento onClick del botón MaximoPuntosConseguidoButton
        // Agrega la función GuardarYSalir al evento onClick del botón undo
        // Si estos botones existen en tu escena, de lo contrario, omite estas líneas
        /*MaximoPuntosConseguidoButton.onClick.AddListener(GoBack);
        undo.onClick.AddListener(GuardarYSalir);*/
    }

    void Update()
    {
        // Actualizar el temporizador
        timer -= Time.deltaTime;

        // Actualizar el texto del temporizador
        int minutos = Mathf.FloorToInt(timer / 60f);
        int segundos = Mathf.FloorToInt(timer % 60f);
        timerText.text = minutos.ToString("0") + ":" + segundos.ToString("00");;
        Points = int.Parse(totalPointsText.text);

        // Si el temporizador llega a cero, ejecuta una acción (por ejemplo, terminar el juego)
        if (timer <= 0f)
        {
            timer = 0f;
            timerText.text = "0:00";
            Debug.Log("¡Tiempo agotado!");
            resultadoFinal.SetActive(true);
            TextoFinal.text = "El tiempo ha terminado.\n" + nombreJugador + ",\ntu puntuación es\n" + Points + " puntos";
        }
    }

    // Función para retroceder a la escena de selección de niveles
    void GoBack()
    {
        GuardarResultadoNivel(nombreJugador, Points);
        SceneManager.LoadScene("Menu principal");
    }

    // Función para guardar los resultados y salir del juego
    void GuardarYSalir()
    {
        // Aquí deberías agregar tu lógica para guardar los resultados y salir del juego
        // Por ejemplo, guardar los puntos en un archivo o en PlayerPrefs
        // Luego, cargar la escena de selección de niveles o salir del juego
    }

    public void SumarPuntos()
    {
        int Puntos = int.Parse(totalPointsText.text);
        Puntos += ValorFicha;
        totalPointsText.text = Puntos.ToString("0000");
    }

    public void SumarSegundos()
    {
        timer += 0.1f;
    }

    public void GuardarResultadoNivel(string nombre, int resultado)
    {
        contrarreloj.GuardarNuevoTiempo(nombre, resultado);
    }

    void ResetBoard() { grid.ClearBoard(); }
}