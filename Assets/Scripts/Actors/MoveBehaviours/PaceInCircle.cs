using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName ="Behaviours/MoveBehaviour/PaceInCircle")]
public class PaceInCircle : MoveBehaviour
{
    public override Vector3 CalculateMove(Actor actor, List<Transform> proximal, List<Transform> view, Vector3 currentVelocity)
    {
        Vector3 perpDirection = actor.interest.position - actor.transform.position;
        float tangentZ = -perpDirection.x;
        float tangentX = perpDirection.z;

        Vector3 tangent = new Vector3(tangentX, perpDirection.y, tangentZ);

        return tangent.normalized;


    }

    public override Quaternion CalculateRotation(Actor actor, Vector3 velocity)
    {
        return Quaternion.LookRotation(Vector3.up);
    }

    public override void ResetValues(Actor actor)
    {
        
    }

    public override Actor.MoveMode ReturnMoveMode()
    {

        return Actor.MoveMode.Behaviour;
    }
}
