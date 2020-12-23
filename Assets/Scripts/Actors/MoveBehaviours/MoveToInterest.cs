using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName ="Behaviours/MoveBehaviour/MoveToInterest")]
public class MoveToInterest : MoveBehaviour
{
    public override Vector3 CalculateMove(Actor actor, List<Transform> proximal, List<Transform> view)
    {
        if(actor.interest.tag == "Player")
        {
            return Vector3.zero;
        }

        Vector3 velocity = actor.interest.position - actor.transform.position;

        return velocity;
    }

    public override Quaternion CalculateRotation(Actor actor, Vector3 velocity)
    {
        return Quaternion.LookRotation(velocity);
    }

    public override void ResetValues(Actor actor)
    {
        
    }

    public override Actor.MoveMode ReturnMoveMode()
    {
       return Actor.MoveMode.Behaviour;
    }
}
