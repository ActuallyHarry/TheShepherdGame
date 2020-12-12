﻿using System.Collections;
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

    Vector3[] pointsInCircle;
    public int numOfPoints= 10;
    void Start()
    {
        pointsInCircle = CalculatePointsAroundCircle();
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
        player.checkPoints = pointsInCircle;
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
            animal.checkPoints = pointsInCircle;
            player.animals.Add(animal);
        } 
    }

    Vector3[] CalculatePointsAroundCircle()
    {
        Vector3[] points = new Vector3[numOfPoints];
        float increment = (Mathf.PI * 2) / numOfPoints;
        for (int i = 0; i < numOfPoints; i++)
        {
            float x = Mathf.Sin(increment * i);
            float y = Mathf.Cos(increment * i);

            points[i] = new Vector3(x, 0, y);
        }

        return points;
    }

    
}
