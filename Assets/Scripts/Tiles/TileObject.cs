using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class TileObject : MonoBehaviour
{
    public Tile thisTile;

    public int code;
    public Exit[] exits;


    public GameObject asset; // this is the thing that actually moves
    public Transform inacitvePos;
    public Transform activePos;
    public float activateTime = 0.5f;
    float t = 0;
    bool active = false;
    public bool activating = false;


    public void Update()
    {
        if (activating)
        {
            asset.SetActive(true);
            if (t < activateTime)
            {
                asset.transform.position = Vector3.Lerp(inacitvePos.position, activePos.position, t / activateTime);
                t += Time.deltaTime;

            }
            else
            {
                asset.transform.position = activePos.position;
                active = true;
                activating = false;
                thisTile.tm.UpdateNavMesh();


            }
        }
        


    }

   

    public bool IsActive()
    {
        return active;
    }
}
