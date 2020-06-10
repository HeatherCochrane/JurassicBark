using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shop : MonoBehaviour
{
    List<EnvironmentTile> tiles = new List<EnvironmentTile>();

    GameObject previous;

    Currency currency;

    int cost = 0;

    AudioManager audioManager;
    // Start is called before the first frame update
    void Start()
    {
        currency = GameObject.Find("CurrencyHandler").GetComponent<Currency>();
        audioManager = GameObject.Find("AudioManager").GetComponent<AudioManager>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void setProductCost(int c)
    {
        cost = c;
    }
    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject != previous && other.tag == "Visitor")
        {
            currency.addMoney(cost);
            previous = other.gameObject;
            audioManager.playIncomeGained();
        }

    }


}
