using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class MoveBehaviour : ScriptableObject
{
    public abstract Actor.MoveMode ReturnMoveMode();

    public abstract Vector3 CalculateMove(Actor actor, List<Transform> context);

    public abstract Quaternion CalculateRotation(Actor actor, Vector3 velocity);

    public abstract void ResetValues(Actor actor);
}
   
