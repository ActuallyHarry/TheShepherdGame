using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Actor : MonoBehaviour
{
    //protected Vector3 move = Vector3.zero;

    public Transform interest; // for behaviours and focus
    public Actor leader; // example shepard is leader of sheep
    public Vector3 target; // for naviagtion grid. -> must be relataoiviley static postions
    protected Transform currentTileTransform;
    protected Transform previousTileTransform; //atm these are for checks for individuals whereas focus tile is for gamemanagement

    [HideInInspector]
    public Vector3[] checkPoints;
    [HideInInspector]
    Vector3 currentVelocity = Vector3.forward;
    Quaternion currentRotation = Quaternion.identity;
    [Range(0,360)]
    public float angleRotationThreshold = 20;

    public enum MoveMode
    {
        NavGrid,
        Behaviour
    }
    MoveMode moveMode;

    
    public MoveBehaviour[] moveBehaviourOptions;
    protected MoveBehaviour currentMoveBehaviour;
    public DetectionBehaviour detB;


    [Header("Navigation")]
    public float speed = 7;
    const float minPathUpdateTime = 0.2f;
    const float pathUpdateMovethreshold = 0.5f;    
    public float turnSpeed = 3;
    public float turnDist = 5;
    public float stoppingDistance;
    Path path;

    [Header("Detection")]
    public float proximityRadius;
    public float viewRadius;
    public float avoidanceRadius = 2f;
    [HideInInspector]
    public float SquareAvoidanceRadius;
    public List<Transform> ItemsInProximity = new List<Transform>();
    public List<Transform> ItemsInView = new List<Transform>();

    //conditions
    protected bool isMoving = false;

    public virtual void Begin()
    {
        SquareAvoidanceRadius = avoidanceRadius * avoidanceRadius;
        currentMoveBehaviour.ResetValues(this);

        if(interest == null)
        {
            interest = transform;
        }

        //target = interest.position;

        StopAllCoroutines();
        StartCoroutine(UpdatePath());
    }
 

    public void BUpdate()
    {
        currentTileTransform = ReturnCurrentTile();
        ItemsInProximity = detB.GetContext(this, proximityRadius);
        ItemsInView = detB.GetContext(this, viewRadius);
        moveMode = currentMoveBehaviour.ReturnMoveMode();
        //Debug.Log(currentMoveBehaviour);
        Vector3 move = Vector3.zero;
        Quaternion rot = transform.rotation;
        //Vector3 rot = transform.rotation.eulerAngles;
        if (currentTileTransform != null)
        {
            move = currentMoveBehaviour.CalculateMove(this, ItemsInProximity, ItemsInView, currentVelocity);
            rot = currentMoveBehaviour.CalculateRotation(this, move);

            if(Quaternion.Angle(rot,currentRotation) > angleRotationThreshold)
            {
                currentRotation = rot;
            }
        }

        currentVelocity = move;
        
       

        switch (moveMode)
        {
            case MoveMode.NavGrid:
                target = move; //here move must be a postion
                break;
            case MoveMode.Behaviour:
                Move(currentVelocity,currentRotation); //here move is required to be a velocity                
                break;

        }
     

       

        
    }

    public Transform ReturnCurrentTile()
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position + Vector3.up, Vector3.down, out hit, 5f, LayerMask.GetMask("Ground")))
        {
            //Debug.Log(hit.transform.parent.parent.name);
            return hit.transform.parent.parent;
        }
        else
        {
            Debug.Log("No Tile");
            return null;
        }
    }

    //for behaviours
    private void Move(Vector3 velocity, Quaternion rotation)    
    {
        velocity = velocity.normalized * speed;
        
        isMoving = velocity.magnitude > 0.01f;


        transform.rotation = Quaternion.Lerp(transform.rotation, rotation, turnSpeed);
        //transform.LookAt(lookAt);
        transform.position += velocity * Time.deltaTime;
    }

    #region Pathfinding
    // when the path is made starts couritine to follow the path.
    public void OnPathFound(Vector3[] waypoints, bool pathSuccessful) 
    {

        if (pathSuccessful)
        {
            //isMoving = true;
            StopCoroutine(FollowPath());
            path = new Path(waypoints, transform.position,turnDist, stoppingDistance);
            StartCoroutine(FollowPath());
        }
    }

    public IEnumerator UpdatePath()
    {

        if (Time.timeSinceLevelLoad < .3f)
        {
            yield return new WaitForSeconds(.3f);
        }
        // PathRequestManager.RequestPath(new PathRequest(transform.position, target, OnPathFound)); //asks to create a path to the target

        float sqrMovethreshold = pathUpdateMovethreshold * pathUpdateMovethreshold;
        Vector3 targetPosOld = target;

        while (true)
        {
            yield return new WaitForSeconds(minPathUpdateTime);

            if ((target - targetPosOld).sqrMagnitude > sqrMovethreshold)
            {
                PathRequestManager.RequestPath(new PathRequest(transform.position, target, OnPathFound));
                targetPosOld = target;
            }


        }
    }

    //i think somewherer in here is what ids causing the shepard to double speed when 
    //the path is interreepted
    IEnumerator FollowPath()
    {
        isMoving = true;
        bool followingPath = true;
        int pathIndex = 0;

        transform.LookAt(path.lookPoints[0]);

        float speedPercent = 1;

        while (followingPath)
        {
            Vector2 pos2D = new Vector2(transform.position.x, transform.position.z);
            while (path.turnBoundaries[pathIndex].HasCrossedLine(pos2D))
            {
                if (pathIndex == path.finishLineIndex)
                {
                    followingPath = false;
                    isMoving = false;
                    //isMoving = false;
                    break;
                }
                else
                {
                    pathIndex++;
                }
            }

            if (followingPath)
            {
                if (pathIndex >= path.slowdownIndex && stoppingDistance > 0)
                {
                    speedPercent = Mathf.Clamp01(path.turnBoundaries[path.finishLineIndex].DistanceFromPoint(pos2D) / stoppingDistance);
                    //Debug.Log(speedPercent);
                    if (speedPercent < 0.01f)
                    {
                        followingPath = false;
                        isMoving = false;
                       // isMoving = false;
                    }
                }


                Quaternion targetRotation = Quaternion.LookRotation(path.lookPoints[pathIndex] - transform.position);
                transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, Time.deltaTime * turnSpeed);
                transform.Translate(Vector3.forward * Time.deltaTime * speed * speedPercent, Space.Self);

            }
            yield return null;
        }
    }
  

   
    #endregion

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, proximityRadius);
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, viewRadius);
        if (path != null)
        {
            path.DrawWithGizmos();
        }
    }
    
}
