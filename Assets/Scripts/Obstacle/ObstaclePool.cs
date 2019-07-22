using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstaclePool 
{
    private static ObstaclePool _instance;

    public List<GameObject> CommonObstacles;
    public List<GameObject> SpecialObstacles;

    public List<GameObject> SpaceObstacles;

    private ObstaclePool()
    {
        GameObject winJar = Resources.Load("WinJar") as GameObject;
        GameObject door = Resources.Load("Door") as GameObject;
        GameObject tower = Resources.Load("Tower") as GameObject;
        GameObject tree = Resources.Load("Tree") as GameObject;
        GameObject lily = Resources.Load("Lily") as GameObject;
        GameObject leaf = Resources.Load("Leaf") as GameObject;
        GameObject fish = Resources.Load("Fish") as GameObject;
        for(int i = 0; i < 10; ++i)
        if (Instance == null)
            _instance = this;
    }

    public static ObstaclePool Instance
    {
        get
        {
            if (_instance == null)
                return new ObstaclePool();
            return _instance;
        }
    }
    
}
