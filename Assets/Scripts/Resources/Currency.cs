using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Currency : MonoBehaviour
{
    [SerializeField]
    int playerCurrency = 100;

    [SerializeField]
    UIHandler UIHandler;

    // Start is called before the first frame update
    void Start()
    {
        UIHandler.updateCurrency(playerCurrency);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void subtractMoney(int cost)
    {
        playerCurrency -= cost;
        UIHandler.updateCurrency(playerCurrency);
    }

    public void addMoney(int cost)
    {
        playerCurrency += cost;
        UIHandler.updateCurrency(playerCurrency);

    }

    public bool sufficientFunds(int cost)
    {
        if(playerCurrency - cost > -1)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}
