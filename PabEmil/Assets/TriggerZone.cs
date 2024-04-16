using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Splines;
using Cinemachine;

public class TriggerZone : MonoBehaviour
{
    public SplineContainer[] splines;
    public CinemachineVirtualCamera[] cameras;

    public bool isTriggered = false;

    private Movements joueur;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(joueur != null)
        {
            
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "player")
        {
            Debug.Log("ca marche");
            isTriggered = true; 

            joueur = other.gameObject.GetComponent<Movements>();
        }
        

    }
}
