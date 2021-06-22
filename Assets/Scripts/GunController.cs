using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class GunController : MonoBehaviourPun
{
    public gunSettings gun;

    public GameObject bulletSpawn;
    public GameObject bulletPrefab;
    /*public int damage = 5;
    public float gunTiltSpeed = 75f;
    public float bulletSpeed = 100f;
    public float shootCoolDownSecs = 0.25f;
    public float aimLazerDistance = 2.5f;
    public LayerMask aimLazerMask;

    [ColorUsage(true, true)]
    public Color lazerNoEnemyColor;
    [ColorUsage(true, true)]
    public Color lazerEnemyColor;*/

    int bulletsShot = 0;
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

            transform.localEulerAngles = new Vector3(transform.localEulerAngles.x - (turn * gun.gunTiltSpeed * Time.deltaTime), 0, 0);

            if(transform.localEulerAngles.x > 45 && transform.localEulerAngles.x < 180)
            {
                transform.localEulerAngles = new Vector3(45, 0, 0);
            }
            else if(transform.localEulerAngles.x < 315 && transform.localEulerAngles.x > 180)
            {
                transform.localEulerAngles = new Vector3(315, 0, 0);
            }

            if(Input.GetMouseButton(0) && canShoot)
            {
                GameObject newBullet = PhotonNetwork.Instantiate(bulletPrefab.name, bulletSpawn.transform.position, Quaternion.identity);
                newBullet.GetComponent<bullet>().bulletspeed = gun.bulletSpeed;
                newBullet.GetComponent<bullet>().damage = gun.damage;
                newBullet.GetComponent<bullet>().bulletDeathSecs = gun.bulletDeathSecs;
                newBullet.transform.forward = transform.forward;
                bulletsShot++;
                StartCoroutine(shootCooldown());
            }

            
            //aimLazer.SetPosition(1, transform.InverseTransformPoint(bulletSpawn.transform.position + transform.forward * aimLazerDistance));
        }
    }

    IEnumerator shootCooldown()
    {
        canShoot = false;
        yield return new WaitForSeconds(gun.shootCoolDownSecs);
        if(!gun.holdToRapidFire)
        {
            yield return new WaitWhile(() => Input.GetMouseButton(0));
        }
        if(gun.gunCoolDown > 0)
        {
            if(bulletsShot % gun.gunCoolDown == 0)
            {
                yield return new WaitForSeconds(gun.gunCoolDownSecs);
            }
        }
        canShoot = true;
        yield return null;
    }

    void FixedUpdate()
    {
        if(photonView.IsMine)
        {
            aimLazerSetPos();
            aimLazerEnemyCheck();
        }
    }

    void aimLazerSetPos()
    {
        aimRay.origin = bulletSpawn.transform.position;
        aimRay.direction = bulletSpawn.transform.forward;

        if (Physics.Raycast(aimRay.origin, aimRay.direction, out aimHit, gun.aimLazerDistance, gun.aimLazerMask, QueryTriggerInteraction.Ignore))
        {
            aimLazer.SetPosition(1, transform.InverseTransformPoint(aimHit.point));
        }
        else
        {
            aimLazer.SetPosition(1, transform.InverseTransformPoint(bulletSpawn.transform.position + transform.forward * gun.aimLazerDistance));
        }
    }

    void aimLazerEnemyCheck()
    {
        aimEnemyRay.origin = bulletSpawn.transform.position;
        aimEnemyRay.direction = bulletSpawn.transform.forward;

        if (Physics.Raycast(aimEnemyRay.origin, aimEnemyRay.direction, out aimEnemyHit, 50f, gun.aimLazerMask, QueryTriggerInteraction.Ignore))
        {
            if (aimEnemyHit.collider.gameObject.tag == "Player")
            {
                aimLazer.material.SetColor("_aimLazerColor", gun.lazerEnemyColor);
            }
            else
            {
                aimLazer.material.SetColor("_aimLazerColor", gun.lazerNoEnemyColor);
            }
        }
        else
        {
            aimLazer.material.SetColor("_aimLazerColor", gun.lazerNoEnemyColor);
        }
    }
}
