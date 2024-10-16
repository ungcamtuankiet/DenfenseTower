using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class LevelManager : Singleton<LevelManager>
{
    [SerializeField]
    private GameObject[] titlePrefeabs;
    
    [SerializeField]
    private CameraMovement cameraMovement;

    [SerializeField]
    private Transform map;

    private Point blueSpawn, redSpawn;
    [SerializeField]
    private GameObject bluePortalPrefab;
    [SerializeField]
    private GameObject redPortalPrefab;

    public Portal BluePortal { get; set; }

    private Point mapSize;

    private Stack<Node> path;

    public Stack<Node> Path
    {
        get
        {
            if(path == null)
            {
                GeneratePath();
            }
            return new Stack<Node>(new Stack<Node>(path));
        }
    }

    public Dictionary<Point, TileScript> Tiles {get; set;}
    public float TitleSize
    {
        get { return titlePrefeabs[0].GetComponent<SpriteRenderer>().sprite.bounds.size.x; }
    }
    // Start is called before the first frame update
    void Start()
    {
        CreateLevel();

        int a = 1;
        int b = 2;

        string aString = "1";
        string bString = "2";

        Swap<string>(ref aString, ref bString);
        Swap<int>(ref a, ref b);

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Swap<T>(ref T a, ref T b)
    {
        T tmp = a;
        a = b;
        b = tmp;
    }

    private void CreateLevel()
    {
        Tiles = new Dictionary<Point, TileScript>();
        
        string[] mapData = ReadLevelText();
        mapSize = new Point(mapData[0].ToCharArray().Length, mapData.Length);

        int mapX = mapData[0].ToCharArray().Length;

        int mapY = mapData.Length;
        
        Vector3 maxTile = Vector3.zero;

        Vector3 worldStart = Camera.main.ScreenToWorldPoint(new Vector3(0, Screen.height));
        for (int y = 0; y < mapY; y++)
        {
            char[] newTitles = mapData[y].ToCharArray();
            for (int x = 0; x < mapX; x++)
            {
                PlaceTitle(newTitles[x].ToString(), x, y, worldStart);
            }
        }
        
        maxTile = Tiles[new Point(mapX - 1, mapY - 1)].transform.position;
        
        cameraMovement.SetLimits(new Vector3(maxTile.x + TitleSize, maxTile.y - TitleSize));

        SpawnPortals();
    }

    private void PlaceTitle(string titleType, int x, int y, Vector3 worldStart)
    {
        int tileIndex = int.Parse(titleType);
        TileScript newTitle = Instantiate(titlePrefeabs[tileIndex]).GetComponent<TileScript>();
        
        newTitle.Setup(new Point(x, y), new Vector3(worldStart.x + (TitleSize * x), worldStart.y - (TitleSize * y), 0), map);
        
        
    }

    private string[] ReadLevelText()
    {
        TextAsset bindData = Resources.Load("Level") as TextAsset;

        string data = bindData.text.Replace(Environment.NewLine, string.Empty);

        return data.Split('-');
    }

    private void SpawnPortals()
    {
        blueSpawn = new Point(0, 0);

        GameObject tmp = (GameObject)Instantiate(bluePortalPrefab, Tiles[blueSpawn].GetComponent<TileScript>().WorldPosition, Quaternion.identity);
        BluePortal = tmp.GetComponent<Portal>();
        BluePortal.name = "BluePortal";

        redSpawn = new Point(11, 6);

        Instantiate(redPortalPrefab, Tiles[redSpawn].GetComponent<TileScript>().WorldPosition, Quaternion.identity);
    }
    public bool InBounds(Point position)
    {
        return position.X >= 0 && position.Y >= 0 && position.X < mapSize.X && position.Y < mapSize.Y;
    }

    public void GeneratePath()
    {
        path = AStar.GetPath(blueSpawn, redSpawn);
    }
}
