using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class MoveBehaviour : ScriptableObject
{
    public abstract Actor.MoveMode ReturnMoveMode();

    public abstract Vector3 CalculateMove(Actor actor);

    public abstract Quaternion CalculateRotation(Actor actor);

    public abstract void ResetValues(Actor actor);
}
   
