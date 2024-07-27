using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerSetup : MonoBehaviour
{
    public PlayerMovement playerMovement;
    public GameObject camera;

    public string nickName;

    public TextMeshPro nickNameText;

    

    public void IsLocalPlayer()
    {
        playerMovement.enabled = true;
        camera.SetActive(true);    
    }

    [PunRPC]    
    public void SetNickName(string _name)
    { 
       nickName = _name;
       nickNameText.text = nickName;    
    }
}
