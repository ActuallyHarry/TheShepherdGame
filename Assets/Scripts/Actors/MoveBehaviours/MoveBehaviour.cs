using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class MoveBehaviour : ScriptableObject
{
    public abstract Vector3 CalculateMove(Actor actor);

    public abstract void ResetValues(Actor actor);
}
   
