using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Photon.Pun;


[RequireComponent(typeof(Rigidbody))]
public class playerController : MonoBehaviourPun, ILivingEntity
{

    public playerSettings plrSettings;

    //float playerSpeed = 2f;
    //float smoothSpeed = 0.5f;
    //float turnSpeed = 75f;
    //float jumpPower = 2.0f;

    bool isGrounded = true;
    bool jump = false;

    BoxCollider boxCollider;

    Rigidbody plrRigidbody;
    Vector2 moveInput;

    float turn;

    [SerializeField]
    private int _health;

    Slider healthBar;

    // Start is called before the first frame update
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        plrRigidbody = GetComponent<Rigidbody>();

        _health = plrSettings.startingHealth;
        if(photonView.IsMine)
        {
            healthBar = GameObject.FindGameObjectWithTag("healthBar").GetComponent<Slider>();
            boxCollider = GetComponent<BoxCollider>();
        }
    }


    void Update()
    {
        turn = Input.GetAxisRaw("Mouse X");

        if(Input.GetKeyDown(KeyCode.Escape))
        {
            leaveRoom();
        }

        if(photonView.IsMine)
        {
            if(_health <= 0)
            {
                //photonView.RPC("localPlayerDied", RpcTarget.AllBuffered);
                localPlayerDied();
            }
            //Jump
            if (Input.GetKeyDown(KeyCode.Space) && isGrounded && !jump)
            {
                jump = true;
            }
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
            Vector3 smoothNewPos = Vector3.Lerp(transform.position, transform.position + newPos, plrSettings.playerSpeed * plrSettings.smoothSpeed * Time.fixedDeltaTime);

            plrRigidbody.MovePosition(smoothNewPos);

            Quaternion newRot = transform.rotation * Quaternion.Euler(transform.up * turn * plrSettings.turnSpeed * Time.fixedDeltaTime);

            plrRigidbody.MoveRotation(newRot);

            //Jump
            if(jump && isGrounded)
            {
                plrRigidbody.velocity = new Vector3(plrRigidbody.velocity.x, 0, plrRigidbody.velocity.z);
                isGrounded = false;
                plrRigidbody.AddForce(transform.up * plrSettings.jumpPower, ForceMode.Impulse);
                isGrounded = false;
                jump = false;
            }
            /*if(!jump && !isGrounded && Physics.Raycast(transform.position, -transform.up, boxCollider.bounds.extents.y + 0.025f))
            {
                isGrounded = true;
            }*/
        }
    }

    //collisions
    void OnCollisionExit(Collision collision)
    {
        if(photonView.IsMine && collision.gameObject.tag == "floor")
        {
            isGrounded = false;
        }
    }
    void OnCollisionStay(Collision collision)
    {
        if (photonView.IsMine && collision.gameObject.tag == "floor")
        {
            isGrounded = true;
        }
    }
    void OnCollisionEnter(Collision collision)
    {
        if (photonView.IsMine && collision.gameObject.tag == "floor")
        {
            //isGrounded = true;
        }
    }

    //ILivingEntity Functions
    [PunRPC]
    public void takeDamage(int damage, Vector3 point)
    {
        if(plrSettings.useBloodEffects)
        {
            Instantiate(plrSettings.bloodParticle, point, Quaternion.identity);
        }

        _health = Mathf.Clamp(_health - damage, 0, plrSettings.startingHealth);
        if (photonView.IsMine)
        {
            healthBar.maxValue = plrSettings.startingHealth;
            healthBar.value = _health;
        }   
    }
    [PunRPC]
    public void gainHealth(int gainAmmount)
    {
        _health = Mathf.Clamp(_health + gainAmmount, 0, plrSettings.startingHealth);
        if (photonView.IsMine)
        {
            healthBar.maxValue = plrSettings.startingHealth;
            healthBar.value = _health;
        }
    }

    //local player death
    void localPlayerDied()
    {
        leaveRoom();
    }

    void leaveRoom()
    {
        Cursor.lockState = CursorLockMode.None;
        PhotonNetwork.LeaveRoom();
        SceneManager.LoadScene("Lobby");
    }

    [PunRPC]
    public void changeColor(Vector3 newColor)
    {
        GetComponent<Renderer>().material.SetColor("_DiffTint", new Color(newColor.x, newColor.y, newColor.z));
    }
}
