using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerrainPainting : MonoBehaviour
{
    Material paint;

    string biome;

    [SerializeField]
    PaddockCreation paddock;

    List<GameObject> paddocks = new List<GameObject>();

    List<GameObject> dogs = new List<GameObject>();
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
        paddocks = paddock.getPaddocks();

        Material[] mats = t.GetComponent<MeshRenderer>().materials;
        mats[1] = paint;
        t.GetComponent<MeshRenderer>().materials = mats;
        t.setTerrainPaint(paint.name);

        t.hasPaint = true;

        if (paddocks.Count > 0)
        {
            for (int i = 0; i < paddocks.Count; i++)
            {
                dogs = paddocks[i].GetComponentInChildren<PaddockControl>().getDogs();

                if (dogs.Count > 0)
                {
                    for (int j = 0; j < dogs.Count; j++)
                    {
                        dogs[i].GetComponent<DogBehaviour>().checkTiles();
                    }
                }
            }
        }
    }
}
