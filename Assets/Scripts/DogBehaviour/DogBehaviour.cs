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

    Dog dog;

    Animator animator;

    bool stop = false;
    int stoppingTime = 0;

    [Range(0, 100)]
    int hungerLevel = 70;
    [Range(0, 100)]
    int thirstLevel = 70;
    [Range(0, 100)]
    int happinessLevel = 70;

    Environment mMap;


    EnvironmentTile[,] paddock;

    [SerializeField]
    int width = 0;
    [SerializeField]
    int height = 0;

    Inventory inventory;

    PaddockControl paddockHandler;

    EnvironmentTile goalTile;

    [SerializeField]
    GameObject profile;
    [SerializeField]
    GameObject stats;

    Game game;

    string previousAnim;

    bool doingAction = false;

    Material terrain;
    int amount = 0;

    DogHandler dogHandler;


    [SerializeField]
    AnimationClip walk;
    [SerializeField]
    AnimationClip idle;
    [SerializeField]
    AnimationClip shake;
    [SerializeField]
    AnimationClip extra;

    int paddockIdentifier = 0;
    int dogIdentifier = 0;

    SaveHandler save;

    ParkRating rating;
    void Start()
    {
        mMap = GameObject.Find("Environment").GetComponent<Environment>();
        inventory = GameObject.Find("InventoryUI").GetComponent<Inventory>();
        game = GameObject.Find("Game").GetComponent<Game>();
        animator = this.GetComponent<Animator>();
        dogHandler = GameObject.Find("DogHandler").GetComponent<DogHandler>();
        save = GameObject.Find("SAVEHANDLER").GetComponent<SaveHandler>();
        rating = GameObject.Find("ParkRating").GetComponent<ParkRating>();

        timer();

        profile = Instantiate(profile);
        profile.transform.parent = GameObject.Find("GameUI").transform;
        profile.transform.position = new Vector3(-70, 0, 0);

        stats = Instantiate(stats);
        stats.transform.parent = GameObject.Find("GameUI").transform;

        profile.SetActive(false);
        stats.SetActive(false);

        this.transform.parent = this.CurrentPosition.transform.parent;

        decreaseHappiness();
        decreaseStats();
    }

    // Update is called once per frame
    void Update()
    {
        if (stats != null)
        {
            if (stats.activeSelf)
            {
                stats.transform.position = Input.mousePosition;
            }
        }
        if (!this.getIfMoving() && !doingAction)
        {
            changeAnimation(idle.name);
        }
    }

    public void decreaseHappiness()
    {
        if (dog.thirstLevel >= 60)
        {
            changeHappiness(5);
        }
        else if (dog.thirstLevel < 100)
        {
            changeHappiness(-10);
        }
        if (dog.hungerLevel >= 60)
        {
            changeHappiness(5);
        }
        else if (dog.hungerLevel < 100)
        {
            changeHappiness(-10);
        }

        updateStats();
        Invoke("decreaseHappiness", 45/Time.timeScale);
    }

    public void decreaseStats()
    {
        if (hungerLevel > 0)
        {
            hungerLevel -= 10;
            dog.hungerLevel = hungerLevel;
        }
        if (thirstLevel > 0)
        {
            thirstLevel -= 10;
            dog.thirstLevel = thirstLevel;
        }

        updateStats();

        Invoke("decreaseStats", 45/Time.timeScale);
    }
    public void setTerrain(Material t, int a)
    {
        terrain = t;
        amount = a;

    }

    public Material getTerrain()
    {
        return terrain;
    }

    public int getTerrainAmount()
    {
        return amount;
    }

    public void setPaddock(EnvironmentTile[,] t)
    {
        paddock = t;
    }
    public void givePaddockControl(GameObject p, bool spawned)
    {
        paddockHandler = p.GetComponentInChildren<PaddockControl>();
        paddock = p.GetComponentInChildren<PaddockControl>().getTiles();

        if (!spawned)
        {
            paddockHandler.addLoadedDog(this.gameObject);
        }
    }

    public void givePaddockSize(EnvironmentTile[,] p, int w, int h, EnvironmentTile current)
    {
        paddock = p;
        width = w;
        height = h;
        this.CurrentPosition = current;
    }

    public void giveDogInfo(string gender, string personality, int age, bool loaded)
    {
        dog.age = age;

        dog.gender = gender;

        dog.personality = personality;

        if (!loaded)
        {
            dog.hungerLevel = hungerLevel;
            dog.thirstLevel = thirstLevel;
            dog.happinessLevel = happinessLevel;
        }

    }

    public string getGender()
    {
        return dog.gender;
    }

    public int getAge()
    {
        return dog.age;
    }

    public string getPersonality()
    {
        return dog.personality;
    }
    void timer()
    {
        decideNextAction();
        Invoke("timer", Random.Range(7, 9) / Time.timeScale);
    }

    void moveDog()
    {
        this.CurrentPosition.IsAccessible = true;
        EnvironmentTile tile = paddock[Random.Range(0, width), Random.Range(0, height)];
        List<EnvironmentTile> route = mMap.Solve(this.CurrentPosition, tile, 2);
        this.GoTo(route, -1);
        changeAnimation(walk.name);
    }

    void getWater()
    {
        this.CurrentPosition.IsAccessible = true;
        List<EnvironmentTile> route = mMap.Solve(this.CurrentPosition, goalTile, 2);
        this.GoTo(route, 0);
        changeAnimation(walk.name);
    }
    void getFood()
    {
        this.CurrentPosition.IsAccessible = true;
        List<EnvironmentTile> route = mMap.Solve(this.CurrentPosition, goalTile, 2);
        this.GoTo(route, 1);
        changeAnimation(walk.name);
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
                    return true;
                }
            }
        }

        return false;

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
                    return true;
                }
            }
        }

        return false;

    }


    void decideNextAction()
    {
        doingAction = false;
        this.CurrentPosition.IsAccessible = true;

        if (dog.hungerLevel < 100 && canGetFood())
        {
            getFood();
        }
        else if (dog.thirstLevel < 100 && canGetWater())
        {
            getWater();
        }
        else
        {
            int rand = Random.Range(1, 4);

            switch (rand)
            {
                case 1:
                    moveDog();
                    break;
                case 2:
                    doRandomAction();
                    doingAction = true;
                    break;
                case 3:
                    moveDog();
                    break;
                case 4:
                    doRandomAction();
                    doingAction = true;
                    break;
                default:
                    moveDog();
                    break;
            }
        }
    }

    void doRandomAction()
    {
        int random = Random.Range(1, 4);

        switch (random)
        {
            case 1:
                changeAnimation(extra.name);
                Invoke("stopAction", 3 / Time.timeScale);
                break;
            case 2:
                changeAnimation(shake.name);
                Invoke("stopAction", 3 / Time.timeScale);
                break;
            case 3:
                changeAnimation(extra.name);
                Invoke("stopAction", 3 / Time.timeScale);
                break;
            case 4:
                changeAnimation(shake.name);
                Invoke("stopAction", 3 / Time.timeScale);
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


    public void eatFood()
    {
        hungerLevel += 30;

        if (hungerLevel >= 100)
        {
            hungerLevel = 100;
        }
        else if (hungerLevel <= 0)
        {
            hungerLevel = 0;
        }

        dog.hungerLevel = hungerLevel;
        changeHappiness(20);
        updateStats();
    }

    public void drinkWater()
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
                if (paddock[i, j].getTerrainPaint() != null)
                {
                    if (paddock[i, j].getTerrainPaint() == terrain.name)
                    {
                        standIn += 1;
                    }
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
        rating.updateDogHappiness(dogIdentifier, dog.happinessLevel);

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

    }

    public void updateStats()
    {
        //First child is only text
        stats.transform.GetChild(1).GetComponent<Slider>().value = dog.hungerLevel;
        stats.transform.GetChild(2).GetComponent<Slider>().value = dog.thirstLevel;
        stats.transform.GetChild(3).GetComponent<Slider>().value = dog.happinessLevel;

    }

    public void saveDogDetails()
    {
        save = GameObject.Find("SAVEHANDLER").GetComponent<SaveHandler>();
        save.updateDogStats(dogIdentifier, this.gameObject);
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
        if (!game.doingAction() && game.getMoveCamera())
        {
            CameraControl.instance.followTransform = transform;

            profile.SetActive(true);
            profile.transform.localScale = new Vector3(1, 1, 1);
            profile.GetComponent<RectTransform>().anchoredPosition = new Vector2(1 - 70, 1);

            profile.transform.GetChild(1).GetComponent<Text>().text = "Gender: " + dog.gender;
            profile.transform.GetChild(2).GetComponent<Text>().text = "Age: " + dog.age.ToString();
            profile.transform.GetChild(3).GetComponent<Text>().text = "Personality: " + dog.personality;
        }

    }

    public void removeFromPaddock()
    {
        paddockHandler.removeDog(this.gameObject);
    }
    private void OnMouseExit()
    {
        stats.SetActive(false);
    }

    void changeAnimation(string Anim)
    {
        animator.Play(Anim);
    }

    public void setDogIdentifier(int set)
    {
        dogIdentifier = set;
    }
    public void setIdentifier(int set)
    {
        paddockIdentifier = set;
    }
    public int getIdentifier()
    {
        return paddockIdentifier;
    }

    public int getDogIdentifier()
    {
        return dogIdentifier;
    }

    public void setHunger(int h)
    {
        hungerLevel = h;
        dog.hungerLevel = hungerLevel;
        updateStats();
    }

    public void setThirst(int h)
    {
        thirstLevel = h;
        dog.thirstLevel = thirstLevel;
        updateStats();
    }

    public void setHappiness(int h)
    {
        happinessLevel = h;
        dog.happinessLevel = happinessLevel;
        updateStats();
    }
}
