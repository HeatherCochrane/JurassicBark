using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tester : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //if(Input.GetKeyDown(KeyCode.Space))
        //{
        //    Save();
        //}

        //if(Input.GetKeyDown(KeyCode.Return))
        //{
        //    Load();
        //}
    }

    public void Load()
    {
        SaveGame.Load();
        transform.position = SaveGame.Instance.PlayerPosition;
    }

    public void Save()
    {
        SaveGame.Instance.PlayerPosition = transform.position;
        SaveGame.Save();
    }
}
