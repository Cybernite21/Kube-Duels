using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Realtime;
using Photon.Pun;

public class createAndJoinRooms : MonoBehaviourPunCallbacks
{

    public InputField createRoomInput;
    public InputField joinRoomInput;
    public InputField playerNameInput;

    public Text errorText;

    void Start()
    {
        if(PlayerPrefs.GetString("localPlayerName") != null)
        {
            playerNameInput.text = PlayerPrefs.GetString("localPlayerName");
        }
    }

    public void createRoom()
    {
        storePlayerName();
        PhotonNetwork.CreateRoom(createRoomInput.text);
    }

    public void joinRoom()
    {
        storePlayerName();
        PhotonNetwork.JoinRoom(joinRoomInput.text);
    }

    //does things when joined room
    public override void OnJoinedRoom()
    {
        base.OnJoinedRoom();

        //deal with problem if there is already a player with same name as local player
        PhotonNetwork.NickName = fixName();

        //load level
        PhotonNetwork.LoadLevel("Game");
    }

    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        base.OnCreateRoomFailed(returnCode, message);
        errorText.text = "Creating room failed!\nTry a different room name";
    }

    public override void OnJoinRoomFailed(short returnCode, string message)
    {
        base.OnJoinRoomFailed(returnCode, message);
        errorText.text = "Joining room failed!\nTry a different room name, or check the spelling";
    }

    //stores Player name in playerPrefs
    void storePlayerName()
    {
        PlayerPrefs.SetString("localPlayerName", playerNameInput.text);
        PlayerPrefs.Save();
        PhotonNetwork.LocalPlayer.NickName = playerNameInput.text;
    }

    //add random number to end of name
    string fixName()
    {
        string newLocalPlayerName = PhotonNetwork.NickName;

        foreach (Player p in PhotonNetwork.PlayerListOthers)
        {
            if (p.NickName == newLocalPlayerName)
            {
                newLocalPlayerName = newLocalPlayerName + Random.Range(0f, 9f);
            }
        }

        return newLocalPlayerName;
    }
}
