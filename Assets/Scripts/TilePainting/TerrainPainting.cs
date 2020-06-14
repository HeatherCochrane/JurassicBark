using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerrainPainting : MonoBehaviour
{
    [SerializeField]
    PaddockCreation paddocksHandler;
    Material paint;

    string biome;

    [SerializeField]
    PaddockCreation paddock;

    List<GameObject> paddocks = new List<GameObject>();


    List<GameObject> dogs = new List<GameObject>();

    [SerializeField]
    SaveHandler save;
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

        if (paddocks != null && paddocks.Count > 0)
        {
            for (int i = 0; i < paddocks.Count; i++)
            {
                dogs = paddocks[i].GetComponentInChildren<PaddockControl>().getDogs();

                if (dogs != null && dogs.Count > 0)
                {
                    for (int j = 0; j < dogs.Count; j++)
                    {
                        dogs[j].GetComponent<DogBehaviour>().checkTiles();
                    }
                }
            }
        }

        save.saveTile(t, false, true);
    }
}
