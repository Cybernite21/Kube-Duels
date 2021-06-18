using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class bullet : MonoBehaviourPun
{
    public float bulletspeed = 10f;
    public float bulletSkin = 0.05f;
    public float bulletDeathSecs = 5f;
    public int damage = 10;
    public LayerMask shootLayerMask;

    Vector3 prevPos;

    Rigidbody rb;
    RaycastHit hit;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();

        if (photonView.IsMine)
        {
            StartCoroutine(bulletDeath());
            StartCoroutine(bulletHitCheck());
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (photonView.IsMine)
        {
            transform.position += transform.forward * bulletspeed * Time.deltaTime;
            prevPos = transform.position;
        }
    }

    IEnumerator bulletHitCheck()
    {
        while(true)
        {
            Ray r = new Ray(transform.position + transform.forward * transform.localScale.z, transform.forward); 
            
            if (Physics.BoxCast(r.origin, transform.localScale, r.direction, out hit, Quaternion.identity, (transform.position - prevPos).magnitude, shootLayerMask, QueryTriggerInteraction.Ignore);)
            {
                print("b");
                //PhotonNetwork.Destroy(photonView);
                if(hit.collider.gameObject.GetComponent<ILivingEntity>()!=null)
                {
                    dealDamage();
                }
                else
                {
                    PhotonNetwork.Destroy(photonView);
                }
            }
            yield return new WaitForFixedUpdate();
        }
        yield return null;
    }

    void dealDamage()
    {
        hit.collider.gameObject.GetComponent<ILivingEntity>().takeDamage(damage);
        PhotonNetwork.Destroy(photonView);
    }

    IEnumerator bulletDeath()
    {
        yield return new WaitForSeconds(bulletDeathSecs);
        PhotonNetwork.Destroy(photonView);
        yield return null;
    }
}
