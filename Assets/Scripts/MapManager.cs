using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MapManager : MonoBehaviour
{

    public int[,] LoadMap()
    {
        int[,] map = new int[,] { };
        //1:player 2:box 3:goal 4:wall
        switch (SceneManager.GetActiveScene().name)
        {
            case "Stage01":
                map = new int[,]
                {
                    {4,4,4,4,4,4,4},
                    {4,0,0,0,0,0,4},
                    {4,0,3,1,3,0,4},
                    {4,0,0,2,0,0,4},
                    {4,0,2,3,2,0,4},
                    {4,0,0,0,0,0,4},
                    {4,4,4,4,4,4,4},
                };
                break;
            case "Stage02":
                map = new int[,]
                {
                    {4,4,4,4,4,4,4},
                    {4,0,0,0,0,0,4},
                    {4,3,0,1,0,3,4},
                    {4,0,0,2,0,0,4},
                    {4,0,2,0,2,0,4},
                    {4,0,0,3,0,0,4},
                    {4,4,4,4,4,4,4},
                };
                break;
            case "Stage03":
                map = new int[,]
                {
                    {4,4,4,4,4,4},
                    {4,3,3,3,0,4},
                    {4,0,0,2,0,4},
                    {4,0,4,2,4,4},
                    {4,0,0,2,0,4},
                    {4,0,0,1,0,4},
                    {4,4,4,4,4,4},
                };
                break;
        }
        return map;
    }
}
