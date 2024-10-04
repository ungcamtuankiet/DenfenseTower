using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class LevelManager : MonoBehaviour
{
    [SerializeField]
    private GameObject[] titlePrefeabs;

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
        string[] mapData = ReadLevelText();

        int mapX = mapData[0].ToCharArray().Length;

        int mapY = mapData.Length;

        Vector3 worldStart = Camera.main.ScreenToWorldPoint(new Vector3(0, Screen.height));
        for (int y = 0; y < mapY; y++)
        {
            char[] newTitles = mapData[y].ToCharArray();
            for (int x = 0; x < mapX; x++)
            {
                PlaceTitle(newTitles[x].ToString(), x, y, worldStart);
            }
        }
    }

    private void PlaceTitle(string titleType, int x, int y, Vector3 worldStart)
    {
        int tileIndex = int.Parse(titleType);
        GameObject newTitle = Instantiate(titlePrefeabs[tileIndex]);
        newTitle.transform.position = new Vector3(worldStart.x + (TitleSize * x), worldStart.y - (TitleSize * y), 0);
    }

    private string[] ReadLevelText()
    {
        TextAsset bindData = Resources.Load("Level") as TextAsset;

        string data = bindData.text.Replace(Environment.NewLine, string.Empty);

        return data.Split('-');
    }
}
