using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Currency : MonoBehaviour
{
    int playerCurrency = 10000;

    [SerializeField]
    UIHandler UIHandler;

    int unlockPoints = 50;

    [SerializeField]
    AudioManager audio;

    [SerializeField]
    SaveHandler save;
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

    public void loadUI()
    {
        SaveGame.Load();

        playerCurrency = int.Parse(SaveGame.Instance.playerCurrency);
        unlockPoints = int.Parse(SaveGame.Instance.playerPoints);

        Debug.Log("Loaded UI: " + SaveGame.Instance.playerCurrency + " " + SaveGame.Instance.playerPoints);

        setMoney(playerCurrency);
        setPoints(unlockPoints);
    }

    public void subtractMoney(int cost)
    {
        playerCurrency -= cost;
        UIHandler.updateCurrency(playerCurrency);

        save.saveCurrency(playerCurrency);
    }

    public void addMoney(int cost)
    {
        playerCurrency += cost;

        UIHandler.updateCurrency(playerCurrency);

        save.saveCurrency(playerCurrency);

    }

    public void setMoney(int set)
    {
        playerCurrency = set;
        UIHandler.updateCurrency(playerCurrency);
    }
    public void setPoints(int set)
    {
        unlockPoints = set;
        UIHandler.updatePoints(unlockPoints);
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
        audio.playPointsGained();

        save.savePoints(unlockPoints);
    }

    public void takePoints(int p)
    {
        unlockPoints -= p;
        UIHandler.updatePoints(unlockPoints);

        save.savePoints(unlockPoints);
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
