using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DogBehaviour : Character
{ 
    IEnumerator decreaseStats()
    {
        Debug.Log("Started");

        while (true)
        {
            if (hungerLevel > -1)
            {
                hungerLevel -= 10;
            }
            if(thirstLevel > -1)
            {
                thirstLevel -= 10;
            }
            paddockHandler.updatePaddockInfo();
            yield return new WaitForSeconds(10);
        }

 
    }
    // Start is called before the first frame update

    bool stop = false;
    int stoppingTime = 0;

    [Range(0, 100)]
    int hungerLevel = 40;
    [Range(0, 100)]
    int thirstLevel = 80;
    [Range(0, 100)]
    int happinessLevel = 100;

    Environment mMap;

    EnvironmentTile[,] paddock;
    int width = 0;
    int height = 0;

    Inventory inventory;

    PaddockControl paddockHandler;

    EnvironmentTile goalTile;
    void Start()
    {
        mMap = GameObject.Find("Environment").GetComponent<Environment>();
        inventory = GameObject.Find("InventoryUI").GetComponent<Inventory>();
        timer();

        StartCoroutine("decreaseStats");
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void FixedUpdate()
    {
        this.CurrentPosition.IsAccessible = false;
    }

    public void givePaddockControl(GameObject p)
    {
        paddockHandler = p.GetComponent<PaddockControl>();
    }
    public void givePaddockSize(EnvironmentTile[,] p, int w, int h, EnvironmentTile current)
    {
        paddock = p;
        width = w;
        height = h;
        this.CurrentPosition = current;
    }
    void timer()
    {
        decideNextAction();
        Invoke("timer", Random.Range(6, 12));
    }

    void moveDog()
    {
        EnvironmentTile tile = paddock[Random.Range(0, width), Random.Range(0, height)];
        List<EnvironmentTile> route = mMap.Solve(this.CurrentPosition, tile);
        this.GoTo(route);
    }

    void getWater()
    {
        for(int i =0; i < width; i++)
        {
            for(int j =0; j < height; j++)
            {
                if(paddock[i, j].hasWaterBowl)
                {
                    goalTile = paddock[i, j];
                }
            }
        }
        if (goalTile != null)
        {
            if (goalTile.IsAccessible)
            {
                List<EnvironmentTile> route = mMap.Solve(this.CurrentPosition, goalTile);
                this.GoTo(route);
                goalTile.IsAccessible = false;
                Invoke("drinkWater", 5);
            }
            else
            {
                moveDog();
            }
        }
        
    }

    void getFood()
    {
        Debug.Log("Go look for food");

        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                if (paddock[i, j].hasFoodBowl)
                {
                    goalTile = paddock[i, j];
                }
            }
        }
        if (goalTile != null)
        {
            if (goalTile.IsAccessible)
            {
                List<EnvironmentTile> route = mMap.Solve(this.CurrentPosition, goalTile);
                this.GoTo(route);
                goalTile.IsAccessible = false;
                Invoke("eatFood", 5);
            }
            else
            {
                moveDog();
            }
        }

    }
    void decideNextAction()
    {

        if(hungerLevel < 60)
        {
            getFood();
        }
        else if(thirstLevel < 60)
        {
            getWater();
        }
        else
        {
            int rand = Random.Range(1, 3);

            switch(rand)
            {
                case 1: moveDog();
                    break;
                case 2: getFood();
                    break;
                case 3: getWater();
                    break;
                default: moveDog();
                    break;
            }
        }     
    }

    public int getHappiness()
    {
        return happinessLevel;
    }

    public int getHunger()
    {
        return hungerLevel;
    }

    public int getThirst()
    {
        return thirstLevel;
    }

    private void OnMouseDown()
    {
        if (!inventory.isDogInventoryFull())
        {
            inventory.storeDog(this.gameObject);
            paddockHandler.removeDog(this.gameObject);
            Destroy(this.gameObject);
        }
        else
        {
            Debug.Log("INVENTORY FULL");
        }
    }

    void eatFood()
    {
        hungerLevel += 20;
        if(hungerLevel >= 100)
        {
            hungerLevel = 100;
        }
        else if(hungerLevel <= 0)
        {
            hungerLevel = 0;
        }
        paddockHandler.updatePaddockInfo();
    }

    void drinkWater()
    {
        thirstLevel += 20;
        if(thirstLevel >= 100)
        {
            thirstLevel = 100;
        }
        else if(thirstLevel <= 0)
        {
            thirstLevel = 0;
        }
        paddockHandler.updatePaddockInfo();
    }
}
