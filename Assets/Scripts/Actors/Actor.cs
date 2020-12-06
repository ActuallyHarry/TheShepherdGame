using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Actor : MonoBehaviour
{
    public MoveBehaviour movB;
    public DetectionBehaviour detB;

    [HideInInspector]
    public ContextFilter filter = new ContextFilter();

    public Transform currentTile;

    [Header("Navigation")]
    public float speed = 8;
    const float minPathUpdateTime = 0.2f;
    const float pathUpdateMovethreshold = 0.5f;    
    public Vector3 target; // for navigation
    public float turnSpeed = 3;
    public float turnDist = 5;
    public float stoppingDistance;

    Path path;

    [Header("Detection")]
    public float proximityRadius;
    public float viewRadius;
    public Transform interest;
    [HideInInspector]
    public List<Transform> ItemsInProximity = new List<Transform>();
    public List<Transform> ItemsInView = new List<Transform>();

    public void Start()
    {

        movB.ResetValues();

        if(interest == null)
        {
            interest = transform;
        }

        StartCoroutine(UpdatePath());

    }
 

    public void Update()
    {
        ItemsInProximity = detB.GetContext(this, proximityRadius);
        ItemsInView = detB.GetContext(this, viewRadius);
        Vector3 pos = movB.CalculateMove(this);



        target = pos;
        
      
    }


    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, proximityRadius);
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, viewRadius);
    }

    public Transform ReturnCurrentTile()
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, Vector3.down, out hit, 5f, LayerMask.GetMask("Ground")))
        {
            return hit.transform.parent.parent;
        }
        else
        {
            Debug.Log("No Tile");
            return null;
        }
    }   



    public void OnPathFound(Vector3[] waypoints, bool pathSuccessful) // when the path is made starts couritine to follow the path.
    {
        if (pathSuccessful)
        {
            path = new Path(waypoints, transform.position, turnDist, stoppingDistance);
            StopCoroutine("FollowPath");
            StartCoroutine("FollowPath");
        }
    }

    IEnumerator UpdatePath()
    {
        if (Time.timeSinceLevelLoad < .3f)
        {
            yield return new WaitForSeconds(.3f);
        }
        PathRequestManager.RequestPath(new PathRequest(transform.position, target, OnPathFound)); //asks to create a path to the target

        float sqrMovethreshold = pathUpdateMovethreshold * pathUpdateMovethreshold;
        Vector3 targetPosOld = target;

        while (true)
        {
            yield return new WaitForSeconds(minPathUpdateTime);

            if ((target - targetPosOld).sqrMagnitude > sqrMovethreshold)
            {
                PathRequestManager.RequestPath(new PathRequest(transform.position, target, OnPathFound)); //asks to create a path to the target
                targetPosOld = target;
            }


        }
    }

    IEnumerator FollowPath()
    {
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
                    if (speedPercent < 0.01f)
                    {
                        followingPath = false;
                    }
                }


                Quaternion targetRotation = Quaternion.LookRotation(path.lookPoints[pathIndex] - transform.position);
                transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, Time.deltaTime * turnSpeed);
                transform.Translate(Vector3.forward * Time.deltaTime * speed * speedPercent, Space.Self);
            }
            yield return null;
        }
    }
}
