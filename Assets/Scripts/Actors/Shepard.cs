using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Shepard : Actor
{
    public List<ShpdAnimal> animals = new List<ShpdAnimal>();
    bool whistleButtonDown;
    bool whistled = false;
    float whistleTimer;
    public float whistleTime = 5; // time that whistle will be togglable until reset;

    public override void Begin()
    {
        currentMoveBehaviour = moveBehaviourOptions[0];
        base.Begin();
    }
    void Update()
    {
        BUpdate();
        whistleButtonDown = Input.GetMouseButtonUp(1);
        UpdateAnimals();
        WTimer();
  
    }

    void WTimer()
    {
        whistleTimer += Time.deltaTime;
        if(whistleTimer > whistleTime)
        {
            whistled = false;
        }
    }

    void UpdateAnimals()
    {
        if (whistleButtonDown)
        {
            whistleTimer = 0;            
            foreach (ShpdAnimal a in animals)
            {
                a.NotifyWhistle(!whistled);
            }
            whistled = !whistled;

        }
    }

   
}
