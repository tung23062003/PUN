using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimation : MonoBehaviour
{
    [SerializeField] private Animator animator;

    private PhotonView photonView;
    private PlayerInput input;

    void Awake()
    {
        photonView = GetComponent<PhotonView>();
        input = GetComponent<PlayerInput>();
    }

    void Update()
    {
        if (photonView.IsMine)
        {
            float speed = input.MovementInput.magnitude;
            animator.SetFloat("Speed", speed);
            photonView.RPC("SyncAnim", RpcTarget.Others, speed);
        }
    }

    [PunRPC]
    void SyncAnim(float speed)
    {
        animator.SetFloat("Speed", speed);
    }
}
