using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName ="Behaviours/MoveBehaviour/CompositeBehaviour")]
public class CompositeMoveBehaviour : MoveBehaviour
{
    public MoveBehaviour[] behaviours;
    public float[] weights;

    public override Vector3 CalculateMove(Actor actor, List<Transform> proximal, List<Transform> view, Vector3 currentVelocity)
    {
        //handle daa mismatch
        if (weights.Length != behaviours.Length)
        {
            Debug.LogError("Data Mismatch in " + name, this);
            return Vector3.zero;
        }

        //setUpMove
        Vector3 move = Vector3.zero;


        //for (int i = 0; i < actor.checkPoints.Length; i++)
        //{
        //    Debug.DrawRay(actor.transform.position, actor.checkPoints[i] * 5f);
        //}

        //iterate through behvoiurs
        Vector3[] weightedMove = new Vector3[actor.checkPoints.Length];
        for (int i = 0; i < behaviours.Length; i++)
        {
            Vector3 originalMove = behaviours[i].CalculateMove(actor, proximal, view, currentVelocity);

            if (originalMove != Vector3.zero) //if any of the behaviours return zero this overrides the effect of any other behaviours so that the actor will remain still
            {
                originalMove = originalMove.normalized;
                Vector3[] taperedVectors = CalculatePartialMove(originalMove, actor.checkPoints); ;


                Debug.Log(i);
                //return Vector3.zero;
                for (int j = 0; j < taperedVectors.Length; j++)
                {
                    //not here otherwise all the same
                    //weightedVectors[j] *= weights[i];
                    //if (weightedVectors[j].sqrMagnitude > weights[i] * weights[i])
                    //{
                    //    weightedVectors[j].Normalize();
                    //    weightedVectors[j] *= weights[i];
                    //}

                    Color color = Color.Lerp(Color.yellow, Color.red, i / (behaviours.Length));
                    Debug.DrawRay(actor.transform.position + Vector3.up, taperedVectors[j] * 2f, color);
                    weightedMove[j] += taperedVectors[j];
                }


                //Debug.Log(taperedVectors.Length);

            }
          


        }

        for (int s = 0; s < actor.checkPoints.Length; s++)
        {
            Color color = Color.Lerp(Color.green, Color.red, actor.checkPoints[s].magnitude / 10);
            //Debug.DrawRay(actor.transform.position + Vector3.up, weightedMove[s], color);
            //weightedMove[s] *= weights[s];
            //if (weightedMove[s].sqrMagnitude > weights[s] * weights[s])
            //{
            //    weightedMove[s].Normalize();
            //    weightedMove[s] *= weights[s];
            //}
            move += weightedMove[s];
        }



        //for (int i = 0; i < behaviours.Length; i++)
        //{
        //    Vector3 partialMove = behaviours[i].CalculateMove(actor, proximal, view) * weights[i];

        //    if (partialMove != Vector3.zero)
        //    {
        //        if (partialMove.sqrMagnitude > weights[i] * weights[i])
        //        {
        //            partialMove.Normalize();
        //            partialMove *= weights[i];
        //        }

        //        move += partialMove;

        //        Debug.DrawRay(actor.transform.position + Vector3.up, partialMove);
        //    }
        //}


        return move;
    }

    Vector3[] CalculatePartialMove(Vector3 originalDirection, Vector3[] checkPoints)
    {
        int c = checkPoints.Length;
        Vector3[] taperedVectors = new Vector3[c];
        
        float minAngle = 360f;
        int startIndex = 0;
        for (int i = 0; i <c; i++)
        {

            float angle = Vector3.Angle(originalDirection, checkPoints[i]);

            if (angle < minAngle)
            {
                startIndex = i;
                minAngle = angle;
            }

        }

        for (int i = 0; i < c; i++)
        {            

            float theta = Vector3.Angle(originalDirection, checkPoints[i]);
            theta = theta < 1 ? 1 : theta;

            //taperedVectors[i] = checkPoints[i] / theta; // first version
            //taperedVectors[i] = checkPoints[i] * (i - startIndex + 1) / theta; // second version
            float v = (i > startIndex) ? (-0.1f * (i-startIndex) * (i-c-startIndex)) : (-0.1f * (i+c-startIndex) *(i-startIndex));
            taperedVectors[i] = checkPoints[i] * v/theta;
            
            //Debug.DrawRay(Vector3.up, taperedVectors[i]);
        }


        taperedVectors[startIndex] = checkPoints[startIndex] * originalDirection.magnitude/2;

        return taperedVectors;
    }


    public override Quaternion CalculateRotation(Actor actor, Vector3 velocity)
    {
        return Quaternion.LookRotation(velocity);
        //return Quaternion.identity;
    }



    public override void ResetValues(Actor actor)
    {
        for (int i = 0; i < behaviours.Length; i++)
        {
            behaviours[i].ResetValues(actor);
        }
    }

    public override Actor.MoveMode ReturnMoveMode()
    {
        return Actor.MoveMode.Behaviour;
    }
}
