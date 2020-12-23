using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIShpdAnimal : MonoBehaviour
{
    MeshRenderer mat;

    float goalHunger; //the value to changeto
    float currentHunger; // the current value

    public Slider hungerBar;

    private void Start()
    {
        mat = GetComponentInChildren<MeshRenderer>();
    }

    public void Initialize(float hungerMax)
    {
        
        hungerBar.value = hungerMax;
        currentHunger = hungerMax;
        goalHunger = hungerMax;
    }

    public void SetValues(float hunger)
    {
        goalHunger = hunger;
       
    }

    void Update()
    {
        
        currentHunger = Mathf.Lerp(currentHunger, goalHunger,0.5f);
        hungerBar.value = currentHunger;
    }

    public void StateDebugging(ShpdAnimal.State s)
    {
        switch (s)
        {
            case ShpdAnimal.State.Stop:
                mat.material.color = Color.black;
                break;
            case ShpdAnimal.State.Dawdle:
                mat.material.color = Color.grey;
                break;
            case ShpdAnimal.State.FollowShepard:
                mat.material.color = Color.green;
                break;
            case ShpdAnimal.State.FindFood:
                mat.material.color = Color.yellow;
                break;
            default:
                mat.material.color = Color.white;
                break;
        }
    }

}
