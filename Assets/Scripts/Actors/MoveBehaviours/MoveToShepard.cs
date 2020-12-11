using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Behaviours/MoveBehaviour/MoveToShepard")]
public class MoveToShepard : MoveBehaviour
{
    public override Vector3 CalculateMove(Actor actor)
    {
        Vector3 velocity = actor.leader.transform.position - actor.transform.position;
        velocity = velocity.normalized;
        velocity *= actor.speed;
        return velocity;
    }

    public override Quaternion CalculateRotation(Actor actor)
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
