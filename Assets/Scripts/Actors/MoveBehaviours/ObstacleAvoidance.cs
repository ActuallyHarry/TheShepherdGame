using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName ="Behaviours/MoveBehaviour/ObstacleAvoidance")]
public class ObstacleAvoidance : MoveBehaviour
{
    public LayerMask mask;
    [Range(0.01f, 2f)]
    public float radius = 0.5f;

    public override Vector3 CalculateMove(Actor actor, List<Transform> proximal, List<Transform> view)
    {
        Vector3 bestDir = actor.transform.forward;
        float furthestUnobstructedDst = 0;
        RaycastHit hit;


        for (int i = 0; i < actor.checkPoints.Length; i++)
        {
            Vector3 dir = actor.transform.TransformDirection(actor.checkPoints[i]);

            if (Physics.SphereCast(actor.transform.position, radius, dir, out hit, actor.avoidanceRadius, mask))
            {
                if (hit.distance > furthestUnobstructedDst)
                {
                    bestDir = dir;
                    furthestUnobstructedDst = hit.distance;
                }

            }
            else
            {

                return dir;
            }

        }

        return bestDir;
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
