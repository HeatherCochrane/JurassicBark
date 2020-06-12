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
    int pointsNeeded = 2;
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

    List<int> dogUnlocks = new List<int>();
    List<int> fenceUnlocks = new List<int>();
    List<int> decoUnlocks = new List<int>();
    List<int> paddockItemUnlocks = new List<int>();
    List<int> pathUnlocks = new List<int>();
    List<int> shopUnlocks = new List<int>();

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
    }

    // Update is called once per frame
    void Update()
    {
        
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
            }
            else
            {
                screenButtons[i].GetComponent<Button>().enabled = false;
                unlockButtonIcons[i].SetActive(true);
            }
        }
    }
    public void checkIfUnlockable()
    {
        if(points.sufficientPoints(pointsNeeded))
        {
            Debug.Log("Button to unlock:" + buttonNumber);
            unlockButtons[buttonNumber] = 1;
            unlockButtonIcons[buttonNumber].SetActive(false);
            points.takePoints(pointsNeeded);

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
    }
}
