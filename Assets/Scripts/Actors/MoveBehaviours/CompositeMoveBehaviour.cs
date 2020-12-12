using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName ="Behaviours/MoveBehaviour/CompositeBehaviour")]
public class CompositeMoveBehaviour : MoveBehaviour
{
    public MoveBehaviour[] behaviours;
    public float[] weights;

    public override Vector3 CalculateMove(Actor actor, List<Transform> context)
    {
        //handle daa mismatch
        if (weights.Length != behaviours.Length)
        {
            Debug.LogError("Data Mismatch in " + name, this);
            return Vector3.zero;
        }

        //setUpMove
        Vector3 move = Vector3.zero;

        //iterate through behvoiurs
        for (int i = 0; i < behaviours.Length; i++)
        {
            Vector3 partialMove = behaviours[i].CalculateMove(actor, context) * weights[i];

            if (partialMove != Vector3.zero)
            {
                if (partialMove.sqrMagnitude > weights[i] * weights[i])
                {
                    partialMove.Normalize();
                    partialMove *= weights[i];
                }

                move += partialMove;
            }
        }

        return move;
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
