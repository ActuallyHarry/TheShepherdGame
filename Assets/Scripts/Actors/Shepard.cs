using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Shepard : Actor
{
    public List<ShpdAnimal> animals = new List<ShpdAnimal>();
    bool whistle;   

    new void Begin()
    {
        base.Begin();
    }
    void Update()
    {
        BUpdate();
        whistle = Input.GetMouseButtonUp(1);
        UpdateAnimals();
  
    }

    void UpdateAnimals()
    {
        if (whistle)
        {
            foreach(ShpdAnimal a in animals)
            {
                a.NotifyWhistle(this);
            }
        }
    }

   
}
