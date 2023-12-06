using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Insectee : MonoBehaviour
{

    private Rigidbody rb;
    private Animator anim;

    public float speed = 0.1f;

    private float rotationSpeed = 4f;
    private float rotationAmplitude = 15f;

    private float rayRange_f = 0.2f;
    private float rayRange_u = 1.5f;

    public bool auSol;
    public Transform origin;

    private Vector3 directionToSurface;

    private float chrono = -10f;

    private Quaternion destination;

    string layerName = "murs";  // Remplacez YourLayerName par le nom de la couche que vous souhaitez détecter
    int layerMask;

    // Start is called before the first frame update
    void Start()
    {
        layerMask  = 1 << LayerMask.NameToLayer(layerName);
        speed = 2f;
        rb = GetComponent<Rigidbody>();
        anim = GetComponent<Animator>();

        anim.SetBool("walk", true);
        anim.SetBool("idle", false);

        CheckGround();
        destination = Quaternion.Euler(0,0,0);
    }

    // Update is called once per frame
    void Update()
    {
       // Debug.DrawRay(transform.position, -transform.up);
    }

    void CheckGround()
    {
        RaycastHit hit_u;
        if(Physics.Raycast(transform.position, -transform.up, out hit_u, rayRange_u))
        {
            if(hit_u.collider.tag == "mur")
            {
                Vector3 directionToSurface = hit_u.point - rb.position;
                Vector3 desiredPosition = rb.position + directionToSurface.normalized * (hit_u.distance - 0.02f);
                rb.MovePosition(desiredPosition);
            }
        }
    }

    Vector3 WalkForward(float vitesse)
    {
        Vector3 toutDroit;

        toutDroit = rb.position + (transform.forward * vitesse * Time.deltaTime);

        return toutDroit;
    }

    Quaternion Oscillation()
    {
        float oscill;
        Quaternion targetRotation;
        float lerpSpeed = 0.1f;

        oscill = Mathf.Sin(Time.time * rotationSpeed) * rotationAmplitude;
    
        Vector3 surfaceNormal = GetSurfaceNormal();

        // Créer une rotation autour de la normale de la surface
        Quaternion rotationAroundSurfaceNormal = Quaternion.FromToRotation(Vector3.up, surfaceNormal);

        // Appliquer l'oscillation uniquement sur l'axe local
        Quaternion oscillationRotation = Quaternion.Euler(0f, oscill, 0f);

        // Combiner les rotations pour obtenir la rotation finale
        targetRotation = rotationAroundSurfaceNormal * oscillationRotation;

        // Interpoler linéairement entre la rotation actuelle et la nouvelle rotation
        targetRotation = Quaternion.Lerp(transform.rotation, targetRotation, lerpSpeed);

        return targetRotation;
    }

    void SetDestination()
    {
        float randomAngle;

        randomAngle = Random.Range(1f, 360f);

        Vector3 surfaceNormal = GetSurfaceNormal();
        Quaternion rotationAroundSurfaceNormal = Quaternion.FromToRotation(Vector3.up, surfaceNormal);
        Quaternion rorotation = Quaternion.Euler(0f, randomAngle, 0f);

        destination = rotationAroundSurfaceNormal * rorotation;
        return;
    }

    bool CheckRotation(Quaternion target)
    {
        if(Quaternion.Angle(transform.rotation, target) < 0.1f)
        {
            return false;
        }
        else
        {
            return true;
        }
    }

    void RotateTowardDestination(Quaternion target)
    {
         float lerpSpeed = 0.1f;

         target = Quaternion.Lerp(transform.rotation, target, lerpSpeed);
         rb.MoveRotation(target);


    }

    bool CheckWalls()
    {
        RaycastHit hit_f;

        if(Physics.Raycast(transform.position, transform.forward, out hit_f, rayRange_f))
        {
            if(hit_f.collider.tag == "mur")
            {
                Quaternion rotation = Quaternion.FromToRotation(transform.up, hit_f.normal);
                    rb.rotation = rotation * transform.rotation;

                return true;
            }
            else
            {
                return false;
            }
        }
        else
        {
            return false;
        }
    }


    bool CheckPit()
    {
        RaycastHit hit_u;

        if(Physics.Raycast(origin.position, -transform.up, out hit_u, rayRange_u, layerMask))
        {
            Debug.Log(hit_u.collider.gameObject.name);
            return false;

        }
        else
        {
            Quaternion rotation = Quaternion.FromToRotation(transform.up, transform.forward);
            rb.rotation = rotation * transform.rotation;
            return true;
        }
    }

    Vector3 GetSurfaceNormal()
    {
        RaycastHit hit_u;

        if(Physics.Raycast(transform.position, -transform.up, out hit_u, rayRange_u))
        {  
            return hit_u.normal;
        }
        else
        {
            return new Vector3(0,0,0);
        }
    }

    bool Timer()
    {
        if(chrono == -10f)
        {
            chrono = Random.Range(1f, 5f);
            return false;
        }
        else if(chrono > 0)
        {
            chrono -= Time.deltaTime;
            return false;
        }
        else
        {
            chrono = -10;
            return true;
        }
    }

    void FixedUpdate()
    {
        /*if(Timer())
        {
           rb.MovePosition(WalkForward());
           rb.MoveRotation(Oscillation()); 
        }*/

        if(Input.GetKeyDown(KeyCode.A))
        {
            CheckPit();
        }

        if(Input.GetKey(KeyCode.Z))
        {
            RotateTowardDestination(destination);
            Debug.Log(CheckRotation(destination));
        }
        

        if(CheckWalls())
        {
            if(destination != Quaternion.Euler(0,0,0))
            {
                destination = Quaternion.Euler(0,0,0);
            }
            rb.MovePosition(WalkForward(speed*10));
            CheckGround();
        }
        else if (CheckPit())
        {
            if(destination != Quaternion.Euler(0,0,0))
            {
                destination = Quaternion.Euler(0,0,0);
            }
            rb.MovePosition(WalkForward(speed*10));
            CheckGround();

        }
        else
        {
            if(destination == Quaternion.Euler(0,0,0))
            {
                rb.MoveRotation(Oscillation());
                if(Timer())
                {
                    SetDestination();
                    CheckGround();
                    return;
                }
            }
            else
            {
                RotateTowardDestination(destination);
                if(!CheckRotation(destination))
                {
                    destination = Quaternion.Euler(0,0,0);
                }
            }
                        rb.MovePosition(WalkForward(speed));
   
        }

    }
}
