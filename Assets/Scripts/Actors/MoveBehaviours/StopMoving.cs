using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName ="Behaviours/MoveBehaviour/StopMoving")]
public class StopMoving : MoveBehaviour
{
    public override Vector3 CalculateMove(Actor actor, List<Transform> proximal, List<Transform> view)
    {
        return Vector3.zero;
    }

    public override Quaternion CalculateRotation(Actor actor, Vector3 velocity)
    {
        return actor.transform.rotation;
    }

    public override void ResetValues(Actor actor)
    {
        
    }

    public override Actor.MoveMode ReturnMoveMode()
    {
        return Actor.MoveMode.Behaviour;
    }
}
