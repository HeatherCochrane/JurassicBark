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
    int minimumDogHappiness;


    int decorationCount = 0;
    int dogCount = 0;
    int shopCount = 0;
    int overallHappiness = 0;

    public struct dog
    {
        [SerializeField]
        public int happiness;
        [SerializeField]
        public int identity;
    }

    List<dog> dogs = new List<dog>();
    dog newDog;

    int parkRating = 1;

    [SerializeField]
    List<Sprite> stars = new List<Sprite>();

    [SerializeField]
    GameObject starObject;

    [SerializeField]
    VisitorHandler visitors;

    [SerializeField]
    SaveHandler save;

    [SerializeField]
    GameObject ratingHelp;

    [SerializeField]
    GameObject wellDoneBanner;
    bool ratingMaxed = false;

    // Start is called before the first frame update
    void Start()
    {
        ratingHelp.SetActive(false);

        ratingHelp.transform.GetChild(0).GetComponent<Text>().text = "Visitors would like to see more decorations!";
        ratingHelp.transform.GetChild(1).GetComponent<Text>().text = "Visitors would like to see more dogs!";
        ratingHelp.transform.GetChild(2).GetComponent<Text>().text = "Visitors would like to have more shops!";
        ratingHelp.transform.GetChild(3).GetComponent<Text>().text = "The dogs look unhappy, the visitors are unhappy!";

        wellDoneBanner.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Return))
        {
            adjustRating();
        }
    }

    public void loadParkRating()
    {
        SaveGame.Load();

        dogCount = SaveGame.Instance.parkRanking.dogCount;
        decorationCount = SaveGame.Instance.parkRanking.decoCount;
        shopCount = SaveGame.Instance.parkRanking.shopCount;
        overallHappiness = SaveGame.Instance.parkRanking.overallHappiness;

        adjustRating();

        ratingHelp.transform.GetChild(0).GetComponent<Text>().text = "Visitors would like to see more decorations!";
        ratingHelp.transform.GetChild(1).GetComponent<Text>().text = "Visitors would like to see more dogs!";
        ratingHelp.transform.GetChild(2).GetComponent<Text>().text = "Visitors would like to have more shops!";
        ratingHelp.transform.GetChild(3).GetComponent<Text>().text = "The dogs look unhappy, the visitors are unhappy!";
    }

    void adjustRating()
    {
        parkRating = 1;
        visitors.setVisitorCount(20);

        if (decorationCount >= minimumDeco)
        {
            parkRating += 1;
            visitors.setVisitorCount(30);
            ratingHelp.transform.GetChild(0).gameObject.SetActive(false);
        }
        else
        {
            ratingHelp.transform.GetChild(0).gameObject.SetActive(true);
            ratingHelp.transform.GetChild(0).GetComponent<Text>().text = "Visitors would like to see more decorations!";
        }
        if (dogCount >= minimumDogs)
        {
            parkRating += 1;
            visitors.setVisitorCount(40);
            ratingHelp.transform.GetChild(1).gameObject.SetActive(false);
        }
        else
        {
            ratingHelp.transform.GetChild(1).gameObject.SetActive(true);
            ratingHelp.transform.GetChild(1).GetComponent<Text>().text = "Visitors would like to see more dogs!";
        }
        if (shopCount >= minimumShops)
        {
            parkRating += 1;
            visitors.setVisitorCount(50);
            ratingHelp.transform.GetChild(2).gameObject.SetActive(false);
        }
        else
        {
            ratingHelp.transform.GetChild(2).gameObject.SetActive(true);
            ratingHelp.transform.GetChild(2).GetComponent<Text>().text = "Visitors would like to have more shops!";
        }
        if(overallHappiness >= minimumDogHappiness)
        {
            parkRating += 1;
            visitors.setVisitorCount(60);
            ratingHelp.transform.GetChild(3).gameObject.SetActive(false);
        }
        else
        {
            ratingHelp.transform.GetChild(3).gameObject.SetActive(true);
            ratingHelp.transform.GetChild(3).GetComponent<Text>().text = "The dogs look unhappy, the visitors are unhappy!";
        }

        if(parkRating == 5)
        {
            ratingHelp.transform.GetChild(0).gameObject.SetActive(true);
            ratingHelp.transform.GetChild(0).GetComponent<Text>().text = "Wow! Your park has reached a 5 star rating, well done!";

            if(ratingMaxed == false)
            {
                wellDoneBanner.SetActive(true);
                ratingMaxed = true;
            }
        }
        starObject.GetComponent<Image>().sprite = stars[parkRating - 1];
        save.saveParkRanking(dogCount, shopCount, decorationCount, overallHappiness);

        
    }

    public void addDecoration()
    {

        decorationCount += 1;
        adjustRating();
    }

    public void removeDecoration()
    {
        decorationCount -= 1;
        if (decorationCount < 1)
        {
            decorationCount = 0;
        }

        adjustRating();
    }

    public void addDog(int identity, int happiness)
    {

        newDog.identity = identity;
        newDog.happiness = happiness;
        dogs.Add(newDog);

        dogCount += 1;


        adjustRating();
    }

    public void removeDog(int identity)
    {

        dogCount -= 1;

        if (dogCount < 1)
        {
            dogCount = 0;
        }

        for(int i =0; i < dogs.Count; i++)
        {
            if(dogs[i].identity == identity)
            {
                dogs.RemoveAt(i);
            }
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

        if (shopCount < 0)
        {
            shopCount = 0;
        }

        adjustRating();
    }

    void calculateDogHappiness()
    {

        if (dogs.Count > 0)
        {
            overallHappiness = 0;


            for (int i = 0; i < dogs.Count; i++)
            {
                int happy = dogs[i].happiness;
                overallHappiness += happy;
            }

            overallHappiness = overallHappiness / dogs.Count;
        }

        adjustRating();
    }
    public void updateDogHappiness(int identifier, int happiness)
    {

        for (int i =0; i < dogs.Count; i++)
        {
            if(dogs[i].identity == identifier)
            {
                newDog.identity = identifier;
                newDog.happiness = happiness;
                dogs[i] = newDog;
            }
        }

        calculateDogHappiness();
    }


    public void showRatingHelp(bool set)
    {
        ratingHelp.SetActive(set);
    }

    public void hideBanner()
    {
        wellDoneBanner.SetActive(false);
    }
}
