using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraBehaviour : MonoBehaviour
{
    [SerializeField] 
    Transform player;

    [SerializeField]
    float offset;

    void Update()
    {
        transform.position = new Vector3(player.position.x, player.position.y + offset, transform.position.z);
    }
}
