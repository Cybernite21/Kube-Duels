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
    public float aimLazerDistance = 2.5f;
    public LayerMask aimLazerMask;

    [ColorUsage(true, true)]
    public Color lazerNoEnemyColor;
    [ColorUsage(true, true)]
    public Color lazerEnemyColor;

    float turn;
    bool canShoot = true;

    LineRenderer aimLazer;

    Ray aimRay;
    RaycastHit aimHit;

    Ray aimEnemyRay;
    RaycastHit aimEnemyHit;

    // Start is called before the first frame update
    void Start()
    {
        if (photonView.IsMine)
        {
            aimLazer = GetComponent<LineRenderer>();
            aimLazer.SetPosition(0, bulletSpawn.transform.localPosition);
            aimLazer.enabled = true;
            aimRay = new Ray(bulletSpawn.transform.position, bulletSpawn.transform.forward);
        }
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

            
            //aimLazer.SetPosition(1, transform.InverseTransformPoint(bulletSpawn.transform.position + transform.forward * aimLazerDistance));
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
        aimLazerSetPos();

    }

    void aimLazerSetPos()
    {
        aimRay.origin = bulletSpawn.transform.position;
        aimRay.direction = bulletSpawn.transform.forward;

        if (Physics.Raycast(aimRay.origin, aimRay.direction, out aimHit, aimLazerDistance, aimLazerMask, QueryTriggerInteraction.Ignore))
        {
            aimLazer.SetPosition(1, transform.InverseTransformPoint(aimHit.point));
        }
        else
        {
            aimLazer.SetPosition(1, transform.InverseTransformPoint(bulletSpawn.transform.position + transform.forward * aimLazerDistance));
        }
    }

    void aimLazerEnemyCheck()
    {
        aimEnemyRay.origin = bulletSpawn.transform.position;
        aimEnemyRay.direction = bulletSpawn.transform.forward;

        if (Physics.Raycast(aimEnemyRay.origin, aimEnemyRay.direction, out aimEnemyHit, 50f, aimLazerMask, QueryTriggerInteraction.Ignore))
        {
            if (aimEnemyHit.collider.gameObject.tag == "Player")
            {
                aimLazer.material.color = lazerEnemyColor;
            }
            else
            {
                aimLazer.material.color = lazerNoEnemyColor;
            }
        }
        else
        {
            aimLazer.material.color = lazerNoEnemyColor;
        }
    }
}
