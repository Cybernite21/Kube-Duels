using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cameraFollowPlayer : MonoBehaviour
{

    public float cameraFollowSmoothSpeed = 0.5f;
    public Vector3 offset;

    Transform plr;

    // Start is called before the first frame update
    void Start()
    {
        plr = GameObject.FindGameObjectWithTag("Player").transform;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void LateUpdate()
    {
        Vector3 newPos = plr.position + plr.forward * offset.z + plr.right * offset.x + plr.up * offset.y;
        Vector3 smoothNewPos = Vector3.Lerp(transform.position, newPos, cameraFollowSmoothSpeed * Time.deltaTime);

        transform.position = smoothNewPos;
        transform.LookAt(plr);
    }
}
