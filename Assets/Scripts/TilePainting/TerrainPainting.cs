using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerrainPainting : MonoBehaviour
{
    Material paint;

    string biome;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void setPaint(Material p)
    {
        paint = p;
    }

    public void setBiome(string b)
    {
        b = biome;
    }
    public void paintTile(EnvironmentTile t)
    {
        Material[] mats = t.GetComponent<MeshRenderer>().materials;
        mats[1] = paint;
        t.GetComponent<MeshRenderer>().materials = mats;

        switch(biome)
        {
            case "Dirt": t.isDirt = true;
                break;
            case "Sand": t.isSand = true;
                break;
            case "LightGrass": t.isLightGrass = true;
                break;
            case "DarkGrass": t.isDarkGrass = true;
                break;
        }

        t.hasPaint = true;
    }
}
