using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(Rigidbody))]
public class playerController : MonoBehaviour
{

    public float playerSpeed = 2f;
    public float smoothSpeed = 0.5f;

    Rigidbody plrRigidbody;
    Vector2 moveInput;

    // Start is called before the first frame update
    void Start()
    {
        plrRigidbody = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        moveInput.x = Input.GetAxisRaw("Horizontal");
        moveInput.y = Input.GetAxisRaw("Vertical");

        if(moveInput != Vector2.zero)
        {
            Vector3 newPos = transform.forward * moveInput.y + transform.right * moveInput.x;
            Vector3 smoothNewPos = Vector3.Lerp(transform.position, transform.position + newPos, smoothSpeed * Time.deltaTime);

            plrRigidbody.MovePosition(smoothNewPos);
        }
    }
}
