using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapProcessor
{

    public Tile[,] ProcessMap(Tile[,] map)
    {
        
        List<List<Coord>> regions = GetRegions(map);
        List<Area> areas = new List<Area>();
        for (int i = 0; i < regions.Count; i++)
        {                     
            for (int j = 0; j < regions[i].Count; j++)
            {
                map[regions[i][j].tileX, regions[i][j].tileY].region = i;
            }
            
        }

        for (int i = 0; i < regions.Count; i++)
        {
            areas.Add(new Area(regions[i], map));
        }
        ConnectAreas(areas, map);

        return map;
    }

    List<List<Coord>> GetRegions(Tile[,] map)
    {
        List<List<Coord>> regions = new List<List<Coord>>();
        int[,] mapFlags = new int[map.GetLength(0), map.GetLength(1)];

        for (int x = 0; x < map.GetLength(0); x++)
        {
            for (int y = 0; y < map.GetLength(1); y++)
            {
                if(mapFlags[x,y] == 0)
                {
                    List<Coord> newRegion = GetRegionTiles(map, x, y);
                    regions.Add(newRegion);

                    foreach(Coord c in newRegion)
                    {
                        mapFlags[c.tileX, c.tileY] = 1;
                    }
                }
            }
        }

        return regions;
    }

    List<Coord> GetRegionTiles(Tile[,] map, int startX, int startY)
    {
        List<Coord> coords = new List<Coord>();
        Queue<Coord> coordQ = new Queue<Coord>();
        int[,] mapFlags = new int[map.GetLength(0), map.GetLength(1)];

        coordQ.Enqueue(new Coord(startX, startY));
        mapFlags[startX, startY] = 1;

        while(coordQ.Count > 0)
        {
            Coord c = coordQ.Dequeue();
            coords.Add(c);
            Tile[] n = map[c.tileX, c.tileY].neighbors.ToArray();
            for (int i = 0; i < n.Length; i++)
            {
                if (mapFlags[n[i].position.tileX, n[i].position.tileY] == 0)
                {
                    mapFlags[n[i].position.tileX, n[i].position.tileY] = 1;
                    coordQ.Enqueue(new Coord(n[i].position.tileX, n[i].position.tileY));
                }
            }
        }

        return coords;
    }

    void ConnectAreas(List<Area> allAreas, Tile[,] map) // sometimes there are adjesenct areas that are not connected but are through other areas this may be a problem but fine for now.
    {
        foreach (Area aA in allAreas)
        {
            foreach (Area aB in allAreas)
            {
                if (aA == aB)
                {
                    continue;
                }
                if (aA.AreaIsConnected(aB))
                {
                    break;
                }

                for (int iA = 0; iA < aA.edgeTiles.Count; iA++)
                {
                    for (int iB = 0; iB < aB.edgeTiles.Count; iB++)
                    {
                        if (!aA.AreaIsConnected(aB))
                        {
                            if (CheckTileIsAdjescent(aA.edgeTiles[iA], aB.edgeTiles[iB]))
                            {
                                CreateConnection(aA, aB, aA.edgeTiles[iA], aB.edgeTiles[iB], map);

                            }
                        }


                    }
                }
            }

            //if (possibleConnectionFound)
            //{
            //    CreateConnection(bestAreaA, bestAreaB, bestCoordA, bestCoordB);
            //}
        }
    }

    bool CheckTileIsAdjescent(Coord a, Coord b)
    {
        return (a.tileX == b.tileX + 1 && a.tileY == b.tileY) || (a.tileX == b.tileX - 1 && a.tileY == b.tileY) || (a.tileX == b.tileX && a.tileY == b.tileY + 1) || (a.tileX == b.tileX && a.tileY == b.tileY - 1);
    }

    void CreateConnection(Area aA, Area aB, Coord cA, Coord cB, Tile[,] map)
    {
        int codeA = map[cA.tileX, cA.tileY].tileCode;
        int codeB = map[cB.tileX, cB.tileY].tileCode;
        if (cA.tileX == cB.tileX + 1 && cA.tileY == cB.tileY) // b is left of a
        {
            codeA = AlterTileCode(codeA, 2);
            codeB = AlterTileCode(codeB, 0);
        }
        else if (cA.tileX == cB.tileX - 1 && cA.tileY == cB.tileY) // b is right of a 
        {
            codeA = AlterTileCode(codeA, 0);
            codeB = AlterTileCode(codeB, 2);
        }
        else if (cA.tileX == cB.tileX && cA.tileY == cB.tileY + 1) // b is below a
        {
            codeA = AlterTileCode(codeA, 3);
            codeB = AlterTileCode(codeB, 1);
        }
        else if (cA.tileX == cB.tileX && cA.tileY == cB.tileY - 1) // b is above a
        {
            codeA = AlterTileCode(codeA, 1);
            codeB = AlterTileCode(codeB, 3);
        }

        Area.ConnectAreas(aA, aB);
        Tile tA = map[cA.tileX, cA.tileY];
        Tile tB = map[cB.tileX, cB.tileY];
        tA.tileCode = codeA;
        tB.tileCode = codeB;

        tA.AddNeighbors(tB);
        tB.AddNeighbors(tA);
    }

    int AlterTileCode(int code, int sideOfNewExit) // may ned testing due to mgen binary totile code
    {
        int[] binary = Converter.CodeToBinary(code);
        binary[sideOfNewExit] = 1;
        return Converter.BinaryToTileCode(binary);
    }

    public MapProcessor()
    {
    }

}

public class Area
{
    public List<Coord> tiles;
    public List<Coord> edgeTiles;
    public List<Area> connectedAreas;
    public int areaSize;

    public Area()
    {

    }

    public Area(List<Coord> tilesInArea, Tile[,] map)
    {
        tiles = tilesInArea;
        areaSize = tilesInArea.Count;
        connectedAreas = new List<Area>();
        edgeTiles = new List<Coord>();

        foreach (Coord coord in tiles)
        {
            for (int x = coord.tileX - 1; x <= coord.tileX + 1; x++)
            {
                for (int y = coord.tileY - 1; y <= coord.tileY + 1; y++)
                {
                    if (x >= 0 && x < map.GetLength(0) && y >= 0 && y < map.GetLength(1))
                    {
                        if (x == coord.tileX || y == coord.tileY)
                        {
                            if (map[x, y].region != map[coord.tileX, coord.tileY].region)
                            {
                                if (!edgeTiles.Contains(coord))
                                {
                                    edgeTiles.Add(coord);
                                }
                                
                            }
                        }
                    }

                }
            }
        }
       
    }

    public static void ConnectAreas(Area areaA, Area areaB)
    {
        areaA.connectedAreas.Add(areaB);
        areaB.connectedAreas.Add(areaA);

    }
    public bool AreaIsConnected(Area otherArea)
    {
        return connectedAreas.Contains(otherArea);
    }
}

public struct Coord
{
    public int tileX;
    public int tileY;

    public Coord(int x, int y) 
    {
        tileX = x;
        tileY = y;
    }
       

}

