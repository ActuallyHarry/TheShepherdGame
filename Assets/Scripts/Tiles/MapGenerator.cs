using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class MapGenerator : MonoBehaviour
{
    MapProcessor mapProcessor;
    public Tile[,] tileMap;
    public int[,] codeMap;
    int[] tileCodes;

    public bool codemapCompleted = false;

    private void Awake()
    {
        tileCodes = new int[15];
        IntializeTileCodes();
         mapProcessor = new MapProcessor();
    }
    

    public Tile[,] GenerateMap(int mapSize, TileManager _tm)
    {
        tileMap = new Tile[mapSize, mapSize];
        codeMap = new int[mapSize, mapSize];
        for (int x = 0; x < mapSize; x++)
        {
            for (int y = 0; y < mapSize; y++)
            {
                codeMap[x, y] = -1;
            }
        }

        AssignTileCodeMap(mapSize);
        
        Tile[,] unprocessedTiles = AssignTiles(codeMap, _tm);

        tileMap = mapProcessor.ProcessMap(unprocessedTiles); // after tiles are processed codeMap is redunatant/ inaccurate
        codemapCompleted = true;
        return tileMap;
        
    }

   Tile[,] AssignTiles(int[,] tileCodeMap, TileManager _tm)
    {
        Tile[,] tiles = new Tile[tileCodeMap.GetLength(0), tileCodeMap.GetLength(1)];
        for (int x = 0; x < tiles.GetLength(0); x++)
        {
            for (int y = 0; y < tiles.GetLength(1); y++)
            {
                tiles[x, y] = new Tile(tileCodeMap[x, y], new Coord(x,y), _tm);
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
        int[] bin = Converter.CodeToBinary(codeMap[x,y]);
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
                codeMap[x, y] = RandomTileCode(x, y, mapSize);
                
            }
        }

       
    }

    int RandomTileCode(int x, int y, int mapSize)
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
       
        return FindViableOption(binaryCode, x ,y);      
        

    }

    
    int FindViableOption(int[] binCode, int x ,int y) // x and y only for debug
    {
        // Debug.Log(binCode);
        int viableOption = 0;
        reshuffle(tileCodes);
        for (int i = 0; i < tileCodes.Length; i++)
        {
            bool viable = true;
            int[] possibleBin = Converter.CodeToBinary(tileCodes[i]);
            for (int j = 0; j < possibleBin.Length; j++)
            {
                if((possibleBin[j] != binCode[j] && binCode[j] != -1))
                {                    
                    viable = false;
                    break;
                }
              
            }
            if (viable)
            {
                //Debug.Log(binCode[0].ToString() + binCode[1].ToString() + binCode[2].ToString() + binCode[3].ToString()+ ' '+ possibleBin[0].ToString() + possibleBin[1].ToString()+ possibleBin[2].ToString()+ possibleBin[3].ToString() + ' ' + tileCodes[i] + ' ' + x + ' ' + y);
                viableOption = tileCodes[i];
                break;
            }

        }
        return viableOption;

    }

   

  

    


    int CheckExit(int x, int y, int side) //side, 0= right, 1 = top, 2=left, 3= bottom of tile being checked
    {
        int i = Converter.GetBinaryForSide(codeMap[x, y], side);
        return i;
       
    }


    void reshuffle(int[] nums)
    {       
        for (int t = 0; t < nums.Length; t++)
        {
            int tmp = nums[t];
            int r = UnityEngine.Random.Range(t, nums.Length);
            nums[t] = nums[r];
            nums[r] = tmp;
        }
    }

    void IntializeTileCodes() // not includoing 0
    {
        tileCodes[0] = 15;
        tileCodes[1] = 1;
        tileCodes[2] = 2;
        tileCodes[3] = 4;
        tileCodes[4] = 8; 
        tileCodes[5] = 5;
        tileCodes[6] = 10;
        tileCodes[7] = 3;
        tileCodes[8] = 6;
        tileCodes[9] = 12;
        tileCodes[10] = 9;
        tileCodes[11] = 11;
        tileCodes[12] = 7;
        tileCodes[13] = 14;
        tileCodes[14] = 13;
        
    }
}

public static class Converter
{

    public static int GetBinaryForSide(int code, int side)
    {

        if (code != -1)
        {
            string s = System.Convert.ToString(code, 2);
            s = s.PadLeft(4, '0');
            return int.Parse(s[side].ToString());
        }
        else
        {
            return -1;
        }



    }

    public static int[] CodeToBinary(int code) //j is for debugging
    {
        int[] bin = new int[4];
        string sbin = Convert.ToString(code, 2);
        sbin = sbin.PadLeft(bin.Length, '0');
        for (int i = 0; i < bin.Length; i++)
        {
            bin[i] = int.Parse(sbin[i].ToString());
        }
        // Debug.Log(bin[0].ToString() + bin[1].ToString() + bin[2].ToString() + bin[3].ToString() + ' ' + code + ' ' + j);
        return bin;
    }

    public static int BinaryToTileCode(int[] code)
    {
        string cs = "";
        for (int i = 0; i < code.Length; i++)
        {
            cs += code[i].ToString();

        }

        return Convert.ToInt32(cs, 2);
    }
}


