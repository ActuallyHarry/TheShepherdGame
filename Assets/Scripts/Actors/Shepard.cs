using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Shepard : Actor
{
    public Animator anim;

    public List<ShpdAnimal> animals = new List<ShpdAnimal>();
    bool whistleButtonDown;
    bool isWhistling= false; // for animation duration
    bool whistled = false; //for toggling whistle
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
        anim.SetBool("isMoving", isMoving);
    }

    void WTimer()
    {
        whistleTimer += Time.deltaTime;

        if (isWhistling)
        {
            if (whistleTimer > anim.GetCurrentAnimatorStateInfo(0).normalizedTime + anim.GetCurrentAnimatorStateInfo(0).length)
            {
                isWhistling = false;
                anim.SetBool("isWhistling", isWhistling);
            }
        }       
        if(whistleTimer > whistleTime)
        {
            isWhistling = false;
            anim.SetBool("isWhistling",isWhistling);
            whistled = false;
        }
    }

    void UpdateAnimals()
    {
        if (whistleButtonDown)
        {
            isWhistling = !whistled;
            anim.SetBool("isWhistling", isWhistling);
            whistleTimer = 0;            
            foreach (ShpdAnimal a in animals)
            {
                a.NotifyWhistle(!whistled);
            }
            whistled = !whistled;

        }
    }

   
}
