using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 5f;

    private PhotonView photonView;
    private PlayerInput input;
    public Vector3 CurrentPosition => transform.position;

    void Awake()
    {
        input = GetComponent<PlayerInput>();
        photonView = GetComponent<PhotonView>();
    }

    void Update()
    {
        if (input != null && photonView.IsMine)
        {
            Vector2 move = input.MovementInput; // l?y t? input system c?a b?n
            Vector3 moveVector = new Vector3(move.x, move.y, 0);

            // Di chuy?n
            transform.Translate(moveVector * moveSpeed * Time.deltaTime, Space.World);

            // Animation
            //if (moveVector == Vector3.zero)
            //{
            //    if (_moving)
            //    {
            //        Character.AnimationManager.SetState(CharacterState.Idle);
            //        _moving = false;
            //    }
            //}
            //else
            //{
            //    Character.AnimationManager.SetState(CharacterState.Run);
            //    Character.SetDirection(move.normalized); // ??i h??ng nhìn theo input (trái/ph?i/lên/xu?ng)
            //    _moving = true;
            //}
        }
    }
}
