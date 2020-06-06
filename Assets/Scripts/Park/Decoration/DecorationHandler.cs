﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DecorationHandler : MonoBehaviour
{
    [SerializeField]
    UIHandler UIHandler;

    GameObject deco;
    int cost = 0;

    List<GameObject> decorations = new List<GameObject>();
    GameObject standIn;

    [SerializeField]
    Currency currency;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void spawnDecoration(Vector3 pos, EnvironmentTile p)
    {
        if (currency.sufficientFunds(cost))
        {
            deco = Instantiate(standIn);
            deco.transform.position = pos;
            deco.transform.SetParent(p.transform);
            decorations.Add(deco);
            p.IsAccessible = false;
        }
    }

    public void getDecoration(int button)
    {
        standIn = UIHandler.getDecoration(button);
        cost = UIHandler.getDecorationCost(button);
    }
}