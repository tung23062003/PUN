using Assets.HeroEditor4D.Common.Scripts.CharacterScripts;
using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour, IPunObservable
{
    [SerializeField] private float moveSpeed = 5f;

    private PhotonView photonView;
    private PlayerInput input;
    private Vector3 lastPosition;
    Character4D Character;
    public bool isMoving;

    void Awake()
    {
        input = GetComponent<PlayerInput>();
        photonView = GetComponent<PhotonView>();
        Character = GetComponent<Character4D>();
    }

    private void Start()
    {
        lastPosition = transform.position;
    }

    void Update()
    {
        if (input != null && photonView.IsMine)
        {
            Vector2 move = input.MovementInput;
            Vector3 moveVector = new Vector3(move.x, move.y, 0);

            isMoving = moveVector != Vector3.zero;


            transform.Translate(moveVector * moveSpeed * Time.deltaTime, Space.World);

            Character.SetDirection(move);
        }
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(isMoving);
        }
        else
        {
            isMoving = (bool)stream.ReceiveNext();
        }
    }
}
