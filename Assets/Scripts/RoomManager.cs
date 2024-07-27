using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class RoomManager : MonoBehaviourPunCallbacks
{

    public static RoomManager instance;

    public GameObject player;



    [Space]
    public Transform[] spawnPoints;

    [Space]
    public GameObject roomCamera;

    [Space]
    public GameObject nameUI;
    public GameObject connectingUI;

    private string nickName = "unnamed";

    private void Awake()
    {
        instance = this;
    }

    public void ChangeNickName(string _name)
    {
        nickName = _name;
    }
    
    public void JoinRoomButtonPressed()
    {
        Debug.Log("Connecting...");
        PhotonNetwork.ConnectUsingSettings();

        nameUI.SetActive(false);
        connectingUI.SetActive(true);
    }


    private void Start()
    {
        Debug.Log("Connecting...");

       // PhotonNetwork.ConnectUsingSettings();
    }

    public override void OnConnectedToMaster()
    {
        base.OnConnectedToMaster();

        Debug.Log("Connected to server");

        PhotonNetwork.JoinLobby();
    }

    public override void OnJoinedLobby()
    {
        base.OnJoinedLobby();

        Debug.Log("We are in the lobby");

        PhotonNetwork.JoinOrCreateRoom("test", null, null);
    }

    public override void OnJoinedRoom()
    {
        base.OnJoinedRoom();

        Debug.Log("We are connected and in a room now");

        roomCamera.SetActive(false);    

        SpawnPlayer();


    }

    public void SpawnPlayer()
    {
        Transform spawnPoint = spawnPoints[UnityEngine.Random.Range(0,spawnPoints.Length)];
        GameObject _player = PhotonNetwork.Instantiate(player.name, spawnPoint.position, Quaternion.identity);
        _player.GetComponent<PlayerSetup>().IsLocalPlayer();
        _player.GetComponent<PlayerHealth>().isLocalPlayer = true;

        _player.GetComponent<PhotonView>().RPC("SetNickName", RpcTarget.AllBuffered,nickName);
    }
}
