using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Visitor : Character
{
    Environment mMap;

    EnvironmentTile[][] map;
    Vector2 mapSize;

    List<EnvironmentTile> paths = new List<EnvironmentTile>();

    PathHandler pathHandler;

    bool leavingPark = false;

    VisitorHandler handler;
    // Start is called before the first frame update
    void Start()
    {
        pathHandler = GameObject.Find("PathHandler").GetComponent<PathHandler>();
        mMap = GameObject.Find("Environment").GetComponent<Environment>();
        handler = GameObject.Find("VisitorHandler").GetComponent<VisitorHandler>();

        map = mMap.getMap();
        mapSize = mMap.getMapSize();
        movement();
    }

    // Update is called once per frame
    void Update()
    {
        if(leavingPark && this.CurrentPosition == map[(int)mMap.getMapSize().x / 2][0])
        {
            Destroy(this.gameObject);
        }
    }

    void movement()
    {
        paths = pathHandler.getAllPaths();

        if (!leavingPark)
        {
            if (paths.Count > 0)
            {
                EnvironmentTile goalTile = paths[Random.Range(0, paths.Count)];
                List<EnvironmentTile> route = mMap.Solve(this.CurrentPosition, goalTile, 1);
                this.GoTo(route);
            }
            else
            {
                Debug.Log("NO PATHS FOUND");
            }

            Invoke("movement", Random.Range(5, 10) / Time.timeScale);
        }
    }

    public void setParkClosed(bool set)
    {
        leavingPark = true;
    }

    public void goToExit(EnvironmentTile t)
    {
        EnvironmentTile goal = t;
        List<EnvironmentTile> route = mMap.Solve(this.CurrentPosition, goal, 0);
        this.GoTo(route);
    }

    public void kill()
    {
        Destroy(this.gameObject);
    }
   
}
