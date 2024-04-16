using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovements : MonoBehaviour
{
    private Rigidbody rb;
    [SerializeField] private float moveSpeed;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        float dirMouvement = Input.GetAxisRaw("Horizontal");
        rb.velocity = new Vector3(dirMouvement * moveSpeed * Time.deltaTime, rb.velocity.y, rb.velocity.z);
    }
}
