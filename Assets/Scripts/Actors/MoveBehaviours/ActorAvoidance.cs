using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName ="Behaviours/MoveBehaviour/ActorAvoidance")]
public class ActorAvoidance : MoveBehaviour
{
    public override Vector3 CalculateMove(Actor actor, List<Transform> proximal, List<Transform> view, Vector3 currentVelocity)
    {
        Vector3 avoidanceMove = currentVelocity;
        //if no neigbours return no adjustment
        if (proximal.Count == 0)
        {
            return avoidanceMove;
        }
        //add all points together and average
       
        int nAvoid = 0;
        List<Transform> filteredContext = ContextFilter.FilterForActors(proximal);
        foreach (Transform item in filteredContext)
        {
            if (Vector3.SqrMagnitude(item.position - actor.transform.position) < actor.SquareAvoidanceRadius)
            {
                nAvoid++;
                avoidanceMove += (actor.transform.position - item.position);
            }
        }

        if (nAvoid > 0)
        {
            avoidanceMove /= nAvoid;
        }

        return avoidanceMove;
    }

    //this is problem need to choose roation properly from actul scripts
    public override Quaternion CalculateRotation(Actor actor, Vector3 velocity)
    {
        return Quaternion.LookRotation(velocity);
    }

    //public override Vector3 CalculateRotation(Actor actor, Vector3 velocity)
    //{
    //    return actor.transform.position + velocity;
    //}

    public override void ResetValues(Actor actor)
    {
        
    }

    public override Actor.MoveMode ReturnMoveMode()
    {
        return Actor.MoveMode.Behaviour;
    }
}
