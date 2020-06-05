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
    void Start()
    { 
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void spawnDog(Vector3 pos, Transform parent, EnvironmentTile current)
    {
        if (dogObject != null)
        {
            dog = Instantiate(dogObject);
            dog.transform.position = pos;

            dog.transform.parent = parent;

            parent.GetComponentInChildren<PaddockControl>().addDog(dog, current);

            dogs.Add(dog);
        }
    }

    public void buyDogType(GameObject d)
    {
        dogObject = d;
    }
}
