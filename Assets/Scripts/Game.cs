using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Game : MonoBehaviour
{
    [SerializeField] private Camera MainCamera;
    [SerializeField] private Character Character;
    [SerializeField] private Canvas Menu;
    [SerializeField] private Canvas Hud;
    [SerializeField] private Transform CharacterStart;

    private RaycastHit[] mRaycastHits;
    private Character mCharacter;
    private Environment mMap;

    private readonly int NumberOfRaycastHits = 1;

    [SerializeField]
    PaddockCreation paddock;

    int clicks = - 1;

    EnvironmentTile standIn;


    EnvironmentTile lastTile;
    EnvironmentTile startingTile;

    [SerializeField]
    DogHandler dogHandle;

    bool creatingPaddocks = false;

    bool deletePaddocks = false;

    bool placeDogs = false;

    bool placeWaterBowl = false;
    bool placeFoodBowl = false;

    void Start()
    {
        mRaycastHits = new RaycastHit[NumberOfRaycastHits];
        mMap = GetComponentInChildren<Environment>();
        mCharacter = Instantiate(Character, transform); 
        ShowMenu(true);
    }

    private void Update()
    {
        // Check to see if the player has clicked a tile and if they have, try to find a path to that 
        // tile. If we find a path then the character will move along it to the clicked tile.

        //List<EnvironmentTile> route = mMap.Solve(mCharacter.CurrentPosition, tile);
        //mCharacter.GoTo(route);


        Ray screenClick = MainCamera.ScreenPointToRay(Input.mousePosition);
        int hits = Physics.RaycastNonAlloc(screenClick, mRaycastHits);

        if (Input.GetMouseButtonDown(0))
        {
            if (hits > 0)
            {
                EnvironmentTile tile = mRaycastHits[0].transform.GetComponent<EnvironmentTile>();
                if (tile != null)
                {
                    //If the selection for creating paddocks has been selected (button)
                    if (creatingPaddocks)
                    {
                        clicks += 1;
                        //First clicks equals the starting tile
                        if (clicks == 0)
                        {
                            paddock.setStartingTile(tile);
                            startingTile = tile;
                        }
                        //Second click equals the end tile
                        else if (clicks == 1)
                        {
                            paddock.setEndTile(tile);
                            creatingPaddocks = false;
                            clicks = -1;
                            startingTile = null;
                        }
                    }

                    if (tile.isPaddock)
                    {
                        if (placeFoodBowl)
                        {
                            Transform parent = tile.transform.parent;
                            parent.GetComponentInChildren<PaddockControl>().placeFoodBowl(tile);
                            placeFoodBowl = false;
                        }
                        else if (placeWaterBowl)
                        {
                            Transform parent = tile.transform.parent;
                            parent.GetComponentInChildren<PaddockControl>().placeWaterBowl(tile);
                            placeWaterBowl = false;
                        }
                        else
                        {
                            dogHandle.spawnDog(new Vector3(tile.transform.position.x, tile.transform.position.y + 3, tile.transform.position.z), tile.transform.parent);
                        }
                    }
                    else
                    {
                        Debug.Log("SPACE NOT VALID");
                    }
                }
            }
        }
        if (creatingPaddocks && paddock.getStartingTile() != null && startingTile == paddock.getStartingTile())
        {
            if (hits > 0)
            {
                EnvironmentTile tile = mRaycastHits[0].transform.GetComponent<EnvironmentTile>();
                if (tile != null && tile != lastTile)
                {
                    paddock.calculatePaddockCost(tile);
                    lastTile = tile;
                }
            }
        }
        if (Input.GetMouseButtonDown(1))
        {
            clicks = -1;
            creatingPaddocks = false;
        }
    }

    public void ShowMenu(bool show)
    {
        if (Menu != null && Hud != null)
        {
            Menu.enabled = show;
            Hud.enabled = !show;

            if( show )
            {
                mCharacter.transform.position = CharacterStart.position;
                mCharacter.transform.rotation = CharacterStart.rotation;
                mMap.CleanUpWorld();
            }
            else
            {
                mCharacter.transform.position = mMap.Start.Position;
                mCharacter.transform.rotation = Quaternion.identity;
                mCharacter.CurrentPosition = mMap.Start;
            }
        }
    }

    public void setCreatingPaddocks()
    {
        creatingPaddocks = true;
    }
    public void setDeleting(bool set)
    {
        deletePaddocks = set;
    }

    public void setPlacingFood(bool set)
    {
        placeFoodBowl = set;
    }

    public void setPlacingWater(bool set)
    {
        placeWaterBowl = set;
    }

    public bool getDeleting()
    {
        return deletePaddocks;
    }
    public void Generate()
    {
        mMap.GenerateWorld();
    }

    public void Exit()
    {
#if !UNITY_EDITOR
        Application.Quit();
#endif
    }
}
