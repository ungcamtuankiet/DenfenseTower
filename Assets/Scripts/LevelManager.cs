using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class LevelManager : MonoBehaviour
{
    [SerializeField]
    private GameObject[] titlePrefeabs;
    
    [SerializeField]
    private CameraMovement cameraMovement;

    private Point blueSpawn, redSpawn;
    [SerializeField]
    private GameObject bluePortalPrefab;
    [SerializeField]
    private GameObject redPortalPrefab;
    
    public Dictionary<Point, TileScript> Tiles {get; set;}
    public float TitleSize
    {
        get { return titlePrefeabs[0].GetComponent<SpriteRenderer>().sprite.bounds.size.x; }
    }
    // Start is called before the first frame update
    void Start()
    {
        CreateLevel();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void CreateLevel()
    {
        Tiles = new Dictionary<Point, TileScript>();
        
        string[] mapData = ReadLevelText();

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
        
        newTitle.Setup(new Point(x, y), new Vector3(worldStart.x + (TitleSize * x), worldStart.y - (TitleSize * y), 0));
        
        Tiles.Add(new Point(x, y), newTitle);
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
        
        Instantiate(bluePortalPrefab, Tiles[blueSpawn].GetComponent<TileScript>().WorldPosition, Quaternion.identity);
        
        redSpawn = new Point(11, 6);
        
        Instantiate(redPortalPrefab, Tiles[redSpawn].GetComponent<TileScript>().WorldPosition, Quaternion.identity);
    }
}
