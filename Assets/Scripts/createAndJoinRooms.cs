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
