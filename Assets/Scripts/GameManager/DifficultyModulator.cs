using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DifficultyModulator : MonoBehaviour
{
    List<ShpdAnimal> animals;

    private void Initialize(List<ShpdAnimal> a)
    {
        animals = a;
    }




    public int NumOfFoodForNextTile(int currentFood)
    {
        float avgHunger = 0;
        for (int i = 0; i < animals.Count; i++)
        {
            avgHunger += animals[i].CurrentHunger;
        }
        avgHunger /= animals.Count*100f;

        int foodnum = Mathf.RoundToInt(currentFood * (1 - avgHunger));
        return foodnum;
    }
}



