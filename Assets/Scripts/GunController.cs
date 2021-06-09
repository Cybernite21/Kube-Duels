using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class GunController : MonoBehaviourPun
{
    public GameObject bulletSpawn;
    public GameObject bulletPrefab;
    public float gunTiltSpeed = 75f;
    public float bulletSpeed = 100f;
    public float shootCoolDownSecs = 0.25f;

    float turn;
    bool canShoot = true;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (photonView.IsMine)
        {
            turn = Input.GetAxisRaw("Mouse Y");

            transform.localEulerAngles = new Vector3(transform.localEulerAngles.x - (turn * gunTiltSpeed * Time.deltaTime), 0, 0);

            if(transform.localEulerAngles.x > 45 && transform.localEulerAngles.x < 180)
            {
                transform.localEulerAngles = new Vector3(45, 0, 0);
            }
            else if(transform.localEulerAngles.x < 315 && transform.localEulerAngles.x > 180)
            {
                transform.localEulerAngles = new Vector3(315, 0, 0);
            }

            if(Input.GetMouseButtonDown(0) && canShoot)
            {
                GameObject newBullet = PhotonNetwork.Instantiate(bulletPrefab.name, bulletSpawn.transform.position, Quaternion.identity);
                newBullet.GetComponent<bullet>().bulletspeed = bulletSpeed;
                newBullet.transform.forward = transform.forward;
                StartCoroutine(shootCooldown());
            }
        }
    }

    IEnumerator shootCooldown()
    {
        canShoot = false;
        yield return new WaitForSeconds(shootCoolDownSecs);
        canShoot = true;
        yield return null;
    }

    void FixedUpdate()
    {

    }
}
