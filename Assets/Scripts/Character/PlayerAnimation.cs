using Assets.HeroEditor4D.Common.Scripts.CharacterScripts;
using Assets.HeroEditor4D.Common.Scripts.Enums;
using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimation : MonoBehaviour
{
    [SerializeField] private Animator animator;

    private PhotonView photonView;
    private PlayerInput input;
    private Character4D Character;
    private PlayerMovement playerMovement;
    private bool hasMove = false;

    void Awake()
    {
        photonView = GetComponent<PhotonView>();
        input = GetComponent<PlayerInput>();
        Character = GetComponent<Character4D>();
        playerMovement = GetComponent<PlayerMovement>();
    }

    void Update()
    {
        if (photonView.IsMine)
        {
            var moveInput = input.MovementInput;
            if (moveInput == Vector2.zero)
            {
                if (hasMove)
                {
                    Character.AnimationManager.SetState(CharacterState.Idle);
                    hasMove = false;
                }
            }
            else
            {
                Character.AnimationManager.SetState(CharacterState.Run);
                hasMove = true;
            }

            if (Input.GetKeyDown(KeyCode.J))
            {
                Character.AnimationManager.Slash(twoHanded: false);
                photonView.RPC(nameof(Attack), RpcTarget.Others);
            }
        }
        else
        {
            if (playerMovement.isMoving)
            {
                Character.AnimationManager.SetState(CharacterState.Run);
                //Debug.Log("Run");
                //playerMovement.isMoving = true;
            }
            else
            {
                Character.AnimationManager.SetState(CharacterState.Idle);
                //Debug.Log("Idle");
                //playerMovement.isMoving = false;
            }
        }
    }

    [PunRPC]
    private void Attack()
    {
        Character.AnimationManager.Slash(twoHanded: false);
    }
}
