using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : Singleton<GameManager>
{
    
    public TowerBtn ClickedBtn { get; set; }

    public int Currency
    {
        get
        {
            return currency;
        }
        set
        {
            this.currency = value;  
            this.currencyTxt.text = value.ToString() + " <color=lime>$</color>";
        }
    }
    private int currency;
    
    [SerializeField]
    private Text currencyTxt;
    
    // Start is called before the first frame update
    void Start()
    {
        Currency = 10;
    }

    // Update is called once per frame
    void Update()
    {
        HandleEscape();
    }

    
    public void PickTower(TowerBtn towerBtn)
    {
        if (Currency >= towerBtn.Price)
        {
            this.ClickedBtn = towerBtn;
            Hover.Instance.Activate(towerBtn.Sprite);
        }
    }

    public void BuyTower()
    {
        if (Currency >= ClickedBtn.Price)
        {
            Currency -= ClickedBtn.Price;
            Hover.Instance.Deactivate();
        }
        
    
    }

    private void HandleEscape()
    {
        if (Input.GetKeyDown(KeyCode.Escape)) {
            Hover.Instance.Deactivate();
        }
    }
}
