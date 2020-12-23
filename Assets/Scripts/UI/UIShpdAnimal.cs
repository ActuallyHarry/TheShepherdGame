using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIShpdAnimal : MonoBehaviour
{

    float goalHunger; //the value to changeto
    float currentHunger; // the current value

    public Slider hungerBar;

    public void Initialize(float hungerMax)
    {
        hungerBar.value = hungerMax;
        currentHunger = hungerMax;
        goalHunger = hungerMax;
    }

    public void SetValues(float hunger)
    {
        currentHunger = hunger;
    }

    void Update()
    {
        currentHunger = Mathf.Lerp(currentHunger, goalHunger,0.5f);
        hungerBar.value = currentHunger;
    }

}
