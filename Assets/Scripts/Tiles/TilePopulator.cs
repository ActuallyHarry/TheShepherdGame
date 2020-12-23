using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TilePopulator : MonoBehaviour
{
    public GameObject foodPrefab;

    public void PopulateTile(TileObject tileObject) // this should eventually factor in the difficulty settings
    {
        Transform[] foodSpawnLoc = tileObject.foodSpawnLocations;
        //Transform[] enititySpawnLoc = tileObject.entitySpawnLocations;

        PopulateFood(foodSpawnLoc, foodSpawnLoc.Length); // the second argument will need to be replaced based on the dififuclty metric
    }

    void PopulateFood(Transform[] spawnLoc, int numOfFood)
    {
        int repetitions = Mathf.Min(spawnLoc.Length, numOfFood);//may be better way but good for now

        for (int i = 0; i < repetitions; i++)
        {
            Instantiate(foodPrefab, spawnLoc[i]);
        }
    }
}
