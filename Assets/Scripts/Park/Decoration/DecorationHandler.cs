using System.Collections;
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

    [SerializeField]
    Game game;

    [SerializeField]
    SaveHandler save;

    [SerializeField]
    ParkRating rating;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void spawnDecoration(Vector3 pos, EnvironmentTile p, Vector3 r)
    {
        if (currency.sufficientFunds(cost))
        {
            deco = Instantiate(standIn);
            deco.transform.position = pos;
            deco.transform.Rotate(r);
            deco.transform.SetParent(p.transform);
            decorations.Add(deco);
            p.IsAccessible = false;

            currency.subtractMoney(cost);

            deco.GetComponentInChildren<ParticleSystem>().Play();


            save.saveTile(p,false);
            rating.addDecoration();
        }
    }

    public void getDecoration(int button)
    {
        standIn = UIHandler.getDecoration(button);
        cost = UIHandler.getDecorationCost(button);
    }

    public GameObject getStandIn(int button)
    {
        return UIHandler.getDecoration(button);
    }

    
}
