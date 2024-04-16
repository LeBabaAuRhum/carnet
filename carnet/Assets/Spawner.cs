using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public GameObject insect;

    public float    x;
    public float    y; 
    public float    z;  
    // Start is called before the first frame update
    void Start()
    {
        y = -180f;
        z = 90f;
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space))
        {
            Vector3 spawn;
            spawn = new Vector3(0.19f, Random.Range(0.3f, 4.3f), Random.Range(9f, 20.5f));
            x = Random.Range(0,360);
            transform.eulerAngles = new Vector3(x, y, z); 
            Instantiate(insect, spawn, transform.rotation);
        }
    }
}
