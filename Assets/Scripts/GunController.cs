using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunController : MonoBehaviour
{
    public GameObject bulletSpawn;
    public float gunTiltSpeed = 75f;

    float turn;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        turn = Input.GetAxisRaw("Mouse Y");

        transform.localEulerAngles = new Vector3(transform.localEulerAngles.x - (turn * gunTiltSpeed * Time.deltaTime), 0, 0);
    }

    void FixedUpdate()
    {

    }
}
