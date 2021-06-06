using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class spawnPlayers : MonoBehaviour
{
    public GameObject plrPrefab;
    public Vector3 plrSpawnMin;
    public Vector3 plrSpawnMax;

    // Start is called before the first frame update
    void Start()
    {
        Vector3 spawnPos = new Vector3(Random.Range(plrSpawnMin.x, plrSpawnMax.x), Random.Range(plrSpawnMin.y, plrSpawnMax.y), Random.Range(plrSpawnMin.z, plrSpawnMax.z));
        GameObject plr = PhotonNetwork.Instantiate(plrPrefab.name, spawnPos, Quaternion.identity);
        Color randomColor = Random.ColorHSV();
        plr.GetComponent<PhotonView>().RPC("changeColor", RpcTarget.AllBuffered, new Vector3(randomColor.r, randomColor.g, randomColor.b));
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
