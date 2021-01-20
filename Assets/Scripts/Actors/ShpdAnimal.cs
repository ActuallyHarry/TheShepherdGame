using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShpdAnimal : Actor
{
    UIShpdAnimal ui;
    Animator anim;

    bool shpdWhistled = false;
    private float hungerPercentage = 100f;
        
    [Header("Shepherd Animal")]
    public float hungerDecrease = 10f;
    public float CurrentHunger { get { return hungerPercentage; } }
    float animationTimer =0;
    bool isEating = false;

    bool isDying = false;


    //order of behaviours in the movebeahviour options for sheep should match the oder of states in this enum
    public enum State
    {
        Stop,
        Dawdle,
        FollowShepard,
        FindFood,
        Die
    }
    public State state;
    State prevState;

    float attentionTime;
    float aTimer = 0;

    public override  void Begin()
    {
        anim = GetComponentInChildren<Animator>();
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
        anim.SetBool("isMoving", isMoving);
        ui.SetValues(hungerPercentage);
        ui.StateDebugging(state, System.Array.IndexOf(moveBehaviourOptions, currentMoveBehaviour));
        CheckStatistics();
        CheckStatus();

        //Debug.Log(move);
      
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
            case State.Die:
                OnDeath();
                break;
        }   
        BUpdate();
    }

    #region StatusCheck
    // checks sheeps statitiscs for the need to change its state
    void CheckStatus()
    {

        if (hungerPercentage < 0)
        {
            state = State.Die;
            return;
        }

        state = State.Dawdle;
        if (shpdWhistled && !isEating)
        {
            prevState = state;
            state = State.FollowShepard;
        }

        if (hungerPercentage < 100 && state != State.FollowShepard)
        {
            if (ContextFilter.FilterContext(ItemsInView, "Plant").Count != 0)
            {
                prevState = state;
                state = State.FindFood;
                return;
            }

        }


        //if (ItemsInProximity.Contains(interest) && (state == State.FollowShepard || state == State.Stop))
        //{
        //    prevState = state;
        //    state = State.Stop;
        //    return;
        //}
        //else if (state == State.Stop)
        //{
        //    if (prevState == State.Stop)
        //    {
        //        state = State.Dawdle;
        //    }
        //    state = prevState;
        //}




    }

    //updates the statics suchas hunger health etc
    void CheckStatistics()
    {
        if(previousTileTransform != currentTileTransform)
        {
            if (previousTileTransform != null)
            {                
              DecreaseHunger();                
            }
            previousTileTransform = currentTileTransform;
        }
    }
    #endregion

    #region StateFunctions

    void OnDeath()
    {
        //Debug.Log("isDying" + animationTimer.ToString() + anim.GetBool("isDying"));
        currentMoveBehaviour = moveBehaviourOptions[(int)State.Die];
        animationTimer += Time.deltaTime;
        isDying = true;
        anim.SetBool("isDying", isDying);
        if (animationTimer > anim.GetCurrentAnimatorStateInfo(0).normalizedTime + anim.GetCurrentAnimatorStateInfo(0).length)
        {
            isDying = false;
            Debug.Log("Dead");
            Destroy(gameObject);
        }
    }
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
        //Debug.Log("Whistle");
        interest = leader.transform;
        AttentionTimer();
        if (!ItemsInProximity.Contains(interest))
        {
            currentMoveBehaviour = moveBehaviourOptions[(int)State.FollowShepard];
        }
        else
        {
            currentMoveBehaviour = moveBehaviourOptions[(int)State.Stop];
        }
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
                    animationTimer = 0;
                    return;
                    
                }
            }
            currentMoveBehaviour = moveBehaviourOptions[(int)State.FollowShepard]; // i think this will be best could be dawdle nut this means that the shepard as to lead them to food
           
            prevState = state;
        }    
        else
        {
            if (ItemsInProximity.Contains(interest))              
            {
                
                Food food = interest.GetComponent<Food>();
                float n = food.nourishementAmount;
                if(isEating == false)
                {
                    isEating = true;
                    anim.SetBool("isEating", isEating);
                }
                currentMoveBehaviour = moveBehaviourOptions[(int)State.Stop];
                animationTimer += Time.deltaTime;
                if (animationTimer > anim.GetCurrentAnimatorStateInfo(0).normalizedTime + anim.GetCurrentAnimatorStateInfo(0).length)
                {
                    isEating = false;
                    anim.SetBool("isEating", isEating);
                    animationTimer = 0;
                    hungerPercentage = Mathf.Min(100, hungerPercentage + n);
                    food.Destroy();
                    state = State.Dawdle;
                }
            }
        } 
   }

    #endregion

    public void DecreaseHunger()
    {       
        hungerPercentage -= hungerDecrease;
        //Debug.Log(hungerPercentage);
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
