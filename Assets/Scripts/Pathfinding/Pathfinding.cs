using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class Pathfinding : MonoBehaviour
{    
    Grid grid;

    void Awake()
    {       
        grid = GetComponent<Grid>();
    }   

    public void FindPath(PathRequest request, Action<PathResult> callback) //uses heap.cs to find a path between the start position and the end one
    {
        Vector3[] waypoints = new Vector3[0];
        bool pathSuccess = false;

        Node startNode = grid.NodeFromWorldPoint(request.pathStart);
        Node targetNode = grid.NodeFromWorldPoint(request.pathEnd);

        if (startNode.walkable && targetNode.walkable) // if the start and end are both on accssesible places find path otherwise dont bother
        {
            Heap<Node> openSet = new Heap<Node>(grid.MaxSize); //openset are nodes to be evaluated
            HashSet<Node> closedSet = new HashSet<Node>();      // closed set are nodes that have already been evaluated
            openSet.Add(startNode);                             // the start node is first in the open set

            while (openSet.Count > 0)                           //while loop to go through each node to generate path
            {   
                Node currentNode = openSet.RemoveFirst();       //heap.cs to find nodes of lowest cost
                closedSet.Add(currentNode);                     // node has been evaluated so now is moved to the closed set

                if (currentNode == targetNode)                  //if the node being evaluated is the end position then the path ahs been found
                {
                    pathSuccess = true;
                    break;
                }

                foreach (Node neighbour in grid.GetNeighbours(currentNode)) // goes through neibourghing nodes to find one of lowest cost
                {
                    if (!neighbour.walkable || closedSet.Contains(neighbour))   //if the neibough is not walkable or is this node doesn't need to be evaluated
                    {
                        continue;
                    }

                    int newMovementCostToNeighbour = currentNode.gCost + GetDistance(currentNode, neighbour) + neighbour.movementPenalty; //calcuclates cost to move to neighbour taking into account gcost, fcost, hcost and any movment penalties.
                    if (newMovementCostToNeighbour < neighbour.gCost || !openSet.Contains(neighbour))   //if the neighbour has a lowercost update the route
                    {
                        neighbour.gCost = newMovementCostToNeighbour;
                        neighbour.hCost = GetDistance(neighbour, targetNode);
                        neighbour.parent = currentNode;     // parented to keep trck of nodes ino rder to retrace the path

                        if (!openSet.Contains(neighbour))
                        {
                            openSet.Add(neighbour);
                        }
                        else
                        {
                            openSet.UpdateItem(neighbour);
                        }
                    }
                }
            }
        }
        
        
        if (pathSuccess)    //when path is found retrace the path
        {
            waypoints = RetracePath(startNode, targetNode);
            pathSuccess = waypoints.Length > 0;
        }
        callback(new PathResult(waypoints, pathSuccess, request.callback));
    }

    Vector3[] RetracePath(Node startNode, Node endNode)
    {
        List<Node> path = new List<Node>(); // list of nodes that are part of the path to end
        Node currentNode = endNode;

        while (currentNode != startNode) // using parents adds to path
        {
            path.Add(currentNode);
            currentNode = currentNode.parent;
        }

        Vector3[] waypoints = SimplifyPath(path); 
        Array.Reverse(waypoints);
        return waypoints;      
    }

    Vector3[] SimplifyPath(List<Node> path) // simplfies the path to only nodes that change in direction
    {
        List<Vector3> waypoints = new List<Vector3>();
        Vector2 directionOld = Vector2.zero;

        for (int i = 1; i < path.Count; i++)
        {
            Vector2 directionNew = new Vector2(path[i - 1].gridX - path[i].gridX, path[i - 1].gridY - path[i].gridY);
            if(directionNew != directionOld)
            {
                waypoints.Add(path[i].worldPosition);

            }
            directionOld = directionNew;
        }
        return waypoints.ToArray();
    }

    int GetDistance(Node nodeA, Node nodeB)
    {
        int dstX = Mathf.Abs(nodeA.gridX - nodeB.gridX);
        int dstY = Mathf.Abs(nodeA.gridY - nodeB.gridY);

        if (dstX > dstY)
            return 14 * dstY + 10 * (dstX - dstY);
        return 14 * dstX + 10 * (dstY - dstX);
    }
}
