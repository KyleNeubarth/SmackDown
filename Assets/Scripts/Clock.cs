using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Clock : MonoBehaviour
{
    public GameObject hand;
    public float speed;

    private void Start()
    {
        if (!hand)
        {
            hand = gameObject;
        }
    }

    void Update()
    {
        hand.transform.Rotate(0,0,Time.deltaTime*speed);
    }
}
