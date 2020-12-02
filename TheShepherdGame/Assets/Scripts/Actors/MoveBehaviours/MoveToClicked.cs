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

    public void Awake()
    {
       
        lastClickedLocation = Vector3.zero;
    }

    public override Vector3 CalculateMove(Actor actor)
    {
        
        if (Input.GetMouseButtonUp((int)click))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, clickable ))
            {
               lastClickedLocation = hit.point;
                
            }
        }
        return lastClickedLocation;
    }

    public override void ResetValues()
    {
        lastClickedLocation = Vector3.zero;
    }
}
