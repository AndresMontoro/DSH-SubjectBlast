using UnityEngine;
using UnityEngine.UI;
using System.IO;

public class TimerControllerVS : MonoBehaviour
{
    public Text timerText; // Referencia al Text donde se mostrará el temporizador
    private float tiempo = 180f;
    private bool isTimerRunning = false; // Estado del temporizador
    public Button undo;

    void Start()
    {
        ComenzarTemporizador();
        undo.onClick.AddListener(GoBack);
    }

    void Update()
    {
        // Si el temporizador está corriendo, actualizar el tiempo restante
        if (isTimerRunning)
        {
            // Restar el tiempo transcurrido desde el último frame para el curso actual
            tiempo -= Time.deltaTime;
            // Si el temporizador llega a cero, detenerlo
            if (tiempo <= 0)
            {
                DetenerTemporizador();
                timerText.text = "00:00";

            }
            // Actualizar el texto del temporizador
            ActualizarTextoTemporizador();
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

    // Función para continuar el temporizador
    public void ContinuarTemporizador()
    {
        isTimerRunning = true;
    }

    // Función para actualizar el texto del temporizador
    private void ActualizarTextoTemporizador()
    {
        int minutos = Mathf.FloorToInt(tiempo / 60);
        int segundos = Mathf.FloorToInt(tiempo % 60);
        timerText.text = minutos.ToString("00") + ":" + segundos.ToString("00");
        Debug.Log("Se ha actualizado el tiempo a " + timerText.text);
    }

    void GoBack()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("Menu principal");
    }
}
