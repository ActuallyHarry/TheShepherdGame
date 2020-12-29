using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Food : Item
{
    public float nourishementAmount = 100f;
    float timer = 0;

    private void Update()
    {
        //sometimes sheep re interupted and so the plant is listed as taken when not actually taken
        if (taken)
        {
            timer += Time.deltaTime;
            if(timer > 10)
            {
                timer = 0;
                taken = false;
            }
        }
    }

}
