using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DogBehaviour : MonoBehaviour
{ 

    // Start is called before the first frame update

    float turnRate = 2.5f;
    bool collidingWithWall = false;

    bool collidingWithDog = false;

    Rigidbody rb;

    bool stop = false;
    int stoppingTime = 0;

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
            movement();
        }
    }
    void movement()
    {
        Vector3 fwd = transform.TransformDirection(Vector3.forward);

        if (Physics.Raycast(transform.position, fwd, 5) || collidingWithWall || collidingWithDog)
        {
            print("There is something in front of the object!");
            transform.eulerAngles += new Vector3(transform.eulerAngles.x, turnRate, transform.eulerAngles.z);
        }

        transform.position += fwd / 10;
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
