using UnityEngine;
using UnityEngine.UI;
using Unity.Netcode;
using UnityEngine.SceneManagement;

public class GameInitializer : MonoBehaviour
{
    private NetworkDiscovery _networkDiscovery;

    public Button hostButton;
    public Button clientButton;
    public Button undo;

    void Start()
    {
        _networkDiscovery = GetComponentInChildren<NetworkDiscovery>();

        hostButton.onClick.AddListener(StartHost);
        clientButton.onClick.AddListener(StartClient);
        undo.onClick.AddListener(GoBack);
    }

    void StartHost()
    {
        if (!NetworkManager.Singleton.IsHost)
        {
            NetworkManager.Singleton.StartHost();
            DontDestroyOnLoad(NetworkManager.Singleton.gameObject);
            if (_networkDiscovery != null)
            {
                _networkDiscovery.enabled = false; // Disable discovery on host
            }
            Debug.Log("Host started, waiting for clients to connect...");

            // Navegar a la escena de juego inmediatamente
            SceneManager.LoadScene("Multiplayer");
        }
        else
        {
            Debug.LogWarning("Cannot start Host while an instance is already running.");
        }
    }

    void StartClient()
    {
        if (!NetworkManager.Singleton.IsClient)
        {
            if (_networkDiscovery != null)
            {
                _networkDiscovery.enabled = true; // Enable discovery on client
            }

            NetworkManager.Singleton.OnClientConnectedCallback += OnClientConnected;
            NetworkManager.Singleton.StartClient();
            DontDestroyOnLoad(NetworkManager.Singleton.gameObject);
        }
        else
        {
            Debug.LogWarning("Cannot start Client while an instance is already running.");
        }
    }

    private void OnClientConnected(ulong clientId)
    {
        if (!NetworkManager.Singleton.IsServer && clientId == NetworkManager.Singleton.LocalClientId)
        {
            Debug.Log("Client connected, moving to game scene...");
            // Esperar confirmación del servidor
            SceneManager.LoadScene("Multiplayer");
        }
    }

    public void GoBack()
    {
        SceneManager.LoadScene("Menu principal");
    }
}