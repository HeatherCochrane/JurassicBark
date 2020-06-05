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
    void Start()
    {
        UIhandler = GameObject.Find("UIHandler").GetComponent<UIHandler>();
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

            dogs.Add(dog);
            currency.subtractMoney(cost);
        }
    }

    public void buyDogType(int num)
    {
        dogObject = UIhandler.getDog(num);
        cost = UIhandler.getDogCost(num);
    }
}
