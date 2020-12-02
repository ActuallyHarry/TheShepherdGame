using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Behaviours/Detection/PrimitiveShape")]
public class PrimtiveShapeDetection : DetectionBehaviour
{
    public enum DetectionShape
    {
        Sphere,
        Box,
        Capsule
    }
    public DetectionShape shape;

    public override List<Transform> GetContext(Actor actor, float inRadius)
    {
        Collider[] col;
        List<Transform> t = new List<Transform>();

        switch (shape)
        {
            case DetectionShape.Sphere:
                col = Physics.OverlapSphere(actor.transform.position, inRadius);
                break;
            case DetectionShape.Box:
                col = Physics.OverlapBox(actor.transform.position, new Vector3(inRadius, inRadius, inRadius));
                break;
            case DetectionShape.Capsule:
                col = Physics.OverlapCapsule(actor.transform.position, actor.transform.position + Vector3.one * inRadius, inRadius);
                break;
            default:
                col = Physics.OverlapSphere(actor.transform.position, inRadius);
                break;
        }

        foreach(Collider c in col)
        {
            t.Add(c.transform);                     
        }

        if (t.Contains(actor.transform))
        {
            t.Remove(actor.transform);
        }
        return t;
    }
}
