using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControl : MonoBehaviour
{
    public Vector3 startingPos;
    public Transform posInsect;
    // Start is called before the first frame update
    void Start()
    {
        transform.position = startingPos;
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = new Vector3(posInsect.position.x, startingPos.y, posInsect.position.z);
    }
}
