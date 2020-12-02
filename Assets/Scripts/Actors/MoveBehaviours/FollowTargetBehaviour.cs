using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Behaviours/MoveBehaviour/ToTarget")]
public class FollowTargetBehaviour : MoveBehaviour
{
    public override Vector3 CalculateMove(Actor actor)
    {
        Vector3 targetPos = actor.interest.position;
        return targetPos;

    }

    public override void ResetValues()
    {
        
    }
}
