using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Behaviours/MoveBehaviour/MoveToShepard")]
public class MoveToShepard : MoveBehaviour
{
    Vector3 currentVelocity;
    public float agentSmoothTime = 0.5f;

    public override Vector3 CalculateMove(Actor actor, List<Transform> proximal, List<Transform> view)
    {
        Vector3 velocity = actor.leader.transform.position - actor.transform.position;
        velocity = velocity.normalized;
        velocity *= actor.speed;
        //velocity = Vector3.SmoothDamp(actor.transform.forward, velocity, ref currentVelocity, agentSmoothTime);
        return velocity;
    }

    public override Quaternion CalculateRotation(Actor actor, Vector3 velocity)
    {
        Quaternion rotation = Quaternion.LookRotation(actor.leader.transform.position - actor.transform.position);
        return rotation;
    }

    public override void ResetValues(Actor actor)
    {

    }

    public override Actor.MoveMode ReturnMoveMode()
    {
        return Actor.MoveMode.Behaviour;
    }
}
