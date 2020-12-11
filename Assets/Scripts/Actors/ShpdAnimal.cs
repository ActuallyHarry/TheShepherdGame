using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShpdAnimal : Actor
{
    public enum State
    {
        Dawdle,
        FollowShepard,
        FindFood
    }
    public State state;

    float attentionTime;
    float aTimer =0;

    public override  void Begin()
    {
        base.Begin();
    }
    public void SetShepard(Shepard _shpd)
    {
        leader = _shpd;
    }

    void Update()
    {
      
        //Debug.Log(target);
        switch (state)
        {
            case State.Dawdle:
                break;
            case State.FollowShepard:
                break;
            case State.FindFood:
                break;
        }

       


        BUpdate();
    }
    void OnHungry()
    {
        //Debug.Log("Hunfry");
        List<Transform> grass = filter.FilterContext(ItemsInView, "Plant");
        
        // will need to do checks if grass is taken
        if(grass[0] != null)
        {
            interest = grass[0];
        }
              //if no plants throws error out of range.
        
    }

    void OnShepardWhistle()
    {
        aTimer += Time.deltaTime;
        if (aTimer > attentionTime)
        {
            aTimer = 0;
            state = State.Dawdle; //will need to be changed to whatever occurs when lost shepards attention;
        }
    }

    public void NotifyWhistle(Shepard shpd)
    {
        interest = shpd.transform;
        state = State.FollowShepard;       
        attentionTime = Random.Range(2, 10);
        OnShepardWhistle();
    }
}
