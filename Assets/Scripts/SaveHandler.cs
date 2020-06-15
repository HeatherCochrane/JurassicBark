﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveHandler : MonoBehaviour
{
    int identity = 0;
    int dogIdentity = 0;
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
        SaveGame.Instance.dogs.Clear();

        SaveGame.Instance.changedTile = new List<SaveGame.mapTile>();
        SaveGame.Instance.allPaddocks = new List<SaveGame.ListWrapper>();
        SaveGame.Instance.paddock = new List<SaveGame.Paddock>();
        SaveGame.Instance.dogs = new List<SaveGame.dog>();

        SaveGame.Instance.playerCurrency = "";
        SaveGame.Instance.playerPoints = "";

        identity = 0;
        dogIdentity = 0;
    }

    //Call this function when a tile is changed such as placing deco, removing objects, paddocks, paint etc;
    public void saveTile(EnvironmentTile t, bool paint)
    {
        if (t.gameObject.transform.childCount > 0)
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
        SaveGame.Instance.Tile.hasFood = t.hasFoodBowl;
        SaveGame.Instance.Tile.hasWater = t.hasWaterBowl;

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
        SaveGame.Instance.paddock.Clear();
        SaveGame.Instance.paddock = new List<SaveGame.Paddock>();

        SaveGame.Instance.newPaddock = new SaveGame.Paddock();
        SaveGame.Instance.paddockLists = new SaveGame.ListWrapper();

        for (int i = 0; i < w; i++)
        {
            for (int j = 0; j < h; j++)
            {
                SaveGame.Instance.newPaddock.name = tiles[i, j].name;
                SaveGame.Instance.newPaddock.size = w * h;
                SaveGame.Instance.newPaddock.width = w;
                SaveGame.Instance.newPaddock.height = h;
                SaveGame.Instance.newPaddock.identifier = identity;

                SaveGame.Instance.paddockLists.paddocks.Add(SaveGame.Instance.newPaddock);
            }
        }
        SaveGame.Instance.allPaddocks.Add(SaveGame.Instance.paddockLists);

        SaveGame.Save();

        identity += 1;
    }

    public void saveDog(GameObject dog, int identifier, EnvironmentTile t)
    {
        string[] name = dog.name.Split('(');
        SaveGame.Instance.newDog.breed = name[0];
        SaveGame.Instance.newDog.paddockIdentifier = identifier;

        SaveGame.Instance.newDog.hunger = dog.GetComponent<DogBehaviour>().getHunger();
        SaveGame.Instance.newDog.thirst = dog.GetComponent<DogBehaviour>().getThirst();
        SaveGame.Instance.newDog.happiness = dog.GetComponent<DogBehaviour>().getHappiness();
        SaveGame.Instance.newDog.tile = t.name;

        SaveGame.Instance.newDog.terrain = dog.GetComponent<DogBehaviour>().getTerrain().name;
        SaveGame.Instance.newDog.terrainAmount = dog.GetComponent<DogBehaviour>().getTerrainAmount();

        SaveGame.Instance.newDog.gender = dog.GetComponent<DogBehaviour>().getGender();
        SaveGame.Instance.newDog.age = dog.GetComponent<DogBehaviour>().getAge().ToString();
        SaveGame.Instance.newDog.personality = dog.GetComponent<DogBehaviour>().getPersonality();

        SaveGame.Instance.newDog.identifier = dogIdentity;

        SaveGame.Instance.dogs.Add(SaveGame.Instance.newDog);

        SaveGame.Save();

        dogIdentity += 1;
    }

    public void updateDogStats(int identity, GameObject dog)
    {
        for(int i =0; i < SaveGame.Instance.dogs.Count; i++)
        {
            if(SaveGame.Instance.dogs[i].identifier == identity)
            {
                SaveGame.Instance.newDog.hunger = dog.GetComponent<DogBehaviour>().getHunger();
                SaveGame.Instance.newDog.thirst = dog.GetComponent<DogBehaviour>().getThirst();
                SaveGame.Instance.newDog.happiness = dog.GetComponent<DogBehaviour>().getHappiness();
            }
        }

        SaveGame.Save();
    }
    public void savePoints(int points)
    {
        SaveGame.Instance.playerPoints = points.ToString();
        SaveGame.Save();
    }

    public void saveCurrency(int currency)
    {
        SaveGame.Instance.playerCurrency = currency.ToString();
        SaveGame.Save();
    }

    public void removePaddock(int identitifier)
    {

        for(int i =0; i < SaveGame.Instance.allPaddocks.Count; i++)
        {
            if(SaveGame.Instance.allPaddocks[i].paddocks[0].identifier == identitifier)
            {
                SaveGame.Instance.allPaddocks.RemoveAt(i);
            }
        }

        SaveGame.Save();
    }

    public void removeDog(int identifier)
    {
        for(int i =0; i < SaveGame.Instance.dogs.Count; i++)
        {
            if(SaveGame.Instance.dogs[i].paddockIdentifier == identifier)
            {
                SaveGame.Instance.dogs.RemoveAt(i);
            }
        }
        SaveGame.Save();
    }

    public void saveUnlocks(List<int> l, int screen)
    {
        switch(screen)
        {
            case 1: SaveGame.Instance.unlockables.dogScreen = l;
                break;
            case 2:SaveGame.Instance.unlockables.fenceScreen = l;
                break;
            case 3:SaveGame.Instance.unlockables.decorationsScreen= l;
                break;
            case 4:SaveGame.Instance.unlockables.paddockItemsScreen= l;
                break;
            case 5:SaveGame.Instance.unlockables.pathsScreen = l;
                break;
            case 6: SaveGame.Instance.unlockables.shopsScreen = l;
                break;
        }

        SaveGame.Save();
    }

}
