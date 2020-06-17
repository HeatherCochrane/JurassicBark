using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


//USE THIS SCRIPT TO HANDLE SWITCHING BETWEEN SHOP MENUS, MAIN MENU ETC
public class UIHandler : MonoBehaviour
{
    [System.Serializable]
    struct items
    {
        [SerializeField]
        public Sprite picture;
        [SerializeField]
        public int cost;
        [SerializeField]
        public GameObject obj;
        [SerializeField]
        public List<GameObject> pieces;
        [SerializeField]
        public Material mat;
        [SerializeField]
        public Material terrain;
        [SerializeField]
        public int terrainAmount;
        [SerializeField]
        public int durability;
    }
    
    GameObject newItem;

    [SerializeField]
    List<GameObject> shopScreens = new List<GameObject>();

    [SerializeField]
    GameObject shopParent;

    [SerializeField]
    GameObject gameUI;

    [SerializeField]
    Text playerCurrency;

    [SerializeField]
    Text playerPoints;

    [SerializeField]
    Game game;

    [SerializeField]
    List<items> dogScreen = new List<items>();


    [SerializeField]
    List<items> fenceScreen = new List<items>();

    [SerializeField]
    List<items> decorationsScreen = new List<items>();

    [SerializeField]
    List<items> paddockItemsScreen = new List<items>();

    [SerializeField]
    List<items> pathItemsScreen = new List<items>();

    [SerializeField]
    List<items> shopItemScreen = new List<items>();

    [SerializeField]
    AudioManager audioManager;

    [SerializeField]
    List<GameObject> pauseMenus = new List<GameObject>();

    [SerializeField]
    CameraControl camera;

    [SerializeField]
    Unlocks unlock;

    [SerializeField]
    GameObject currencyPoints;

    bool inMainMenu = false;
    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < shopScreens.Count; i++)
        {
            shopScreens[i].SetActive(false);
        }

        for (int i = 0; i < pauseMenus.Count; i++)
        {
            pauseMenus[i].SetActive(false);
        }

        shopParent.SetActive(false);
        game.setRayOnButton(false);

        setGameUI(false);
        setConstantUI(false);


        populateShopScreens(dogScreen, 0);
        populateShopScreens(fenceScreen, 1);
        populateShopScreens(decorationsScreen, 2);
        populateShopScreens(paddockItemsScreen, 3);
        populateShopScreens(pathItemsScreen, 4);
        populateShopScreens(shopItemScreen, 5);


        currencyPoints.SetActive(false);

    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            pauseGame();
        }
    }


    void populateShopScreens(List<items> list, int child)
    {
        for (int i =0; i < shopScreens[child].transform.childCount; i++)
        {
            shopScreens[child].transform.GetChild(i).GetComponent<Image>().sprite = list[i].picture;
            shopScreens[child].transform.GetChild(i).GetChild(0).GetComponent<Text>().text = "£" + list[i].cost.ToString();

            if(shopScreens[child].transform.GetChild(i).childCount > 1)
            {
                if (child == 0)
                {
                    shopScreens[child].transform.GetChild(i).GetChild(1).GetComponent<Text>().text = "Terrain: " + list[i].terrain.name + "Tiles Needed: " + list[i].terrainAmount.ToString();
                }
                else if(child == 3)
                {
                    shopScreens[child].transform.GetChild(i).GetChild(1).GetComponent<Text>().text = "Durability: " + list[i].durability.ToString();
                }
            }
        }
    }

    public void changeShopScreen(GameObject screen)
    {
        for(int i =0; i < shopScreens.Count;i++)
        {
            if(shopScreens[i] == screen)
            {
                shopScreens[i].SetActive(true);
                unlock.setScreen(i);

            }
            else
            {
                shopScreens[i].SetActive(false);
            }
        }

        audioManager.playOpen();
    }

    public void updateCurrency(int currency)
    {
        playerCurrency.text = currency.ToString();
    }

    public void updatePoints(int points)
    {
        playerPoints.text = points.ToString();
    }
    public void setShops(bool set)
    {
        shopParent.SetActive(set);
        game.setRayOnButton(set);

        if (set)
        {
            audioManager.playOpen();
        }
        else
        {
            audioManager.playClose();
        }
    }

    public void setGameUI(bool set)
    {
        gameUI.SetActive(set);
    }

    public void setConstantUI(bool set)
    {
        currencyPoints.SetActive(set);
    }

   public int getDogCost(int button)
    {
        return dogScreen[button].cost;
    }

    public int getFenceCost(int button)
    {
        return fenceScreen[button].cost;
    }
    public int getItemCost(int button)
    {
        return paddockItemsScreen[button].cost;
    }

    public int getPathCost(int button)
    {
        return pathItemsScreen[button].cost;
    }

    public int getDecorationCost(int button)
    {
        return decorationsScreen[button].cost;
    }

    public int getPaddockItemsCost(int button)
    {
        return paddockItemsScreen[button].cost;
    }

    public int getShopCost(int button)
    {
        return shopItemScreen[button].cost;
    }

    public GameObject getShopItem(int button)
    {
        return shopItemScreen[button].obj;
    }
    public GameObject getPaddockItems(int button)
    {
        return paddockItemsScreen[button].obj;
    }
    public GameObject getItem(int button)
    {
        return paddockItemsScreen[button].obj;
    }
    public GameObject getDog(int button)
    {
        return dogScreen[button].obj;
    }

    public List<GameObject> getFencePieces(int button)
    {
        return fenceScreen[button].pieces;
    }

    public Material getPathType(int button)
    {
        return pathItemsScreen[button].mat;
    }

    public GameObject getDecoration(int button)
    {
        return decorationsScreen[button].obj;
    }

    public Material getTerrain(int button)
    {
        return dogScreen[button].terrain;
    }

    public int getTerrainAmount(int button)
    {
        return dogScreen[button].terrainAmount;
    }

    public int getDurability(int button)
    {
        return paddockItemsScreen[button].durability;
    }
    public void pauseGame()
    {
        pauseMenus[0].SetActive(true);
        camera.setCamera(false);

        gameUI.SetActive(false);
    }

    public void changePauseMenu(GameObject c)
    {
        if (!inMainMenu)
        {
            for (int i = 0; i < pauseMenus.Count; i++)
            {
                if (pauseMenus[i] == c)
                {
                    pauseMenus[i].SetActive(true);

                    if (pauseMenus[i].transform.childCount > 0)
                    {
                        if (pauseMenus[i].GetComponentInChildren<Scrollbar>())
                        {
                            pauseMenus[i].GetComponentInChildren<Scrollbar>().value = 1;
                        }
                    }
                }
                else
                {
                    pauseMenus[i].SetActive(false);
                }
            }
        }
        else
        {
            for (int i = 0; i < pauseMenus.Count; i++)
            {
                pauseMenus[i].SetActive(false);
            }
            inMainMenu = false;
        }
    }

    public void closeMenu()
    {
        for (int i = 0; i < pauseMenus.Count; i++)
        {
            pauseMenus[i].SetActive(false);
        }

        camera.setCamera(true);
        Time.timeScale = 1;
        gameUI.SetActive(true);
    }
    
    public void closeGame()
    {
        Application.Quit();
    }

    public void showTutorial()
    {
        pauseMenus[1].SetActive(true);
        pauseMenus[1].GetComponentInChildren<Scrollbar>().value = 1;

        inMainMenu = true;
    }

    public void showSoundSettings()
    {
        pauseMenus[2].SetActive(true);

        inMainMenu = true;
    }
}
