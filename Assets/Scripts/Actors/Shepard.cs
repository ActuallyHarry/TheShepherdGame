using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Shepard : Actor
{
    public List<ShpdAnimal> animals = new List<ShpdAnimal>();
    bool whistle;   

    public override void Begin()
    {
        currentMoveBehaviour = moveBehaviourOptions[0];
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
