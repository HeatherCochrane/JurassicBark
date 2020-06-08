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
            if (hungerLevel > -1)
            {
                hungerLevel -= 10;
            }
            if(thirstLevel > -1)
            {
                thirstLevel -= 10;
            }
            paddockHandler.updatePaddockInfo();
            updateStats();
            yield return new WaitForSeconds(10);
        }

 
    }

    Dog dog;

    Animator animator;

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


    GameObject profile;
    GameObject stats;

    Game game;

    string previousAnim;

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
    }

    // Update is called once per frame
    void Update()
    {
        if (stats.activeSelf)
        {
            stats.transform.position = Input.mousePosition;
        }
        if (!this.getIfMoving())
        {
            changeAnimation("IdleTest");
        }
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
        Invoke("timer", Random.Range(6, 12));
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
        for(int i =0; i < width; i++)
        {
            for(int j =0; j < height; j++)
            {
                if(paddock[i, j].hasWaterBowl && paddock[i,j].IsAccessible)
                {
                    goalTile = paddock[i, j];
                }
            }
        }
        if (goalTile != null)
        {
            if (goalTile.IsAccessible)
            {
                this.CurrentPosition.IsAccessible = true;
                List<EnvironmentTile> route = mMap.Solve(this.CurrentPosition, goalTile, 2);
                this.GoTo(route);
                goalTile.IsAccessible = false;
                Invoke("drinkWater", 5);
            }
            else if(dog.hungerLevel < 60)
            {
                getFood();
            }
        }
        else 
        {
            moveDog();
        }

    }

    void getFood()
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
        if (goalTile != null)
        {
            if (goalTile.IsAccessible)
            {
                this.CurrentPosition.IsAccessible = true;
                List<EnvironmentTile> route = mMap.Solve(this.CurrentPosition, goalTile, 2);
                this.GoTo(route);
                
                goalTile.IsAccessible = false;
                Invoke("eatFood", 5);
            }
            else if(dog.thirstLevel < 60)
            {
                getWater();
            }
        }
        else 
        {
            moveDog();
        }

    }
    void decideNextAction()
    {
        if(dog.hungerLevel < 60)
        {
            getFood();
        }
        else if(dog.thirstLevel < 60)
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
        paddockHandler.updatePaddockInfo();
        updateStats();
    }

    void drinkWater()
    {
        thirstLevel += 30;

        if(thirstLevel >= 100)
        {
            thirstLevel = 100;
        }
        else if(thirstLevel <= 0)
        {
            thirstLevel = 0;
        }

        dog.thirstLevel = thirstLevel;
        paddockHandler.updatePaddockInfo();
        updateStats();
    }

    void calculateHappiness()
    {
        happinessLevel = dog.hungerLevel +dog.thirstLevel / 2;
        dog.happinessLevel = happinessLevel;
    }
    void updateStats()
    {
        //First child is only text
        calculateHappiness();
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

        if (!game.doingAction())
        {
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
