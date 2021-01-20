using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Behaviours/MoveBehaviour/StayOnTileRadius")]
public class StayOnTile : MoveBehaviour
{
   public float radius = 20f;

    public override Vector3 CalculateMove(Actor actor, List<Transform> proximal, List<Transform> view, Vector3 currentVelocity)
    {
        Vector3 center = actor.ReturnCurrentTile().position;
        Vector3 centerOffset = center - actor.transform.position;
        float t = centerOffset.magnitude / radius;
        if (t < 0.8f)
        {
            //Debug.Log(currentVelocity);
            return currentVelocity;
        }

        return centerOffset * t * t;
        //Vector3 bestDir = actor.transform.forward;
        //float furthestUnobstructedDst = 0;
        //RaycastHit hit;


        //for (int i = 0; i < actor.checkPoints.Length; i++)
        //{
        //    Vector3 dir = actor.transform.TransformDirection(actor.checkPoints[i]);
        //    if (Physics.Raycast(actor.transform.position, dir, out hit, actor.avoidanceRadius))
        //    {
        //        if(hit.transform.tag == "Exit")
        //        {
        //            if (hit.distance > furthestUnobstructedDst)
        //            {
        //                bestDir = dir;
        //                furthestUnobstructedDst = hit.distance;
        //            }
        //        }


        //    }
        //    else
        //    {

        //        return dir;
        //    }

        //}

        //return bestDir;
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
