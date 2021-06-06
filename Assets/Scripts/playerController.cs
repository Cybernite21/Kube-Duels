using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;


[RequireComponent(typeof(Rigidbody))]
public class playerController : MonoBehaviour
{

    public float playerSpeed = 2f;
    public float smoothSpeed = 0.5f;

    Rigidbody plrRigidbody;
    Vector2 moveInput;

    PhotonView photonView;

    // Start is called before the first frame update
    void Start()
    {
        plrRigidbody = GetComponent<Rigidbody>();
        photonView = GetComponent<PhotonView>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (photonView.IsMine)
        {
            moveInput.x = Input.GetAxisRaw("Horizontal");
            moveInput.y = Input.GetAxisRaw("Vertical");

            Vector3 newPos = transform.forward * moveInput.y + transform.right * moveInput.x;
            Vector3 smoothNewPos = Vector3.Lerp(transform.position, transform.position + newPos, playerSpeed * smoothSpeed * Time.deltaTime);

            plrRigidbody.MovePosition(smoothNewPos);
        }
    }
}
