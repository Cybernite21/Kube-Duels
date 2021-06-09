using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class cameraFollowPlayer : MonoBehaviour
{

    public float cameraFollowSmoothSpeed = 0.5f;
    public Vector3 lookAtOffset;
    public float zoomSpped = 4f;
    public Vector3 offset;
    public Vector2 minMaxZoom = new Vector2(1f, 5f);

    float currentZoom = 1f;

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

        currentZoom -= Input.GetAxis("Mouse ScrollWheel") * zoomSpped;
        currentZoom = Mathf.Clamp(currentZoom, minMaxZoom.x, minMaxZoom.y);
    }

    void FixedUpdate()
    {
        Vector3 offsetWithZoom = offset * currentZoom;

        Vector3 newPos = plr.position + plr.forward * offsetWithZoom.z + plr.right * offsetWithZoom.x + plr.up * offsetWithZoom.y;
        Vector3 smoothNewPos = Vector3.Lerp(transform.position, newPos, cameraFollowSmoothSpeed * Time.deltaTime);

        transform.position = smoothNewPos;
        transform.LookAt(plr.position + plr.right * lookAtOffset.x + plr.forward * lookAtOffset.z + plr.up * lookAtOffset.y);
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
