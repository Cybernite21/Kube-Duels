using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;

public class createAndJoinRooms : MonoBehaviourPunCallbacks
{

    public InputField createRoomInput;
    public InputField joinRoomInput;

    public void createRoom()
    {
        PhotonNetwork.CreateRoom(createRoomInput.text);
    }

    public void joinRoom()
    {
        PhotonNetwork.JoinRoom(joinRoomInput.text);
    }

    //does things when joined room
    public override void OnJoinedRoom()
    {
        base.OnJoinedRoom();
        PhotonNetwork.LoadLevel("Game");
    }
}
