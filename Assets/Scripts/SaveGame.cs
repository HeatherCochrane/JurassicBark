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
        public bool removeChild;
        [SerializeField]
        public bool isAccesible;
        [SerializeField]
        public bool isPath;
        [SerializeField]
        public bool isPaddock;
        [SerializeField]
        public bool hasPaint;
        [SerializeField]
        public bool matChanged;
        [SerializeField]
        public int x;
        [SerializeField]
        public int y;
        [SerializeField]
        public string parentMat;
    }

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