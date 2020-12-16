using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName ="Behaviours/MoveBehaviour/ActorAvoidance")]
public class ActorAvoidance : MoveBehaviour
{
    public override Vector3 CalculateMove(Actor actor, List<Transform> proximal, List<Transform> view)
    {
        //if no neigbours return no adjustment
        if (proximal.Count == 0)
        {
            return Vector3.zero;
        }

        //add all points together and average
        Vector3 avoidanceMove = Vector3.zero;
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
