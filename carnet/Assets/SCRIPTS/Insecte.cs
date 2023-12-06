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

    private float rotationSpeed = 2f;
    private float rotationAmplitude = 30f;

    //private float rotateFactor;
    //private float lastRotate;

    private float rayRange_f = 0.2f;
    private float rayRange_u = 2f;

    public bool enTrajet = false;
    public bool auSol;
    private bool goodHeight = true;

    public Vector3 maDestination;
    private float forceSurface;
    private float forceFacteur;
    private Vector3 surfaceNormal;
    private Vector3 surfaceSol;

    public int transition;

    public int test = 1;

    // Start is called before the first frame update
    void Start()
    {
        transition = 0;
        rb = GetComponent<Rigidbody>();
        anim = GetComponent<Animator>();
        lastPos = transform.position;
        //lastRotate = 1f;
        anim.SetBool("walk", true);
        anim.SetBool("idle", false);

    }

    // Update is called once per frame
    void Update()
    {
         //Ajustements();
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
        
        if(transition == 0)
        {
            
            
            CheckTransition();
            IsOnGround();
            
        }
        


        /*if (anim.GetCurrentAnimatorStateInfo(0).IsName("walk") && !enTrajet)
        {

           
            
        }
        if (Input.GetKeyDown(KeyCode.E) && !enTrajet)
        {
            enTrajet = true;
        }       
        if(enTrajet)
        {
            RejoindreDestination(maDestination);
        }*/
        


    }

    void FixedUpdate()
    {
        /*if(!enTrajet)
        {
            //WalkingForward();
            //rb.MovePosition(rb.position + (transform.forward*speed*Time.deltaTime) - (forceSurface * forceFacteur));
            //Debug.DrawRay(rb.position, rb.position + (transform.forward*speed*Time.deltaTime) - (forceSurface * forceFacteur) );

        }*/
          //Deplacement(auSol, enTrajet);

        if(transition > 0)
        {
            if(transition == 1)
            {
                if(!goodHeight)
                {
                    goodHeight = true;
                    Debug.Log("on monte");
                    maDestination = rb.position + transform.up * 0.21f;
                    rb.MovePosition(maDestination);
                    
                }
                else
                {
                    Debug.Log("yihi");
                    Quaternion rotation = Quaternion.FromToRotation(transform.up, surfaceNormal);
                    rb.rotation = rotation * transform.rotation;
                    transition = 0;
                    //goodHeight = false;
                }
            }
            else if (transition == 2)
            {
                if(!goodHeight)
                {
                    maDestination = rb.position - transform.up * 0.21f;
                    goodHeight = true;
                    rb.MovePosition(maDestination);
                }
                else
                {
                    Quaternion rotation = Quaternion.FromToRotation(transform.up, transform.forward); // ROTATION POUR "DESCENDRE" UN MUR
                    rb.rotation = rotation * transform.rotation;
                    transition = 0;
                    //goodHeight = false;
                }

            }
            
        }
        else
        {
            goodHeight = false;
            if(!auSol)
            {
                maDestination = rb.position - surfaceSol * 0.05f;
                rb.MovePosition(maDestination); 
            }
            else
            {
                maDestination = rb.position + ( transform.forward * speed * Time.deltaTime);
                float oscillation = Mathf.Sin(Time.time * rotationSpeed) * rotationAmplitude;
                Quaternion targetRotation = Quaternion.Euler(0f, oscillation, 0f);
                rb.MoveRotation(targetRotation);
                rb.MovePosition(maDestination);
            }
        }      
        
        lastPos = transform.position;
    }


    /*void WalkingForward()
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
        Vector3 rotationV = new Vector3 (0,rotateFactor * test,0);
        Quaternion rotation = Quaternion.Euler(rotationV * Time.fixedDeltaTime);
        rb.MoveRotation(rotation * rb.rotation);
        lastRotate = rotateFactor;
        Debug.Log("je marche");
        return;
    }
    

    private void Ajustements()
    {
        RaycastHit hit_f;
        RaycastHit hit_u;

        if(Physics.Raycast(transform.position, transform.forward, out hit_f, rayRange_f)) //ROTATION POUR GRIMPER AU MUR
        {
            if(hit_f.collider.tag == "mur")
            {
                Vector3 surfaceNormal;

                surfaceNormal = hit_f.normal;
                Quaternion rotation = Quaternion.FromToRotation(transform.up, surfaceNormal);
                rb.position += transform.up * 0.2f;
                rb.rotation = rotation * transform.rotation; 
            }
        }
        else
        {
            if(Physics.Raycast(transform.position, -transform.up, out hit_u, rayRange_u))  // SE RAPPROCHER DU SOL
            {
                if(hit_u.collider.tag == "mur" && hit_u.distance > 0.02f)
                {
                    //Vector3 surfaceNormal;

                    forceSurface = hit_u.normal;
                    //forceFacteur = hit_u.distance - 0.02f;
                    //rb.AddForce(-surfaceNormal * 10f, ForceMode.Force);
                    auSol = false;

                }
                else
                {
                    auSol = true;
                }
            }
            else
            {
                Quaternion rotation = Quaternion.FromToRotation(transform.up, transform.forward); // ROTATION POUR "DESCENDRE" UN MUR
                rb.rotation = rotation * transform.rotation;
            }
            
        }
    }


        void RejoindreDestination(Vector3 destination)
        {
            rb.velocity = new Vector3(0,0,0);
            if(transform.position == destination)
            {
                enTrajet = false;
            }
            else
            {
                if(Mathf.Round(destination.y) == Mathf.Round(transform.position.y)) // A REFAIRE EN MIEUX
                {
                    RaycastHit hit;

                    Debug.DrawRay(transform.position, destination - transform.position);

                    if(Physics.Raycast(transform.position, destination - transform.position, out hit, Vector3.Distance(transform.position, destination)))
                    {
                        if(hit.collider.tag =="mur")
                        {
                            Debug.Log("obstacle");
                        }
                        else
                        {
                            Debug.Log("pas d'obstacle");

                            float t = 0.5f * Time.deltaTime;
                            Vector3 positionInterpolee = Vector3.Lerp(transform.position, destination, t);

                            rb.MovePosition(positionInterpolee);
                        }
                        
                    }
                    else
                    {
                        Debug.Log("pas d'obstacle2");

                        float t = 0.5f * Time.deltaTime;
                        Vector3 positionInterpolee = Vector3.Lerp(transform.position, destination, t);

                        rb.MovePosition(positionInterpolee);
                    }
                }
                else
                {
                    Debug.Log("Mauvais étage");
                }
            }
           
           return;
        }


        private void Deplacement(bool auSol, bool enTrajet)
        {
            if(!auSol)
            {
                Debug.Log("pas au sol");
                rb.MovePosition(transform.position - (forceSurface * 0.1f));
            }
            else
            {
                if(!enTrajet)
                {
                    rb.MovePosition(transform.position + (transform.forward * speed * Time.deltaTime));
                }
            }
        }

*/
        private void IsOnGround()
        {
            RaycastHit hit_u;
            Debug.DrawRay(transform.position, -transform.up);
            if(Physics.Raycast(transform.position, -transform.up, out hit_u, rayRange_u))  // SE RAPPROCHER DU SOL
            {
                if(hit_u.collider.tag == "mur" && hit_u.distance > 0.051f)
                {
                    surfaceSol = hit_u.normal;
                    forceSurface = hit_u.distance + 0.02f;
                    auSol = false;
                }
                else
                {
                    auSol = true;
                }
            }
            else
            {
                if(transition == 0)
                {
                  transition = 2;
                    auSol = false;  
                }
                
            }
        }

        private void CheckTransition()
        {
            RaycastHit hit_f;
            Debug.DrawRay(transform.position, transform.forward);
            if(Physics.Raycast(transform.position, transform.forward, out hit_f, rayRange_f)) //ROTATION POUR GRIMPER AU MUR
            {
                if(hit_f.collider.tag == "mur" && auSol)
                {
                    surfaceNormal = hit_f.normal;
                    transition = 1;
                }
            }
        }
}

    
