using UnityEngine;

public class TopDownCam : MonoBehaviour
{
    [SerializeField]
    private Transform playerTransform;
    [SerializeField]
    private Vector3 _offset = new Vector3(0f,10f,0f);
    [SerializeField]
    private float _smoothTime = 0.3f;

    private Vector3 _playerPosition;
    private Vector3 velocity = Vector3.zero;

    private void LateUpdate()
    {
        if (playerTransform == null) return;

        Vector3 targetPosition = playerTransform.position + _offset;
        transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref velocity, _smoothTime);
    }
}
