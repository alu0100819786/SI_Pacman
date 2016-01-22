﻿using UnityEngine;
using System.Collections;
using System;

public class AStarMove : MonoBehaviour {

    public GameObject maze;
    public float speed;
    int[] vecinos = new int[4];
    private Vector2 dest = Vector2.zero;
    private Vector2 position = Vector2.zero;

    private ArrayList openList;
    private ArrayList closedList;

    

    // Start
    void Start()
    {
        Debug.Log("Start");
        dest = (Vector2)transform.localPosition;
        position = dest;
        //vecinos = maze.GetComponent<nivel>().getVecinos((int)position.x, (int)position.y);

        FindPath(position , new Vector3(2,2,0));
    }


    // Update
    void FixedUpdate()
    {


        //Mueve el pacman teniendo en cuenta la velocidad
        float step = speed * Time.deltaTime;
        Vector2 dest2 = Vector2.MoveTowards(transform.localPosition, dest, step);
        transform.localPosition = dest2;
        //Si ya ha llegado al destino actualiza los vecinos (solo en las posiciones enteras)
        if ((dest2.x == position.x + 1) || (dest2.x == position.x - 1) || (dest2.y == position.y + 1) || (dest2.y == position.y - 1))
        {
            position = dest;
            if (maze.GetComponent<nivel>().hayPastilla((int)position.x, (int)position.y))
            {
                maze.GetComponent<nivel>().eliminarPastilla((int)position.x, (int)position.y);
                GetComponent<pacmanLogic>().scoreUp(10);
            }
            vecinos = maze.GetComponent<nivel>().getVecinos((int)position.x, (int)position.y);
        }

        // Anima al pacman
        Vector2 dir = dest - (Vector2)transform.localPosition;
        GetComponent<Animator>().SetFloat("DirX", dir.x);
        GetComponent<Animator>().SetFloat("DirY", dir.y);
    }


    // Coste heuristico del aStar
    private static float HeuristicEstimateCost(Vector3 curNode, Vector3 goalNode)
    {
        //Vector3 vecCost = curNode.position - goalNode.position;
        //return  //vecCost.magnitude;
        return Math.Abs(curNode.x - goalNode.x) + Math.Abs(curNode.y - goalNode.y);
       
    }


    public ArrayList FindPath(Vector3 start, Vector3 goal)
    {
        Debug.Log("Empezando FindPath");
        openList = new ArrayList();
        start.z = HeuristicEstimateCost(start, goal) + 0.0f;
        openList.Add(start);

        Debug.Log("(" + start.x + ", " + start.y + ", " + start.z + ")" );
        Debug.Log("(" + goal.x + ", " + goal.y + ", " + goal.z + ")");

        //start.nodeTotalCost = 0.0f;
        //start.estimatedCost = HeuristicEstimateCost(start, goal);
        

        closedList = new ArrayList();
        Vector3 node = Vector3.zero;

        while (openList.Count != 0)
        {
            Debug.Log("OpenList no vacía");
            node = (Vector3)openList[0];

            
            //Check if the current node is the goal node
            if (node.x == goal.x && node.y == goal.y)
            {
                Debug.Log("Final, calculamos RUTA obtenida");
                return CalculatePath(node);
            }

            //Create an ArrayList to store the neighboring nodes
            ArrayList neighbours = new ArrayList();
            // GridManager.instance.GetNeighbours(node, neighbours);

            neighbours = maze.GetComponent<nivel>().getNeighbours((int)position.x, (int)position.y);
            for (int i = 0; i < neighbours.Count; i++)
            {
                Vector3 neighbourNode = (Vector3)neighbours[i];
                if (!closedList.Contains(neighbourNode))
                {


                     float cost = HeuristicEstimateCost(neighbourNode, goal);
                    //Debug.Log("COST: " + cost);
                    /*float totalCost = /*node.nodeTotalCost neighbourNode.z +  cost; */

                    //float neighbourNodeEstCost = HeuristicEstimateCost(neighbourNode, goal);
                    neighbourNode.z /*nodeTotalCost*/ = cost;
                    /*neighbourNode.parent = node;*/
                    /*neighbourNode. estimatedCost = totalCost + neighbourNodeEstCost;*/
                    if (!openList.Contains(neighbourNode))
                    {
                        //Debug.Log("NODE: (" + node.x + ", " + node.y + ", " + node.z + ")");
                        Debug.Log("NEIGHBOUR: (" + neighbourNode.x + ", " + neighbourNode.y + ", " + neighbourNode.z + ")");

                        Debug.Log("Añadimos a Openlist");
                        //openList.Add(neighbourNode);
                    }
                }
            }
            //Push the current node to the closed list
            closedList.Add(node);
            //and remove it from openList
            openList.Remove(node);
        }
        if (node.x != goal.x && node.y != goal.y)
        {
            Debug.LogError("Goal Not Found");
            return null;
        }
        return CalculatePath(node);
    }

    private static ArrayList CalculatePath(Vector3 node)
    {
        ArrayList list = new ArrayList();
        while (node != null)
        {
            list.Add(node);
            //node = node.parent;
        }
        list.Reverse();
        return list;
    }

    public bool edquals(Vector3 o)
    {
        Vector3 x = (Vector3)o;
        if (this.x == o.x) return true;
        return false;
    }

    public override bool Equals(Object obj)
    {
        Vector3 x = (Vector3)o;
        if (this.x == o.x) return true;
        return false;
    }


}

