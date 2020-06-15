using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PathHandler : MonoBehaviour
{
    [SerializeField]
    Environment environment;

    EnvironmentTile[][] mMap;

    EnvironmentTile startTile;
    EnvironmentTile endTile;

    Vector2 mapSize;

    int width = 0;
    int height = 0;

    int xPos = 0;
    int zPos = 0;

    
    public List<EnvironmentTile> createdPath = new List<EnvironmentTile>();

    int widthTile = 0;
    int heightTile = 0;

    EnvironmentTile[,] path;
    Color temp;
    Material pathType;

    [SerializeField]
    Text pathCost;
    int pathTypeCost = 0;

    [SerializeField]
    UIHandler UIhandler;

    int buttonPressed = 0;

    [SerializeField]
    VisitorHandler visitorHandler;

    [SerializeField]
    SaveHandler save;
    // Start is called before the first frame update
    void Start()
    {
        pathCost.gameObject.SetActive(false);
        createdPath = environment.getEntrance();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void setMap(EnvironmentTile[][] map, Vector2 s)
    {
        mMap = map;
        mapSize = s;
    }

    public void setStartingTile(EnvironmentTile s)
    {
        startTile = s;
    }

    public EnvironmentTile getStartingTile()
    {
        return startTile;
    }

    public void setEndTile(EnvironmentTile e)
    {
        endTile = e;
        calculatePathSize();
    }
    public void calculatePathCost(EnvironmentTile t)
    {
        pathCost.gameObject.SetActive(true);

        width = (int)t.transform.position.x - (int)startTile.transform.position.x;
        height = (int)t.transform.position.z - (int)startTile.transform.position.z;

        width /= 10;
        height /= 10;

        width += 1;
        height += 1;

        pathCost.gameObject.SetActive(true);
        pathCost.gameObject.transform.position = Input.mousePosition;
        pathCost.text = "£" + ((width * height) * pathTypeCost).ToString() + " " + width + "X" + height;

        if (width < 0 || height < 0)
        {
            pathCost.text = "INVALID";
        }

        for (int y = 0; y < mapSize.x; y++)
        {
            for (int k = 0; k < mapSize.y; k++)
            {
                if (mMap[y][k] == startTile)
                {
                    xPos = y;
                    zPos = k;
                }
            }
        }

        widthTile = xPos + width;
        heightTile = zPos + height;

        int x = 0;
        int z = 0;
        int standIn = 0;
        path = new EnvironmentTile[widthTile, heightTile];

        for (int i = xPos; i < (int)widthTile; i++)
        {
            for (int j = zPos; j < (int)heightTile; j++)
            {
                if (!mMap[i][j].isPath  && !mMap[i][j].hasPaint)
                {
                    path[x, z] = mMap[i][j];
                    Material[] grass = path[x, z].GetComponent<MeshRenderer>().materials;

                    temp = path[x, z].GetComponent<MeshRenderer>().material.color;
                    temp.r -= 0.8f;
                    grass[1].color = temp;

                    path[x, z].GetComponent<MeshRenderer>().materials = grass;
                    z++;
                    standIn = z;
                }
            }
            x++;
            z = 0;
        }

        for (int i = 0; i < mapSize.x; i++)
        {
            for (int j = 0; j < mapSize.y; j++)
            {
                if (i < xPos || i > xPos + width || j < zPos || j > zPos + height)
                {
                    if (!mMap[i][j].isPath && !mMap[i][j].hasPaint)
                    {
                        Material[] mat = mMap[i][j].GetComponent<MeshRenderer>().materials;

                        temp = mMap[i][j].GetComponent<MeshRenderer>().material.color;
                        temp.r = 0.6f;
                        mat[1].color = temp;

                        mMap[i][j].GetComponent<MeshRenderer>().materials = mat;
                    }
                }
            }
        }
    }
    void calculatePathSize()
    {
        //createdPath.Clear();

        width = (int)endTile.transform.position.x - (int)startTile.transform.position.x;
        height = (int)endTile.transform.position.z - (int)startTile.transform.position.z;

        width /= 10;
        height /= 10;

        width += 1;
        height += 1;

        //finalPaddockCost = (width * height) * fenceCost;

        if (width >= 1 && height >= 1)// && currency.sufficientFunds(finalPaddockCost))
        {
            for (int y = 0; y < mapSize.x; y++)
            {
                for (int k = 0; k < mapSize.y; k++)
                {
                    if (mMap[y][k] == startTile)
                    {
                        xPos = y;
                        zPos = k;
                    }
                }
            }

            widthTile = xPos + width;
            heightTile = zPos + height;

            //Should the paddock be drawn within another paddock, this loop with set the bool to true and the paddock will not be created
            bool intersectingPaddock = false;

            for (int i = xPos; i < (int)widthTile; i++)
            {
                for (int j = zPos; j < (int)heightTile; j++)
                {
                    if (mMap[i][j].isPaddock)
                    {
                        intersectingPaddock = true;
                    }
                }
            }

            if (!intersectingPaddock)
            {
                int x = 0;
                int z = 0;
                int standIn = 0;
                path = new EnvironmentTile[widthTile, heightTile];

                for (int i = xPos; i < (int)widthTile; i++)
                {
                    for (int j = zPos; j < (int)heightTile; j++)
                    {
                        mMap[i][j].isPath = true;
                        path[x, z] = mMap[i][j];
                        
                        Material[] grass = path[x, z].GetComponent<MeshRenderer>().materials;
                        path[x, z].setOriginalMaterial(grass[1].name);
                        pathType = path[x, z].GetComponent<MeshRenderer>().material;
                        pathType = UIhandler.getPathType(buttonPressed);
                        grass[1] = pathType;

                        path[x, z].GetComponent<MeshRenderer>().materials = grass;
                        path[x, z].GetComponent<MeshRenderer>().material.mainTextureScale = new Vector2(1, 1);
                        createdPath.Add(mMap[i][j]);           
                        z++;
                        standIn = z;
                    }
                    x++;
                    z = 0;
                }
                x = 0;
                z = 0;
                //currency.subtractMoney(finalPaddockCost);
            }
            else
            {
                cancelCreation();
            }
            //paddockCost.gameObject.SetActive(false);
        }
        else
        {
            cancelCreation();
        }


        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                save.saveTile(path[i, j], true);
            }
        }

        pathCost.gameObject.SetActive(false);
    }

    public void cancelCreation()
    {
        //paddockCost.gameObject.SetActive(false);
        setMapColour();

        startTile = null;
        endTile = null;
        pathCost.gameObject.SetActive(false);
    }

    public void setMapColour()
    {
        for (int i = 0; i < mapSize.x; i++)
        {
            for (int j = 0; j < mapSize.y; j++)
            {
                if (!mMap[i][j].isPath && !mMap[i][j].hasPaint)
                {
                    Material[] mat = mMap[i][j].GetComponent<MeshRenderer>().materials;

                    temp = mMap[i][j].GetComponent<MeshRenderer>().material.color;
                    temp.r = 0.6f;
                    mat[1].color = temp;

                    mMap[i][j].GetComponent<MeshRenderer>().materials = mat;
                }
               
            }
        }

    }

    public void setTileColor(EnvironmentTile tile)
    {
        Material[] mat = tile.GetComponent<MeshRenderer>().materials;

        temp = tile.GetComponent<MeshRenderer>().material.color;
        temp.r = 0.6f;
        mat[1].color = temp;

        tile.GetComponent<MeshRenderer>().materials = mat;
    }

    public void setPathType(int button)
    {
        pathTypeCost = UIhandler.getPathCost(button);
        buttonPressed = button;
    }

    public List<EnvironmentTile> getAllPaths()
    {
        return createdPath;
    }
}
