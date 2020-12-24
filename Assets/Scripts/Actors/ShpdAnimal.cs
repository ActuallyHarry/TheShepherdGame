using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShpdAnimal : Actor
{
    UIShpdAnimal ui;

    bool shpdWhistled = false;
    private float hungerPercentage = 100f;   
    public float hungerDecrease = 10f;

    //order of behaviours in the movebeahviour options for sheep should match the oder of states in this enum
    public enum State
    {
        Stop,
        Dawdle,
        FollowShepard,
        FindFood
    }
    public State state;
    State prevState;

    float attentionTime;
    float aTimer = 0;

    public override  void Begin()
    {

        ui = GetComponent<UIShpdAnimal>();
        ui.Initialize(hungerPercentage);
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
        ui.SetValues(hungerPercentage);
        ui.StateDebugging(state, System.Array.IndexOf(moveBehaviourOptions, currentMoveBehaviour));
        CheckStatus();
      
        //Debug.Log(target);
        switch (state)
        {
            case State.Stop:
                OnStop();
                break;
            case State.Dawdle:
                OnDawdle();
                break;
            case State.FollowShepard:
                OnFollowShepard();
                break;
            case State.FindFood:
                OnFindFood();
                break;
        }   
        BUpdate();
    }

    // checks sheeps statitiscs for the need to change its state
    void CheckStatus()
    {
        state = State.Dawdle;
        if (shpdWhistled)
        {
            state = State.FollowShepard;
        }

        if(hungerPercentage < 100 && state != State.FollowShepard)
        {
            if(ContextFilter.FilterContext(ItemsInView, "Plant").Count != 0)
            {
                prevState = state;
                state = State.FindFood;
                return;
            }           
        }

        if (ItemsInProximity.Contains(interest) && (state == State.FollowShepard || state == State.Stop))
        {
            prevState = state;
            state = State.Stop;
            return;
        }
        else if (state == State.Stop)
        {
            if(prevState == State.Stop)
            {
                state = State.Dawdle;
            }
            state = prevState;
        }

        // state = State.Dawdle;
    }

    #region StateFunctions
    void OnStop()
    {
        AttentionTimer();
        currentMoveBehaviour = moveBehaviourOptions[(int)State.Stop];                   
        //prevState = state; stop shouldntreally ever be a previous state;

    }

    void OnDawdle()
    {
        //if(prevState != state)
        //{
            interest = transform;
            currentMoveBehaviour = moveBehaviourOptions[(int)State.Dawdle];
            //interest = transform;
        //}
       
    }

    void OnFollowShepard()
    {
        Debug.Log("Whistle");
        interest = leader.transform;
        AttentionTimer();
        currentMoveBehaviour = moveBehaviourOptions[(int)State.FollowShepard];             
    }

    void OnFindFood()
    {      
        //Debug.Log(interest.gameObject.tag);
        if(interest.gameObject.tag != "Plant") // need to handle in case of no food
        {
            List<Transform> foodInView = ContextFilter.FilterContext(ItemsInView, "Plant");
            foreach (Transform item in foodInView)
            {
               Food food = item.GetComponent<Food>();

                if(food.ReturnCurrentTile() != leader.ReturnCurrentTile())
                {                    
                    continue;
                }
                if (!food.taken)
                {
                    food.taken = true;
                    interest = item;
                    currentMoveBehaviour = moveBehaviourOptions[(int)State.FindFood];
                    return;
                }
                
            }
            currentMoveBehaviour = moveBehaviourOptions[(int)State.FollowShepard]; // i think this will be best could be dawdle nut this means that the shepard as to lead them to food
           
            prevState = state;
        }
        else // interest is a food in this case due to tag being plant
        {
            if (ItemsInProximity.Contains(interest))
            {
                Food food = interest.GetComponent<Food>();
                float n = food.nourishementAmount;
                hungerPercentage = Mathf.Min(100, hungerPercentage + n);
                food.Destroy();
                state = State.Dawdle;
            }
        }

       
      
    }

    #endregion

    public void DecreaseHunger()
    {       
        hungerPercentage -= hungerDecrease;
        Debug.Log(hungerPercentage);
    }

    void AttentionTimer()
    {
        aTimer += Time.deltaTime;
        if (aTimer > attentionTime)
        {
            aTimer = 0;
            shpdWhistled = false; //will need to be changed to whatever occurs when lost shepards attention;
            interest = transform;
        }
    }

    public void NotifyWhistle(bool follow)
    {
        shpdWhistled = follow;
        attentionTime = Random.Range(8, 15);
    }
}
