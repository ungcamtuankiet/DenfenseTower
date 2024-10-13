using UnityEngine;
using System.Collections.Generic;
using System;

public class LevelManager : Singleton<LevelManager>
{
    /// <summary>
    /// An array of tilePrefabs, these are used for creating the tiles in the game
    /// </summary>
    [SerializeField]
    private GameObject[] tilePrefabs;

    [SerializeField]
    private CameraMovement cameraMovement;

    [SerializeField]
    private Transform map;

    private Point blueSpawn, redSpawn;

    /// <summary>
    /// Prefab for spawning the blue portal
    /// </summary>
    [SerializeField]
    private GameObject bluePortalPrefab;

    /// <summary>
    /// Prefab for spawning the red portal
    /// </summary>
    [SerializeField]
    private GameObject redPortalPrefab;

    /// <summary>
    /// A dictionary that conatins all tiles in our game
    /// </summary>
    public Dictionary<Point, TileScript> Tiles { get; set; }

    /// <summary>
    /// A property for returning the size of a tile
    /// </summary>
    public float TileSize
    {
        get { return tilePrefabs[0].GetComponent<SpriteRenderer>().sprite.bounds.size.x; }
    }

    // Use this for initialization
    void Start()
    {
        //Executes the create level function
        CreateLevel();
    }

    // Update is called once per frame
    void Update()
    {

    }

    /// <summary>
    /// Creates our level
    /// </summary>
    private void CreateLevel()
    {
        Tiles = new Dictionary<Point, TileScript>();

        //A tmp instantioation of the tile map, we will use a text document to load this later.
        string[] mapData = ReadLevelText();

        //Calculates the x map size
        int mapX = mapData[0].ToCharArray().Length;

        //Calculates the y map size
        int mapY = mapData.Length;

        Vector3 maxTile = Vector3.zero;

        //Calculates the world start point, this is the top left corner of the screen
        Vector3 worldStart = Camera.main.ScreenToWorldPoint(new Vector3(0, Screen.height));

        for (int y = 0; y < mapY; y++) //The y positions
        {
            char[] newTiles = mapData[y].ToCharArray(); //Gets all the tiles, that we need to place on the current horizontal line

            for (int x = 0; x < mapX; x++) //The x positions
            {
                //Places the tile in the world
                PlaceTile(newTiles[x].ToString(), x, y, worldStart);
            }
        }

        maxTile = Tiles[new Point(mapX - 1, mapY - 1)].transform.position;

        //Sets the camera limits to the max tile position
        cameraMovement.SetLimits(new Vector3(maxTile.x + TileSize, maxTile.y - TileSize));

        SpawnPortals();

    }

    /// <summary>
    /// Places a tile in the gameworld
    /// </summary>
    /// <param name="tileType">The type of tile to palce for example 0</param>
    /// <param name="x">x position of the tile</param>
    /// <param name="y">y position of the tile</param>
    /// <param name="worldStart">The world start position</param>
    private void PlaceTile(string tileType, int x, int y, Vector3 worldStart)
    {
        //Parses the tiletype to an int, so that we can use it as an indexer when we create a new tile
        int tileIndex = int.Parse(tileType);

        //Creates a new tile and makes a reference to that tile in the newTile variable
        TileScript newTile = Instantiate(tilePrefabs[tileIndex]).GetComponent<TileScript>();

        //Uses the new tile variable to change the position of the tile
        newTile.Setup(new Point(x, y), new Vector3(worldStart.x + (TileSize * x), worldStart.y - (TileSize * y), 0),map);


    }

    private string[] ReadLevelText()
    {
        TextAsset bindData = Resources.Load("Level") as TextAsset;

        string data = bindData.text.Replace(Environment.NewLine, string.Empty);

        return data.Split('-');
    }

    /// <summary>
    /// Spawns the portals in the game
    /// </summary>
    private void SpawnPortals()
    {
        //Spawns the blue portal
        blueSpawn = new Point(0, 0);
        Instantiate(bluePortalPrefab, Tiles[blueSpawn].GetComponent<TileScript>().WorldPosition, Quaternion.identity);

        //Spawns the red portal
        redSpawn = new Point(11, 6);
        Instantiate(redPortalPrefab, Tiles[redSpawn].GetComponent<TileScript>().WorldPosition, Quaternion.identity);
    }
}
