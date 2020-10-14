using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Destructable : MonoBehaviour
{

    public bool interactable = true;
    
    private void OnCollisionEnter(Collision other)
    {
        Debug.Log("hit");
        
        if ( interactable && other.collider.CompareTag("dangerous"))
        {
            //get destroyed!
            Debug.Log("ouch!");

            Bullet obj = ((GameObject)Instantiate(Resources.Load("Bullet"), transform.position, Quaternion.identity)).GetComponent<Bullet>();
            obj.dir = Vector3.left;
            interactable = false;
            transform.position += transform.forward * .4f;
            GetComponent<Collider2D>().enabled = false;
        }
    }
}
