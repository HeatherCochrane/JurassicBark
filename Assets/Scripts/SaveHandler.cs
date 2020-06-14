using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveHandler : MonoBehaviour
{
    int identity = 0;
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {

    }

   public void clearList()
    {
        SaveGame.Instance.changedTile.Clear();
        SaveGame.Instance.allPaddocks.Clear();
        SaveGame.Instance.paddock.Clear();

        SaveGame.Instance.changedTile = new List<SaveGame.mapTile>();
        SaveGame.Instance.allPaddocks = new List<List<SaveGame.Paddock>>();
        SaveGame.Instance.paddock = new List<SaveGame.Paddock>();
    }

    //Call this function when a tile is changed such as placing deco, removing objects, paddocks, paint etc;
    public void saveTile(EnvironmentTile t, bool paint)
    {
        if (t.gameObject.transform.childCount > 0 && !paint)
        {
            string[] name = t.gameObject.transform.GetChild(0).name.Split('(');
            SaveGame.Instance.Tile.childModel = name[0];
            SaveGame.Instance.Tile.rot = t.gameObject.transform.GetChild(0).eulerAngles;
            SaveGame.Instance.Tile.childPos = t.gameObject.transform.GetChild(0).position;
            SaveGame.Instance.Tile.hasChild = true;
        }
        else
        {
            SaveGame.Instance.Tile.childModel = null;
            SaveGame.Instance.Tile.hasChild = false;
        }


        SaveGame.Instance.Tile.isAccesible = t.IsAccessible;
        SaveGame.Instance.Tile.isPaddock = t.isPaddock;
        SaveGame.Instance.Tile.isPath = t.isPath;
        SaveGame.Instance.Tile.hasPaint = t.hasPaint;
        SaveGame.Instance.Tile.hasFence = t.hasFence;

        string[] pos = t.name.Split(',');
        SaveGame.Instance.Tile.x = int.Parse(pos[0]);
        SaveGame.Instance.Tile.y = int.Parse(pos[1]);

        if (paint)
        {
            Material[] mats = t.GetComponent<MeshRenderer>().materials;

            string[] matName = mats[1].name.Split(' ');
            SaveGame.Instance.Tile.parentMat = matName[0];

            SaveGame.Instance.Tile.matChanged = true;
        }
        else
        {
            SaveGame.Instance.Tile.matChanged = false;
        }
        for(int i =0; i < SaveGame.Instance.changedTile.Count; i++)
        {
            if(SaveGame.Instance.Tile.x == SaveGame.Instance.changedTile[i].x && SaveGame.Instance.Tile.y == SaveGame.Instance.changedTile[i].y)
            {
                SaveGame.Instance.changedTile.RemoveAt(i);
            }
        }

        SaveGame.Instance.changedTile.Add(SaveGame.Instance.Tile);

        SaveGame.Save();
    }

    public void savePaddock(EnvironmentTile[,] tiles, int w, int h)
    {
        for (int i = 0; i < w; i++)
        {
            for (int j = 0; j < h; j++)
            {
                string[] name = tiles[i, j].name.Split(',');
                SaveGame.Instance.newPaddock.x = int.Parse(name[0]);
                SaveGame.Instance.newPaddock.y = int.Parse(name[1]);
                SaveGame.Instance.newPaddock.size = w * h;
                SaveGame.Instance.newPaddock.width = w;
                SaveGame.Instance.newPaddock.height = h;
                SaveGame.Instance.newPaddock.identifier = identity;
                Debug.Log("Loop");

                SaveGame.Instance.paddock.Add(SaveGame.Instance.newPaddock);
            }
        }

        SaveGame.Instance.allPaddocks.Add(SaveGame.Instance.paddock);
        Debug.Log(SaveGame.Instance.allPaddocks.Count);

        Debug.Log("Saved paddock");
        SaveGame.Save();
    }
}
