using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShpdAnimal : Actor
{
    public enum State
    {
        ShpdWhistled,
        Hungry
    }
    public State state;

    float attentionTime;
    float aTimer =0;

    new  void Begin()
    {
        base.Begin();
    }
    new void Update()
    {
        base.Update();

        switch (state)
        {
            case State.ShpdWhistled:
                OnShepardWhistle();
                break;
            case State.Hungry:
                OnHungry();
                break;
        }


        
        
        
    }
    void OnHungry()
    {
        Debug.Log("Hunfry");
        List<Transform> grass = filter.FilterContext(ItemsInView, "Plant");
        
        interest = grass[0];       //if no plants throws error out of range.
        
    }

    void OnShepardWhistle()
    {
        aTimer += Time.deltaTime;
        if (aTimer > attentionTime)
        {
            aTimer = 0;
            state = State.Hungry; //will need to be changed to whatever occurs when lost shepards attention;
        }
    }

    public void NotifyWhistle(Shepard shpd)
    {
        interest = shpd.transform;
        state = State.ShpdWhistled;
        attentionTime = Random.Range(2, 10);
    }
}
