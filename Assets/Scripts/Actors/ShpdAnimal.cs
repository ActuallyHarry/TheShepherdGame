using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShpdAnimal : Actor
{
    //order of behaviours in the movebeahviour options for sheep should match the oder of states in this enum
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
        state = State.Dawdle;
        currentMoveBehaviour = moveBehaviourOptions[(int)State.Dawdle];
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
                OnDawdle();
                break;
            case State.FollowShepard:
                OnFollowShepard();
                break;
            case State.FindFood:
                break;
        }

       


        BUpdate();
    }

    #region StateFunctions
    void OnDawdle()
    {
        currentMoveBehaviour = moveBehaviourOptions[(int)State.Dawdle];
        interest = transform;
    }

    void OnFollowShepard()
    {
        Debug.Log("Whistle");
        AttentionTimer();
        currentMoveBehaviour = moveBehaviourOptions[(int)State.FollowShepard];
        interest = leader.transform;
    }

    void OnHungry()
    {
        ////Debug.Log("Hunfry");
        //List<Transform> grass = filter.FilterContext(ItemsInView, "Plant");
        
        //// will need to do checks if grass is taken
        //if(grass[0] != null)
        //{
        //    interest = grass[0];
        //}
        //      //if no plants throws error out of range.
        
    }
    #endregion

    void AttentionTimer()
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
        
        state = State.FollowShepard;       
        attentionTime = Random.Range(8, 15);
    }
}
