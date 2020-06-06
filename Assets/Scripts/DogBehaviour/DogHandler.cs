﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DogHandler : MonoBehaviour
{
    // Start is called before the first frame update

    GameObject dogObject;

    GameObject dog;

    GameObject boughtDog;
    List<GameObject> dogs = new List<GameObject>();

    float space = 2f;

    int cost = 0;

    [SerializeField]
    Currency currency;

    UIHandler UIhandler;

    [SerializeField]
    GameObject dogProfile;
    [SerializeField]
    GameObject dogStats;

    [SerializeField]
    List<string> personalities = new List<string>();

    [SerializeField]
    List<string> genders = new List<string>();

    string personality;
    string gender;
    int age = 0;
    void Start()
    {
        UIhandler = GameObject.Find("UIHandler").GetComponent<UIHandler>();
        dogProfile.SetActive(false);
        dogStats.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void spawnDog(Vector3 pos, Transform parent, EnvironmentTile current)
    {
        if (dogObject != null && currency.sufficientFunds(cost))
        {
            dog = Instantiate(dogObject);
            dog.transform.position = pos;

            dog.transform.parent = parent;

            parent.GetComponentInChildren<PaddockControl>().addDog(dog, current);
            dog.GetComponent<DogBehaviour>().giveProfile(dogProfile, dogStats);

            age = Random.Range(1, 4);
            personality = personalities[Random.Range(0, personalities.Count)];
            gender = genders[Random.Range(0, genders.Count)];

            dog.GetComponent<DogBehaviour>().giveDogInfo(gender, personality, age);

            dogs.Add(dog);
            currency.subtractMoney(cost);
        }
    }

    public void buyDogType(int num)
    {
        dogObject = UIhandler.getDog(num);
        cost = UIhandler.getDogCost(num);
    }

    public GameObject getStandIn(int button)
    {
        return UIhandler.getDog(button);
    }

    public void setDogProfile(bool set)
    {
        dogProfile.SetActive(set);
    }
}
