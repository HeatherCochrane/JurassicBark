﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Task : MonoBehaviour
{

    int taskLength = 0;
    int timer = 0;
    int cost = 0;
    int unlockablePoints = 0;
    bool inprogress = false;


    Currency pointHandler;

    ResearchTaskHandler taskHandler;

    [SerializeField]
    Slider progressBar;
    // Start is called before the first frame update
    void Start()
    {
        taskHandler = GameObject.Find("ResearchTaskHandler").GetComponent<ResearchTaskHandler>();
        pointHandler = GameObject.Find("CurrencyHandler").GetComponent<Currency>();
        

        this.gameObject.transform.GetChild(3).gameObject.SetActive(false);
        progressBar.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void startTask()
    {
        if (pointHandler.sufficientFunds(cost) && !inprogress)
        {
            pointHandler.subtractMoney(cost);
            progressBar.maxValue = taskLength;
            this.transform.GetChild(1).gameObject.SetActive(false);
            progressBar.gameObject.SetActive(true);
            doTask();
        }
    }

    public void doTask()
    {
        inprogress = true;
        
        if (taskLength > 0)
        {
            taskLength -= 1;
            progressBar.value -= 1;
        }
        else
        {
            this.gameObject.transform.GetChild(3).gameObject.SetActive(true);

            if (GameObject.Find("ResearchTasksButton"))
            {
                GameObject.Find("ResearchTasksButton").transform.GetChild(1).gameObject.SetActive(true);
                progressBar.gameObject.SetActive(false);
            }
        }

        this.transform.GetChild(1).GetComponent<Text>().text = "Time: " + taskLength.ToString();

        Invoke("doTask", 1);

    }
    public void setTaskTime(int t)
    {
        taskLength = t;
    }

    public void setReward(int u)
    {
        unlockablePoints = u;
    }
    public void setCost(int c)
    {
        cost = c;
    }
    public int getReward()
    {
        return unlockablePoints;
    }

    public int getTime()
    {
        return taskLength;
    }
    public int getCost()
    {
        return cost;
    }
    public bool alreadyStarted()
    {
        return inprogress;
    }

    public void completeTask()
    {
        pointHandler.addPoints(unlockablePoints);
        taskHandler.completeTask();
        Destroy(this.gameObject);
        inprogress = false;
    }
}
