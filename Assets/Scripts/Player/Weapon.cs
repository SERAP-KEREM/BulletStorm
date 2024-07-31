using Photon.Pun;
using Photon.Pun.UtilityScripts;
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
    public TextMeshProUGUI ammoText;

    [Header("Animation")]

    public Animator weaponAnimator;

    [Header("Recoil Settings")]
    [Range(0, 2)]
    public float recoverPercent = 0.7f;

    public float recoilUp = 1f;
    public float recoilBack = 0f;

    private Vector3 originalPosition;
    private Vector3 recoilVelocity = Vector3.zero;
    private float recoilLength;
    private float recoverLenth;
    private bool recoiling;
    public bool recovering;

    private void Start()
    {
        ammoText.text = ammo + "/" + magAmmo.ToString();
        originalPosition = transform.localPosition;
        recoilLength = 0;
        recoverLenth = 1 / fireRate * recoverPercent;
    }

    private void Update()
    {
        if (nextFire > 0)
        {
            nextFire -= Time.deltaTime;
        }

        if (Input.GetButton("Fire1") && nextFire <= 0 && ammo > 0 /*&& weaponAnim.isPlaying == false*/)
        {
            Debug.Log("ateş");
            nextFire = 1 / fireRate;
            ammo--;
            ammoText.text = ammo + "/" + magAmmo.ToString();
            Fire();
         
        }
        if (Input.GetKeyDown(KeyCode.R) && magAmmo > 0)
        {
            Reload();
        }
        if (magAmmo == 0 && ammo == 0)
        {
            ammoText.color = Color.red;
        }
        if (recoiling)
        {
            Recoil();
        }
        if (recovering)
        {
            Recovering();
        }

        // Sürekli raycast çiz
        DrawContinuousRaycast();
    }

    void Reload()
    {
        weaponAnimator.SetBool("reload", true);
       // weaponAnim.Play(reload.name);
        if (magAmmo > 0)
        {
            magAmmo -= fireCount - ammo;
            ammo = fireCount;
        }
        ammoText.text = ammo + "/" + magAmmo.ToString();
    }

    void Fire()
    {
        recoiling = true;
        recovering = false;

        Ray ray = new Ray(camera.transform.position, camera.transform.forward);
        RaycastHit hit;

        PhotonNetwork.LocalPlayer.AddScore(1);

        if (Physics.Raycast(ray.origin, ray.direction, out hit, 100f))
        {
            PhotonNetwork.Instantiate(hitVFX.name, hit.point, Quaternion.identity);
            if (hit.transform.gameObject.GetComponent<PlayerHealth>())
            {

                PhotonNetwork.LocalPlayer.AddScore(damage);
                if (damage>=hit.transform.gameObject.GetComponent<PlayerHealth>().health)
                {
                    //kill

                    RoomManager.instance.kills++;
                    RoomManager.instance.SetHashes();
                    PhotonNetwork.LocalPlayer.AddScore(100);
                }
                hit.transform.gameObject.GetComponent<PhotonView>().RPC("TakeDamage", RpcTarget.All, damage);
            }
        }
        
    }

    void DrawContinuousRaycast()
    {
        Ray ray = new Ray(camera.transform.position, camera.transform.forward);
        Debug.DrawRay(ray.origin, ray.direction * 100f, Color.red);
    }

    void Recoil()
    {
        Vector3 finalPosition = new Vector3(originalPosition.x, originalPosition.y + recoilUp, originalPosition.z - recoilBack);
        transform.localPosition = Vector3.SmoothDamp(transform.localPosition, finalPosition, ref recoilVelocity, recoilLength);
        if (transform.localPosition == finalPosition)
        {
            recoiling = false;
            recovering = true;
        }
    }

    void Recovering()
    {
        Vector3 finalPosition = originalPosition;
        transform.localPosition = Vector3.SmoothDamp(transform.localPosition, finalPosition, ref recoilVelocity, recoverLenth);
        if (transform.localPosition == finalPosition)
        {
            recoiling = false;
            recovering = false;
        }
    }
}
