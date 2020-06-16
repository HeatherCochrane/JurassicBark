using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ParkRating : MonoBehaviour
{
    [SerializeField]
    int minimumDogs;
    [SerializeField]
    int minimumShops;
    [SerializeField]
    int minimumDeco;
    [SerializeField]
    int minimumPaddockHappiness;


    int decorationCount = 0;
    int dogCount = 0;
    int shopCount = 0;

    int numPaddocks = 0;
    int averagePaddockHappiness = 0;

    int parkRating = 1;

    [SerializeField]
    List<Sprite> stars = new List<Sprite>();

    [SerializeField]
    GameObject starObject;
    // Start is called before the first frame update
    void Start()
    {
        adjustRating();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void adjustRating()
    {
        parkRating = 1;
        

        if(decorationCount >= minimumDeco)
        {
            parkRating += 1;
        }
        if(dogCount >= minimumDogs)
        {
            parkRating += 1;
        }
        if(shopCount >= minimumShops)
        {
            parkRating += 1;
        }

        starObject.GetComponent<Image>().sprite = stars[parkRating - 1];
      
        Debug.Log("Park Rating: " + parkRating);
    }

    public void addDecoration()
    {
        decorationCount += 1;
        adjustRating();
    }

    public void removeDecoration()
    {
        decorationCount -= 1;
        if(decorationCount < 1)
        {
            decorationCount = 0;
        }
        adjustRating();
    }

    public void addDog()
    {
        dogCount += 1;
        adjustRating();
    }

    public void removeDog()
    {
        dogCount -= 1;

        if(dogCount < 1)
        {
            dogCount = 0;
        }
        adjustRating();
    }

    public void addShop()
    {
        shopCount += 1;
        adjustRating();
    }

    public void removeShop()
    {
        shopCount -= 1;
        if(shopCount < 0)
        {
            shopCount = 0;
        }
        adjustRating();
    }

    //public void addPaddock(int happiness)
    //{
    //    numPaddocks += 1;
    //    averagePaddockHappiness += happiness;

    //    averagePaddockHappiness /= numPaddocks;
    //}

    //public void removePaddock(int happiness)
    //{
    //    numPaddocks -= 1;
    //    averagePaddockHappiness -= happiness;

    //    if(numPaddocks < 1)
    //    {
    //        numPaddocks = 0;
    //    }
    //    if(averagePaddockHappiness < 1)
    //    {
    //        averagePaddockHappiness = 0;
    //    }

    //    if(numPaddocks > 0 && averagePaddockHappiness > 0)
    //    {
    //        averagePaddockHappiness /= numPaddocks;
    //    }
    //}
}
