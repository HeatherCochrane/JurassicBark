using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PaddockCreation : MonoBehaviour
{
    EnvironmentTile startTile;

    EnvironmentTile endTile;

    [SerializeField]
    int width = 0;
    [SerializeField]
    int height = 0;


    EnvironmentTile[][] mMap;

    int xPos = 0;
    int zPos = 0;

    Vector2 mapSize;

    EnvironmentTile[,] paddock;

    //Width and height of the paddock being created
    int widthTile;
    int heightTile;

    //Keep track of the paddocks within the area
    List<EnvironmentTile> createdPaddock = new List<EnvironmentTile>();
    List<GameObject> allPaddocks = new List<GameObject>();

    //Fence Objects
    [SerializeField]
    GameObject fenceH;
    [SerializeField]
    GameObject fenceV;
    [SerializeField]
    GameObject cornerPiece;
    [SerializeField]
    GameObject cornerTarget;
    //Stand in fence object - used for instantiating
    GameObject fencePiece;

    //Object for paddocks
    [SerializeField]
    GameObject paddockParent;
    GameObject pParent;

    [SerializeField]
    Text paddockCost;

    [SerializeField]
    Color[] grassColor;

    int fenceCost = 0;
    int finalPaddockCost = 0;

    [SerializeField]
    Currency currency;

    Color temp;

    [SerializeField]
    UIHandler UIhandler;
    List<GameObject> fenceSet = new List<GameObject>();


    int itemCost = 0;
    GameObject item;


    [SerializeField]
    Game game;

    // Start is called before the first frame update
    void Start()
    {
        paddockCost.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //Get the entire map
    public void setMap(EnvironmentTile[][] m, Vector2 mSize)
    {
        mMap = m;
        mapSize = mSize;
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
        calculatePaddockSize();
    }

    public void calculatePaddockCost(EnvironmentTile t)
    {
        width = (int)t.transform.position.x - (int)startTile.transform.position.x;
        height = (int)t.transform.position.z - (int)startTile.transform.position.z;

        width /= 10;
        height /= 10;

        width += 1;
        height += 1;

        paddockCost.gameObject.SetActive(true);
        paddockCost.gameObject.transform.position = Input.mousePosition;
        paddockCost.text = "£" + ((width * height) * fenceCost).ToString() + " " + width + "X" + height;

        if(width < 0 || height < 0)
        {
            paddockCost.text = "INVALID";
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
        paddock = new EnvironmentTile[widthTile, heightTile];

        for (int i = xPos; i < (int)widthTile; i++)
        {
            for (int j = zPos; j < (int)heightTile; j++)
            {
                if (!mMap[i][j].isPath && !mMap[i][j].isEntrance && !mMap[i][j].hasPaint)
                {
                    paddock[x, z] = mMap[i][j];
                    Material[] grass = paddock[x, z].GetComponent<MeshRenderer>().materials;

                    temp = paddock[x, z].GetComponent<MeshRenderer>().material.color;
                    temp.r -= 0.8f;
                    grass[1].color = temp;

                    paddock[x, z].GetComponent<MeshRenderer>().materials = grass;
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
                    if (!mMap[i][j].isPath && !mMap[i][j].isEntrance && !mMap[i][j].hasPaint)
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
    void calculatePaddockSize()
    { 
        createdPaddock.Clear();

        width = (int)endTile.transform.position.x - (int)startTile.transform.position.x;
        height = (int)endTile.transform.position.z - (int)startTile.transform.position.z;

        width /= 10;
        height /= 10;

        width += 1;
        height += 1;

        finalPaddockCost = (width * height) * fenceCost;

        if (currency.sufficientFunds(finalPaddockCost))
        {
            

            if (width >= 2 && height >= 2 && currency.sufficientFunds(finalPaddockCost))
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
                        if (mMap[i][j].isPaddock || mMap[i][j].isPath || mMap[i][j].isEntrance)
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
                    paddock = new EnvironmentTile[widthTile, heightTile];

                    for (int i = xPos; i < (int)widthTile; i++)
                    {
                        for (int j = zPos; j < (int)heightTile; j++)
                        {
                            mMap[i][j].isPaddock = true;
                            paddock[x, z] = mMap[i][j];
                            Material[] grass = paddock[x, z].GetComponent<MeshRenderer>().materials;

                            temp = paddock[x, z].GetComponent<MeshRenderer>().material.color;
                            temp.r -= 0.8f;
                            grass[1].color = temp;

                            paddock[x, z].GetComponent<MeshRenderer>().materials = grass;
                            createdPaddock.Add(mMap[i][j]);
                            z++;
                            standIn = z;
                        }
                        x++;
                        z = 0;
                    }

                    //Fill paddock tiles
                    generateFences(paddock, x, standIn);
                    x = 0;
                    z = 0;


                    currency.subtractMoney(finalPaddockCost);

                }
                else
                {
                    cancelCreation();
                }
            }
        }
        else
        {
            cancelCreation();
        }

        paddockCost.gameObject.SetActive(false);
    }

    void generateFences(EnvironmentTile[,] tiles, int width, int height)
    {
        pParent = Instantiate(paddockParent);

        //Delete Obstacles within the paddock
        for (int i =0; i < width; i++)
        {
            for(int j =0; j < height; j++)
            {
                if(!tiles[i, j].IsAccessible)
                {
                    Destroy(tiles[i, j].transform.GetChild(0).gameObject);
                    tiles[i, j].IsAccessible = true;

                }
                tiles[i, j].isPaddock = true;
                tiles[i, j].transform.parent = pParent.transform;
            }
        }

        allPaddocks.Add(pParent);

        //Spawn corner pieces first
        fencePiece = Instantiate(fenceSet[0]);
        fencePiece.transform.position = new Vector3(tiles[0, height - 1].transform.position.x, tiles[0, height - 1].transform.position.y + 3, tiles[0, height - 1].transform.position.z);
        fencePiece.transform.parent = pParent.transform;
        tiles[0, height - 1].hasFence = true;

        fencePiece = Instantiate(fenceSet[0]);
        fencePiece.transform.position = new Vector3(tiles[width - 1, height - 1].transform.position.x, tiles[width - 1, height - 1].transform.position.y + 3, tiles[width - 1, height - 1].transform.position.z + 10);
        fencePiece.transform.Rotate(new Vector3(0, 1, 0), 90);
        fencePiece.transform.parent = pParent.transform;
        tiles[width - 1, height - 1].hasFence = true;

        fencePiece = Instantiate(fenceSet[0]);
        fencePiece.transform.position = new Vector3(tiles[width - 1, 0].transform.position.x + 10, tiles[width - 1, 0].transform.position.y + 3, tiles[width - 1, 0].transform.position.z + 10);
        fencePiece.transform.Rotate(new Vector3(0, 1, 0), 180);
        fencePiece.transform.parent = pParent.transform;
        tiles[width - 1, 0].hasFence = true;

        fencePiece = Instantiate(fenceSet[1]);
        fencePiece.transform.position = new Vector3(tiles[0, 0].transform.position.x + 10, tiles[0, 0].transform.position.y + 3, tiles[0, 0].transform.position.z);
        fencePiece.transform.Rotate(new Vector3(0, 1, 0), 270);
        fencePiece.transform.parent = pParent.transform;
        fencePiece.GetComponentInChildren<PaddockControl>().setTiles(tiles, width, height);
        tiles[0, 0].hasFence = true;


        //x = width, y = height
        //Height
        for (int i = 0; i < height; i++)
        {
            if (!tiles[0, i].hasFence)
            {
                //tiles[0, i].transform.position = new Vector3(tiles[0, i].transform.position.x, tiles[0, i].transform.position.y + 1, tiles[0, i].transform.position.z);
                fencePiece = Instantiate(fenceSet[2]);
                fencePiece.transform.position = new Vector3(tiles[0, i].transform.position.x, tiles[0, i].transform.position.y + 3, tiles[0, i].transform.position.z + 5);
                fencePiece.transform.parent = pParent.transform;
                tiles[0, i].hasFence = true;
            }
            if (!tiles[width - 1, i].hasFence)
            {
                //tiles[width - 1, i].transform.position = new Vector3(tiles[width - 1, i].transform.position.x, tiles[width - 1, i].transform.position.y + 1, tiles[width - 1, i].transform.position.z);
                fencePiece = Instantiate(fenceSet[2]);
                fencePiece.transform.position = new Vector3(tiles[width - 1, i].transform.position.x + 10, tiles[width - 1, i].transform.position.y + 3, tiles[width - 1, i].transform.position.z + 5);
                fencePiece.transform.parent = pParent.transform;
                tiles[width - 1, i].hasFence = true;
            }
        }

        //Width
        for (int i = 0; i < width; i++)
        {
            if (!tiles[i, 0].hasFence)
            {
                //tiles[i, 0].transform.position = new Vector3(tiles[i, 0].transform.position.x, tiles[i, 0].transform.position.y + 1, tiles[i, 0].transform.position.z);
                fencePiece = Instantiate(fenceSet[3]);
                fencePiece.transform.position = new Vector3(tiles[i, 0].transform.position.x + 5, tiles[i, 0].transform.position.y + 3, tiles[i, 0].transform.position.z);
                fencePiece.transform.parent = pParent.transform;
                tiles[i, 0].hasFence = true;
            }
            if (!tiles[i, height - 1].hasFence)
            {
                //tiles[i, height - 1].transform.position = new Vector3(tiles[i, height - 1].transform.position.x, tiles[i, height - 1].transform.position.y + 1, tiles[i, height - 1].transform.position.z);
                fencePiece = Instantiate(fenceSet[3]);
                fencePiece.transform.position = new Vector3(tiles[i, height - 1].transform.position.x + 5, tiles[i, height - 1].transform.position.y + 3, tiles[i, height - 1].transform.position.z + 10);
                fencePiece.transform.parent = pParent.transform;
                tiles[i, height - 1].hasFence = true;
            }
        }

        setMapColour();

    }

    public void cancelCreation()
    {
        paddockCost.gameObject.SetActive(false);
        setMapColour();

        startTile = null;
        endTile = null;
    }

    public void setMapColour()
    {
        for(int i = 0; i < mapSize.x; i++)
        {
            for (int j = 0; j < mapSize.y; j++)
            {
                if (!mMap[i][j].isPath && !mMap[i][j].isEntrance && !mMap[i][j].hasPaint)
                {
                    Material[] mat = mMap[i][j].GetComponent<MeshRenderer>().materials;

                    temp = mMap[i][j].GetComponent<MeshRenderer>().material.color;
                    temp.r = 0.6f;
                    mat[1].color = temp;

                    mMap[i][j].GetComponent<MeshRenderer>().materials = mat;
                }
                //else
                //{
                //    Material[] mat = mMap[i][j].GetComponent<MeshRenderer>().materials;

                //    temp = mMap[i][j].GetComponent<MeshRenderer>().material.color;
                //    temp = Color.white;
                //    mat[1].color = temp;

                //    mMap[i][j].GetComponent<MeshRenderer>().materials = mat;
                //}
            }
        }

    }

    public void setFencePieces(int button)
    {
        fenceSet = UIhandler.getFencePieces(button);
        fenceCost = UIhandler.getFenceCost(button);
    }

    public void setItemCost(int button)
    {
        itemCost = UIhandler.getItemCost(button);
        item = UIhandler.getItem(button);
    }

    public List<GameObject> getPaddocks()
    {
        return allPaddocks;
    }
}
