using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PaddockControl : MonoBehaviour
{
    EnvironmentTile[,] tiles;
    int width = 0;
    int height = 0;

    [SerializeField]
    Color[] grassColor;


    Game game;
    // Start is called before the first frame update
    void Start()
    {
        game = GameObject.Find("Game").GetComponent<Game>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void setTiles(EnvironmentTile[,] t, int w, int h)
    {
        tiles = t;
        width = w;
        height = h;
    }

    void returnTiles()
    {
        for(int i = 0; i < width; i++)
        {
            for(int j = 0; j < height; j++)
            {
                tiles[i, j].isPaddock = false;
                tiles[i, j].hasFence = false;

                //Set color of tiles back to one of the two original colours
                Material[] mat = tiles[i, j].GetComponent<MeshRenderer>().materials;
                mat[1].color = grassColor[Random.Range(0, 1)];
                tiles[i, j].GetComponent<MeshRenderer>().materials = mat;
            }
        }
    }
    private void OnMouseDown()
    {
        if (game.getDeleting())
        {
            returnTiles();
            Destroy(this.transform.parent.parent.gameObject);
            game.setDeleting(false);
        }
    }
}
