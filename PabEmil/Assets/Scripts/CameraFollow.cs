using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{

    public Transform player;
    [SerializeField] private float smoothSpeed;
    [SerializeField] private Vector3 offset;
    // Start is called before the first frame update
    void Start()
    {
        smoothSpeed = 10f;
    }

    // Update is called once per frame
    void LateUpdate()
    {
        Vector3 desiredPosition = player.position + offset;
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed * Time.deltaTime);

        transform.position = smoothedPosition;
        transform.LookAt(player);
    }
}
