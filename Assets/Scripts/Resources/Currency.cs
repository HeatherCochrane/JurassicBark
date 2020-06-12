using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Currency : MonoBehaviour
{
    [SerializeField]
    int playerCurrency = 10000;

    [SerializeField]
    UIHandler UIHandler;

    [SerializeField]
    int unlockPoints = 5;

    // Start is called before the first frame update
    void Start()
    {
        UIHandler.updateCurrency(playerCurrency);
        UIHandler.updatePoints(unlockPoints);
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

    public void addPoints(int p)
    {
        unlockPoints += p;
        UIHandler.updatePoints(unlockPoints);
    }

    public void takePoints(int p)
    {
        unlockPoints -= p;
        UIHandler.updatePoints(unlockPoints);
    }

    public bool sufficientPoints(int p)
    {
        if(unlockPoints - p > -1)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}
