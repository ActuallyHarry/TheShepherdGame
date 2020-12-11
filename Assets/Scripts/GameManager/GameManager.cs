using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public CameraController camCon;
    public  TileManager tMan;
    public Grid navGrid;

    public int numOfStartingSheep;

    public GameObject playerPrefab;
    public GameObject herdPrefab;
    [HideInInspector]
    public Shepard player;


    void Start()
    {
        tMan.MakeMap();
        Physics.SyncTransforms(); // this is required because the tiles are made in same frame as the nav grid is
        navGrid.CreateGrid();       
        SetUpPlayer();
        SetUpHerd();

    }

    public void SetUpPlayer()
    {      
        player = Instantiate(playerPrefab, new Vector3(tMan.tileOffset*tMan.tileScale,0,tMan.tileOffset*tMan.tileScale), transform.rotation, null).GetComponent<Shepard>();
        camCon.player = player;
        tMan.player = player;
        player.Begin();
     
    }

    public void SetUpHerd()
    {
        // will need to check that it is not colliding with another sheep or object when spawned/ have predetermined sheep spawning on tile.
        // spawn in randomly determined location or predefind,but os that they do not spawn in same place.
        for (int i = 0; i < numOfStartingSheep; i++)
        {
            // will need to be changed
            ShpdAnimal animal = Instantiate(herdPrefab, new Vector3(tMan.tileOffset * tMan.tileScale + i, 0, tMan.tileOffset * tMan.tileScale + i), transform.rotation, null).GetComponent<ShpdAnimal>();
            animal.Begin();
            animal.SetShepard(player);
            player.animals.Add(animal);
        } 
    }

    
}
