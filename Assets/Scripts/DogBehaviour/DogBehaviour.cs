using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class DogBehaviour : Character
{ 
    struct Dog
    {
        public int hungerLevel;
        public int thirstLevel;
        public int happinessLevel;
        public string gender;
        public int age;
        public string personality;
    }

    IEnumerator decreaseStats()
    {
        while (true)
        {
            if (dog.hungerLevel >= 0)
            {
                hungerLevel -= 10;
                dog.hungerLevel = hungerLevel;
            }
            if(dog.thirstLevel >= 0)
            {
                thirstLevel -= 10;
                dog.thirstLevel = thirstLevel;
            }

            updateStats();
            yield return new WaitForSeconds(20);
        }

    }

    IEnumerator happinessTracker()
    {
        while(true)
        {
            if (dog.thirstLevel >= 60)
            {
                changeHappiness(10);
            }
            else if (dog.thirstLevel < 100)
            {
                changeHappiness(-10);
            }
            if (dog.hungerLevel >= 60)
            {
                changeHappiness(10);
            }
            else if (dog.hungerLevel < 100)
            {
                changeHappiness(-10);
            }

            updateStats();
            yield return new WaitForSeconds(10);
        }
    }
    Dog dog;

    Animator animator;

    bool stop = false;
    int stoppingTime = 0;

    [Range(0, 100)]
    int hungerLevel = 60;
    [Range(0, 100)]
    int thirstLevel = 60;
    [Range(0, 100)]
    int happinessLevel = 60;

    Environment mMap;

    EnvironmentTile[,] paddock;
    int width = 0;
    int height = 0;

    Inventory inventory;

    PaddockControl paddockHandler;

    EnvironmentTile goalTile;


    GameObject profile;
    GameObject stats;

    Game game;

    string previousAnim;

    bool doingAction = false;

    Material terrain;
    int amount = 0;

    void Start()
    {
        mMap = GameObject.Find("Environment").GetComponent<Environment>();
        inventory = GameObject.Find("InventoryUI").GetComponent<Inventory>();
        game = GameObject.Find("Game").GetComponent<Game>();
        animator = this.GetComponent<Animator>();
        timer();

        profile.SetActive(false);
        stats.SetActive(false);

        updateStats();
        StartCoroutine("decreaseStats");
        StartCoroutine("happinessTracker");
    }

    // Update is called once per frame
    void Update()
    {
        if (stats.activeSelf)
        {
            stats.transform.position = Input.mousePosition;
        }
        if (!this.getIfMoving() && !doingAction)
        {
            changeAnimation("IdleTest");
        }
    }

    public void setTerrain(Material t, int a)
    {
        terrain = t;
        amount = a;
    }
    public void giveProfile(GameObject prof, GameObject s)
    {
        profile = prof;
        stats = s;
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

    public void giveDogInfo(string gender, string personality, int age)
    {
        dog.age = age;

        dog.gender = gender;

        dog.personality = personality;

        dog.hungerLevel = hungerLevel;
        dog.thirstLevel = thirstLevel;
        dog.happinessLevel = happinessLevel;
        
    }
    void timer()
    {
        decideNextAction();
        Invoke("timer", Random.Range(7, 9));
    }

    void moveDog()
    {
        this.CurrentPosition.IsAccessible = true;
        EnvironmentTile tile = paddock[Random.Range(0, width), Random.Range(0, height)];
        List<EnvironmentTile> route = mMap.Solve(this.CurrentPosition, tile, 2);
        this.GoTo(route);
        changeAnimation("WalkTest");
    }

    void getWater()
    {
        this.CurrentPosition.IsAccessible = true;
        List<EnvironmentTile> route = mMap.Solve(this.CurrentPosition, goalTile, 2);
        this.GoTo(route);
        Invoke("drinkWater", 5);
        changeAnimation("WalkTest");
    }
    void getFood()
    {
        this.CurrentPosition.IsAccessible = true;
        List<EnvironmentTile> route = mMap.Solve(this.CurrentPosition, goalTile, 2);
        this.GoTo(route);
        Invoke("eatFood", 5);
        changeAnimation("WalkTest");
    }

    bool canGetFood()
    {
        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                if (paddock[i, j].hasFoodBowl && paddock[i, j].IsAccessible)
                {
                    goalTile = paddock[i, j];
                }
            }
        }

        if(goalTile != null)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    bool canGetWater()
    {
        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                if (paddock[i, j].hasWaterBowl && paddock[i, j].IsAccessible)
                {
                    goalTile = paddock[i, j];
                }
            }
        }

        if (goalTile != null)
        {
            return true;
        }
        else
        {
            return false;
        }
    }


    void decideNextAction()
    {
        this.CurrentPosition.IsAccessible = true;
        doingAction = false;

        if(dog.hungerLevel < 100 && paddockHandler.hasFood && canGetFood())
        { 
            getFood();
        }
        else if(dog.thirstLevel < 100 && paddockHandler.hasWater && canGetWater())
        {
            getWater();
        }
        else
        {
            int rand = Random.Range(1, 4);

            switch(rand)
            {
                case 1: moveDog();
                    break;
                case 2: doRandomAction();
                    doingAction = true;
                    break;
                case 3: moveDog();
                    break;
                case 4: doRandomAction();
                    doingAction = true;
                    break;
                default: moveDog();
                    break;
            }
        }     
    }

    void doRandomAction()
    {        
        int random = Random.Range(1, 4);

        switch(random)
        {
            case 1:
                changeAnimation("Look");
                Invoke("stopAction", 3);
                break;
            case 2:
                changeAnimation("Wiggle");
                Invoke("stopAction", 3);
                break;
            case 3:
                changeAnimation("Look");
                Invoke("stopAction", 3);
                break;
            case 4: changeAnimation("Wiggle");
                Invoke("stopAction", 3);
                break;

        }
    }

    void stopAction()
    {
        doingAction = false;
    }
    public int getHappiness()
    {
        return dog.happinessLevel;
    }

    public int getHunger()
    {
        return dog.hungerLevel;
    }

    public int getThirst()
    {
        return dog.thirstLevel;
    }


    void eatFood()
    {
        hungerLevel += 30;

        if(hungerLevel >= 100)
        {
            hungerLevel = 100;
        }
        else if(hungerLevel <= 0)
        {
            hungerLevel = 0;
        }

        dog.hungerLevel = hungerLevel;
        changeHappiness(20);
        updateStats();
    }

    void drinkWater()
    {
        thirstLevel += 30;

        if (thirstLevel >= 100)
        {
            thirstLevel = 100;
        }
        else if (thirstLevel <= 0)
        {
            thirstLevel = 0;
        }

        dog.thirstLevel = thirstLevel;
        changeHappiness(20);
        updateStats();
    }

    void changeHappiness(int c)
    {
        int standIn = 0;

        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                if (paddock[i, j].getTerrainPaint() == terrain.name)
                {
                    standIn += 1;
                }
            }
        }

        happinessLevel += c;

        dog.happinessLevel = happinessLevel;

        //If terrain needs are met, set the baseline at 60
        if (standIn >= amount)
        {
            dog.happinessLevel = Mathf.Clamp(dog.happinessLevel, 60, 100);
        }
        else
        {
            dog.happinessLevel = Mathf.Clamp(dog.happinessLevel, 0, 70);
        }

        paddockHandler.updatePaddockInfo();

    }

    public void checkTiles()
    {
        int standIn = 0;

        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                if (paddock[i, j].getTerrainPaint() == terrain.name)
                {
                    standIn += 1;
                }
            }
        }

        dog.happinessLevel = happinessLevel;

        //If terrain needs are met, set the baseline at 60
        if (standIn >= amount)
        {
            dog.happinessLevel = Mathf.Clamp(dog.happinessLevel, 70, 100);
        }
        else
        {
            dog.happinessLevel = Mathf.Clamp(dog.happinessLevel, 0, 70);
        }

        paddockHandler.updatePaddockInfo();

        Debug.Log("Dog Happiness: " + dog.happinessLevel);

    }

    public void updateStats()
    {
        //First child is only text
        stats.transform.GetChild(1).GetComponent<Slider>().value = dog.hungerLevel;
        stats.transform.GetChild(2).GetComponent<Slider>().value = dog.thirstLevel;
        stats.transform.GetChild(3).GetComponent<Slider>().value = dog.happinessLevel;
    }
    private void OnMouseEnter()
    {
        if (!game.doingAction())
        {
            stats.SetActive(true);
            updateStats();
        }
    }

    private void OnMouseDown()
    {
        //if (!inventory.isDogInventoryFull())
        //{
        //    inventory.storeDog(this.gameObject);
        //    paddockHandler.removeDog(this.gameObject);
        //    Destroy(this.gameObject);
        //}
        //else
        //{
        //    Debug.Log("INVENTORY FULL");
        //}

        if (!game.doingAction() && game.getMoveCamera())
        {
            CameraControl.instance.followTransform = transform;

            profile.SetActive(true);

            profile.transform.GetChild(1).GetComponent<Text>().text = "Gender: " + dog.gender;
            profile.transform.GetChild(2).GetComponent<Text>().text = "Age: " + dog.age.ToString();
            profile.transform.GetChild(3).GetComponent<Text>().text = "Personality: " + dog.personality;
        }

    }

    private void OnMouseExit()
    {
        stats.SetActive(false);
    }

    void changeAnimation(string Anim)
    {
        animator.Play(Anim);
    }
}
