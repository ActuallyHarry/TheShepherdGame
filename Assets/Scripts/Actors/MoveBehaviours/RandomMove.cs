using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Behaviours/MoveBehaviour/RandomMove")]
public class RandomMove : MoveBehaviour
{
    public float randomMoveWait = 4;
    float timer = 0;
    Vector3 oldVelocity = Vector3.zero;
    public override Vector3 CalculateMove(Actor actor, List<Transform> context)
    {
        Vector3 velocity = oldVelocity;
        timer += Time.deltaTime;
       // Debug.Log(timer);
        if(timer> randomMoveWait)
        {
           
            Vector2 random = Random.insideUnitCircle;
            random *= Random.Range(0, actor.viewRadius);
            velocity = new Vector3(random.x, 0, random.y);
            oldVelocity = velocity;
            Debug.Log(velocity);
            timer = 0;
        }
       

        return velocity;

        
    }

    public override Quaternion CalculateRotation(Actor actor, Vector3 velocity)
    {
        return Quaternion.LookRotation(velocity);
    }

    public override void ResetValues(Actor actor)
    {
        timer = randomMoveWait;
        oldVelocity = Vector3.zero;
    }

    public override Actor.MoveMode ReturnMoveMode()
    {
        return Actor.MoveMode.Behaviour;
    }
}
