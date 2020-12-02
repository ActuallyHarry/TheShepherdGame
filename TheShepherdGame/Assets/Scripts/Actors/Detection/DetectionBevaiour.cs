using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class DetectionBehaviour: ScriptableObject
{
    public abstract List<Transform> GetContext(Actor actor, float inRadius);
}
