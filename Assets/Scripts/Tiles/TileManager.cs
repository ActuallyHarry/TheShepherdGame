﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


public class TileManager : MonoBehaviour
{
    MapGenerator mapGen;
    TileSetter tileSet;
    Tile[,] tileMap;

    [Header("MapData")]
    public int mapSize;
    public int tileScale = 40;
    public int tileOffset =-4;
    [Range(0,8)]
    public int endTilePossibleSpawnArea;

    [HideInInspector]
    public Actor player;
    Tile focusTile; // this is the tile that the player is on.
    Tile previousFocusTile;

    [Header("TileResources")]
    public GameObject levelEndObject;
    public Transform tileParent;
   // defualts for each type: -> pasture is named with an _ so that it is always 0 in list
    public static List<TileObject> ALL = new List<TileObject>();
    public static List<TileObject> JUNCTION = new List<TileObject>();
    public static List<TileObject> STRAIGHT = new List<TileObject>();
    public static List<TileObject> SINGLE = new List<TileObject>();
    public static List<TileObject> CORNER = new List<TileObject>();

    public Queue<Tile> activatingTiles = new Queue<Tile>();
   

    private void Awake()
    {
        GameObject [] gAll = Resources.LoadAll<GameObject>("Tiles/tAll");
        foreach (GameObject g in gAll)
        {
            ALL.Add(g.GetComponent<TileObject>());
        }

        GameObject[] gJunction = Resources.LoadAll<GameObject>("Tiles/tJunction");
        foreach (GameObject g in gJunction)
        {
            JUNCTION.Add(g.GetComponent<TileObject>());
        }

        GameObject[] gStraight = Resources.LoadAll<GameObject>("Tiles/tStraight");
        foreach (GameObject g in gStraight)
        {
            STRAIGHT.Add(g.GetComponent<TileObject>());
        }

        GameObject[] gSingle = Resources.LoadAll<GameObject>("Tiles/tSingle");
        foreach (GameObject g in gSingle)
        {
            SINGLE.Add(g.GetComponent<TileObject>());
        }

        GameObject[] gCorner = Resources.LoadAll<GameObject>("Tiles/tCorner");
        foreach (GameObject g in gCorner)
        {
            CORNER.Add(g.GetComponent<TileObject>());
        }

    }

    void Start()
    {
        mapGen = GetComponent<MapGenerator>();
        tileSet = GetComponent<TileSetter>();
       
    }

    private void Update()
    {
     
        focusTile = player.ReturnCurrentTile().GetComponent<TileObject>().thisTile;
    
        CheckCurrentTile();
        UpdateTiles();
    }

    void CheckCurrentTile()
    {

            //checking if player is in an exit;
            for (int i = 0; i < focusTile.tileObject.exits.Length; i++)
            {

                if (focusTile.tileObject.exits[i] == null)
                {
                    continue;
                }
                if (focusTile.tileObject.exits[i].exiting)
                {
                   
                    switch (i)
                    {
                        case 0:
                            activatingTiles.Enqueue(tileMap[focusTile.position.tileX + 1, focusTile.position.tileY]);
                           
                            break;
                        case 1:
                            activatingTiles.Enqueue(tileMap[focusTile.position.tileX, focusTile.position.tileY + 1]);
                            
                            break;
                        case 2:
                            activatingTiles.Enqueue(tileMap[focusTile.position.tileX - 1, focusTile.position.tileY]);
                            
                            break;
                        case 3:
                            activatingTiles.Enqueue(tileMap[focusTile.position.tileX, focusTile.position.tileY - 1]);
                           
                            break;
                    }
                    focusTile.tileObject.exits[i].exiting = false;
                    Destroy(focusTile.tileObject.exits[i]);
            }
            }

            if(previousFocusTile != focusTile)
            {
                if(previousFocusTile != null)
                {
                    for (int i = 0; i < previousFocusTile.neighbors.Count; i++)
                    {
                        previousFocusTile.neighbors[i].tileObject.TurnOnColliders(false);
                    }
                }

                focusTile.tileObject.TurnOnColliders(true);
                for (int i = 0; i < focusTile.neighbors.Count; i++)
                {
                    focusTile.neighbors[i].tileObject.TurnOnColliders(true);
                }

                previousFocusTile = focusTile;
            }
           
       
        
    }

    void UpdateTiles()
    {
        if (activatingTiles.Count > 0)
        {
            Tile tile = activatingTiles.Dequeue();
            tile.tileObject.activating = true;
            //Debug.Log(activatingTiles.Count);
        }     
    }




    public void MakeMap()
    {        
        tileMap = mapGen.GenerateMap(mapSize, this);
        int xEnd = Random.Range(mapSize - endTilePossibleSpawnArea - 1, mapSize - 1);
        int yEnd = Random.Range(mapSize - endTilePossibleSpawnArea - 1, mapSize - 1);
        tileMap = tileSet.AttachTileObjects(tileMap, tileScale, tileOffset, tileParent.parent, xEnd, yEnd);
        tileSet.PlaceEndLevelObject(levelEndObject, xEnd, yEnd, tileScale, tileOffset);
        HideMap();

    }

    void HideMap()
    {
        for (int x = 0; x < mapSize; x++)
        {
            for (int y = 0; y < mapSize; y++)
            {
                Transform tile = tileMap[x, y].tileObject.asset.transform;
                tile.position = tileMap[x, y].tileObject.inacitvePos.position;
                tile.gameObject.SetActive(false);
            }
        }
    }

    

    //this occurs after a navigation mesh has been made in game manager
    public void StartMap()
    {
        for (int x = 0; x < mapSize; x++)
        {
            for (int y = 0; y < mapSize; y++)
            {
                tileMap[x, y].tileObject.TurnOnColliders(false);
            }
        }
        tileMap[0, 0].tileObject.TurnOnColliders(true);
        tileMap[0, 0].tileObject.gameObject.SetActive(true);
        tileMap[0, 0].tileObject.activateTime = 0;
        tileMap[0, 0].tileObject.activating = true;
      
    }

    

    public void ActivateTile(Tile t)
    {
        activatingTiles.Enqueue(t);
    }    
}
