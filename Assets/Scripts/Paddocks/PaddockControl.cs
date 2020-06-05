using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PaddockControl : MonoBehaviour
{
    EnvironmentTile[,] tiles;
    int width = 0;
    int height = 0;

    [SerializeField]
    Color[] grassColor;


    Game game;

    [SerializeField]
    GameObject paddockUI;
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
    // Start is called before the first frame update
    void Start()
    {
        game = GameObject.Find("Game").GetComponent<Game>();
        UICanvas = GameObject.Find("GameUI");
        inventory = GameObject.Find("InventoryUI").GetComponent<Inventory>();
        paddockMap = GameObject.Find("PaddockHandler").GetComponent<PaddockCreation>();

        //Show the paddock stats on the game canvas
        paddockUI = Instantiate(paddockUI);
        paddockUI.transform.parent = UICanvas.transform;
        paddockUI.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if(showPaddockUI)
        {
            paddockUI.SetActive(true);
            paddockUI.transform.position = new Vector3(Input.mousePosition.x, Input.mousePosition.y + 10, Input.mousePosition.z);
        }
        else
        {
            paddockUI.SetActive(false);
        }
    }

    public void setTiles(EnvironmentTile[,] t, int w, int h)
    {
        tiles = t;
        width = w;
        height = h;
    }

    void returnTiles()
    {
        for(int i = 0; i < width; i++)
        {
            for(int j = 0; j < height; j++)
            {
                tiles[i, j].isPaddock = false;
                tiles[i, j].hasFence = false;
                tiles[i, j].IsAccessible = true;

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

            Destroy(this.transform.parent.parent.gameObject);
            game.setDeleting(false);
            Destroy(paddockUI);
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

    public void placeFoodBowl(EnvironmentTile pos)
    {
        bowl = Instantiate(foodBowl);
       
        bowl.transform.position = new Vector3(pos.Position.x, pos.Position.y, pos.Position.z);

        pos.IsAccessible = false;

        game.setPlacingFood(false);
    }

    public void placeWaterBowl(EnvironmentTile pos)
    {
        bowl = Instantiate(waterBowl);

        bowl.transform.position = new Vector3(pos.Position.x, pos.Position.y, pos.Position.z);

        pos.IsAccessible = false;

        game.setPlacingWater(false);
    }

    void updatePaddockInfo()
    {
        
        //Reset values
        overallHappiness = 0;
        overallHunger = 0;
        overallThirst = 0;

        //First child handles the number of dogs within the paddock
        paddockUI.transform.GetChild(0).GetComponent<Text>().text = "Number of dogs: " + dogsInPaddock.Count.ToString();

        if (dogsInPaddock.Count > 0)
        {
            //Second child handles happiness
            for (int i = 0; i < dogsInPaddock.Count; i++)
            {
                overallHappiness += dogsInPaddock[i].GetComponentInChildren<DogBehaviour>().getHappiness();
                overallHunger += dogsInPaddock[i].GetComponentInChildren<DogBehaviour>().getHunger();
                overallThirst += dogsInPaddock[i].GetComponentInChildren<DogBehaviour>().getThirst();
            }

            overallHappiness = overallThirst / dogsInPaddock.Count;
            overallHunger = overallHunger / dogsInPaddock.Count;
            overallThirst = overallThirst / dogsInPaddock.Count;
        }

        paddockUI.transform.GetChild(1).GetComponent<Text>().text = "Overall Happiness: " + overallHappiness.ToString();
        paddockUI.transform.GetChild(2).GetComponent<Text>().text = "Overall Hunger: " + overallHunger.ToString();
        paddockUI.transform.GetChild(3).GetComponent<Text>().text = "Overall Thirst: " + overallThirst.ToString();

    }
    public void addDog(GameObject d, EnvironmentTile current)
    {
        d.GetComponent<DogBehaviour>().givePaddockSize(tiles, width, height, current);
        d.GetComponent<DogBehaviour>().givePaddockControl(this.gameObject);
        dogsInPaddock.Add(d);
        updatePaddockInfo();
    }

    public void removeDog(GameObject d)
    {
        dogsInPaddock.Remove(d);
        updatePaddockInfo();
        Debug.Log(dogsInPaddock.Count);
    }
}
