using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Currency : MonoBehaviour
{
    int playerCurrency = 600;

    [SerializeField]
    UIHandler UIHandler;

    int unlockPoints = 50;

    [SerializeField]
    AudioManager audio;

    [SerializeField]
    SaveHandler save;

    [SerializeField]
    GameObject fundsAlert;
    // Start is called before the first frame update
    void Start()
    {
        UIHandler.updateCurrency(playerCurrency);
        UIHandler.updatePoints(unlockPoints);
        fundsAlert.SetActive(false);

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void loadUI()
    {
        SaveGame.Load();

        playerCurrency = SaveGame.Instance.UIelements.currency;
        unlockPoints = SaveGame.Instance.UIelements.points;

        setMoney(playerCurrency);
        setPoints(unlockPoints);
        saveUI();
    }

    public void subtractMoney(int cost)
    {
        playerCurrency -= cost;
        UIHandler.updateCurrency(playerCurrency);

        saveUI();
    }

    public void addMoney(int cost)
    {
        playerCurrency += cost;

        UIHandler.updateCurrency(playerCurrency);

        saveUI();
    }

    public void setMoney(int set)
    {
        playerCurrency = set;
        UIHandler.updateCurrency(playerCurrency);
        saveUI();
    }
    public void setPoints(int set)
    {
        unlockPoints = set;
        UIHandler.updatePoints(unlockPoints);
        saveUI();
    }
    public void saveUI()
    {
        save.saveUI(playerCurrency, unlockPoints);
    }

    public bool sufficientFunds(int cost)
    {
        if(playerCurrency - cost > -1)
        {
            return true;
        }
        else
        {
            showInsufficientFunds();
            return false;
        }
    }

    public void addPoints(int p)
    {
        unlockPoints += p;
        UIHandler.updatePoints(unlockPoints);
        audio.playPointsGained();

        saveUI();
    }

    public void takePoints(int p)
    {
        unlockPoints -= p;
        UIHandler.updatePoints(unlockPoints);

        saveUI();
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

    public void saveCurrency()
    {
        save.saveUI(playerCurrency, unlockPoints);
    }

    public void showInsufficientFunds()
    {
        fundsAlert.SetActive(true);
        Invoke("hideInsufficientFunds", 3);
    }

    public void hideInsufficientFunds()
    {
        fundsAlert.SetActive(false);
    }
}
