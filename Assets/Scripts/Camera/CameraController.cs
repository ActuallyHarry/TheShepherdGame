using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [HideInInspector]
    public Actor player;
    public Transform cameraFocus;
    Transform tileTarget;


    public float speed = 0.2f;
    //public float bumpSpeed = 0.5f;
    //public Vector2 playerBumpBounds;
    //public Vector2 cameraBounds;
    //Vector3 cameraFocusMove = new Vector3(0, 0);

    private void Update()
    {
       
        if (player != null)
        {
            tileTarget = player.ReturnCurrentTile();
            transform.position = Vector3.Lerp(transform.position, tileTarget.position, speed);
        }
       
    //    float x;
    //    float y;
    //    Vector2 playerPos = player.transform.position;
    //    if (playerPos.y > transform.position.z + playerBumpBounds.y || playerPos.y < transform.position.z - playerBumpBounds.y)
    //    {
    //        y = Mathf.Clamp( player.transform.position.z, transform.position.z-cameraBounds.y, transform.position.z + cameraBounds.y);


    //    }
    //    else
    //    {
    //        y = transform.position.z;
    //    }
    //    if (playerPos.x > transform.position.x + playerBumpBounds.x || playerPos.x < transform.position.x - playerBumpBounds.x)
    //    {
    //        x = Mathf.Clamp( player.transform.position.x, transform.position.x - cameraBounds.x, transform.position.x + cameraBounds.x);
    //    }
    //    else
    //    {
    //        x = transform.position.x;
    //    }
    //    cameraFocusMove.x = x;
    //    cameraFocusMove.z = y;
    //    cameraFocus.position = Vector3.Lerp(cameraFocus.position,cameraFocusMove, bumpSpeed);


       
        
    }
}
