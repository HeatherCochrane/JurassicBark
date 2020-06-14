using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveHandler : MonoBehaviour
{
    
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

        SaveGame.Instance.changedTile = new List<SaveGame.mapTile>();
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
            Debug.Log(SaveGame.Instance.Tile.rot);
            Debug.Log(name[0]);
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

            Debug.Log(SaveGame.Instance.Tile.parentMat);
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

        Debug.Log(SaveGame.Instance.changedTile.Count);
        SaveGame.Save();
    }

    public void savePaddock(EnvironmentTile[,] t, int w, int h, GameObject parent)
    {
        for(int i =0; i < w; i++)
        {
            for(int j =0; j < h; j++)
            {
                SaveGame.Instance.newPaddock.tiles.Add(t[i, j].name);
            }
        }

        SaveGame.Instance.newPaddock.parent = parent.name;

        SaveGame.Instance.paddocks.Add(SaveGame.Instance.newPaddock);

    }
}
