using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Actor : MonoBehaviour
{
    public MoveBehaviour movB;
    public DetectionBehaviour detB;

    [HideInInspector]
    public ContextFilter filter = new ContextFilter();

    NavMeshAgent agent;
    public Transform currentTile;

    public float speed = 8;
    public float proximityRadius;
    public float viewRadius;

    public Transform interest;
    [HideInInspector]
    public List<Transform> ItemsInProximity = new List<Transform>();
    public List<Transform> ItemsInView = new List<Transform>();

    public void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.speed = speed;

        movB.ResetValues();

        if(interest == null)
        {
            interest = transform;
        }
        
    }
 

    public void Update()
    {
        ItemsInProximity = detB.GetContext(this, proximityRadius);
        ItemsInView = detB.GetContext(this, viewRadius);
        Vector3 pos = movB.CalculateMove(this);

      

        Move(pos);
        
      
    }


    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, proximityRadius);
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, viewRadius);
    }

    private void Move(Vector3 position)
    {
        if (filter.ContextContainsSpecific(ItemsInProximity, interest))
        {
            agent.isStopped = true;
        }
        else
        {
            agent.isStopped = false;
        }

        agent.SetDestination(position);
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
}
