﻿using UnityEngine;
using UnityEngine.EventSystems;

/// <summary>
/// This script is used for all tiles in the game
/// </summary>
public class TileScript : MonoBehaviour
{
    /// <summary>
    /// The tiles gris position
    /// </summary>
    public Point GridPosition { get; private set; }

    public bool IsEmpty { get; private set; }

    /// <summary>
    /// the color of the tile, when its full, this is used while hovering the tile with the mouse
    /// </summary>
    private Color32 fullColor = new Color32(255, 118, 118, 255);

    /// <summary>
    /// The color of the tile when is empty, this is used when hover the tile with the mouse
    /// </summary>
    private Color32 emptyColor = new Color32(96, 255, 90, 255);

    private SpriteRenderer spriteRenderer;

    /// <summary>
    /// The tile's center world position
    /// </summary>
    public Vector2 WorldPosition
    {
        get
        {
            return new Vector2(transform.position.x + (GetComponent<SpriteRenderer>().bounds.size.x / 2), transform.position.y - (GetComponent<SpriteRenderer>().bounds.size.y / 2));
        }
    }

    // Use this for initialization
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    /// <summary>
    /// Sets up the tile, this is an alternative to a constructor
    /// </summary>
    /// <param name="gridPos">The tiles grid position</param>
    /// <param name="worldPos">The tiles world postion</param>
    public void Setup(Point gridPos, Vector3 worldPos, Transform parent)
    {
        IsEmpty = true;
        this.GridPosition = gridPos;
        transform.position = worldPos;
        transform.SetParent(parent);
        LevelManager.Instance.Tiles.Add(gridPos, this);
    }

    /// <summary>
    /// Mouseover, this is executed when the player mouse over the tile
    /// </summary>
    private void OnMouseOver()
    {
        if (!EventSystem.current.IsPointerOverGameObject() && GameManager.Instance.ClickedBtn != null)
        {
            if (IsEmpty)//Colors the tile green
            {
                ColorTile(emptyColor);
            }
            if (!IsEmpty)//Colors the tile red
            {
                ColorTile(fullColor);
            }
            else if (Input.GetMouseButtonDown(0))//Places a tower if there is room on the tile
            {
                PlaceTower();
            }
        }
    }

    private void OnMouseExit()
    {
        ColorTile(Color.white);
    }

    /// <summary>
    /// Places a tower on the tile
    /// </summary>
    private void PlaceTower()
    {
        GameObject tower = (GameObject)Instantiate(GameManager.Instance.ClickedBtn.TowerPrefab, transform.position, Quaternion.identity);
        tower.GetComponent<SpriteRenderer>().sortingOrder = GridPosition.Y;

        tower.transform.SetParent(transform);

        IsEmpty = false;

        ColorTile(Color.white);

        GameManager.Instance.BuyTower();
    }

    /// <summary>
    /// Sets the color on the tile
    /// </summary>
    /// <param name="newColor"></param>
    public void ColorTile(Color newColor)
    {
        //Sets the color on the tile
        spriteRenderer.color = newColor;
    }
}
