using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Unlocks : MonoBehaviour
{
    // Start is called before the first frame update

    [SerializeField]
    List<GameObject> dogScreenButtons = new List<GameObject>();

    [SerializeField]
    List<GameObject> fenceScreenButtons = new List<GameObject>();

    [SerializeField]
    List<GameObject> decoScreenButtons = new List<GameObject>();

    [SerializeField]
    List<GameObject> paddockItemsScreenButtons = new List<GameObject>();

    [SerializeField]
    List<GameObject> pathScreenButtons = new List<GameObject>();

    [SerializeField]
    List<GameObject> shopsScreenButtons = new List<GameObject>();


    int buttonNumber = 0;
    int screen = 0;
    int pointsNeeded = 10;
    int currentScreen = 0;

    [SerializeField]
    Currency points;

    List<GameObject> screenButtons = new List<GameObject>();
    [SerializeField]
    List<GameObject> unlockButtonIcons = new List<GameObject>();
    [SerializeField]
    List<int> unlockButtons = new List<int>();

    [SerializeField]
    UIHandler UIhandler;

    [SerializeField]
    List<int> dogUnlocks = new List<int>();
    List<int> fenceUnlocks = new List<int>();
    List<int> decoUnlocks = new List<int>();
    List<int> paddockItemUnlocks = new List<int>();
    List<int> pathUnlocks = new List<int>();
    List<int> shopUnlocks = new List<int>();

    [SerializeField]
    AudioManager audio;

    [SerializeField]
    SaveHandler save;

    void Start()
    {
        setUnlocks(dogUnlocks);
        setUnlocks(fenceUnlocks);
        setUnlocks(decoUnlocks);
        setUnlocks(paddockItemUnlocks);
        setUnlocks(pathUnlocks);
        setUnlocks(shopUnlocks);

        for(int i=0; i < 6; i++)
        {
            unlockButtons.Add(0);
        }

        for(int j = 0; j < unlockButtonIcons.Count; j++)
        {
            unlockButtonIcons[j].transform.GetChild(0).GetComponent<Text>().text = "Cost: " + pointsNeeded.ToString() + " points";
            pointsNeeded += 15;
        }

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void loadUnlocks()
    {
        SaveGame.Load();

        dogUnlocks = SaveGame.Instance.unlockables.dogScreen;
        fenceUnlocks = SaveGame.Instance.unlockables.fenceScreen;
        decoUnlocks = SaveGame.Instance.unlockables.decorationsScreen;
        paddockItemUnlocks = SaveGame.Instance.unlockables.paddockItemsScreen;
        pathUnlocks = SaveGame.Instance.unlockables.pathsScreen;
        shopUnlocks = SaveGame.Instance.unlockables.shopsScreen;

    }
    void setUnlocks(List<int> l)
    {
        l.Add(1);
        l.Add(1);
        for(int i =2 ; i < 6; i++)
        {
            l.Add(0);
        }
    }
    public void setButton(int b)
    {
        buttonNumber = b;
    }

    public void setScreen(int s)
    {
        switch(s)
        {
            case 1: screenButtons = dogScreenButtons;
                unlockButtons = dogUnlocks;
                break;
            case 2: screenButtons = fenceScreenButtons;
                unlockButtons = fenceUnlocks;
                break;
            case 3: screenButtons = decoScreenButtons;
                unlockButtons = decoUnlocks;
                break;
            case 4: screenButtons = paddockItemsScreenButtons;
                unlockButtons = paddockItemUnlocks;
                break;
            case 5: screenButtons = pathScreenButtons;
                unlockButtons = pathUnlocks;
                break;
            case 6: screenButtons = shopsScreenButtons;
                unlockButtons = shopUnlocks;
                break;
        }

        currentScreen = s;
        updateUnlockButtons();
    }

    void updateUnlockButtons()
    {
        for(int i = 0; i < screenButtons.Count; i++)
        {
            if(unlockButtons[i] == 1)
            {
                screenButtons[i].GetComponent<Button>().enabled = true;
                unlockButtonIcons[i].SetActive(false);

                screenButtons[i].transform.GetChild(0).gameObject.SetActive(true);

                if (screenButtons[i].transform.childCount > 1)
                {
                    screenButtons[i].transform.GetChild(1).gameObject.SetActive(true);
                }
            }
            else
            {
                screenButtons[i].GetComponent<Button>().enabled = false;
                unlockButtonIcons[i].SetActive(true);

                screenButtons[i].transform.GetChild(0).gameObject.SetActive(false);
                if (screenButtons[i].transform.childCount > 1)
                {
                    screenButtons[i].transform.GetChild(1).gameObject.SetActive(false);
                }
            }
        }
    }
    public void checkIfUnlockable()
    {
        int cost = (buttonNumber + 1) * 10;
        if(points.sufficientPoints(cost))
        {
            Debug.Log("Button to unlock:" + buttonNumber);
            unlockButtons[buttonNumber] = 1;
            unlockButtonIcons[buttonNumber].SetActive(false);
            points.takePoints(cost);

            audio.playUnlock();
            updateUnlockButtons();
            updateLists();
        }
    }

    public void updateLists()
    {
        switch (currentScreen)
        {
            case 1:
                dogScreenButtons = screenButtons;
                dogUnlocks = unlockButtons;
                break;
            case 2:
                fenceScreenButtons = screenButtons;
                fenceUnlocks = unlockButtons;
                break;
            case 3:
                decoScreenButtons = screenButtons;
                decoUnlocks = unlockButtons;
                break;
            case 4:
                paddockItemsScreenButtons = screenButtons;
                paddockItemUnlocks = unlockButtons;
                break;
            case 5:
                pathScreenButtons = screenButtons;
                pathUnlocks = unlockButtons;
                break;
            case 6:
                shopsScreenButtons = screenButtons;
                shopUnlocks = unlockButtons;
                break;
        }

        save.saveUnlocks(dogUnlocks, 1);
        save.saveUnlocks(fenceUnlocks, 2);
        save.saveUnlocks(decoUnlocks, 3);
        save.saveUnlocks(paddockItemUnlocks, 4);
        save.saveUnlocks(pathUnlocks, 5);
        save.saveUnlocks(shopUnlocks, 6);
    }
}
