using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public Vector3 dir;
    public float speed = 5;
    void Update()
    {
        GetComponent<Rigidbody2D>().MovePosition(transform.position+dir*Time.deltaTime*speed);
        //transform.position += ;
    }
}
