using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DogBehaviour : Character
{ 

    // Start is called before the first frame update

    float turnRate = 6f;
    bool collidingWithWall = false;

    bool collidingWithDog = false;


    bool stop = false;
    int stoppingTime = 0;

    int hungerLevel = 100;
    int thirstLevel = 100;
    int happinessLevel = 100;

    Environment mMap;

    EnvironmentTile[,] paddock;
    int width = 0;
    int height = 0;

    Inventory inventory;
    void Start()
    {
        mMap = GameObject.Find("Environment").GetComponent<Environment>();
        inventory = GameObject.Find("InventoryUI").GetComponent<Inventory>();

        stoppingTime = Random.Range(5, 10);
        timer();
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void FixedUpdate()
    {
       
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
        Invoke("timer", stoppingTime);
    }

    void moveDog()
    {
        EnvironmentTile tile = paddock[Random.Range(0, width), Random.Range(0, height)];
        List<EnvironmentTile> route = mMap.Solve(this.CurrentPosition, tile);
        this.GoTo(route);
    }

    void decideNextAction()
    {
        int ran = Random.Range(1, 4);

        switch(ran)
        {
            //Walk around
            case 1: moveDog();
                break;
            //Go for food
            case 2:
                break;
            //Go for water
            case 3:
                break;
            //Take a nap
            case 4:
                break;
            default: moveDog();
                break;
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


    private void OnCollisionEnter(Collision collision)
    {
        if(collision.transform.tag == "Border")
        {
            collidingWithWall = true;
        }
        if (collision.transform.tag == "Dog")
        {
            collidingWithDog = true;
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.transform.tag == "Border")
        {
            collidingWithWall = false;
        }
        if (collision.transform.tag == "Dog")
        {
            collidingWithDog = false;
        }
    }

    private void OnMouseDown()
    {
        if (!inventory.isDogInventoryFull())
        {
            inventory.storeDog(this.gameObject);
            Destroy(this.gameObject);
        }
        else
        {
            Debug.Log("INVENTORY FULL");
        }
    }
}
