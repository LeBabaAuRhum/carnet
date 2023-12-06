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
    private float rayRange_u = 1f;

    public bool auSol;

    private Vector3 directionToSurface;

    private float chrono = -10f;


    // Start is called before the first frame update
    void Start()
    {
        speed = 0.1f;
        rb = GetComponent<Rigidbody>();
        anim = GetComponent<Animator>();

        anim.SetBool("walk", true);
        anim.SetBool("idle", false);
    }

    // Update is called once per frame
    void Update()
    {
        Debug.DrawRay(transform.position, -transform.up);
    }

    void CheckGround()
    {
        RaycastHit hit_u;
        if(Physics.Raycast(transform.position, -transform.up, out hit_u, rayRange_u))
        {
            if(hit_u.collider.tag == "mur")
            {
                Vector3 directionToSurface = hit_u.point - rb.position;
                Vector3 desiredPosition = rb.position + directionToSurface.normalized * (hit_u.distance - 0.02f); // Ajustez la distance selon vos besoins
                rb.MovePosition(desiredPosition);
            }
        }
    }

    Vector3 WalkForward()
    {
        Vector3 toutDroit;

        toutDroit = rb.position + (transform.forward * speed * Time.deltaTime);

        return toutDroit;
    }

    Quaternion Oscillation()
    {
        float oscill;
        Quaternion targetRotation;
        float lerpSpeed = 0.1f;

        oscill = Mathf.Sin(Time.time * rotationSpeed) * rotationAmplitude;
        // Obtenez la normale de la surface sur laquelle l'insecte se trouve
        Vector3 surfaceNormal = GetSurfaceNormal(); // Vous devez implémenter cette fonction en fonction de votre logique

        // Créer une rotation autour de la normale de la surface
        Quaternion rotationAroundSurfaceNormal = Quaternion.FromToRotation(Vector3.up, surfaceNormal);

        // Appliquer l'oscillation uniquement sur l'axe local transform.right (axe "gauche-droite" de l'insecte)
        Quaternion oscillationRotation = Quaternion.Euler(0f, oscill, 0f);

        // Combiner les rotations pour obtenir la rotation finale
        targetRotation = rotationAroundSurfaceNormal * oscillationRotation;

        // Interpoler linéairement entre la rotation actuelle et la nouvelle rotation
        targetRotation = Quaternion.Lerp(transform.rotation, targetRotation, lerpSpeed);

        return targetRotation;
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

        if(Physics.Raycast(transform.position, -transform.up, out hit_u, rayRange_u))
        {
            
            return false;

        }
        else
        {
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
            Debug.Log("lancement d'un chrono de" + chrono + "secondes");
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
            Debug.Log("Fin du chrono");
            return true;
        }
    }

    void FixedUpdate()
    {
        if(Timer())
        {
           rb.MovePosition(WalkForward());
           rb.MoveRotation(Oscillation()); 
        }
        

        if(CheckWalls())
        {

            Debug.Log("MUUUUR");
        }

        if(CheckPit())
        {
            Debug.Log("TROUUU");
        }

        if(Input.GetKey(KeyCode.E))
        {
            CheckGround();
        }
    }
}
