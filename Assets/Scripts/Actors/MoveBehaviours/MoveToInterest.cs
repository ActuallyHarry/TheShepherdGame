using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName ="Behaviours/MoveBehaviour/MoveToInterest")]
public class MoveToInterest : MoveBehaviour
{
    public float stoppingDistance = 1f;
    public bool includeLooking = false;
    float sqrStoppingDistance = 1f;

    public override Vector3 CalculateMove(Actor actor, List<Transform> proximal, List<Transform> view, Vector3 currentVelocity)
    {
        if(actor.interest.tag == "Player")
        {
            return currentVelocity;
        }

        if((actor.interest.position - actor.transform.position).sqrMagnitude < sqrStoppingDistance) // this does not woek
        {
            return Vector3.zero;
        }

        Vector3 velocity = actor.interest.position - actor.transform.position;

        return velocity;
    }

    public override Quaternion CalculateRotation(Actor actor, Vector3 velocity)
    {
        Vector3 lookDirection = Vector3.zero;
        if (includeLooking)
        {
            lookDirection = actor.interest.position - actor.transform.position;
        }
        return Quaternion.LookRotation(lookDirection);
    }

    public override void ResetValues(Actor actor)
    {
        sqrStoppingDistance = stoppingDistance * stoppingDistance;
    }

    public override Actor.MoveMode ReturnMoveMode()
    {
       return Actor.MoveMode.Behaviour;
    }
}
