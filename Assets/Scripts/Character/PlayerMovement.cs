using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 5f;

    private PlayerInput input;
    public Vector3 CurrentPosition => transform.position;

    void Awake()
    {
        input = GetComponent<PlayerInput>();
    }

    void Update()
    {
        if (input != null)
        {
            Vector3 move = new Vector3(input.MovementInput.x, input.MovementInput.y, 0);
            transform.Translate(move * moveSpeed * Time.deltaTime, Space.World);

            if (move != Vector3.zero)
                transform.up = move;
        }
    }
}
