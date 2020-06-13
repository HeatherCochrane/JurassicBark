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

   
    //Call this function when a tile is changed such as placing deco, removing objects, paddocks, paint etc;
    public void saveTile(EnvironmentTile t)
    {
        if (t.gameObject.transform.childCount > 0)
        {
            string[] name = t.gameObject.transform.GetChild(0).name.Split('(');
            SaveGame.Instance.Tile.childModel = name[0];
            Debug.Log(name[0]);
        }

        SaveGame.Instance.Tile.isAccesible = t.IsAccessible;
        SaveGame.Instance.Tile.isPaddock = t.isPaddock;
        SaveGame.Instance.Tile.isPath = t.isPath;
        SaveGame.Instance.Tile.hasPaint = t.hasPaint;

        string[] pos = t.name.Split(',');
        SaveGame.Instance.Tile.x = int.Parse(pos[0]);
        SaveGame.Instance.Tile.y = int.Parse(pos[1]);

        SaveGame.Instance.Tile.parentMat = t.GetComponent<MeshRenderer>().material;

        SaveGame.Instance.Tile.parentPos = t.transform.position;

        SaveGame.Instance.changedTile.Add(SaveGame.Instance.Tile);

        SaveGame.Save();
    }
}
