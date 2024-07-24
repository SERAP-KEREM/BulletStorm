using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    public int damage;

    public float fireRate;
    private float nextFire;

    public Camera camera;

    [Header("VFX")]
    public GameObject hitVFX;

    [Header("Ammo")]
    public int fireCount = 30;

    public int ammo = 30;
    public int magAmmo = 150;

    [Header("UI")]
   // public TextMeshProUGUI magText;
    public TextMeshProUGUI ammoText;

    [Header("Animation")]
    public Animation weaponAnim;
    public AnimationClip reload;


    private void Start()
    {
      //  magText.text = mag.ToString();
        ammoText.text = ammo + "/" + magAmmo.ToString();
    }

    private void Update()
    {
        if (nextFire > 0)
        {
            nextFire -= Time.deltaTime;
        }

        if (Input.GetButton("Fire1") && nextFire <= 0 && ammo>0 && weaponAnim.isPlaying==false)
        {
            Debug.Log("ateş");
            nextFire = 1 / fireRate;
            ammo--;

           // magText.text = mag.ToString();
            ammoText.text = ammo + "/" + magAmmo.ToString();


            Fire();
        }
        if(Input.GetKeyDown(KeyCode.R))
        {
            Reload();
           
            
        }
        if (magAmmo == 0 && ammo==0)
        {
            ammoText.color = Color.red;
        }


    }
    void Reload()
    {
        weaponAnim.Play(reload.name);
        if(magAmmo>0)
        {
            magAmmo -= fireCount-ammo;
            ammo = fireCount;
           
            
        }
       

        //magText.text=mag.ToString();
        ammoText.text=ammo+"/"+magAmmo.ToString();
    }
    void Fire()
    {
        Ray ray = new Ray(camera.transform.position, camera.transform.forward);

        RaycastHit hit;

        if (Physics.Raycast(ray.origin, ray.direction, out hit, 100f))
        {
            PhotonNetwork.Instantiate(hitVFX.name, hit.point, Quaternion.identity);
            if (hit.transform.gameObject.GetComponent<PlayerHealth>())
            {
                hit.transform.gameObject.GetComponent<PhotonView>().RPC("TakeDamage", RpcTarget.All, damage);
            }
        }
    }
   
}
