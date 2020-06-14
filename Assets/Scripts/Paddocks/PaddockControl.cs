﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PaddockControl : MonoBehaviour
{

    EnvironmentTile[,] tiles;
    [SerializeField]
    int width = 0;
    [SerializeField]
    int height = 0;

    [SerializeField]
    Color[] grassColor;


    Game game;

    [SerializeField]
    GameObject paddockUI;
    GameObject paddockui;
    bool showPaddockUI = false;

    GameObject UICanvas;

    bool hasFoodBowl { get; set; }
    bool hasWaterBowl { get; set; }

    [SerializeField]
    GameObject foodBowl;
    [SerializeField]
    GameObject waterBowl;

    GameObject bowl;

    [SerializeField]
    List<GameObject> dogsInPaddock = new List<GameObject>();
    int overallHappiness = 0;
    int overallHunger = 0;
    int overallThirst = 0;

    Inventory inventory;

    PaddockCreation paddockMap;

    int maxDogCount = 0;

    public bool hasWater{ get; set; }
    public bool hasFood { get; set; }


    AudioManager audioManager;

    GameObject controlObj;

    int paddockIdentifier = 0;
    // Start is called before the first frame update
    void Start()
    {
        game = GameObject.Find("Game").GetComponent<Game>();
        UICanvas = GameObject.Find("GameUI");
        inventory = GameObject.Find("InventoryUI").GetComponent<Inventory>();
        paddockMap = GameObject.Find("PaddockHandler").GetComponent<PaddockCreation>();
        audioManager = GameObject.Find("AudioManager").GetComponent<AudioManager>();

        //Show the paddock stats on the game canvas
        paddockui = Instantiate(paddockUI);
        paddockui.transform.SetParent(UICanvas.transform);
        paddockui.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if(showPaddockUI)
        {
            paddockui.SetActive(true);
            paddockui.transform.position = new Vector3(Input.mousePosition.x, Input.mousePosition.y + 10, Input.mousePosition.z);
        }
        else
        {
            paddockui.SetActive(false);
        }
    }

    public void setTiles(EnvironmentTile[,] t, int w, int h)
    {
        tiles = t;
        width = w;
        height = h;
        
        maxDogCount = (width + height) / 2;

    }

    void returnTiles()
    {
        for(int i = 0; i < width; i++)
        {
            for(int j = 0; j < height; j++)
            {
                if (tiles[i, j].transform.childCount > 0)
                {
                    Destroy(tiles[i, j].transform.GetChild(0).gameObject);
                }

                tiles[i, j].isPaddock = false;
                tiles[i, j].hasFence = false;
                tiles[i, j].IsAccessible = true;
                tiles[i, j].hasFoodBowl = false;
                tiles[i, j].hasWaterBowl = false;
                tiles[i, j].transform.parent = GameObject.Find("Environment").transform;
            }
        }

        paddockMap.setMapColour();
    }
    private void OnMouseDown()
    {
        if (game.getDeleting())
        {
            returnTiles();

            if (dogsInPaddock.Count > 0)
            {
                for (int i = 0; i < dogsInPaddock.Count; i++)
                {
                    if (!inventory.isDogInventoryFull())
                    {
                        inventory.storeDog(dogsInPaddock[i]);
                    }
                    else
                    {
                        Debug.Log("INVENTORY NOW FULL");
                        break;
                    }
                }
            }

            paddockMap.removePaddock(this.transform.parent.parent.gameObject);
            audioManager.playDestroy();
            Destroy(paddockui);
            Destroy(this.transform.parent.parent.gameObject);

        }
        else if (!game.getDeleting())
        {
            showPaddockUI = true;
        }
    }

    private void OnMouseExit()
    {
        showPaddockUI = false;
    }


    public void updatePaddockInfo()
    {
        //Reset values
        overallHappiness = 0;
        overallHunger = 0;
        overallThirst = 0;

        //First child handles the number of dogs within the paddock
        paddockui.transform.GetChild(0).GetComponent<Text>().text = "Number of dogs: " + dogsInPaddock.Count.ToString();

        if (dogsInPaddock.Count > 0)
        {
            //Second child handles happiness
            for (int i = 0; i < dogsInPaddock.Count; i++)
            {
                overallHappiness += dogsInPaddock[i].GetComponentInChildren<DogBehaviour>().getHappiness();
                overallHunger += dogsInPaddock[i].GetComponentInChildren<DogBehaviour>().getHunger();
                overallThirst += dogsInPaddock[i].GetComponentInChildren<DogBehaviour>().getThirst();
            }

            overallHappiness = overallHappiness / dogsInPaddock.Count;
            overallHunger = overallHunger / dogsInPaddock.Count;
            overallThirst = overallThirst / dogsInPaddock.Count;
        }

        paddockui.transform.GetChild(1).GetComponent<Text>().text = "Overall Happiness: " + overallHappiness.ToString();
        paddockui.transform.GetChild(2).GetComponent<Text>().text = "Overall Hunger: " + overallHunger.ToString();
        paddockui.transform.GetChild(3).GetComponent<Text>().text = "Overall Thirst: " + overallThirst.ToString();

    }
    public void addDog(GameObject d, EnvironmentTile current)
    {
        d.GetComponent<DogBehaviour>().givePaddockSize(tiles, width, height, current);
        d.GetComponent<DogBehaviour>().givePaddockControl(this.gameObject);
        dogsInPaddock.Add(d);
        updatePaddockInfo();
    }

    public void addLoadedDog(GameObject d)
    {
        dogsInPaddock.Add(d);
    }
    public void removeDog(GameObject d)
    {
        dogsInPaddock.Remove(d);
        updatePaddockInfo();
        Debug.Log(dogsInPaddock.Count);
    }

    public bool canPlaceDog()
    {
        if(dogsInPaddock.Count == maxDogCount)
        {
            return false;
        }
        else
        {
            return true;
        }
    }

    public List<GameObject> getDogs()
    {
        return dogsInPaddock;
    }

    public void setControlObject(GameObject c)
    {
        controlObj = c;
    }

    public GameObject getControlObject()
    {
        return controlObj;
    }

    public void setIdentifier(int set)
    {
        paddockIdentifier = set;
    }

    public int getPaddockIdentifier()
    {
        return paddockIdentifier;
    }

    public void setTiles(EnvironmentTile[,] t)
    {
        tiles = t;
    }
    public EnvironmentTile[,] getTiles()
    {
        return tiles;
    }
}
