using UnityEngine;
using Photon.Pun;

public class Launcher : MonoBehaviourPunCallbacks
{
    public PhotonView player;
    public Transform spawnPoint;

    // Start is called before the first frame update
    void Start()
    {
        PhotonNetwork.ConnectUsingSettings();
    }

    public override void OnConnectedToMaster()
    {
        Debug.Log("Connected to Master");
        if (PhotonNetwork.CountOfRooms == 0)
        {
            Debug.Log("No hay salas, creando una nueva.");
            PhotonNetwork.CreateRoom(null);
        }
        else
        {
            Debug.Log("Uniendo a una sala existente.");
            PhotonNetwork.JoinRandomRoom();
        }
    }

    public override void OnJoinedRoom()
    {
        Debug.Log("Joined Room");
        PhotonNetwork.Instantiate("Player", spawnPoint.position, spawnPoint.rotation);
    }
}