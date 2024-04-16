using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Splines;

public class Movements : MonoBehaviour
{
    //Composants du perso
    private Animator animator;
    private CharacterController charControl;

    //variables de déplacement
    [SerializeField] private float _moveSpeed = 5f; 
    private bool _isWalking = false;
    private Vector3 _lastPos;
    private float _pourcentageRoute;
    private float _longueurRoute;

    //Route(s) à suivre
    public SplineContainer spline;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        charControl = GetComponent<CharacterController>();

        transform.position = spline.EvaluatePosition(0);
        _lastPos = transform.position;
        _pourcentageRoute = 0;
        _longueurRoute = spline.CalculateLength();
    }

    // Update is called once per frame
    void Update()
    {
        handleMovement();
        HandleAnimation();
    }

    void handleMovement()
    {
        float x = Input.GetAxisRaw("Horizontal");
        //float y;

        _pourcentageRoute += x * _moveSpeed * Time.deltaTime / _longueurRoute;
        Vector3 cible = spline.EvaluatePosition(_pourcentageRoute);
        Vector3 destination = cible - _lastPos;
        

        //Debug.Log("je suis en " + _lastPos + "je veux aller en " + cible + "je vais donc me déplacer de " + destination);

        charControl.Move(destination);
        _lastPos = transform.position;

        if(x != 0)
        {
            Vector3 nextDestination = spline.EvaluatePosition(_pourcentageRoute + x * 0.01f);
            Vector3 direction = nextDestination - cible;
            transform.rotation = Quaternion.LookRotation(direction, transform.up);
            if(!_isWalking) {_isWalking = true;}
        }
        else
        {
            _isWalking = false;
        }
    }

    void HandleAnimation()
    {
        bool isWalking = animator.GetBool("IsWalking");

        if(_isWalking && !isWalking)
        {
            animator.SetBool("IsWalking", true);
        }
        else if (!_isWalking && isWalking)
        {
            animator.SetBool("IsWalking", false);
        }
    }

}
