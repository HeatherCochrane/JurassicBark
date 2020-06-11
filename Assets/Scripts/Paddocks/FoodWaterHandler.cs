using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoodWaterHandler : MonoBehaviour
{
    [SerializeField]
    UIHandler UIHandle;

    GameObject standIn;
    int cost;

    [SerializeField]
    Currency currency;

    GameObject item;

    int b = 0;

    int max = 0;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void spawnItem(Vector3 pos, EnvironmentTile p, Vector3 r)
    {
        if (currency.sufficientFunds(cost))
        {
            item = Instantiate(standIn);
            item.transform.position = pos;
            item.transform.Rotate(r);
            item.transform.parent = p.transform;

            Transform control = p.transform.parent;

            if (b == 0)
            {
                control.GetComponentInChildren<PaddockControl>().hasFood = true;
                p.hasFoodBowl = true;
            }
            else
            {
                control.GetComponentInChildren<PaddockControl>().hasWater = true;
                p.hasWaterBowl = true;
            }

            item.GetComponent<FoodWater>().setMax(max);
            currency.subtractMoney(cost);

            item.GetComponentInChildren<ParticleSystem>().Play();

        }

    }

    public void getItem(int button)
    {
        standIn = UIHandle.getPaddockItems(button);
        cost = UIHandle.getPaddockItemsCost(button);
        max = UIHandle.getDurability(button);
        b = button;
    }
    public GameObject getStandIn(int button)
    {
        return UIHandle.getPaddockItems(button);
    }
}
