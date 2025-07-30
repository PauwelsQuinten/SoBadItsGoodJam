using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class CameraCollision : MonoBehaviour
{
    private Transform player;
    private Vector3 _offset;
    private float _initDistanceToPlayer;

    [SerializeField]
    private float _cameraMoveSpeed = 5f;

    private void Awake()
    {
        player = transform.parent;
        _offset = transform.position - player.position;// If this does work try with an offset
        _initDistanceToPlayer = Vector3.Distance(player.localPosition, transform.localPosition);
    }

    private void Update()
    {
        //Ray ray = new Ray(transform.position, offset);
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
                //transform.position = Vector3.Lerp(transform.position, player.position + _offset
                //    , _cameraMoveSpeed/2 * Time.deltaTime);

                transform.position = Vector3.MoveTowards(transform.position, player.position - transform.forward *5f , 1f);
            }
        }



        transform.LookAt(player.position);
    }
}
