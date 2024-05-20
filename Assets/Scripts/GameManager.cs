using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviourPunCallbacks
{
    public Text timerText;
    public Text player1ScoreText;
    public Text player2ScoreText;
    public Button undo;

    private float tiempo = 10f;
    private bool isTimerRunning = false;
    private bool isGameStarted = false;
    private int player1Score = 0;
    private int player2Score = 0;
    public GameObject HasGanado;
    public GameObject HasGanadoAbandono;
    public GameObject HaGanadoElRival;
    public GameObject Empate;
    public Button HasGanadoButton;
    public Button HasGanadoAbandonoButton;
    public Button HaGanadoElRivalButton;
    public Button EmpateButton;

    private int playerIndex;

    void Start()
    {
        undo.onClick.AddListener(GoBack);
        HasGanadoButton.onClick.AddListener(GoBack);
        HasGanadoAbandonoButton.onClick.AddListener(GoBack);
        HaGanadoElRivalButton.onClick.AddListener(GoBack);
        EmpateButton.onClick.AddListener(GoBack);

        Debug.Log("GameManager PhotonView: " + photonView.ViewID);
    }

    void Update()
    {
        if (PhotonNetwork.IsConnected)
        {
            if (!isGameStarted)
            {
                CheckStartTimerIfTwoPlayersInRoom();
            }

            if (isTimerRunning)
            {
                tiempo -= Time.deltaTime;
                player1Score = int.Parse(player1ScoreText.text);
                player2Score = int.Parse(player2ScoreText.text);
                if (tiempo <= 0)
                {
                    tiempo = 0;
                    StopTimer();
                    Resultados(playerIndex);
                }
                UpdateTimerText();
            }
        }
    }

    void CheckStartTimerIfTwoPlayersInRoom()
    {
        if (PhotonNetwork.CurrentRoom != null && PhotonNetwork.CurrentRoom.PlayerCount == 2 && !isGameStarted)
        {
            isGameStarted = true;
            StartTimerLocally();
        }
    }

    void StartTimerLocally()
    {
        isTimerRunning = true;
        Debug.Log("Timer started locally on client: " + PhotonNetwork.LocalPlayer.NickName);
    }

    void StopTimer()
    {
        isTimerRunning = false;
    }

    void UpdateTimerText()
    {
        int minutos = Mathf.FloorToInt(tiempo / 60);
        int segundos = Mathf.FloorToInt(tiempo % 60);
        timerText.text = minutos.ToString("0") + ":" + segundos.ToString("00");
    }

    public void OnPlayer1ScoreChanged(int newValue)
    {
        player1ScoreText.text = newValue.ToString("0000");
    }

    public void OnPlayer2ScoreChanged(int newValue)
    {
        player2ScoreText.text = newValue.ToString("0000");
    }

    public void UpdatePlayerScore(int playerIndex, int newValue)
    {
        photonView.RPC("RPC_UpdatePlayerScore", RpcTarget.AllBuffered, playerIndex, newValue);
    }

    [PunRPC]
    private void RPC_UpdatePlayerScore(int playerIndex, int newValue)
    {
        if (playerIndex == 1)
        {
            OnPlayer1ScoreChanged(newValue);
        }
        else if (playerIndex == 2)
        {
            OnPlayer2ScoreChanged(newValue);
        }
    }

    public override void OnJoinedRoom()
    {
        Debug.Log("Joined room.");
        playerIndex = PhotonNetwork.LocalPlayer.ActorNumber; // Asignar el índice del jugador basado en su ActorNumber
        Debug.Log("Soy el index " + playerIndex);
        CheckStartTimerIfTwoPlayersInRoom();
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        Debug.Log("Player entered room: " + newPlayer.NickName);
        CheckStartTimerIfTwoPlayersInRoom();
    }

    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        Debug.Log("Player left room: " + otherPlayer.NickName);

        // Verificar si hay sólo un jugador restante y si el juego ha comenzado
        if (PhotonNetwork.CurrentRoom.PlayerCount == 1 && isGameStarted)
        {
            // Mostrar mensaje indicando que el jugador restante ha ganado
            Player remainingPlayer = PhotonNetwork.PlayerList[0];
            HasGanadoAbandono.SetActive(true);
            Debug.Log("Player " + remainingPlayer.NickName + " has won the game!");
            tiempo = 180f;
        }

        // Opcional: Detener el juego o hacer otras acciones necesarias
        StopTimer();
        isGameStarted = false;
    }

    public override void OnLeftRoom()
    {
        SceneManager.LoadScene("Menu principal");
    }

    public void GoBack()
    {
        PhotonNetwork.LeaveRoom();
    }

    public void IncreasePlayerScore(int playerIndex, int increment)
    {
        if (playerIndex == 1)
        {
            player1Score += increment;
            Debug.Log("Se aumenta la puntuacion del jugador " + playerIndex + " a " + player1Score + " puntos");
            UpdatePlayerScore(playerIndex, player1Score);
        }
        else if (playerIndex == 2)
        {
            player2Score += increment;
            UpdatePlayerScore(playerIndex, player2Score);
        }
    }

    void Resultados(int index)
    {
        Debug.Log("Jugador1: " + player1Score + "\nJugador2: " + player2Score);
        if (index == 1)
        {
            if (player1Score > player2Score)
            {
                HasGanado.SetActive(true);
            }
            else if (player1Score < player2Score)
            {
                HaGanadoElRival.SetActive(true);
            }
            else
            {
                Empate.SetActive(true);
            }
        }
        else if (index == 2)
        {
            if (player1Score < player2Score)
            {
                HasGanado.SetActive(true);
            }
            else if (player1Score > player2Score)
            {
                HaGanadoElRival.SetActive(true);
            }
            else
            {
                Empate.SetActive(true);
            }
        }
    }
}