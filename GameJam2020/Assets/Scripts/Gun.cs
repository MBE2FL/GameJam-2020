using Photon;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : PunBehaviour
{
    [SerializeField]
    float _reloadTime = 1.0f;
    float _currReloadTimer = 0.0f;
    [SerializeField]
    float _bulletSpeed = 50.0f;

    [SerializeField]
    Bullet _bulletPrefab;

    // Start is called before the first frame update
    void Start()
    {
        _currReloadTimer = _reloadTime;
    }

    // Update is called once per frame
    void Update()
    {
        _currReloadTimer += Time.deltaTime;

        if (Input.GetKey(KeyCode.Mouse0) && (_currReloadTimer >= _reloadTime))
        {
            //photonView.RPC("fire", PhotonTargets.All);

            GameObject bullet = PhotonNetwork.Instantiate(_bulletPrefab.name, transform.position + transform.forward * 10.0f, transform.rotation, 0);

            Vector3 force = transform.forward * _bulletSpeed;
            bullet.GetComponent<Rigidbody>().AddForce(force);

            _currReloadTimer = 0.0f;
        }
    }

    //[PunRPC]
    //void fire()
    //{
    //    GameObject bullet = PhotonNetwork.Instantiate(_bulletPrefab.name, transform.position, transform.rotation, 0);

    //    Vector3 force = transform.forward * _bulletSpeed;
    //    bullet.GetComponent<Rigidbody>().AddForce(force);
    //}
}
