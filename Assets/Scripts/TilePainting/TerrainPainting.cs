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

    [SerializeField]
    DogHandler dogHandler;



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
        t.isPath = false;

        if (t.getControlObj() != null)
        {
            List<GameObject> dogs = dogHandler.getDogTypes();

            if (dogs.Count > 0)
            {
                for (int i = 0; i < dogs.Count; i++)
                {
                    dogs[i].GetComponent<DogBehaviour>().checkTiles();
                }   
            }
        }

        save.saveTile(t);
    }
}
