using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelEnd : MonoBehaviour
{
    GameManager gm;

    private void Start()
    {
        gm = FindObjectOfType<GameManager>();
    }

    void OnTriggerEnter(Collider col)
    {
        if(col.tag == "Player")
        {
            gm.OnEndConditionsMet();
        }
    }
}
