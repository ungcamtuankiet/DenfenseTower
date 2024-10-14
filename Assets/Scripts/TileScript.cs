using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class TileScript : MonoBehaviour
{
    public Point GridPosition {get; private set;}

    public bool IsEmpty {get; private set;}

    private Color32 fullColor = new Color32(255, 118, 118, 255);

    private Color32 emptyColor = new Color32(96, 255, 90, 255);



    public bool WalkAble { get; set; }

    private SpriteRenderer spriteRenderer;
    public bool Debugging { get; set; }
    public Vector2 WorldPosition
    {
        get
        {
            return new Vector2(transform.position.x + (GetComponent<SpriteRenderer>().bounds.size.x/2), transform.position.y - (GetComponent<SpriteRenderer>().bounds.size.y/2));
        }
    }
    
    // Start is called before the first frame update
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Setup(Point gridPos, Vector3 wordPos, Transform parent)
    {
        WalkAble = true;
        IsEmpty = true;
        this.GridPosition = gridPos;
        transform.position = wordPos;
        transform.SetParent(parent);
        LevelManager.Instance.Tiles.Add(gridPos, this);

    }

    private void OnMouseOver()
    {
        
            if (!EventSystem.current.IsPointerOverGameObject() && GameManager.Instance.ClickedBtn != null)
                   {
                        if (IsEmpty && !Debugging) 
                         {
                              ColorTile(emptyColor);
                         } 
                        if (!IsEmpty && !Debugging) 
                         {
                              ColorTile(fullColor);
                         }
                        else if (Input.GetMouseButtonDown(0))
                         {
                              PlaceTower();
                         }
                   }

    }

    private void OnMouseExit()
    {
        if(!Debugging)
        {
            ColorTile(Color.white);
        }
    }



    private void PlaceTower()
    {  
        Instantiate(GameManager.Instance.ClickedBtn.TowerPrefab, transform.position, Quaternion.identity);

        GameManager.Instance.BuyTower();

        WalkAble = false;

        IsEmpty = false;
        ColorTile(Color.white);
    }

    private void ColorTile(Color newColor)
    {
        spriteRenderer.color = newColor;
    }
}
