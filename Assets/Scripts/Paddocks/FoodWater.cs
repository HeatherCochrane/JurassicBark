using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoodWater : MonoBehaviour
{
    int max = 3;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void removePiece()
    {
        Debug.Log("Called");

        max -= 1;
        if(max <= 0)
        {
            this.transform.parent.GetComponent<EnvironmentTile>().IsAccessible = true;
            this.transform.parent.GetComponent<EnvironmentTile>().hasFoodBowl = false;
            this.transform.parent.GetComponent<EnvironmentTile>().hasWaterBowl = false;
            Destroy(this.gameObject);
        }
    }
}
