using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    private float _h;
    private float _v;
    private bool _jump;
    [Header("Char attributes")]
    public float moveSpeed = 1;


    public Material hurtMat;
    
    public TextMeshProUGUI healthUI;
    public TextMeshProUGUI scoreUI;
    public int health = 3;
    public int score = 0;
    public float iFrames = 0;
    public float iTime = 1;
    
    [Space]

    [Header("Jumping")] 
    public JumpState jumping = JumpState.GROUNDED;

    private float iTimer = 0;
    
    [Space]
    
    [Header("References")]
    private Rigidbody _rb;
    public HealthBar healthBar;
    private GameObject _target;
    private Rigidbody _trb;
    private GameObject _shockwave;
    private MeshRenderer _sRenderer;
    //private GameObject _jumpParabola;
    private ParabolaController pController;
    private Transform _pStart;
    private Transform _pMid;
    private Transform _pEnd;

    private BoxCollider _collider;

    private float jumpTime = 0;

    public void Start()
    {
        _rb = GetComponent<Rigidbody>();
        _target = GameObject.Find("JumpTarget").gameObject;
        _trb = _target.GetComponent<Rigidbody>();
        _target.SetActive(false);
        _shockwave = transform.Find("Shockwave").gameObject;
        _sRenderer = _shockwave.GetComponent<MeshRenderer>();
        _shockwave.SetActive(false);
        pController = GetComponent<ParabolaController>();
        Transform _jumpParabola = GameObject.Find("JumpParabola").transform;
        _pStart = _jumpParabola.GetChild(2);
        _pMid = _jumpParabola.GetChild(1);
        _pEnd = _jumpParabola.GetChild(0);

        _collider = GetComponent<BoxCollider>();
    }
    public void Update()
    {
        _h = Input.GetAxis("Horizontal");
        _v = Input.GetAxis("Vertical");
        if (!_jump)
        {
            _jump = Input.GetButton("Jump");
        }
        scoreUI.text = "Score:" + score;
        healthUI.text = "Health:" + health;
    }

    public void FixedUpdate()
    {
        if (iFrames >= 0)
        {
            iFrames -= Time.deltaTime;
        }

        if (health == 0)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }

        if (jumping == JumpState.GROUNDED)
        {
            _rb.velocity = new Vector2(_h, _v).normalized * moveSpeed;
            
            //transform.LookAt(transform.position+new Vector3(_h, _v,0).normalized * moveSpeed);
            //proceed with jump
            if (_jump && healthBar.IsFull())
            {
                _target.SetActive(true);
                _target.transform.position = transform.position + Vector3.forward * .45f;
                _rb.velocity = Vector2.zero;
                healthBar.direction = -2;
                Time.timeScale = .5f;
                jumping = JumpState.PLANNING;
            }
        }
        else if (jumping == JumpState.PLANNING)
        {
            _trb.velocity = new Vector2(_h, _v).normalized * moveSpeed;

            //end the jump
            if (!_jump || healthBar.IsEmpty())
            {
                Debug.Log("Starting the jump!");
                _pStart.position = transform.position;
                _pMid.position = transform.position + .5f * (_target.transform.position +Vector3.back*.499f - transform.position)+Vector3.back*5;
                _pEnd.position = _target.transform.position+Vector3.back*.499f;
                pController.Speed = 12;
                pController.FollowParabola();
                _trb.velocity = Vector2.zero;
                healthBar.direction = 0;

                _collider.enabled = false;
                
                jumping = JumpState.JUMPING;
                Time.timeScale = 1;
                jumpTime = 0;
            }
        }
        else if (jumping == JumpState.JUMPING)
        {
            if (!pController.Animation)
            {
                Debug.Log("Ending the Jump!");
                _target.SetActive(false);
                transform.position = new Vector3(transform.position.x,transform.position.y,0);
                healthBar.direction = 1;
                //pController.Speed = 5;
                _shockwave.SetActive(true);
                iTimer = 1;
                
                jumping = JumpState.ENDING;
                
                //did I land on solid ground?
                RaycastHit hit;
                if (Physics.Raycast(transform.position,Vector3.forward,out hit, 1,LayerMask.GetMask("Environment")))
                {
                    Debug.Log("solid ground!");
                }
                else
                {
                    Debug.Log("falling!!");
                }
            }
            else
            {
                jumpTime += Time.deltaTime;
                /*if (pController.Speed == 5 && pController.animationTime > pController.GetDuration()*1f/3f)
                {
                    pController.Speed = 12;
                }*/
            }
        }
        else if (jumping == JumpState.ENDING)
        {
            transform.position = new Vector3(transform.position.x,transform.position.y,0);
            iTimer -= Time.deltaTime;
            Debug.Log(jumpTime);
            _rb.velocity = Vector3.zero;
            _sRenderer.material.color = new Color(_sRenderer.material.color.r,_sRenderer.material.color.g,_sRenderer.material.color.b,iTimer);
            if (iTimer <= 0)
            {
                _collider.enabled = true;
                _shockwave.SetActive(false);
                jumping = JumpState.GROUNDED;
            }
        }


        _jump = false;
    }

    private void OnCollisionEnter(Collision other)
    {
        if (jumping != JumpState.ENDING && (other.collider.CompareTag("enemy") || other.collider.CompareTag("dangerous")) && iFrames <= 0)
        {
            Debug.Log("Ouch");
            health--;
            iFrames = iTime;
        }
    }


    public enum JumpState
    {
        GROUNDED,
        PLANNING,
        JUMPING,
        ENDING
    }
}
