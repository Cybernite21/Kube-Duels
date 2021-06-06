using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Photon.Pun;

public class connectToServer : MonoBehaviourPunCallbacks
{
    // Start is called before the first frame update
    void Start()
    {
        PhotonNetwork.ConnectUsingSettings();
    }

    //when connected to Main server, join lobby to give access to rooms
    public override void OnConnectedToMaster()
    {
        base.OnConnectedToMaster();
        PhotonNetwork.JoinLobby();
    }

    //called when joined main server lobby
    public override void OnJoinedLobby()
    {
        base.OnJoinedLobby();

        //open lobby scene
        SceneManager.LoadScene("Lobby");
    }
}
