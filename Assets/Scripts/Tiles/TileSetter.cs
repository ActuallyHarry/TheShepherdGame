using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileSetter : MonoBehaviour
{
    public Tile[,] AttachTileObjects(Tile[,] map, int tileScale, Transform parent)
    {
        for (int x = 0; x < map.GetLength(0); x++)
        {
            for (int y = 0; y < map.GetLength(1); y++)
            {
                AttachTile(map[x, y], tileScale, parent);

            }
        }

        //map = ConnectTileExits(map);


        return map;
    }

    //Tile[,] ConnectTileExits(Tile[,] map)
    //{
    //    for (int x = 0; x < map.GetLength(0); x++)
    //    {
    //        for (int y = 0; y < map.GetLength(1); y++)
    //        {
    //            map[x, y].ConnectExits();
    //        }
    //    }

    //    return map;
    //}

    void AttachTile(Tile tile, int tileScale, Transform parent)
    {
        GameObject t = null;
        TileObject to = null;       
        if (tile.tileCode == 1 || tile.tileCode == 2 || tile.tileCode == 4 || tile.tileCode == 8)
        {
            t = Instantiate(TileManager.SINGLE[Random.Range(0, TileManager.SINGLE.Count)].gameObject, parent);
            to = t.GetComponent<TileObject>();         
        }
        else if (tile.tileCode == 5 || tile.tileCode == 10)
        {
            t = Instantiate(TileManager.STRAIGHT[Random.Range(0, TileManager.STRAIGHT.Count)].gameObject, parent);
            to = t.GetComponent<TileObject>();          
        }
        else if (tile.tileCode == 11 || tile.tileCode == 7 || tile.tileCode == 13 || tile.tileCode == 14)
        {
            t = Instantiate(TileManager.JUNCTION[Random.Range(0, TileManager.JUNCTION.Count)].gameObject, parent);
            to = t.GetComponent<TileObject>();
        }
        else if (tile.tileCode == 3 || tile.tileCode == 6 || tile.tileCode == 12 || tile.tileCode == 9)
        {
            t = Instantiate(TileManager.CORNER[Random.Range(0, TileManager.CORNER.Count)].gameObject, parent);
            to = t.GetComponent<TileObject>();                
            
        }
        else if (tile.tileCode == 15)
        {
             t = Instantiate(TileManager.ALL[Random.Range(0, TileManager.ALL.Count)].gameObject,parent);
            to = t.GetComponent<TileObject>();            
        }
        
        PositionTile(t, tile.position, tileScale);       
        to = OrientTile(to.code, tile.tileCode, to);
        to.thisTile = tile;
        tile.tileObject = to;

    }

    void PositionTile(GameObject g, Coord pos, int tileScale)
    {
        g.transform.position = new Vector3(pos.tileX * tileScale, 0, pos.tileY * tileScale);
    }

    TileObject OrientTile(int defaultCode, int toCode, TileObject t) // bad implementation can only handle specific to and froms
    {
        switch (defaultCode)
        {
            case 8:
                if (toCode == 8)
                {
                    return t;
                }
                else if (toCode == 4)
                {
                    t = ReassignExitsAndRotate(1, t);
                    t.code = 4;
                }
                else if (toCode == 2)
                {
                    t = ReassignExitsAndRotate(2, t);
                    t.code = 2;
                }
                else if (toCode == 1)
                {
                    t = ReassignExitsAndRotate(3, t);
                    t.code = 1;
                }
                break;
            case 10:
                if (toCode == 10)
                {
                    return t;
                }
                else if (toCode == 5)
                {
                    t = ReassignExitsAndRotate(1, t);
                    t.code = 5;
                }
                break;
            case 12:
                if (toCode == 12)
                {
                    return t;
                }
                else if (toCode == 6)
                {
                    t = ReassignExitsAndRotate(1, t);
                    t.code = 6;
                }
                else if (toCode == 3)
                {
                    t = ReassignExitsAndRotate(2, t);
                    t.code = 3;
                }
                else if (toCode == 9)
                {
                    t = ReassignExitsAndRotate(3, t);
                    t.code = 9;
                }
                break;
            case 14:
                if (toCode == 14)
                {
                    return t;
                }
                else if (toCode == 7)
                {
                    t = ReassignExitsAndRotate(1, t);
                    t.code = 7;
                }
                else if (toCode == 11)
                {
                    t = ReassignExitsAndRotate(2, t);
                    t.code = 11;
                }
                else if (toCode == 13)
                {
                   t = ReassignExitsAndRotate(3, t);
                    t.code = 13;
                }
                break;
            case 15:
                return t;
            default:
                Debug.LogError("Not A defaultCode");
                break;
        }

        return t;
    }

    TileObject ReassignExitsAndRotate(int numOfRotations, TileObject t)
    {
        Vector3 euler = new Vector3(0, -90 * numOfRotations, 0);
        t.asset.transform.Rotate(euler);
        for (int i = 0; i < numOfRotations; i++)
        {
            Exit temp = t.exits[0];
            t.exits[0] = t.exits[3];
            t.exits[3] = t.exits[2];
            t.exits[2] = t.exits[1];
            t.exits[1] = temp;
        }

        return t;

    }
}
