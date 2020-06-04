using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DogHandler : MonoBehaviour
{
    // Start is called before the first frame update

    [SerializeField]
    GameObject dogObject;

    GameObject dog;

    List<GameObject> dogs = new List<GameObject>();

    float space = 2f;
    void Start()
    { 
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void spawnDogs()
    {
        for (int i = 0; i < 14; i++)
        {
            dog = Instantiate(dogObject);
            dog.transform.position += new Vector3(0, 0, space);
            dogs.Add(dog);

            space += 2f;
        }
    }

    public void spawnDog(Vector3 pos, Transform parent, EnvironmentTile current)
    {
        dog = Instantiate(dogObject);
        dog.transform.position = pos;

        dog.transform.parent = parent;

        parent.GetComponentInChildren<PaddockControl>().addDog(dog, current);

        dogs.Add(dog);
    }
}
