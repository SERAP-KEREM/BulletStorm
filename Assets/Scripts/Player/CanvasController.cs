using UnityEngine;
using Photon.Pun;

public class CanvasController : MonoBehaviour
{
    private PhotonView photonView;

    void Start()
    {
        photonView = GetComponentInParent<PhotonView>();
        if (!photonView.IsMine)
        {
            gameObject.SetActive(false);
        }
    }
}
