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

    [SerializeField]
    PaddockCreation paddockCreation;

    bool placePath = false;
    [SerializeField]
    PathHandler pathHandler;

    bool placingDeco = false;
    [SerializeField]
    DecorationHandler decoration;

    bool rayOnButton = false;
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

        if (!rayOnButton)
        {
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
                        else if (placePath)
                        {
                            clicks += 1;
                            //First clicks equals the starting tile
                            if (clicks == 0)
                            {
                                pathHandler.setStartingTile(tile);
                                startingTile = tile;
                            }
                            //Second click equals the end tile
                            else if (clicks == 1)
                            {
                                pathHandler.setEndTile(tile);
                                placePath = false;
                                clicks = -1;
                                startingTile = null;
                            }
                        }
                        else if (placingDeco)
                        {
                            if (!tile.isPath && tile.IsAccessible)
                            {
                                decoration.spawnDecoration(new Vector3(tile.transform.position.x + 5, tile.transform.position.y + 3, tile.transform.position.z + 5), tile);
                                placingDeco = false;
                            }
                        }

                        if (tile.isPaddock)
                        {
                            if (placeFoodBowl)
                            {
                                Transform parent = tile.transform.parent;
                                paddockCreation.placeFoodBowl(tile, parent);
                                placeFoodBowl = false;
                            }
                            else if (placeWaterBowl)
                            {

                                Transform parent = tile.transform.parent;
                                paddockCreation.placeWaterBowl(tile, parent);
                                placeWaterBowl = false;
                            }
                            else if (placeDogs && tile.IsAccessible)
                            {
                                if (tile.transform.parent.GetComponentInChildren<PaddockControl>().canPlaceDog())
                                {
                                    dogHandle.spawnDog(new Vector3(tile.transform.position.x + 5, tile.transform.position.y + 3, tile.transform.position.z + 5), tile.transform.parent, tile);
                                }
                            }
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
            else if (placePath && pathHandler.getStartingTile() != null && startingTile == pathHandler.getStartingTile())
            {
                if (hits > 0)
                {
                    EnvironmentTile tile = mRaycastHits[0].transform.GetComponent<EnvironmentTile>();
                    if (tile != null && tile != lastTile)
                    {
                        pathHandler.calculatePathCost(tile);
                        lastTile = tile;
                    }
                }
            }
        }
        if (Input.GetMouseButtonDown(1))
        {
            stopAllActions();
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

    public void setCreatingPaddocks(bool set)
    {
        creatingPaddocks = set;
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

    public void setPlacingDogs(bool set)
    {
        placeDogs = set;
    }

    public void setPlacingPaths(bool set)
    {
        placePath = set;
    }
    public void setPlacingDeco(bool set)
    {
        placingDeco = set;
    }
    public bool getDeleting()
    {
        return deletePaddocks;
    }
    public void Generate()
    {
        mMap.GenerateWorld();
    }

    public void setRayOnButton(bool set)
    {
        rayOnButton = set;
    }
    public void stopAllActions()
    {
        clicks = -1;
        creatingPaddocks = false;
        placeWaterBowl = false;
        placeFoodBowl = false;
        placeDogs = false;
        placePath = false;
        placingDeco = false;

        paddock.cancelCreation();
        pathHandler.cancelCreation();
    }
    public void Exit()
    {
#if !UNITY_EDITOR
        Application.Quit();
#endif
    }
}
