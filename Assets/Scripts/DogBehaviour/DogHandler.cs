using System.Collections;
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
    List<string> personalities = new List<string>();

    [SerializeField]
    List<string> genders = new List<string>();

    string personality;
    int age = 0;

    Material terrain;

    int terrainAmount;

    int but = 0;

    [SerializeField]
    AudioManager audioManager;

    [SerializeField]
    SaveHandler save;

    [SerializeField]
    ParkRating rating;

    TextAsset dogNames;
    string dogName;

    List<GameObject> dogTypes = new List<GameObject>();
    void Start()
    {
        UIhandler = GameObject.Find("UIHandler").GetComponent<UIHandler>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void spawnDog(Vector3 pos, Transform parent, EnvironmentTile current, GameObject control)
    {
        if (dogObject != null && currency.sufficientFunds(cost))
        {
            dog = Instantiate(dogObject);
            dog.transform.position = pos;

            dog.transform.parent = parent;

            control.GetComponentInChildren<PaddockControl>().addDog(dog, current);
            dog.GetComponent<DogBehaviour>().setIdentifier(control.GetComponentInChildren<PaddockControl>().getPaddockIdentifier());


            age = Random.Range(1, 4);
            personality = personalities[Random.Range(0, personalities.Count)];

            dogNames = Resources.Load("DogNames") as TextAsset;
            List<string> names = new List<string>(dogNames.text.Split('\n'));
            dogName = names[Random.Range(0, names.Count - 1)];

            dog.GetComponent<DogBehaviour>().giveDogInfo(personality, age, dogName, false);

            dog.GetComponent<DogBehaviour>().setTerrain(getTerrain(), getTerrainAmount());

            dogs.Add(dog);
            currency.subtractMoney(cost);

            audioManager.playDogBark();

            
            bool duplicateModel = false;
            if (dogTypes.Count > 0)
            {
                for (int i = 0; i < dogTypes.Count; i++)
                {
                    if (dog.name == dogTypes[i].name)
                    {
                        duplicateModel = true;
                    }
                }
            }

            if(!duplicateModel)
            {
                dogTypes.Add(dog);
            }
            saveDog(dog);
            rating.addDog(dog.GetComponent<DogBehaviour>().getDogIdentifier(), dog.GetComponent<DogBehaviour>().getHappiness());
        }
    }

    public void buyDogType(int num)
    {
        dogObject = UIhandler.getDog(num);
        cost = UIhandler.getDogCost(num);
        but = num;
    }

    public GameObject getStandIn(int button)
    {
        return UIhandler.getDog(button);
    }

    public Material getTerrain()
    {
        return UIhandler.getTerrain(but);
    }
    public int getTerrainAmount()
    {
        return UIhandler.getTerrainAmount(but);
    }

    public void saveDog(GameObject d)
    {
        save.saveDog(dog, dog.GetComponent<DogBehaviour>().getDogIdentifier(), dog.GetComponent<DogBehaviour>().CurrentPosition);
    }

    public void saveDogs()
    {
        if (dogs.Count > 0)
        {
            for (int i = 0; i < dogs.Count; i++)
            {
                dogs[i].GetComponent<DogBehaviour>().saveDogDetails();
            }
        }
    }

    public List<GameObject> getDogs()
    {
        return dogs;
    }

    public List<GameObject> getDogTypes()
    {
        return dogTypes;
    }
}
