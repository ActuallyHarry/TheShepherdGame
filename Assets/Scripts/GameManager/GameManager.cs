using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public CameraController camCon;
    public  TileManager tMan;
    public Grid navGrid;
    public GameObject playerPrefab;
    [HideInInspector]
    public Actor player;

    void Start()
    {
        tMan.MakeMap();
        navGrid.CreateGrid();
        SetUpPlayer();
    }

    public void SetUpPlayer()
    {      
        player = Instantiate(playerPrefab, transform).GetComponent<Actor>();
        camCon.player = player;
        tMan.player = player;
        player.Begin();
     
    }

    
}
