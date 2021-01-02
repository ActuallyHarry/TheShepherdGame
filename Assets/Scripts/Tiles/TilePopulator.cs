using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TilePopulator : MonoBehaviour
{
    public GameObject foodPrefab;
    public void PopulateTile(TileObject tileObject, QuantityData quantity) // this should eventually factor in the difficulty settings
    {
        Transform[] foodSpawnLoc = tileObject.foodSpawnLocations;
        //Transform[] enititySpawnLoc = tileObject.entitySpawnLocations;

        int numOfFood = quantity.foodNum;
        //int nbumOf entity
        tileObject.food = PopulateFood(foodSpawnLoc, numOfFood); // the second argument will need to be replaced based on the dififuclty metric
    }

    List<Food> PopulateFood(Transform[] spawnLoc, int numOfFood)
    {
        int repetitions = Mathf.Min(spawnLoc.Length, numOfFood);//may be better way but good for now
        List<Food> food = new List<Food>();
        for (int i = 0; i < repetitions; i++)
        {
            food.Add(Instantiate(foodPrefab, spawnLoc[i]).GetComponent<Food>());
        }
        return food;
    }

    public void DePopulateTile(TileObject tileObject)
    {
        List<Food> food = tileObject.food;
        for (int i = 0; i < food.Count; i++)
        {
            food[i].Destroy();
        }
        tileObject.food.Clear();
    }
}
