using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Behaviours/MoveBehaviour/StayOnTileRadius")]
public class StayOnTile : MoveBehaviour
{
    public float radius = 20f;

    public override Vector3 CalculateMove(Actor actor, List<Transform> proximal, List<Transform> view)
    {
        Vector3 center = actor.ReturnCurrentTile().position;
        Vector3 centerOffset = center - actor.transform.position;
        float t = centerOffset.magnitude / radius;
        if (t < 0.9f)
        {
            return Vector3.zero;
        }

        return centerOffset * t * t;
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
