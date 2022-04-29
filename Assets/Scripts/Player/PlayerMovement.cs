using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public enum Direction 
{
    Left = -1,
    Right = 1
}

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] LayerMask lm_Terrain;
    [SerializeField] List<Transform> pivots;

    [HideInInspector] public UnityEvent e_LeftMove = new UnityEvent();
    [HideInInspector] public UnityEvent e_RightMove = new UnityEvent();

    PlayerStats stats;
    Rigidbody2D rig2D;
    BoxCollider2D boxCollider2D;
    Direction moveDirection = Direction.Right;
    bool isGrounded = false;
    bool isMoving = true;
    float jumpForceCharge;


    void Awake()
    {
        stats = GetComponent<PlayerStats>();
        rig2D = GetComponent<Rigidbody2D>();
        boxCollider2D = GetComponent<BoxCollider2D>();
    }

    void Update()
    {
        Move();
        Jump();
    }

    void OnDrawGizmos()
    {
        Gizmos.DrawCube(transform.position + new Vector3(0, -1.8f, 0), new Vector3(3.5f, 1f, 0.5f));
        Gizmos.DrawCube(transform.position + new Vector3(1.8f, 0, 0), new Vector3(0.5f, 3.2f, 0.5f));
        Gizmos.DrawCube(transform.position + new Vector3(-1.8f, 0, 0), new Vector3(0.5f, 3.2f, 0.5f));
    }

    void Move()
    {
        Collider2D coll1 = Physics2D.OverlapBox(
            transform.position + new Vector3(-1.8f, 0, 0),
            new Vector2(0.5f, 3.2f), 
            0, 
            lm_Terrain
        );
        Collider2D coll2 = Physics2D.OverlapBox(
            transform.position + new Vector3(1.8f, 0, 0), 
            new Vector2(0.5f, 3.2f), 
            0, 
            lm_Terrain
        );

        float x = Input.GetAxis("Horizontal") * stats.velocity * Time.deltaTime;

        if (coll1)
            if (x < 0)
                x = 0;
        if (coll2)
            if (x > 0)
                x = 0;

        if (x > 0)
        {
            moveDirection = Direction.Right;
            e_RightMove.Invoke();
        }
        if (x < 0)
        {
            moveDirection = Direction.Left;
            e_LeftMove.Invoke();
        }

        if (isGrounded && isMoving)
        {
            rig2D.AddForce(new Vector2(x, 0), ForceMode2D.Impulse);
        }
    }

    void Jump()
    {
        Collider2D coll1 = Physics2D.OverlapBox(
            transform.position + new Vector3(0, -1.8f, 0),
            new Vector2(3.5f, 1f),
            0,
            lm_Terrain
        );

        if (coll1)
        {
            isGrounded = true;
        }
        else
        {
            isGrounded = false;
        }

        if (isGrounded && Input.GetKeyDown(KeyCode.Space))
        {
            isMoving = false;
        }

        if (isGrounded && Input.GetKey(KeyCode.Space))
        {
            if (jumpForceCharge < 1f)
                jumpForceCharge += Time.deltaTime;
        }

        if (isGrounded && Input.GetKeyUp(KeyCode.Space))
        {
            float distance = 0;
            Vector2 direction = new Vector2();
            Vector2 force = new Vector2();
            switch (moveDirection)
            {
                case Direction.Right:
                    Vector2 rightJump = Vector2.Lerp(pivots[0].position, pivots[1].position, jumpForceCharge);
                    distance = Vector2.Distance(transform.position, rightJump);
                    direction = (new Vector2(transform.position.x, transform.position.y) - rightJump).normalized;
                    force = direction * distance * stats.jumpForce;
                    break;
                case Direction.Left:
                    Vector2 leftJump = Vector2.Lerp(pivots[2].position, pivots[3].position, jumpForceCharge);
                    distance = Vector2.Distance(transform.position, leftJump);
                    direction = (new Vector2(transform.position.x, transform.position.y) - leftJump).normalized;
                    force = direction * distance * stats.jumpForce;
                    break;
            }
            rig2D.AddForce(force, ForceMode2D.Impulse);
            isMoving = true;
            jumpForceCharge = 0;
        }
    }
}
