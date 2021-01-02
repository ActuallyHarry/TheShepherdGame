using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DifficultyModulator : MonoBehaviour
{
    List<ShpdAnimal> animals;
    int previousFood = 0;

    public void Initialize(List<ShpdAnimal> a)
    {
        animals = a;
    }

    public QuantityData CalcForNewTile()
    {
        int foodnum = NumOfFoodForNextTile();

        QuantityData qData= new QuantityData(foodnum);
        return qData;
    }


    public int NumOfFoodForNextTile()
    {
        float n = Random.Range(0, 300)/100f;

        float avgSaturation = 0;
        for (int i = 0; i < animals.Count; i++)
        {
            avgSaturation += animals[i].CurrentHunger;
        }
        avgSaturation /= animals.Count*100f;

        float foodnum = animals.Count * n *(1.5f - avgSaturation) - 0.5f * previousFood;

        previousFood = Mathf.RoundToInt(foodnum);
       
        return previousFood;
    }
}

public struct QuantityData
{
    public readonly int foodNum;
    //public readonly int wolfNum;

    public QuantityData(int _foodNum)
    {
        foodNum = _foodNum;
    }
}



