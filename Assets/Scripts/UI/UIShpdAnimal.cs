using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIShpdAnimal : MonoBehaviour
{
    public MeshRenderer stateMat;
    public MeshRenderer mbMat;

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
       
        goalHunger = hunger;

    }

    void Update()
    {

        
        currentHunger = Mathf.Lerp(currentHunger, goalHunger,0.5f);
        hungerBar.value = currentHunger;
    }

    public void StateDebugging(ShpdAnimal.State s, int m)
    {
        switch (s)
        {
            case ShpdAnimal.State.Stop:
                stateMat.material.color = Color.black;
                break;
            case ShpdAnimal.State.Dawdle:
                stateMat.material.color = Color.grey;
                break;
            case ShpdAnimal.State.FollowShepard:
                stateMat.material.color = Color.green;
                break;
            case ShpdAnimal.State.FindFood:
                stateMat.material.color = Color.yellow;
                break;
            default:
                stateMat.material.color = Color.white;
                break;
        }

        switch (m)
        {
            case 0:
                mbMat.material.color = Color.black;
                break;
            case 1:
                mbMat.material.color = Color.grey;
                break;
            case 2:
                mbMat.material.color = Color.green;
                break;
            case 3:
                mbMat.material.color = Color.yellow;
                break;
            default:
                mbMat.material.color = Color.white;
                break;
        }
    }

}
