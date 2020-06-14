using UnityEngine;
using System.Collections.Generic;


public class SaveGame
{

    //serialized
    //public string PlayerName = "Player";
    //public int XP = 0;
    //public Vector3 PlayerPosition;
    //public GameObject fullGame;


    //Changed tiles from the original map
    [System.Serializable]
    public struct mapTile
    {
        [SerializeField]
        public string childModel;
        [SerializeField]
        public bool hasChild;
        [SerializeField]
        public bool isAccesible;
        [SerializeField]
        public bool isPath;
        [SerializeField]
        public bool isPaddock;
        [SerializeField]
        public bool hasPaint;
        [SerializeField]
        public bool hasFence;
        [SerializeField]
        public bool matChanged;
        [SerializeField]
        public int x;
        [SerializeField]
        public int y;
        [SerializeField]
        public string parentMat;
        [SerializeField]
        public Vector3 rot;
        [SerializeField]
        public Vector3 childPos;
        [SerializeField]
        public List<Paddock> paddockTiles;
    }

    [System.Serializable]
    public struct Paddock
    {
        [SerializeField]
        public string name;
        [SerializeField]
        public int identifier;
        [SerializeField]
        public int size;
        [SerializeField]
        public int width;
        [SerializeField]
        public int height;
    }
    [System.Serializable]
    public struct dog
    {
        [SerializeField]
        public string breed;
        [SerializeField]
        int hunger;
        [SerializeField]
        int thirst;
        [SerializeField]
        int happiness;
    }

    [System.Serializable]
    public class ListWrapper
    {
        [SerializeField]
        public List<Paddock> paddocks = new List<Paddock>();
    }

    [SerializeField]
    public ListWrapper paddockLists;

    [SerializeField]
    public List<ListWrapper> allPaddocks = new List<ListWrapper>();
    [SerializeField]
    public List<Paddock> paddock = new List<Paddock>();
    [SerializeField]
    public Paddock newPaddock;

    [SerializeField]
    public List<mapTile> changedTile = new List<mapTile>();
    [SerializeField]
    public mapTile Tile;

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