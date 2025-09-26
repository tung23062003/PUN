using Assets.HeroEditor4D.Common.Scripts.CharacterScripts;
using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerNetwork : MonoBehaviour, IPunObservable
{
    public Character4D Character;
    private PhotonView photonView;

    private Vector3 networkPosition;
    private Vector3 networkDirection;
    private float lastPacketTime;

    private float moveSpeed = 5f;
    private float lerpRate = 10f;

    private PlayerInput playerInput;

    void Awake()
    {
        photonView = GetComponent<PhotonView>();
        playerInput = GetComponent<PlayerInput>();
        networkPosition = transform.position;
    }

    void Update()
    {
        if (!photonView.IsMine)
        {
            float lag = (float)(PhotonNetwork.Time - lastPacketTime);

            Vector3 predictedPosition = networkPosition + networkDirection * moveSpeed * lag;

            transform.position = Vector3.Lerp(transform.position, predictedPosition, Time.deltaTime * lerpRate);

            if (networkDirection != Vector3.zero)
                transform.up = Vector3.Lerp(transform.up, networkDirection, Time.deltaTime * lerpRate);
        }
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            Vector3 moveDir = playerInput.MovementInput;
            stream.SendNext(transform.position);
            stream.SendNext(moveDir);
        }
        else
        {
            networkPosition = (Vector3)stream.ReceiveNext();
            networkDirection = (Vector3)stream.ReceiveNext();
            lastPacketTime = (float)info.SentServerTime;
        }
    }
}
