using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    private Slider _slider;

    public float fillSpeed = 1;

    public int direction = 1;
    
    void Start()
    {
        _slider = GetComponent<Slider>();
    }

    void FixedUpdate()
    { 
        _slider.value += Time.deltaTime*fillSpeed*direction;
    }

    public bool IsFull()
    {
        return _slider.value == _slider.maxValue;
    }
    public bool IsEmpty()
    {
        return _slider.value == 0;
    }
}
