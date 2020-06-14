using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Environment : MonoBehaviour
{

    [SerializeField] private List<EnvironmentTile> AccessibleTiles;
    [SerializeField] private List<EnvironmentTile> InaccessibleTiles;
    [SerializeField] private Vector2Int Size;
    [SerializeField] private float AccessiblePercentage;

    private EnvironmentTile[][] mMap;
    private List<EnvironmentTile> mAll;
    private List<EnvironmentTile> mToBeTested;
    private List<EnvironmentTile> mLastSolution;

    private readonly Vector3 NodeSize = Vector3.one * 9.0f;
    private const float TileSize = 10.0f;
    private const float TileHeight = 2.5f;

    public EnvironmentTile Start { get; private set; }

    [SerializeField]
    PaddockCreation paddock;

    [SerializeField]
    PathHandler pathHandler;

    [SerializeField]
    VisitorHandler visitorHandler;

    [SerializeField]
    Game game;

    [SerializeField]
    Material[] entranceMat;

    List<EnvironmentTile> entranceTiles = new List<EnvironmentTile>();

    [SerializeField]
    GameObject archway;

    GameObject spawner;

    [SerializeField]
    CameraControl camera;


    private void Awake()
    {
        Random.InitState(10);
        
        mAll = new List<EnvironmentTile>();
        mToBeTested = new List<EnvironmentTile>();
    }

    private void Update()
    {
    }

    public Vector2 getMapSize()
    {
        return Size;
    }

    public EnvironmentTile[][] getMap()
    {
        return mMap;
    }
    private void OnDrawGizmos()
    {
        // Draw the environment nodes and connections if we have them
        if (mMap != null)
        {
            for (int x = 0; x < Size.x; ++x)
            {
                for (int y = 0; y < Size.y; ++y)
                {
                    if (mMap[x][y].Connections != null)
                    {
                        for (int n = 0; n < mMap[x][y].Connections.Count; ++n)
                        {
                            Gizmos.color = Color.blue;
                            Gizmos.DrawLine(mMap[x][y].Position, mMap[x][y].Connections[n].Position);
                        }
                    }

                    // Use different colours to represent the state of the nodes
                    Color c = Color.white;
                    if (!mMap[x][y].IsAccessible)
                    {
                        c = Color.red;
                    }
                    else
                    {
                        if (mLastSolution != null && mLastSolution.Contains(mMap[x][y]))
                        {
                            c = Color.green;
                        }
                        else if (mMap[x][y].Visited)
                        {
                            c = Color.yellow;
                        }
                    }

                    Gizmos.color = c;
                    Gizmos.DrawWireCube(mMap[x][y].Position, NodeSize);
                }
            }
        }
    }

    private void Generate()
    {
        // Setup the map of the environment tiles according to the specified width and height
        // Generate tiles from the list of accessible and inaccessible prefabs using a random
        // and the specified accessible percentage
        mMap = new EnvironmentTile[Size.x][];

        int halfWidth = Size.x / 2;
        int halfHeight = Size.y / 2;
        Vector3 position = new Vector3(-(halfWidth * TileSize), 0.0f, -(halfHeight * TileSize));
        bool start = true;

        for (int x = 0; x < Size.x; ++x)
        {
            mMap[x] = new EnvironmentTile[Size.y];
            for (int y = 0; y < Size.y; ++y)
            {
                bool isAccessible = start || Random.value < AccessiblePercentage;
                List<EnvironmentTile> tiles = isAccessible ? AccessibleTiles : InaccessibleTiles;
                EnvironmentTile prefab = tiles[Random.Range(0, tiles.Count)];
                EnvironmentTile tile = Instantiate(prefab, position, Quaternion.identity, transform);
                tile.Position = new Vector3(position.x + (TileSize / 2), TileHeight, position.z + (TileSize / 2));
                tile.IsAccessible = isAccessible;
                tile.gameObject.name = x.ToString() + "," + y.ToString();
                mMap[x][y] = tile;
                mAll.Add(tile);

                if (start)
                {
                    Start = tile;
                }

                position.z += TileSize;
                start = false;


            }

            position.x += TileSize;
            position.z = -(halfHeight * TileSize);
        }

        paddock.setMap(mMap, Size);
        pathHandler.setMap(mMap, Size);
        visitorHandler.setSpawnPoint(mMap[halfWidth][0]);
        game.checkSpawnTile(mMap[halfWidth][0]);

        setEntrance(halfWidth);
    }

    public void loadChangedTiles()
    {
        Debug.Log("Called");

        GameObject newChild;

        SaveGame.Load();

        for (int i = 0; i < Size.x; i++)
        {
            for (int j = 0; j < Size.y; j++)
            {
                for (int k = 0; k < SaveGame.Instance.changedTile.Count; k++)
                {
                    //Tile currently on equals one of the saved tiles
                    if (SaveGame.Instance.changedTile[k].x == i && SaveGame.Instance.changedTile[k].y == j)
                    {
                        mMap[i][j].IsAccessible = SaveGame.Instance.changedTile[k].isAccesible;
                        mMap[i][j].isPaddock = SaveGame.Instance.changedTile[k].isPaddock;
                        mMap[i][j].isPath = SaveGame.Instance.changedTile[k].isPath;
                        mMap[i][j].hasPaint = SaveGame.Instance.changedTile[k].hasPaint;

                        if (SaveGame.Instance.changedTile[k].matChanged)
                        {
                            Material[] mats = mMap[i][j].GetComponent<MeshRenderer>().materials;
                            Debug.Log(SaveGame.Instance.changedTile[k].parentMat);

                            Material newMat = Resources.Load(SaveGame.Instance.changedTile[k].parentMat, typeof(Material)) as Material;
                            Debug.Log(newMat);
                            mats[1] = newMat;

                            mMap[i][j].GetComponent<MeshRenderer>().materials = mats;
                            mMap[i][j].setTerrainPaint(SaveGame.Instance.changedTile[k].parentMat);
                        }

                        if (!SaveGame.Instance.changedTile[k].removeChild && !SaveGame.Instance.changedTile[k].matChanged)
                        {
                            newChild = Instantiate(Resources.Load(SaveGame.Instance.changedTile[k].childModel) as GameObject);
                            newChild.transform.parent = mMap[i][j].gameObject.transform;
                            newChild.transform.position = new Vector3(mMap[i][j].transform.position.x + 5, mMap[i][j].transform.position.y + 3, mMap[i][j].transform.position.z + 5);
                        }
                        else
                        {
                            if (mMap[i][j].transform.childCount > 0)
                            {
                                Destroy(mMap[i][j].transform.GetChild(0).gameObject);
                            }
                        }
                    }
                }
            }
        }
    }

    void setEntrance(int w)
    {
        w -= 2;

        for (int i = 0; i < 3; i++)
        {
            for (int j = w; j < w + 5; j++)
            {
                Material[] grass = mMap[j][i].GetComponent<MeshRenderer>().materials;

                Material temp = entranceMat[0];
                grass[1] = temp;

                mMap[j][i].GetComponent<MeshRenderer>().materials = grass;
                mMap[j][i].isPath = true;

                if (!mMap[j][i].IsAccessible)
                {
                    Destroy(mMap[j][i].transform.GetChild(0).gameObject);
                }

                if (i > 0)
                {
                    entranceTiles.Add(mMap[j][i]);
                }
            }
        }

        //Spawn Front entrance
        spawner = Instantiate(archway);
        spawner.transform.position = new Vector3(entranceTiles[2].transform.position.x + 5, entranceTiles[2].transform.position.y + 3, entranceTiles[2].transform.position.z - 5);

    }

    public List<EnvironmentTile> getEntrance()
    {
        return entranceTiles;
    }
    private void SetupConnections()
    {
        // Currently we are only setting up connections between adjacnt nodes
        for (int x = 0; x < Size.x; ++x)
        {
            for (int y = 0; y < Size.y; ++y)
            {
                EnvironmentTile tile = mMap[x][y];
                tile.Connections = new List<EnvironmentTile>();
                if (x > 0)
                {
                    tile.Connections.Add(mMap[x - 1][y]);
                }

                if (x < Size.x - 1)
                {
                    tile.Connections.Add(mMap[x + 1][y]);
                }

                if (y > 0)
                {
                    tile.Connections.Add(mMap[x][y - 1]);
                }

                if (y < Size.y - 1)
                {
                    tile.Connections.Add(mMap[x][y + 1]);
                }
            }
        }
    }

    private float Distance(EnvironmentTile a, EnvironmentTile b)
    {
        // Use the length of the connection between these two nodes to find the distance, this 
        // is used to calculate the local goal during the search for a path to a location
        float result = float.MaxValue;
        EnvironmentTile directConnection = a.Connections.Find(c => c == b);
        if (directConnection != null)
        {
            result = TileSize;
        }
        return result;
    }

    private float Heuristic(EnvironmentTile a, EnvironmentTile b)
    {
        // Use the locations of the node to estimate how close they are by line of sight
        // experiment here with better ways of estimating the distance. This is used  to
        // calculate the global goal and work out the best order to prossess nodes in
        return Vector3.Distance(a.Position, b.Position);
    }

    public void GenerateWorld()
    {
        Generate();
        SetupConnections();
    }

    public void CleanUpWorld()
    {
        if (mMap != null)
        {
            for (int x = 0; x < Size.x; ++x)
            {
                for (int y = 0; y < Size.y; ++y)
                {
                    Destroy(mMap[x][y].gameObject);
                }
            }
        }
    }

    public List<EnvironmentTile> Solve(EnvironmentTile begin, EnvironmentTile destination, int characterType)
    {
        List<EnvironmentTile> result = null;
        if (begin != null && destination != null)
        {
            // Nothing to solve if there is a direct connection between these two locations
            EnvironmentTile directConnection = begin.Connections.Find(c => c == destination);
            if (directConnection == null)
            {
                // Set all the state to its starting values
                mToBeTested.Clear();

                for (int count = 0; count < mAll.Count; ++count)
                {
                    mAll[count].Parent = null;
                    mAll[count].Global = float.MaxValue;
                    mAll[count].Local = float.MaxValue;
                    mAll[count].Visited = false;
                }

                // Setup the start node to be zero away from start and estimate distance to target
                EnvironmentTile currentNode = begin;
                currentNode.Local = 0.0f;
                currentNode.Global = Heuristic(begin, destination);

                // Maintain a list of nodes to be tested and begin with the start node, keep going
                // as long as we still have nodes to test and we haven't reached the destination
                mToBeTested.Add(currentNode);

                while (mToBeTested.Count > 0 && currentNode != destination)
                {
                    // Begin by sorting the list each time by the heuristic
                    mToBeTested.Sort((a, b) => (int)(a.Global - b.Global));

                    // Remove any tiles that have already been visited
                    mToBeTested.RemoveAll(n => n.Visited);

                    // Check that we still have locations to visit
                    if (mToBeTested.Count > 0)
                    {
                        // Mark this note visited and then process it
                        currentNode = mToBeTested[0];
                        currentNode.Visited = true;

                        // Check each neighbour, if it is accessible and hasn't already been 
                        // processed then add it to the list to be tested 
                        for (int count = 0; count < currentNode.Connections.Count; ++count)
                        {
                            EnvironmentTile neighbour = currentNode.Connections[count];

                            //if (!neighbour.Visited && neighbour.IsAccessible)
                            //{
                            //    mToBeTested.Add(neighbour);
                            //}

                            if (characterType == 0)
                            {
                                if (!neighbour.Visited && neighbour.IsAccessible && !neighbour.isPaddock)
                                {
                                    mToBeTested.Add(neighbour);
                                }
                            }
                            //Visitors
                            else if (characterType == 1)
                            {
                                if (!neighbour.Visited && neighbour.isPath && neighbour.IsAccessible)
                                {
                                    mToBeTested.Add(neighbour);
                                }
                            }
                            //Dogs
                            else if (characterType == 2)
                            {
                                if (!neighbour.Visited && neighbour.isPaddock && neighbour.IsAccessible)
                                {
                                    mToBeTested.Add(neighbour);
                                }
                            }

                            // Calculate the local goal of this location from our current location and 
                            // test if it is lower than the local goal it currently holds, if so then
                            // we can update it to be owned by the current node instead 
                            float possibleLocalGoal = currentNode.Local + Distance(currentNode, neighbour);
                            if (possibleLocalGoal < neighbour.Local)
                            {
                                neighbour.Parent = currentNode;
                                neighbour.Local = possibleLocalGoal;
                                neighbour.Global = neighbour.Local + Heuristic(neighbour, destination);
                            }
                        }
                    }
                }

                // Build path if we found one, by checking if the destination was visited, if so then 
                // we have a solution, trace it back through the parents and return the reverse route
                if (destination.Visited)
                {
                    result = new List<EnvironmentTile>();
                    EnvironmentTile routeNode = destination;

                    while (routeNode.Parent != null)
                    {
                        result.Add(routeNode);
                        routeNode = routeNode.Parent;
                    }
                    result.Add(routeNode);
                    result.Reverse();
                }
                else
                {
                    Debug.LogWarning("Path Not Found");
                }
            }
            else
            {
                result = new List<EnvironmentTile>();
                result.Add(begin);
                result.Add(destination);
            }
        }
        else
        {
            Debug.LogWarning("Cannot find path for invalid nodes");
        }

        mLastSolution = result;

        return result;
    }
}
