using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Behaviours/MoveBehaviour/UnconformMove")]
public class UnConformMove : MoveBehaviour
{
    Vector3 currentVelocity;
    public float agentSmoothTime = 0.5f;

    public override Vector3 CalculateMove(Actor actor, List<Transform> proximal, List<Transform> view, Vector3 currentVelocity)
    {
        Vector3 velocity = currentVelocity; //not sure if this is the best for all behavious
 
        if(view.Count == 0)
        {
            return velocity;
        }
        List<Transform> filteredContext = ContextFilter.FilterForHerd(view);
        if (filteredContext.Count == 0)
        {
            return velocity;
        }

        foreach (Transform item in filteredContext)
        {
            velocity += item.position;
        }
      
        velocity /= filteredContext.Count;
        velocity -= actor.transform.position;
        velocity = -velocity;

        //velocity = Vector3.SmoothDamp(actor.transform.forward, velocity, ref currentVelocity, agentSmoothTime);
        return velocity;
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
