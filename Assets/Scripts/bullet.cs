using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class bullet : MonoBehaviourPun
{
    public float bulletspeed = 10f;
    public float bulletSkin = 0.05f;
    public float bulletDeathSecs = 5f;
    public LayerMask shootLayerMask;

    Vector3 prevPos;

    Rigidbody rb;

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
            RaycastHit[] hit = Physics.BoxCastAll(r.origin, transform.localScale, r.direction, Quaternion.identity, (transform.position-prevPos).magnitude, shootLayerMask, QueryTriggerInteraction.Ignore);
            if (hit.Length != 0)
            {
                print("b");
                PhotonNetwork.Destroy(photonView);
            }
            yield return new WaitForFixedUpdate();
        }
        yield return null;
    }

    IEnumerator bulletDeath()
    {
        yield return new WaitForSeconds(bulletDeathSecs);
        PhotonNetwork.Destroy(photonView);
        yield return null;
    }
}
