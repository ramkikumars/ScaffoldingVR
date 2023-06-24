using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
public class NetworkManager : MonoBehaviourPunCallbacks
{
    // Start is called before the first frame update
    void Start()
    {
        PhotonNetwork.SendRate=20;
        PhotonNetwork.SerializationRate=5;
        ConnectToServer();
    }

    // Update is called once per frame
    void Update()
    {

    }

    void ConnectToServer(){
        PhotonNetwork.ConnectUsingSettings();
        Debug.Log("Try Connect to Server");

    }

    public override void OnConnectedToMaster()
    {
        Debug.Log("Connectd To Server.");
        base.OnConnectedToMaster();
        RoomOptions roomOptions=new RoomOptions();
        roomOptions.MaxPlayers=10;
        roomOptions.IsVisible=true;
        roomOptions.IsOpen=true;
        PhotonNetwork.JoinOrCreateRoom("Room 1",roomOptions,TypedLobby.Default);
    }

    public override void OnJoinedRoom()
    {

        Debug.Log("Joined a Room");
        base.OnJoinedRoom();
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        Debug.Log("A new plater joined the room");
        base.OnPlayerEnteredRoom(newPlayer);
    }
}
