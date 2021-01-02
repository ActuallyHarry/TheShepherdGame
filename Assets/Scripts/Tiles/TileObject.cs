using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class TileObject : MonoBehaviour
{
    public Tile thisTile;
    [Header("Data")]
    public int[] code;
    public Exit[] exits;
    //public GameObject[] navMeshData;
    public GameObject NavigationData;

    public float activateTime = 0.5f;
    float t = 0;
    bool active = false;
    public bool rising = false;

    [Header("Assets")]
    public GameObject asset; // this is the thing that actually moves
    public Transform inacitvePos;
    public Transform activePos;

    [Header("SpawningPoints")]
    public Transform[] foodSpawnLocations;
    public Transform[] entitySpawnLocations;

    public List<Food> food;
    

    // alla this does is make the tile rise
    public void Update()
    {
        if (rising)
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
                rising = false;     
            }
        }
        


    }

   public void TurnOnColliders(bool u)
    {
        NavigationData.SetActive(u);
    }

    public bool IsActive()
    {
        return active;
    }
}
