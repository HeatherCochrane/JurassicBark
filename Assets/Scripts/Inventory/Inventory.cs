using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Inventory : MonoBehaviour
{
    [System.Serializable]
    struct DogInventory
    {
        [SerializeField]
        public GameObject dog;
        [SerializeField]
        public GameObject inventoryPlace;
    }

    [SerializeField]
    List<DogInventory> storedDogs = new List<DogInventory>();
    int maxStoredDogs = 9;


    [SerializeField]
    GameObject inventoryParent;

    [SerializeField]
    GameObject inventorySlot;

    GameObject newSlot;

    DogInventory newDog;

    [SerializeField]
    Text capacity;
    int dogCapacity = 0;

    [SerializeField]
    GameObject inventoryDetails;

    bool inventoryToggle = false;
    // Start is called before the first frame update
    void Start()
    {
        inventoryParent.SetActive(false);
        inventoryDetails.SetActive(false);
        capacity.text = dogCapacity.ToString() + "/" + maxStoredDogs.ToString();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void showInventory()
    {
       if(inventoryToggle)
        {
            inventoryToggle = false;
        }
        else
        {
            inventoryToggle = true;
        }

        inventoryParent.SetActive(inventoryToggle);
        inventoryDetails.SetActive(inventoryToggle);
    }

    public bool isDogInventoryFull()
    {
        if(storedDogs.Count == maxStoredDogs)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public int getDogInventorySize()
    {
        return storedDogs.Count;
    }
    public void storeDog(GameObject d)
    {
        newSlot = Instantiate(inventorySlot);
        newSlot.transform.parent = inventoryParent.transform;

        newDog.dog = d;
        newDog.inventoryPlace = newSlot;

        storedDogs.Add(newDog);
        dogCapacity += 1;
        capacity.text = dogCapacity.ToString() + "/" + maxStoredDogs.ToString();
        
    }

}
