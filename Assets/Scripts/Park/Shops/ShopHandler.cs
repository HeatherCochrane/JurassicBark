using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopHandler : MonoBehaviour
{
    [SerializeField]
    UIHandler UIhandler;


    [SerializeField]
    Currency currency;

    GameObject shop;
    int cost = 0;


    List<GameObject> shops = new List<GameObject>();

    [SerializeField]
    Game game;

    GameObject standIn;


    int x = 0;
    int y = 0;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void spawnShop(Vector3 pos, EnvironmentTile t1, Vector3 r, int button)
    {
        if (currency.sufficientFunds(cost))
        {
            shop = Instantiate(standIn);
            shop.transform.position = pos;
            shop.transform.Rotate(r);
            shop.transform.SetParent(t1.transform);
            shops.Add(shop);

            t1.IsAccessible = false;

            currency.subtractMoney(cost);

            if (button == 0)
            {
                shop.GetComponent<Shop>().setProductCost(10);
            }
            else
            {
                shop.GetComponent<Shop>().setProductCost(20);
            }

            shop.GetComponentInChildren<ParticleSystem>().Play();
        }
    }

    public void getShop(int button)
    {
        standIn = UIhandler.getShopItem(button);
        cost = UIhandler.getShopCost(button);
    }

    public GameObject getStandIn(int button)
    {
        return UIhandler.getShopItem(button);
    }

}
