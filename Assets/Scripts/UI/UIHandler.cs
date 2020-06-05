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
    List<items> dogScreen = new List<items>();

    [SerializeField]
    List<items> fenceScreen = new List<items>();

    [SerializeField]
    List<items> decorationsScreen = new List<items>();

    [SerializeField]
    List<items> paddockItemsScreen = new List<items>();

    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < shopScreens.Count; i++)
        {
            shopScreens[i].SetActive(false);
        }

        setShops(false);
        setGameUI(false);
        setConstantUI(false);

        populateShopScreens(dogScreen, 0);
        populateShopScreens(fenceScreen, 1);
        populateShopScreens(decorationsScreen, 2);
        populateShopScreens(paddockItemsScreen, 3);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void populateShopScreens(List<items> list, int child)
    {
        for(int i =0; i < shopScreens[child].transform.childCount; i++)
        {
            shopScreens[child].transform.GetChild(i).GetComponent<Image>().sprite = list[i].picture;
            shopScreens[child].transform.GetChild(i).GetChild(0).GetComponent<Text>().text = "£" + list[i].cost.ToString();
        }
    }

    public void changeShopScreen(GameObject screen)
    {
        for(int i =0; i < shopScreens.Count;i++)
        {
            if(shopScreens[i] == screen)
            {
                shopScreens[i].SetActive(true);
            }
            else
            {
                shopScreens[i].SetActive(false);
            }
        }
    }

    public void updateCurrency(int currency)
    {
        playerCurrency.text = "£" + currency.ToString();
    }
    public void setShops(bool set)
    {
        shopParent.SetActive(set);
    }

    public void setGameUI(bool set)
    {
        gameUI.SetActive(set);
    }

    public void setConstantUI(bool set)
    {
        playerCurrency.gameObject.SetActive(set);
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
}
