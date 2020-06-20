using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoodWater : MonoBehaviour
{
    int max = 3;

    SaveHandler save;

    GameObject p;
    // Start is called before the first frame update
    void Start()
    {
        save = GameObject.Find("SAVEHANDLER").GetComponent<SaveHandler>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void setMax(int m)
    {
        max = m;
    }

    public void removePiece()
    {
        max -= 1;

        if(max <= 0)
        {
            this.transform.parent.GetComponent<EnvironmentTile>().IsAccessible = true;
            this.transform.parent.GetComponent<EnvironmentTile>().hasFoodBowl = false;
            this.transform.parent.GetComponent<EnvironmentTile>().hasWaterBowl = false;

            p = this.transform.parent.gameObject;
            this.transform.parent = null;

            p.GetComponent<EnvironmentTile>().hasFoodBowl = false;
            p.GetComponent<EnvironmentTile>().hasWaterBowl = false;
            save.saveTile(p.GetComponent<EnvironmentTile>(), false);

            Destroy(this.gameObject);
        }
    }
}
