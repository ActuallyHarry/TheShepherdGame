using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wolf : Actor
{
    [Header("Wolf")]
    public List<Wolf> otherWolves;
    float healthPercentage =100f;
   
    public enum State
    {
        Rest,
        Stalk,
        Attack,
        Run
    }

    public State state;

    void Start()
    {
        Begin();
    }

    public override void Begin()
    {
        
        state = State.Rest;
        currentMoveBehaviour = moveBehaviourOptions[(int)State.Rest];
        base.Begin();
    }


    void Update()
    {
        //CheckStatus();

        switch (state)
        {
            case State.Rest:
                OnRest();
                break;
            case State.Stalk:
                OnStalk();
                break;
            case State.Attack:
                OnAttack();
                break;
            case State.Run:
                OnRun();
                break;
        }

        BUpdate();
    }
    void CheckStatus()
    {
        state = State.Rest;
        List<Transform> animals = ContextFilter.FilterForHerd(ItemsInView);
        if (animals.Count > 0)
        {
            state = State.Stalk;
            interest = animals[0];
        }

        //when delegated should change to attack bothb ased on a timer and whether other wolves are attacking

    }


    void OnRest()
    {
        currentMoveBehaviour = moveBehaviourOptions[(int)State.Rest];
    }

    void OnStalk()
    {
        currentMoveBehaviour = moveBehaviourOptions[(int)State.Stalk];
    }

    void OnAttack()
    {
        currentMoveBehaviour = moveBehaviourOptions[(int)State.Attack];
    }

    void OnRun()
    {
        currentMoveBehaviour = moveBehaviourOptions[(int)State.Run];
    }

   
}
