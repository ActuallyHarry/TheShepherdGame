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
        Physics.SyncTransforms(); // this is required because the tiles are made in same frame as the nav grid is
        navGrid.CreateGrid();
        SetUpPlayer();
    }

    public void SetUpPlayer()
    {      
        player = Instantiate(playerPrefab, new Vector3(tMan.tileOffset*tMan.tileScale,0,tMan.tileOffset*tMan.tileScale), transform.rotation, null).GetComponent<Actor>();
        camCon.player = player;
        tMan.player = player;
        player.Begin();
     
    }

    
}
