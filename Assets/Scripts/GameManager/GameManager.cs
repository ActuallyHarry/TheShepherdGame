using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [Header("ReferenceScripts")]
    public CameraController camCon;
    public  TileManager tMan;
    public Grid navGrid;
    public DifficultyModulator diffMod;

    [Header("ActorVars")]  
    public int numOfStartingSheep;
    public GameObject herdPrefab;    
    public GameObject playerPrefab;
    [HideInInspector]
    public Shepard player;

    [Header("Data")]
    public int numOfPoints = 10;
    Vector3[] pointsInCircle;
    Tile focusTile;
    Tile previousFocusTile;
   

    void Start()
    {
        pointsInCircle = CalculatePointsAroundCircle();
        tMan.MakeMap();
        Physics.SyncTransforms(); // this is required because the tiles are made in same frame as the nav grid is
        navGrid.CreateGrid();
        tMan.StartMap();
        SetUpPlayer();
        SetUpHerd();
        diffMod.Initialize(player.animals);

    }
    public void SetUpPlayer()
    {

        player = Instantiate(playerPrefab, new Vector3(tMan.tileOffset * tMan.tileScale, 0, tMan.tileOffset * tMan.tileScale), transform.rotation, null).GetComponent<Shepard>();
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
            animal.checkPoints = pointsInCircle; ;
            player.animals.Add(animal);
        }
    }


    private void Update()
    {
        focusTile = player.ReturnCurrentTile().GetComponent<TileObject>().thisTile;

        tMan.TileUpdate(focusTile);
        if (focusTile != previousFocusTile)
        {   
            //if prevtile not null was here
            OnNextTile();                      
            previousFocusTile = focusTile;
        }
    }

    //handles the tile loop,eg adjust sheep hunger etc
    void OnNextTile()
    {
        QuantityData quantityData = diffMod.CalcForNewTile();
        tMan.OnNextTile(focusTile, previousFocusTile, quantityData);
        
        
        if(previousFocusTile != null)
        {

            List<ShpdAnimal> animals = player.animals;
            foreach (ShpdAnimal animal in animals)
            {
                animal.DecreaseHunger();// th8s may need to be changed to a sheeo centric methoid rsather than player centruc
            }
        }
       
    }

    

    public void OnEndConditionsMet() // end conditions will be determined by the end level script? at least this function is called by it.
    {
        //adjust num of sheep
        //save progress
        //begin new level
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
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
