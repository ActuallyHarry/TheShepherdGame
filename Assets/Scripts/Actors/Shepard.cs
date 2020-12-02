using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Shepard : Actor
{
    public List<ShpdAnimal> animals;
    bool whistle;   

    new void Start()
    {
        base.Start();
    }
    new void Update()
    {
        base.Update();
        whistle = Input.GetMouseButtonUp(1);
        UpdateAnimals();
        currentTile = ReturnCurrentTile();
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
