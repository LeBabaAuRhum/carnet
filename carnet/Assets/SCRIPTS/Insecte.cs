using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Insecte : MonoBehaviour
{
    private Rigidbody rb;
    private Animator anim;
    public float speed = 20f;
    private Vector3 walkSpeed;
    private Vector3 lastPos;
    private float rotateFactor;
    private float lastRotate;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        anim = GetComponent<Animator>();
        lastPos = transform.position;
        lastRotate = 1f;

    }

    // Update is called once per frame
    void Update()
    {

        if (anim.GetCurrentAnimatorStateInfo(0).IsName("idle"))
        {
            //print("idle");
            anim.SetBool("takeoff", false);
            anim.SetBool("landing", false);
            anim.SetBool("fly", false);
            anim.SetBool("hit", false);
        }

        if(lastPos == transform.position)
        {
           anim.SetBool("idle", true);
            anim.SetBool("walk", false);
            anim.SetBool("turnleft", false);
            anim.SetBool("turnright", false);
            anim.SetBool("fly", true);
            anim.SetBool("flyleft", false);
            anim.SetBool("flyright", false); 
        }
        else
        {
            anim.SetBool("walk", true);
            anim.SetBool("idle", false);
            anim.speed = speed;
        }
        lastPos = transform.position;

        if (anim.GetCurrentAnimatorStateInfo(0).IsName("walk"))
        {
            WalkingForward();
        }       
        
        


    }

    void FixedUpdate()
    {
        walkSpeed = (transform.TransformDirection(new Vector3(0, 0, speed)));
        rb.velocity = walkSpeed;
    }


    void WalkingForward()
    {
        if(lastRotate >= 0)
        {
            if(lastRotate >= 2.5f)
            {
                rotateFactor = Random.Range(-5f, -2.5f);
            }
            else
            {
                rotateFactor = Random.Range(-5f,0);

            }
        }
        else
        {
            if(lastRotate <= -2.5f)
            {
                rotateFactor = Random.Range(5f, 2.5f);
            }
            else
            {
                rotateFactor = Random.Range(5f,0);

            }          
        }
        transform.Rotate(0,rotateFactor,0);
        print(rotateFactor);
        lastRotate = rotateFactor;
    }
    
    void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "mur")
        {
            transform.Rotate(-90f,0,0);
        }
    }
}
