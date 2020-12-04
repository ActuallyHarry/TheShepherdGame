using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class MapGenerator : MonoBehaviour
{
    MapProcessor mapProcessor;
    public Tile[,] tileMap;
    public int[,,] codeMap;
    int[,] tileCodes;

    public bool codemapCompleted = false;

    private void Awake()
    {
        IntializeTileCodes();
         mapProcessor = new MapProcessor();
    }
    

    public Tile[,] GenerateMap(int mapSize, TileManager _tm)
    {
        tileMap = new Tile[mapSize, mapSize];
        codeMap = new int[mapSize, mapSize, 4];
        for (int x = 0; x < mapSize; x++)
        {
            for (int y = 0; y < mapSize; y++)
            {
                for (int i = 0; i < codeMap.GetLength(2); i++)
                {
                    codeMap[x, y, i] = -1;
                }
                
            }
        }

        AssignTileCodeMap(mapSize);
        
        Tile[,] unprocessedTiles = AssignTiles(codeMap, _tm);

        // after tiles are processed codeMap is redunatant/ inaccurate so why the fuck did i do it
        tileMap = mapProcessor.ProcessMap(unprocessedTiles); 
        codemapCompleted = true;
        return tileMap;
        
    }

   Tile[,] AssignTiles(int[,,] tileCodeMap, TileManager _tm)
    {
        Tile[,] tiles = new Tile[tileCodeMap.GetLength(0), tileCodeMap.GetLength(1)];
        for (int x = 0; x < tiles.GetLength(0); x++)
        {
            for (int y = 0; y < tiles.GetLength(1); y++)
            {
                tiles[x, y] = new Tile(GetCode3D(tileCodeMap, x, y), new Coord(x,y), _tm);
            }
        }

        for (int x = 0; x < tiles.GetLength(0); x++)
        {
            for (int y = 0; y < tiles.GetLength(1); y++)
            {
                tiles[x, y].AddNeighbors(GetNeigbours(tiles ,x, y));
            }
        }
        
        return tiles;
    }

    Tile[] GetNeigbours(Tile[,] tiles, int x, int y)
    {
        List<Tile> n = new List<Tile>();
        int[] bin = GetCode3D(codeMap, x, y);
        if(bin[0] == 1)
        {
            n.Add(tiles[x + 1, y]);
        }
        if(bin[1] == 1)
        {
            n.Add(tiles[x, y + 1]);
        }
        if(bin[2] == 1)
        {
            n.Add(tiles[x - 1, y]);
        }
        if(bin[3] == 1)
        {
            n.Add(tiles[x, y - 1]);
        }
        //Debug.Log(n.Count.ToString() + ' ' + x.ToString() + ' ' + y.ToString());

        return n.ToArray();
    }
  

   void AssignTileCodeMap(int mapSize)
    {
        for (int x = 0; x < mapSize; x++)
        {
            for (int y = 0; y < mapSize; y++)
            {
                int[] code = RandomTileCode(x, y, mapSize);
                for (int i = 0; i < code.Length; i++)
                {
                    codeMap[x, y, i] = code[i];
                }
            }
        }

       
    }

    int[] RandomTileCode(int x, int y, int mapSize)
    {

        int bottom = (y-1 < 0)?0:CheckExit(x, y - 1, 1);
        int top = (y+1 >= mapSize)?0:CheckExit(x, y+1, 3);
        int right = (x+1 >= mapSize)?0:CheckExit(x+1, y,2);
        int left = (x-1 < 0)?0:CheckExit(x-1, y, 0);

        //Debug.Log(right.ToString()+top.ToString() + left.ToString() + bottom.ToString());
        int[] binaryCode = new int[4];
        binaryCode[0] = right;
        binaryCode[1] = top;
        binaryCode[2] = left;
        binaryCode[3] = bottom;
        //Debug.Log(binaryCode[0].ToString() + binaryCode[1].ToString() + binaryCode[2].ToString() + binaryCode[3].ToString());
       
        return FindViableOption(binaryCode);      
        

    }

    
    int[] FindViableOption(int[] binCode) // x and y only for debug
    {
        // Debug.Log(binCode);
        int[] viableOption = new int[4];
        reshuffle(tileCodes);
        for (int i = 0; i < tileCodes.GetLength(0); i++)
        {
            bool viable = true;
            int[] possible = GetCode2D(tileCodes, i);
            //Debug.Log(possible[0].ToString() + possible[1].ToString() + possible[2].ToString() + possible[3].ToString() + ' ' + binCode[0].ToString() + binCode[1].ToString() + binCode[2].ToString() + binCode[3].ToString());
            for (int j = 0; j < possible.Length; j++)
            {
                if((possible[j] != binCode[j] && binCode[j] != -1))
                {                    
                    viable = false;
                    break;
                }
              
            }
            if (viable)
            {
                //Debug.Log(possible[0].ToString() + possible[1].ToString() + possible[2].ToString() + possible[3].ToString());//+ ' '+ possibleBin[0].ToString() + possibleBin[1].ToString()+ possibleBin[2].ToString()+ possibleBin[3].ToString() + ' ' + tileCodes[i] + ' ' + x + ' ' + y);
                viableOption = possible;
                break;
            }

        }
        //Debug.Log(viableOption[0].ToString() + viableOption[1].ToString() + viableOption[2].ToString() + viableOption[3].ToString());
        return viableOption;

    }

   

  

    


    int CheckExit(int x, int y, int side) //side, 0= right, 1 = top, 2=left, 3= bottom of tile being checked
    {
        return codeMap[x, y, side] == -1 ? -1 : codeMap[x, y, side];
       
    }


    void reshuffle(int[,] nums)
    {       
        for (int t = 0; t < nums.GetLength(0); t++)
        {
            int[] tmp = new int[4];           
            int r = UnityEngine.Random.Range(t, nums.GetLength(0));
            for (int i = 0; i < nums.GetLength(1); i++)
            {
                tmp[i] = nums[t, i];
                nums[t, i] = nums[r, i];
                nums[r, i] = tmp[i];
            }
        }

        
    }

    int[] GetCode2D( int[,] codes, int index)
    {
        
        int[] tmp = new int[codes.GetLength(1)];
        for (int i = 0; i < codes.GetLength(1); i++)
        {
            tmp[i] = codes[index, i];
        }
        return tmp;
    }

    int[] GetCode3D(int[,,] codes, int x, int y)
    {
        int[] tmp = new int[codes.GetLength(2)];
        for (int i = 0; i < codes.GetLength(2); i++)
        {
            tmp[i] = codes[x, y, i];
        }
        return tmp;
    }

    void IntializeTileCodes() // not includoing 0
    {
        tileCodes = new int[15, 4] {
            {0,0,0,1},
            {0,0,1,0},
            {0,0,1,1},
            {0,1,0,0},
            {0,1,0,1},
            {0,1,1,0},
            {0,1,1,1},
            {1,0,0,0},
            {1,0,0,1},
            {1,0,1,0},
            {1,0,1,1},
            {1,1,0,0},
            {1,1,0,1},
            {1,1,1,0},
            {1,1,1,1}
        };

    }
}




