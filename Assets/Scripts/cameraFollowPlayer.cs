using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class cameraFollowPlayer : MonoBehaviour
{

    public float cameraFollowSmoothSpeed = 0.5f;
    public Vector3 offset;

    Transform plr;

    bool findingPlr = false;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(findLocalPlayer());
    }

    // Update is called once per frame
    void Update()
    {
        if(plr == null && !findingPlr)
        {
            StartCoroutine(findLocalPlayer());
        }
    }

    void LateUpdate()
    {
        Vector3 newPos = plr.position + plr.forward * offset.z + plr.right * offset.x + plr.up * offset.y;
        Vector3 smoothNewPos = Vector3.Lerp(transform.position, newPos, cameraFollowSmoothSpeed * Time.deltaTime);

        transform.position = smoothNewPos;
        transform.LookAt(plr);
    }

    IEnumerator findLocalPlayer()
    {
        findingPlr = true;
        GameObject[] plrs = GameObject.FindGameObjectsWithTag("Player");

        foreach (GameObject g in plrs)
        {
            if (g.GetComponent<PhotonView>().IsMine)
            {
                plr = g.transform;
                break;
            }
        }

        findingPlr = false;
        yield return null;
    }
}
