using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//USE THIS SCRIPT TO HANDLE SWITCHING BETWEEN SHOP MENUS, MAIN MENU ETC
public class UIHandler : MonoBehaviour
{
    [SerializeField]
    List<GameObject> shopScreens = new List<GameObject>();

    [SerializeField]
    GameObject shopParent;
    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < shopScreens.Count; i++)
        {
            shopScreens[i].SetActive(false);
        }

        setShops(false);
    }

    // Update is called once per frame
    void Update()
    {
        
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

    public void setShops(bool set)
    {
        shopParent.SetActive(set);
    }
}
