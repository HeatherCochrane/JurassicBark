using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveHandler : MonoBehaviour
{

    [SerializeField]
    GameObject fullGame;

    GameObject previousGame;


    
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    //public void Load()
    //{
    //    SaveGame.Load();

    //    previousGame = fullGame;

    //    fullGame = Instantiate(SaveGame.Instance.fullGame);

    //    string[] Name = fullGame.transform.name.Split('(');
    //    fullGame.transform.name = Name[0];

    //    for (int i =0; i < fullGame.transform.childCount; i++)
    //    {
    //        string[] newName = fullGame.transform.GetChild(i).name.Split('(');
    //        fullGame.transform.GetChild(i).name = newName[0];
    //        for(int j =0; j < fullGame.transform.GetChild(i).childCount; j++)
    //        {
    //            string[] newName1 = fullGame.transform.GetChild(i).GetChild(j).name.Split('(');
    //            fullGame.transform.GetChild(i).name = newName1[0];
    //        }
    //    }

    //}

    //public void Save()
    //{
    //    SaveGame.Instance.fullGame = fullGame;
    //    SaveGame.Save();
    //}
}
