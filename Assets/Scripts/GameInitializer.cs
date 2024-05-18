using UnityEngine;
using UnityEngine.UI;
using Unity.Netcode;

public class GameInitializer : MonoBehaviour
{
    public Button hostButton;
    public Button clientButton;

    void Start()
    {
        hostButton.onClick.AddListener(StartHost);
        clientButton.onClick.AddListener(StartClient);
    }

    void StartHost()
    {
        NetworkManager.Singleton.StartHost();
        GetComponent<NetworkDiscovery>().enabled = false; // Disable discovery on host
    }

    void StartClient()
    {
        GetComponent<NetworkDiscovery>().enabled = true; // Enable discovery on client
    }
}
