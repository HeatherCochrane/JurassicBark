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
    }

    //Call this function when a tile is changed such as placing deco, removing objects, paddocks, paint etc;
    public void saveTile(EnvironmentTile t, bool remove, bool paint)
    {
        if (t.gameObject.transform.childCount > 0 && !paint && !remove)
        {
            string[] name = t.gameObject.transform.GetChild(0).name.Split('(');
            SaveGame.Instance.Tile.childModel = name[0];
            Debug.Log(name[0]);
        }
        else
        {
            SaveGame.Instance.Tile.childModel = null;
        }

        SaveGame.Instance.Tile.removeChild = remove;

        SaveGame.Instance.Tile.isAccesible = t.IsAccessible;
        SaveGame.Instance.Tile.isPaddock = t.isPaddock;
        SaveGame.Instance.Tile.isPath = t.isPath;
        SaveGame.Instance.Tile.hasPaint = t.hasPaint;

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

        SaveGame.Instance.changedTile.Add(SaveGame.Instance.Tile);

        Debug.Log(SaveGame.Instance.changedTile.Count);
        SaveGame.Save();
    }
}
