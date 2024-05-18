using UnityEngine;
using UnityEngine.UI;
using Unity.Netcode;

public class GameManager : NetworkBehaviour
{
    public Text timerText; // Referencia al Text donde se mostrará el temporizador
    public Text player1ScoreText; // Referencia al Text donde se mostrará el puntaje del jugador 1
    public Text player2ScoreText; // Referencia al Text donde se mostrará el puntaje del jugador 2

    private float tiempo = 180f; // Tiempo inicial en segundos (3 minutos)
    private NetworkVariable<float> networkTiempo = new NetworkVariable<float>(180f); // Variable de red para el tiempo
    private NetworkVariable<int> player1Score = new NetworkVariable<int>(0); // Variable de red para el puntaje del jugador 1
    private NetworkVariable<int> player2Score = new NetworkVariable<int>(0); // Variable de red para el puntaje del jugador 2

    private bool isTimerRunning = false; // Estado del temporizador

    void Start()
    {
        if (IsServer)
        {
            ComenzarTemporizador();
        }

        networkTiempo.OnValueChanged += OnTiempoChanged;
        player1Score.OnValueChanged += OnPlayer1ScoreChanged;
        player2Score.OnValueChanged += OnPlayer2ScoreChanged;

        ActualizarTextoTemporizador();
        ActualizarPuntajes();
    }

    void Update()
    {
        if (IsServer && isTimerRunning)
        {
            tiempo -= Time.deltaTime;
            if (tiempo <= 0)
            {
                tiempo = 0;
                DetenerTemporizador();
            }
            networkTiempo.Value = tiempo;
        }
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

    // Función para actualizar el texto del temporizador
    private void ActualizarTextoTemporizador()
    {
        int minutos = Mathf.FloorToInt(networkTiempo.Value / 60);
        int segundos = Mathf.FloorToInt(networkTiempo.Value % 60);
        timerText.text = minutos.ToString("0") + ":" + segundos.ToString("00");
    }

    private void OnTiempoChanged(float oldValue, float newValue)
    {
        ActualizarTextoTemporizador();
    }

    private void OnPlayer1ScoreChanged(int oldValue, int newValue)
    {
        player1ScoreText.text = newValue.ToString("0000");
    }

    private void OnPlayer2ScoreChanged(int oldValue, int newValue)
    {
        player2ScoreText.text = newValue.ToString("0000");
    }

    public void ActualizarPuntajeJugador1(int puntaje)
    {
        if (IsServer)
        {
            player1Score.Value = puntaje;
        }
    }

    public void ActualizarPuntajeJugador2(int puntaje)
    {
        if (IsServer)
        {
            player2Score.Value = puntaje;
        }
    }

    private void ActualizarPuntajes()
    {
        player1ScoreText.text = player1Score.Value.ToString("0000");
        player2ScoreText.text = player2Score.Value.ToString("0000");
    }
}