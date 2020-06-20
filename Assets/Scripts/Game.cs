using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Game : MonoBehaviour
{
    [SerializeField] private Camera MainCamera;
    //[SerializeField] private Character Character;
    [SerializeField] private GameObject Menu;
    [SerializeField] private Canvas Hud;
    [SerializeField] private Transform CharacterStart;

    private RaycastHit[] mRaycastHits;
    //private Character mCharacter;
    private Environment mMap;

    private readonly int NumberOfRaycastHits = 1;

    [SerializeField]
    PaddockCreation paddock;

    int clicks = -1;

    EnvironmentTile standIn;


    EnvironmentTile lastTile;
    EnvironmentTile startingTile;

    [SerializeField]
    DogHandler dogHandle;

    bool creatingPaddocks = false;

    bool deleteObjects = false;

    bool placeDogs = false;

    bool placePaddockItem = false;

    [SerializeField]
    PaddockCreation paddockCreation;

    bool placePath = false;
    [SerializeField]
    PathHandler pathHandler;

    bool placingDeco = false;
    [SerializeField]
    DecorationHandler decoration;

    bool rayOnButton = false;

    GameObject standInObject;
    int standInButton = 0;

    [SerializeField]
    Color[] standInColours;

    [SerializeField]
    FoodWaterHandler foodWaterHandle;

    [SerializeField]
    TerrainPainting painting;

    bool isPainting = false;
    bool placingShop = false;
    bool moveCamera = false;

    [SerializeField]
    ShopHandler shopHandler;

    bool speedUpTime = false;

    [SerializeField]
    GameObject pauseMenu;

    [SerializeField]
    AudioManager audioManager;

    [SerializeField]
    GameObject terrainPalette;
    bool showTerrainPaints = false;

    [SerializeField]
    GameObject actionIcon;

    [SerializeField]
    List<Sprite> actionSprites = new List<Sprite>();


    [SerializeField]
    SaveHandler save;

    [SerializeField]
    ParkRating rating;
    void Start()
    {
        mRaycastHits = new RaycastHit[NumberOfRaycastHits];
        mMap = GameObject.Find("Environment").GetComponent<Environment>();
        // mCharacter = Instantiate(Character, transform);


        rayOnButton = true;
        ShowMenu(true);
        pauseMenu.SetActive(false);
        terrainPalette.SetActive(false);

        actionIcon.SetActive(false);
    }

    private void Update()
    {
        // Check to see if the player has clicked a tile and if they have, try to find a path to that 
        // tile. If we find a path then the character will move along it to the clicked tile.

        //List<EnvironmentTile> route = mMap.Solve(mCharacter.CurrentPosition, tile);
        //mCharacter.GoTo(route);

        if (speedUpTime)
        {
            Time.timeScale = 2;
        }
        else
        {
            Time.timeScale = 1;
        }

        actionIcon.transform.position = Input.mousePosition;


        if (!rayOnButton)
        {
            Ray screenClick = MainCamera.ScreenPointToRay(Input.mousePosition);
            int hits = Physics.RaycastNonAlloc(screenClick, mRaycastHits);

            EnvironmentTile tile = mRaycastHits[0].transform.GetComponent<EnvironmentTile>();
            if (tile != null)
            {
                if (Input.GetMouseButtonDown(0))
                {
                    if (isPainting)
                    {
                        painting.paintTile(tile);
                    }
                }

                if (Input.GetMouseButtonDown(0))
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
                            //creatingPaddocks = false;
                            clicks = -1;
                            startingTile = null;
                            audioManager.playWood();
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
                            //placePath = false;
                            clicks = -1;
                            startingTile = null;
                            audioManager.playStone();
                        }
                    }
                    else if (placingDeco)
                    {
                        if (tile.IsAccessible)
                        {
                            decoration.spawnDecoration(new Vector3(tile.transform.position.x + 5, tile.transform.position.y + 3, tile.transform.position.z + 5), tile, standInObject.transform.eulerAngles);
                            audioManager.playStone();
                        }
                    }
                    else if (placingShop)
                    {
                        if (tile.IsAccessible)
                        {
                            shopHandler.spawnShop(new Vector3(tile.transform.position.x + 5, tile.transform.position.y + 3, tile.transform.position.z + 5), tile, standInObject.transform.eulerAngles, standInButton);
                            audioManager.playWood();
                        }
                    }
                    else if (deleteObjects)
                    {
                        if (tile.transform.childCount > 0 && !tile.hasFence)
                        {
                            if (tile.transform.GetChild(0).tag == "Shop")
                            {
                                rating.removeShop();
                            }
                            else if (tile.transform.GetChild(0).tag == "Decoration")
                            {
                                rating.removeDecoration();
                            }

                            Destroy(tile.transform.GetChild(0).gameObject);
                            audioManager.playDestroy();

                            if (!tile.IsAccessible)
                            {
                                tile.IsAccessible = true;
                            }

                            tile.transform.DetachChildren();

                            save.saveTile(tile, false);
                        }
                        else if (tile.isPath)
                        {
                            Material[] mat = tile.GetComponent<MeshRenderer>().materials;
                            mat[1] = Resources.Load("Grass2") as Material;
                            tile.GetComponent<MeshRenderer>().materials = mat;
                            tile.isPath = false;
                            tile.IsAccessible = true;
                            pathHandler.setTileColor(tile);
                            save.saveTile(tile, false);
                        }
                    }

                    if (tile.isPaddock)
                    {
                        if (placePaddockItem && !tile.hasFoodBowl && !tile.hasWaterBowl)
                        {
                            Transform parent = tile.transform.parent;
                            foodWaterHandle.spawnItem(new Vector3(tile.transform.position.x + 5, tile.transform.position.y + 3, tile.transform.position.z + 5), tile, standInObject.transform.eulerAngles);
                            audioManager.playWood();
                        }
                        else if (placeDogs && tile.IsAccessible)
                        {
                            if (tile.getControlObj().GetComponentInChildren<PaddockControl>().canPlaceDog())
                            {
                                dogHandle.spawnDog(new Vector3(tile.transform.position.x + 5, tile.transform.position.y + 3, tile.transform.position.z + 5), tile.transform.parent, tile, tile.getControlObj());
                            }
                        }
                    }

                }

                if (standInObject != null)
                {
                    standInObject.transform.position = new Vector3(tile.transform.position.x + 5, tile.transform.position.y + 3, tile.transform.position.z + 5);
                }
                if (Input.GetMouseButtonDown(2) && standInObject != null || Input.GetKeyDown(KeyCode.Space) && standInObject != null)
                {
                    standInObject.transform.Rotate(0, 45, 0);
                }
            }

            if (creatingPaddocks && paddock.getStartingTile() != null && startingTile == paddock.getStartingTile())
            {
                if (hits > 0)
                {
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
            Menu.SetActive(show);
            Hud.enabled = !show;

            if (show)
            {
                // mCharacter.transform.position = CharacterStart.position;
                //mCharacter.transform.rotation = CharacterStart.rotation;
                //mMap.CleanUpWorld();
                audioManager.playMusic();
            }
            else
            {
                //mCharacter.transform.position = mMap.Start.Position;
                //mCharacter.transform.rotation = Quaternion.identity;
                //mCharacter.CurrentPosition = mMap.Start;
            }
        }
    }

    public void setPaintingTerrain(bool set)
    {
        isPainting = true;

        if (set)
        {
            actionIcon.GetComponent<Image>().sprite = actionSprites[1];
            actionIcon.SetActive(true);
        }
        else
        {
            actionIcon.SetActive(false);
        }
    }
    public void setCreatingPaddocks(bool set)
    {
        creatingPaddocks = set;

        if (set)
        {
            actionIcon.GetComponent<Image>().sprite = actionSprites[2];
            actionIcon.SetActive(true);
        }
        else
        {
            actionIcon.SetActive(false);
        }
    }
    public void setDeleting(bool set)
    {
        deleteObjects = set;
        if (set)
        {
            actionIcon.GetComponent<Image>().sprite = actionSprites[0];
            actionIcon.SetActive(true);
        }
        else
        {
            actionIcon.SetActive(false);
        }
    }

    public void setPlacingShop(bool set)
    {
        placingShop = set;
    }
    public void setPlacingPaddockItem(bool set)
    {
        placePaddockItem = set;
    }

    public void setPlacingDogs(bool set)
    {
        placeDogs = set;
    }

    public void setPlacingPaths(bool set)
    {
        placePath = set;

        if (set)
        {
            actionIcon.GetComponent<Image>().sprite = actionSprites[2];
            actionIcon.SetActive(true);
        }
        else
        {
            actionIcon.SetActive(false);
        }
    }
    public void setPlacingDeco(bool set)
    {
        placingDeco = set;
    }
    public bool getDeleting()
    {
        return deleteObjects;
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
        placePaddockItem = false;
        placeDogs = false;
        placePath = false;
        placingDeco = false;
        deleteObjects = false;
        isPainting = false;
        placingShop = false;
        showTerrainPaints = false;

        CameraControl.instance.followTransform = null;

        if (standInObject != null)
        {
            Destroy(standInObject);
        }

        paddock.cancelCreation();
        pathHandler.cancelCreation();

        terrainPalette.SetActive(false);

        actionIcon.SetActive(false);
    }

    public bool doingAction()
    {
        if (creatingPaddocks || placePaddockItem || placeDogs || placePath || placingDeco || placingShop)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public bool canMoveCamera()
    {
        return moveCamera;
    }
    public void setStandInButton(int button)
    {
        standInButton = button;
    }
    public void getStandIn(int type)
    {
        switch (type)
        {
            //Dogs
            case 1:
                standInObject = Instantiate(dogHandle.getStandIn(standInButton));
                Destroy(standInObject.GetComponent<DogBehaviour>());
                break;
            //Decorations
            case 2:
                standInObject = Instantiate(decoration.getStandIn(standInButton));
                break;
            case 3:
                standInObject = Instantiate(foodWaterHandle.getStandIn(standInButton));
                break;
            case 4:
                standInObject = Instantiate(shopHandler.getStandIn(standInButton));
                Destroy(standInObject.GetComponent<Shop>());
                break;
        }

    }

    void changeChildColours()
    {
        List<Material> standIns = new List<Material>();

        for (int i = 0; i < standInObject.transform.childCount; i++)
        {
            Debug.Log("Loop");
            if (standIn.transform.GetChild(i).tag == "Model")
            {
                standIns.Add(standInObject.transform.GetChild(i).GetComponent<SkinnedMeshRenderer>().materials[0]);
            }
        }

        for (int i = 0; i < standInObject.transform.childCount; i++)
        {
            if (standIn.transform.GetChild(i).tag == "Model")
            {
                standInObject.transform.GetChild(i).GetComponent<SkinnedMeshRenderer>().materials[0].color = standInColours[i];
            }
        }
    }

    void changeColours()
    {
        List<Material> standIns = new List<Material>();

        for (int i = 0; i < standInObject.GetComponent<MeshRenderer>().materials.Length; i++)
        {
            standIns.Add(standInObject.GetComponent<MeshRenderer>().materials[i]);
        }

        for (int i = 0; i < standInObject.GetComponent<MeshRenderer>().materials.Length; i++)
        {
            standInObject.GetComponent<MeshRenderer>().materials[i].color = standInColours[i];
        }
    }

    public void setMoveCamera(bool set)
    {
        moveCamera = set;
    }
    public bool getMoveCamera()
    {
        return moveCamera;
    }

    public void setSpeedUp()
    {
        if (speedUpTime)
        {
            speedUpTime = false;
        }
        else
        {
            speedUpTime = true;
        }
    }

    public void togglePaints()
    {
        if (showTerrainPaints)
        {
            showTerrainPaints = false;
            terrainPalette.SetActive(false);

        }
        else
        {
            showTerrainPaints = true;
            terrainPalette.SetActive(true);
        }
    }
    public void checkSpawnTile(EnvironmentTile m)
    {

        if (!m.IsAccessible && m.transform.childCount > 0)
        {
            Destroy(m.transform.GetChild(0).gameObject);
        }
    }
    public void Exit()
    {
#if !UNITY_EDITOR
        Application.Quit();
#endif
    }
}
