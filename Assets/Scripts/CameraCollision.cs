using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class CameraCollision : MonoBehaviour
{
    private Transform player;

    private void Awake()
    {
        player = transform.parent;
    }

    private void Update()
    {
        RaycastHit hit;

        Physics.Raycast(player.position, -transform.forward, out hit
            , 5f);

        if(hit.collider != null)
        {
            if (hit.collider.gameObject.layer == LayerMask.NameToLayer("Obstacles"))
            {
                transform.position = hit.point + transform.forward * 0.1f; 
            }
        }
        else
        {
            float playerCamera = Vector3.Distance(player.position, transform.position);

            if (playerCamera < 5f)
            {
                transform.position = Vector3.MoveTowards(transform.position, player.position - transform.forward *5f , 1f);
            }
        }

        transform.LookAt(player.position);
    }
}
