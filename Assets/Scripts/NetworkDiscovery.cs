using LiteNetLib;
using LiteNetLib.Utils;
using UnityEngine;
using Unity.Netcode;
using Unity.Netcode.Transports.UTP;
using System.Net.Sockets;
using System.Net;

public class NetworkDiscovery : MonoBehaviour, INetEventListener
{
    private NetManager _client;
    private NetManager _server;
    private NetDataWriter _writer;

    public string DiscoveryMessage = "DISCOVERY_REQUEST";

    void Start()
    {
        _writer = new NetDataWriter();
        _writer.Put(DiscoveryMessage);

        _client = new NetManager(this);
        _client.Start();
        _client.SendBroadcast(_writer, 5000);

        _server = new NetManager(this);
        _server.Start(5000);
    }

    void Update()
    {
        _client.PollEvents();
        _server.PollEvents();
    }

    public void OnPeerConnected(NetPeer peer) { }
    public void OnPeerDisconnected(NetPeer peer, DisconnectInfo disconnectInfo) { }
    public void OnNetworkError(IPEndPoint endPoint, SocketError socketError) { }
    public void OnNetworkReceive(NetPeer peer, NetPacketReader reader, DeliveryMethod deliveryMethod) { }
    public void OnNetworkReceiveUnconnected(IPEndPoint remoteEndPoint, NetPacketReader reader, UnconnectedMessageType messageType)
    {
        if (messageType == UnconnectedMessageType.Broadcast && reader.GetString() == DiscoveryMessage)
        {
            // Respond to discovery message
            NetDataWriter responseWriter = new NetDataWriter();
            responseWriter.Put("DISCOVERY_RESPONSE");
            _server.SendUnconnectedMessage(responseWriter, remoteEndPoint);
        }
        else if (reader.GetString() == "DISCOVERY_RESPONSE")
        {
            // Received a discovery response
            string serverIP = remoteEndPoint.Address.ToString();
            ConnectToServer(serverIP);
        }
    }
    public void OnNetworkLatencyUpdate(NetPeer peer, int latency) { }

    public void OnConnectionRequest(ConnectionRequest request) { }

    private void ConnectToServer(string serverIP)
    {
        var transport = NetworkManager.Singleton.GetComponent<UnityTransport>();
        transport.ConnectionData.Address = serverIP;
        NetworkManager.Singleton.StartClient();
    }

    public void OnNetworkReceive(NetPeer peer, NetPacketReader reader, byte channelNumber, DeliveryMethod deliveryMethod)
    {
        throw new System.NotImplementedException();
    }
}