using UnityEngine;
using System.Collections.Generic;


public class SaveGame
{

    //serialized
    public string PlayerName = "Player";
    public int XP = 0;
    public Vector3 PlayerPosition;
    public GameObject fullGame;


    //Changed tiles from the original map
    public struct mapTile
    {
        public GameObject parentObj;
        public GameObject childObj;
        public bool isAccesible;
        public bool isPath;
        public bool isPaddock;
        public bool hasPaint;
        public int x;
        public int y;
    }

    public List<mapTile> changedTile = new List<mapTile>();
    public mapTile mapTileStandIn;

    private static string _gameDataFileName = "data.json";

    private static SaveGame _instance;
    public static SaveGame Instance
    {
        get
        {
            if (_instance == null)
                Load();
            return _instance;
        }

    }

    public static void Save()
    {
        FileManager.Save(_gameDataFileName, _instance);
    }

    public static void Load()
    {
        _instance = FileManager.Load<SaveGame>(_gameDataFileName);
    }

}