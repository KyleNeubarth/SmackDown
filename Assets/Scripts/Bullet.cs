using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public Vector3 dir;
    public float speed = 5;
    public int comboNum = 1;
    private Rigidbody rb;

    public float age = 0;
    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        rb.velocity = dir*Time.deltaTime*speed*10;
        //transform.position += ;
        age += Time.deltaTime;
        if (age > 10)
            Destroy(gameObject);
    }

    private void OnCollisionEnter(Collision other)
    {
        Destroy(gameObject);
    }
}
