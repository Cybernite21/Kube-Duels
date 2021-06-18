using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Photon.Pun;


[RequireComponent(typeof(Rigidbody))]
public class playerController : MonoBehaviourPun, ILivingEntity
{

    public float playerSpeed = 2f;
    public float smoothSpeed = 0.5f;
    public float turnSpeed = 75f;

    Rigidbody plrRigidbody;
    Vector2 moveInput;

    float turn;

    [SerializeField]
    private int _health = 100;

    // Start is called before the first frame update
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        plrRigidbody = GetComponent<Rigidbody>();
    }


    void Update()
    {
        turn = Input.GetAxisRaw("Mouse X");

        if(Input.GetKeyDown(KeyCode.Escape))
        {
            Cursor.lockState = CursorLockMode.None;
            PhotonNetwork.LeaveRoom();
            SceneManager.LoadScene("Lobby");
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (photonView.IsMine)
        {
            moveInput.x = Input.GetAxisRaw("Horizontal");
            moveInput.y = Input.GetAxisRaw("Vertical");

            Vector3 newPos = transform.forward * moveInput.y + transform.right * moveInput.x;
            Vector3 smoothNewPos = Vector3.Lerp(transform.position, transform.position + newPos, playerSpeed * smoothSpeed * Time.fixedDeltaTime);

            plrRigidbody.MovePosition(smoothNewPos);

            Quaternion newRot = transform.rotation * Quaternion.Euler(transform.up * turn * turnSpeed * Time.fixedDeltaTime);

            plrRigidbody.MoveRotation(newRot);
        }
    }

    //ILivingEntity Functions
    [PunRPC]
    public void takeDamage(int damage)
    {
        _health = Mathf.Clamp(_health - damage, 0, 100);
    }
    [PunRPC]
    public void gainHealth(int gainAmmount)
    {
        _health = Mathf.Clamp(_health + gainAmmount, 0, 100);
    }

    [PunRPC]
    public void changeColor(Vector3 newColor)
    {
        GetComponent<Renderer>().material.SetColor("_DiffTint", new Color(newColor.x, newColor.y, newColor.z));
    }
}
