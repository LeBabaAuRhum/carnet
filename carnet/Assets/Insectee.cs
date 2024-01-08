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

    public int surfaceCible;
    private Surface surfaceActuelle;

    public Vector3 directionSuivie;

    private bool aRetirer;//A RETIRER

    string layerName = "murs";  // Remplacez YourLayerName par le nom de la couche que vous souhaitez détecter
    int layerMask;

    // Start is called before the first frame update
    void Start()
    {

        aRetirer = false;//A RETIRER

        layerMask  = 1 << LayerMask.NameToLayer(layerName);
        speed = 2f;
        rb = GetComponent<Rigidbody>();
        anim = GetComponent<Animator>();
        chrono = -10f;

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
                rb.position = desiredPosition;
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
                    rb.position = rb.position + (transform.up * 0.075f);
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

        if(Physics.Raycast(transform.position, -transform.up, out hit_u, rayRange_u, layerMask))
        {
            surfaceActuelle = hit_u.transform.gameObject.GetComponent<Surface>();
            return false;
        }
        else
        {
            Quaternion rotation = Quaternion.FromToRotation(transform.up, transform.forward);
            rb.position = rb.position + (transform.forward * 0.075f) + (-transform.up * 0.075f);
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

    bool sontParalleles(Vector3 vecteur1, Vector3 vecteur2)
    {
        float produitScalaire = Vector3.Dot(vecteur1.normalized, vecteur2.normalized);
        float margeErreur = 0.99f;
        return Mathf.Abs(produitScalaire) > margeErreur;
    }

    void DeplacementInsecteEnTrajet(Vector3 directionCible)
    {
        if(sontParalleles(transform.forward, directionCible))
        {
            rb.MovePosition(WalkForward(speed));
        }
        else
        {
            Quaternion rotationCible = Quaternion.LookRotation(directionCible, transform.up);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, rotationCible, Time.deltaTime * 180f);
        }
    }

    void FixedUpdate()
    {
        
        if(CheckWalls())
        {
            if(destination != Quaternion.Euler(0,0,0))
            {
                destination = Quaternion.Euler(0,0,0);
            }
            //rb.position += transform.forward * 0.5f;
            //CheckGround();
        }
        else if (CheckPit())
        {
            if(destination != Quaternion.Euler(0,0,0))
            {
                destination = Quaternion.Euler(0,0,0);
            }
            //rb.position += transform.up * 0.5f;
            //CheckGround();

        }
        else
        {
            /*if(destination == Quaternion.Euler(0,0,0))
            {
                
                rb.MoveRotation(Oscillation());
                if(Timer())
                {
                    SetDestination();
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
                    CheckGround();
                    rb.MovePosition(WalkForward(speed));*/

            CheckGround();

            if(surfaceActuelle.monIndex == surfaceCible)
            {
                Debug.Log("ARRIVE");
            }
            if(Input.GetKeyDown(KeyCode.X))
            {
                directionSuivie = surfaceActuelle.SetDestination(surfaceCible);
                aRetirer = true;
                Debug.Log("Je me dirige vers " + surfaceCible + "en suivant le vecteur" + directionSuivie);
            }

            if(aRetirer)
            {
                DeplacementInsecteEnTrajet(directionSuivie);
            }
   
        }

    }
}
