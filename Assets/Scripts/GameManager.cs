using System.Collections;
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

    public ObjectPool Pool { get; set; }

    private void Awake()
    {
        Pool = GetComponent<ObjectPool>();
    }

    // Start is called before the first frame update
    void Start()
    {
        Currency = 100;
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

    public void StartWave()
    {
        StartCoroutine(SpwanWave());
    }

    private IEnumerator SpwanWave()
    {
        LevelManager.Instance.GeneratePath();

        int monsterIndex = 1; //Random.Range(0, 4);

        string type = string.Empty;

        switch (monsterIndex)
        {
            case 0:
                type = "BlueMonster";
                break;
            case 1:
                type = "RedMonster";
                break;
            case 2:
                type = "GreenMonster";
                break;
            case 3:
                type = "PurpleMonster";
                break;
        }

        Monster monster =  Pool.GetObject(type).GetComponent<Monster>();
        monster.Spawn();
        yield return new WaitForSeconds(2.5f);
    }
}
