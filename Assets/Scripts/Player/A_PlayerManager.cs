using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class A_PlayerManager : MonoBehaviour
{
    [SerializeField] SpriteRenderer sprite_Player;

    PlayerMovement playerMovement;

    void Awake()
    {
        playerMovement = GetComponent<PlayerMovement>();

        AssignEvents();
    }

    void AssignEvents()
    {
        playerMovement.e_LeftMove.AddListener(ChangeLookToLeft);
        playerMovement.e_RightMove.AddListener(ChangeLookToRight);
    }

    void ChangeLookToRight()
    {
        sprite_Player.flipX = false;
    }

    void ChangeLookToLeft()
    {
        sprite_Player.flipX = true;
    }
}
