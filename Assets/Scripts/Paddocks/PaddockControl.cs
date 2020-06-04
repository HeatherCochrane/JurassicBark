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

    [SerializeField]
    GameObject paddockUI;
    bool showPaddockUI = false;

    GameObject UICanvas;
    // Start is called before the first frame update
    void Start()
    {
        game = GameObject.Find("Game").GetComponent<Game>();
        UICanvas = GameObject.Find("GameUI");

        //Show the paddock stats on the game canvas
        paddockUI = Instantiate(paddockUI);
        paddockUI.transform.parent = UICanvas.transform;
        paddockUI.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if(showPaddockUI)
        {
            paddockUI.SetActive(true);
            paddockUI.transform.position = new Vector3(Input.mousePosition.x, Input.mousePosition.y + 10, Input.mousePosition.z);
        }
        else
        {
            paddockUI.SetActive(false);
        }
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
            Destroy(paddockUI);
        }
    }

    private void OnMouseOver()
    {
        if(!game.getDeleting())
        {
            showPaddockUI = true;
        }
    }

    private void OnMouseExit()
    {
        showPaddockUI = false;
    }
}
