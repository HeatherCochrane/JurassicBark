using UnityEngine;
using System.Collections.Generic;


public class SaveGame
{
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
        public bool hasFood;
        [SerializeField]
        public bool hasWater;
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
        public int identifier;
        [SerializeField]
        public string breed;
        [SerializeField]
        public int hunger;
        [SerializeField]
        public int thirst;
        [SerializeField]
        public int happiness;
        [SerializeField]
        public string gender;
        [SerializeField]
        public string age;
        [SerializeField]
        public string personality;
        [SerializeField]
        public int paddockIdentifier;
        [SerializeField]
        public string tile;
        [SerializeField]
        public string terrain;
        [SerializeField]
        public int terrainAmount;
    }

    [System.Serializable]
    public class ListWrapper
    {
        [SerializeField]
        public List<Paddock> paddocks = new List<Paddock>();
    }

    [System.Serializable]
    public struct unlocks
    {
        [SerializeField]
        public List<int> dogScreen;
        [SerializeField]
        public List<int> fenceScreen;
        [SerializeField]
        public List<int> decorationsScreen;
        [SerializeField]
        public List<int> paddockItemsScreen;
        [SerializeField]
        public List<int> pathsScreen;
        [SerializeField]
        public List<int> shopsScreen;
    }

    [System.Serializable]
    public struct UI
    {
        [SerializeField]
        public int currency;
        [SerializeField]
        public int points;
    }

    [SerializeField]
    public unlocks unlockables;

    [SerializeField]
    public UI UIelements;

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

    [SerializeField]
    public List<dog> dogs = new List<dog>();
    [SerializeField]
    public dog newDog;

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