using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(MapGenerator))]
public class MapEditor :Editor
{


    public  void OnSceneGUI()
    {
        var mgen = (MapGenerator)target;

        if (Application.isPlaying && mgen.codemapCompleted && mgen.showMapDebug)
        {
           
            Tile[,] map = mgen.tileMap;
            for (int x = 0; x < map.GetLength(0); x++)
            {
                for (int y = 0; y < map.GetLength(1); y++)
                {
                    
                    Handles.color = Color.white;
                    
                    int[] i = map[x, y].tileCode;
                  
                    if (i[0] == 1)
                    {
                        Handles.DrawLine(new Vector3(x, 0, y), new Vector3(x + 0.5f, 0, y)); // right
                    }
                    if (i[1] == 1)
                    {
                        Handles.DrawLine(new Vector3(x, 0, y), new Vector3(x, 0, y + 0.5f)); // top
                    }
                    if (i[2] == 1)
                    {
                        Handles.DrawLine(new Vector3(x, 0, y), new Vector3(x - 0.5f, 0, y)); // left
                    }
                    if (i[3] == 1)
                    {
                        Handles.DrawLine(new Vector3(x, 0, y), new Vector3(x, 0, y - 0.5f)); // bottom
                    }

                    switch (mgen.tileMap[x, y].region)
                    {
                        case 0:
                            Handles.color = Color.red;
                            break;
                        case 1:
                            Handles.color = Color.blue;
                            break;
                        case 2:
                            Handles.color = Color.yellow;
                            break;
                        case 3:
                            Handles.color = Color.green;
                            break;
                        case 4:
                            Handles.color = Color.grey;
                            break;
                        default:
                            Handles.color = Color.black;
                            break;
                    }
                    
                       
                        Handles.CubeHandleCap(0, new Vector3(x, 0f, y), mgen.transform.rotation, 0.5f, EventType.Repaint);
                    
                        

                }
            }
           
        }


    }
}
