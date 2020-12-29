using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[CreateAssetMenu(menuName ="Behaviours/MoveBehaviour/ToClicked")]
public class MoveToClicked : MoveBehaviour
{
    public LayerMask clickable;
    public enum MouseButton
    {
        LeftClick,
        RightClick,
        MiddleClick,
    }

    public Vector3 lastClickedLocation;

    public MouseButton click = MouseButton.LeftClick;

 

    public override Vector3 CalculateMove(Actor actor, List<Transform> proximal, List<Transform> view)
    {
        if (Input.GetMouseButtonUp((int)click))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, clickable ))
            {
                //Debug.Log("click");
                lastClickedLocation = hit.point;
            }
        }
        return lastClickedLocation;
        
    }

    public override void ResetValues(Actor actor)
    {
        lastClickedLocation = actor.transform.position;
    }

    public override Actor.MoveMode ReturnMoveMode()
    {
        return Actor.MoveMode.NavGrid;
    }

    public override Quaternion CalculateRotation(Actor actor, Vector3 velocity)
    {

        Vector3 rot = velocity - actor.transform.position;
        return rot == Vector3.zero? actor.transform.rotation: Quaternion.LookRotation(rot);
    }
}
