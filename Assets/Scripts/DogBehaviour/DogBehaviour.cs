using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DogBehaviour : MonoBehaviour
{ 

    // Start is called before the first frame update

    float turnRate = 6f;
    bool collidingWithWall = false;

    bool collidingWithDog = false;

    Rigidbody rb;

    bool stop = false;
    int stoppingTime = 0;

    float hungerLevel = 0;
    float thirstLevel = 0;
    float happinessLevel = 0;

    void Start()
    {
        stoppingTime = Random.Range(5, 15);
        timer();
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void FixedUpdate()
    {
        if (!stop)
        {

        }
    }

    void timer()
    {
        if(stop)
        {
            stop = false;
        }
        else
        {
            stop = true;
        }

        Invoke("timer", stoppingTime);
    }

    void decideNextAction()
    {
        int ran = Random.Range(1, 4);

        switch(ran)
        {
            //Walk around
            case 1:
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
            default:
                break;
        }
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
}
