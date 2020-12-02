using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile 
{
    public TileObject tileObject;
    public TileManager tm;

    public int tileCode;
    public List<Tile> neighbors = new List<Tile>();
    public Coord position;
    public int region;



    public Tile()
    {

    }
    public Tile(int _tileCode, Coord _position, TileManager _tm)
    {
        tm = _tm;
        tileCode = _tileCode;
        position = _position;
    }

    public void AddNeighbors(params Tile[] _neighbors)
    {
        for (int i = 0; i < _neighbors.Length; i++)
        {
            neighbors.Add(_neighbors[i]);
        }

    }


    
}
