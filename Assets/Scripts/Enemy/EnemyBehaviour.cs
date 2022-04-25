using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBehaviour : MonoBehaviour
{
    void OnCollisionEnter2D(Collision2D collision)
    {
        float colDif = collision.transform.position.y - 0.06f;
        float myDif = transform.position.y + 0.06f;

        if (colDif < myDif)
            Destroy(collision.gameObject);
        else
            Destroy(gameObject);
    }
}
