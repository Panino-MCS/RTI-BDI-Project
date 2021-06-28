using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Collector : MonoBehaviour
{
    [SerializeField]
    private string name;
    [SerializeField]
    private int posX;
    [SerializeField]
    private int posY;
    [SerializeField]
    private int batteryAmount;
    [SerializeField]
    private int woodAmount;
    [SerializeField]
    private int stoneAmount;
    [SerializeField]
    private Sprite normalSprite;
    [SerializeField]
    private Sprite sprite_1;
    [SerializeField]
    private Sprite sprite_2;
    [SerializeField]
    private Sprite sprite_3;

	private bool dragging = false;
	private GameObject underneathTile = null;
	private float distance;
	private string runningCoroutine = "";

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
		//Inspect Object
		if (Input.GetMouseButtonDown(1))
		{
			Collider2D hitCollider = Physics2D.OverlapPoint(Camera.main.ScreenToWorldPoint(Input.mousePosition));
			if (hitCollider != null && hitCollider.transform == gameObject.transform)
			{
				UIManager.SetVisibleCollector(this.name, this.posX, this.posY, this.batteryAmount, this.woodAmount, this.stoneAmount, this.normalSprite);
			}
		}

		if (dragging)
		{
			Collider2D hitCollider = Physics2D.OverlapPoint(Camera.main.ScreenToWorldPoint(Input.mousePosition), LayerMask.GetMask("Ground"));
			if (hitCollider != null && hitCollider.CompareTag("Ground"))
			{
				if (underneathTile == null)
				{
					underneathTile = hitCollider.gameObject;
				}

				if (underneathTile != null && hitCollider.gameObject != underneathTile)
				{
					underneathTile.GetComponent<SpriteRenderer>().color = Color.white;
					underneathTile = hitCollider.gameObject;
				}

				hitCollider.gameObject.GetComponent<SpriteRenderer>().color = new Color(0, 221, 255, 255);
			}


			Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
			Vector3 rayPoint = ray.GetPoint(distance);
			transform.position = new Vector3(rayPoint.x - GameManager.GetTileSize()/2, rayPoint.y + GameManager.GetTileSize() / 2, rayPoint.z);
		}
	}

    public void SetName(string name)
    {
        this.name = name;
    }

    public string GetName()
    {
        return this.name;
    }

    public void SetAttribute(string attribute, string value)
    {
        switch (attribute)
        {
            case "battery-amount":
                this.batteryAmount = int.Parse(value);
                break;
            case "posX":
                this.posX = int.Parse(value);
                break;
            case "posY":
                this.posY = int.Parse(value);
                break;
            case "wood-amount":
                this.woodAmount = int.Parse(value);
                break;
            case "stone-amount":
                this.stoneAmount = int.Parse(value);
                break;
            default:
                break;
        }
    }

    public void MoveToDestination(float tileSize, Vector2 position)
    {
        gameObject.transform.position = new Vector2(position.x + (posX * tileSize) - (tileSize / 2), position.y + (posY * tileSize) + (tileSize / 2));
    }

	public void MoveUp()
	{
		StartCoroutine(MoveUp(GameManager.GetTileSize(), GameManager.GetBatteryDecrease("move-up")));
	}

	public IEnumerator MoveUp(float tileSize, int batteryDecrease)
    {
        int actionTime = 120;
        for (int i = 0; i < actionTime; i++)
        {
            gameObject.transform.position = new Vector2(gameObject.transform.position.x, gameObject.transform.position.y + tileSize / actionTime);
            yield return null;
        }
		this.posY++;
		this.batteryAmount -= batteryDecrease;

        //TODO - UpdateBeliefs

    }

	public void MoveDown()
	{
		StartCoroutine(MoveDown(GameManager.GetTileSize(), GameManager.GetBatteryDecrease("move-down")));
	}

	public IEnumerator MoveDown(float tileSize, int batteryDecrease)
    {
        int actionTime = 120;
        for (int i = 0; i < actionTime; i++)
        {
            gameObject.transform.position = new Vector2(gameObject.transform.position.x, gameObject.transform.position.y + tileSize / actionTime);
            yield return null;
        }
		this.batteryAmount -= batteryDecrease;
        this.posY--;

        //TODO - UpdateBeliefs
    }

	public void MoveRight()
	{
		StartCoroutine(MoveRight(GameManager.GetTileSize(), GameManager.GetBatteryDecrease("move-right")));
	}

	public IEnumerator MoveRight(float tileSize, int batteryDecrease)
    {
        int actionTime = 120;
        for (int i = 0; i < actionTime; i++)
        {
            gameObject.transform.position = new Vector2(gameObject.transform.position.x + tileSize / actionTime, gameObject.transform.position.y);
            yield return null;
        }
		this.batteryAmount -= batteryDecrease;
        this.posX++;

        //TODO - UpdateBeliefs
    }

	public void MoveLeft()
	{
		StartCoroutine(MoveLeft(GameManager.GetTileSize(), GameManager.GetBatteryDecrease("move-left")));
	}

	public IEnumerator MoveLeft(float tileSize, int batteryDecrease)
    {
        int actionTime = 120;
        for (int i = 0; i < actionTime; i++)
        {
            gameObject.transform.position = new Vector2(gameObject.transform.position.x - tileSize / actionTime, gameObject.transform.position.y);
            yield return null;
        }
		this.batteryAmount -= batteryDecrease;
        this.posX--;

        //TODO - UpdateBeliefs

    }

	public void CollectWood()
	{
		StartCoroutine(CollectWood(GameManager.GetTileSize(), GameManager.GetBatteryDecrease("collect-wood")));
	}

	public IEnumerator CollectWood(float tileSize, int batteryDecrease)
    {
        //Adding text
        GameObject newGO = new GameObject("myTextGO");
        newGO.transform.SetParent(this.transform);
        newGO.transform.position = new Vector2(gameObject.transform.position.x, gameObject.transform.position.y + (tileSize / 1.5f));
        TextMeshPro myText = newGO.AddComponent<TextMeshPro>();
        myText.autoSizeTextContainer = true;
        myText.fontSize = 4.5f;
        myText.text = "Collecting Wood...";

        int actionTime = 120;
        //Animation
        for (int i = 0; i < actionTime; i++)
        {

            if(i > 0 && i < actionTime / 10)
            {
                gameObject.GetComponent<SpriteRenderer>().sprite = this.sprite_1;
                myText.text = "Collecting Wood";
            }
            if (i > (actionTime / 10) && i < (actionTime / 10) * 2)
            {
                gameObject.GetComponent<SpriteRenderer>().sprite = this.sprite_2;
                myText.text = "Collecting Wood.";
            }
            if (i > (actionTime / 10) * 2 && i < (actionTime / 10) * 3)
            {
                gameObject.GetComponent<SpriteRenderer>().sprite = this.sprite_3;
                myText.text = "Collecting Wood..";
            }
            if (i > (actionTime / 10) * 3 && i < (actionTime / 10) * 4)
            {
                gameObject.GetComponent<SpriteRenderer>().sprite = this.sprite_2;
                myText.text = "Collecting Wood...";
            }
            if (i > (actionTime / 10) * 4 && i < (actionTime / 10) * 5)
            {
                gameObject.GetComponent<SpriteRenderer>().sprite = this.sprite_1;
                myText.text = "Collecting Wood";
            }
            if (i > (actionTime / 10) * 5 && i < (actionTime / 10) * 6)
            {
                gameObject.GetComponent<SpriteRenderer>().sprite = this.sprite_1;
                myText.text = "Collecting Wood";
            }
            if (i > (actionTime / 10) * 6 && i < (actionTime / 10) * 7)
            {
                gameObject.GetComponent<SpriteRenderer>().sprite = this.sprite_2;
                myText.text = "Collecting Wood.";
            }
            if (i > (actionTime / 10) * 7 && i < (actionTime / 10) * 8)
            {
                gameObject.GetComponent<SpriteRenderer>().sprite = this.sprite_3;
                myText.text = "Collecting Wood..";
            }
            if (i > (actionTime / 10) * 8 && i < (actionTime / 10) * 9)
            {
                gameObject.GetComponent<SpriteRenderer>().sprite = this.sprite_2;
                myText.text = "Collecting Wood...";
            }
            if (i > (actionTime / 10) * 9 && i < (actionTime / 10) * 10)
            {
                gameObject.GetComponent<SpriteRenderer>().sprite = this.sprite_1;
                myText.text = "Collecting Wood";
            }
            yield return null;
        }
		//Actual effect
		this.batteryAmount -= batteryDecrease;
        this.woodAmount++;

        //Reset Animation and Destroy the text
        gameObject.GetComponent<SpriteRenderer>().sprite = this.normalSprite;
        Destroy(newGO);

        //TODO - UpdateBeliefs
    }

	public void CollectStone()
	{
		StartCoroutine(CollectStone(GameManager.GetTileSize(), GameManager.GetBatteryDecrease("collect-stone")));
	}

	public IEnumerator CollectStone(float tileSize, int batteryDecrease)
    {
        //Adding text
        GameObject newGO = new GameObject("myTextGO");
        newGO.transform.SetParent(this.transform);
        newGO.transform.position = new Vector2(gameObject.transform.position.x, gameObject.transform.position.y + (tileSize / 1.5f));
        TextMeshPro myText = newGO.AddComponent<TextMeshPro>();
        myText.autoSizeTextContainer = true;
        myText.fontSize = 4.5f;
        myText.text = "Collecting Stone...";

        int actionTime = 120;
        //Animation
        for (int i = 0; i < actionTime; i++)
        {

            if (i > 0 && i < actionTime / 10)
            {
                gameObject.GetComponent<SpriteRenderer>().sprite = this.sprite_1;
                myText.text = "Collecting Stone";
            }
            if (i > (actionTime / 10) && i < (actionTime / 10) * 2)
            {
                gameObject.GetComponent<SpriteRenderer>().sprite = this.sprite_2;
                myText.text = "Collecting Stone.";
            }
            if (i > (actionTime / 10) * 2 && i < (actionTime / 10) * 3)
            {
                gameObject.GetComponent<SpriteRenderer>().sprite = this.sprite_3;
                myText.text = "Collecting Stone..";
            }
            if (i > (actionTime / 10) * 3 && i < (actionTime / 10) * 4)
            {
                gameObject.GetComponent<SpriteRenderer>().sprite = this.sprite_2;
                myText.text = "Collecting Stone...";
            }
            if (i > (actionTime / 10) * 4 && i < (actionTime / 10) * 5)
            {
                gameObject.GetComponent<SpriteRenderer>().sprite = this.sprite_1;
                myText.text = "Collecting Stone";
            }
            if (i > (actionTime / 10) * 5 && i < (actionTime / 10) * 6)
            {
                gameObject.GetComponent<SpriteRenderer>().sprite = this.sprite_1;
                myText.text = "Collecting Stone";
            }
            if (i > (actionTime / 10) * 6 && i < (actionTime / 10) * 7)
            {
                gameObject.GetComponent<SpriteRenderer>().sprite = this.sprite_2;
                myText.text = "Collecting Stone.";
            }
            if (i > (actionTime / 10) * 7 && i < (actionTime / 10) * 8)
            {
                gameObject.GetComponent<SpriteRenderer>().sprite = this.sprite_3;
                myText.text = "Collecting Stone..";
            }
            if (i > (actionTime / 10) * 8 && i < (actionTime / 10) * 9)
            {
                gameObject.GetComponent<SpriteRenderer>().sprite = this.sprite_2;
                myText.text = "Collecting Stone...";
            }
            if (i > (actionTime / 10) * 9 && i < (actionTime / 10) * 10)
            {
                gameObject.GetComponent<SpriteRenderer>().sprite = this.sprite_1;
                myText.text = "Collecting Stone";
            }
            yield return null;
        }
		//Actual effect
		this.batteryAmount -= batteryDecrease;
        this.stoneAmount++;

        //Reset Animation and Destroy the text
        gameObject.GetComponent<SpriteRenderer>().sprite = this.normalSprite;
        Destroy(newGO);

        //TODO - UpdateBeliefs
    }

	public void Recharge()
	{
		StartCoroutine(Recharge(GameManager.GetTileSize(), GameManager.GetConstants()["battery-amount"], 120));
	}

	public IEnumerator Recharge(float tileSize, int batteryCapacity, int actionTime)
    {
        //Adding text
        GameObject newGO = new GameObject("myTextGO");
        newGO.transform.SetParent(this.transform);
        newGO.transform.position = new Vector2(gameObject.transform.position.x, gameObject.transform.position.y + (tileSize / 1.5f));
        TextMeshPro myText = newGO.AddComponent<TextMeshPro>();
        myText.autoSizeTextContainer = true;
        myText.fontSize = 4.5f;
        myText.text = "Recharging...";

		runningCoroutine = "Recharge";
        //Animation
        for (int i = 0; i < actionTime; i++)
        {

            if (i > 0 && i < actionTime / 10)
            {
                gameObject.GetComponent<SpriteRenderer>().sprite = this.sprite_1;
                myText.text = "Recharging";
            }
            if (i > (actionTime / 10) && i < (actionTime / 10) * 2)
            {
                gameObject.GetComponent<SpriteRenderer>().sprite = this.sprite_2;
                myText.text = "Recharging.";
            }
            if (i > (actionTime / 10) * 2 && i < (actionTime / 10) * 3)
            {
                gameObject.GetComponent<SpriteRenderer>().sprite = this.sprite_3;
                myText.text = "Recharging..";
            }
            if (i > (actionTime / 10) * 3 && i < (actionTime / 10) * 4)
            {
                gameObject.GetComponent<SpriteRenderer>().sprite = this.sprite_2;
                myText.text = "Recharging...";
            }
            if (i > (actionTime / 10) * 4 && i < (actionTime / 10) * 5)
            {
                gameObject.GetComponent<SpriteRenderer>().sprite = this.sprite_1;
                myText.text = "Recharging";
            }
            if (i > (actionTime / 10) * 5 && i < (actionTime / 10) * 6)
            {
                gameObject.GetComponent<SpriteRenderer>().sprite = this.sprite_1;
                myText.text = "Recharging";
            }
            if (i > (actionTime / 10) * 6 && i < (actionTime / 10) * 7)
            {
                gameObject.GetComponent<SpriteRenderer>().sprite = this.sprite_2;
                myText.text = "Recharging.";
            }
            if (i > (actionTime / 10) * 7 && i < (actionTime / 10) * 8)
            {
                gameObject.GetComponent<SpriteRenderer>().sprite = this.sprite_3;
                myText.text = "Recharging..";
            }
            if (i > (actionTime / 10) * 8 && i < (actionTime / 10) * 9)
            {
                gameObject.GetComponent<SpriteRenderer>().sprite = this.sprite_2;
                myText.text = "Recharging...";
            }
            if (i > (actionTime / 10) * 9 && i < (actionTime / 10) * 10)
            {
                gameObject.GetComponent<SpriteRenderer>().sprite = this.sprite_1;
                myText.text = "Recharging";
            }
            yield return null;
        }
        //Actual effect
        this.batteryAmount = batteryCapacity;

        //Reset Animation and Destroy the text
        gameObject.GetComponent<SpriteRenderer>().sprite = this.normalSprite;
        Destroy(newGO);

        //TODO - UpdateBeliefs
    }

	public void StoreWood()
	{
		StartCoroutine(StoreWood(GameManager.GetTileSize(), GameManager.GetBatteryDecrease("store-wood")));
	}

	public IEnumerator StoreWood(float tileSize, int batteryDecrease)
    {
        //Adding text
        GameObject newGO = new GameObject("myTextGO");
        newGO.transform.SetParent(this.transform);
        newGO.transform.position = new Vector2(gameObject.transform.position.x, gameObject.transform.position.y + (tileSize / 1.5f));
        TextMeshPro myText = newGO.AddComponent<TextMeshPro>();
        myText.autoSizeTextContainer = true;
        myText.fontSize = 4.5f;
        myText.text = "Storing Wood...";

        int actionTime = 120;
        //Animation
        for (int i = 0; i < actionTime; i++)
        {

            if (i > 0 && i < actionTime / 10)
            {
                gameObject.GetComponent<SpriteRenderer>().sprite = this.sprite_1;
                myText.text = "Storing Wood";
            }
            if (i > (actionTime / 10) && i < (actionTime / 10) * 2)
            {
                gameObject.GetComponent<SpriteRenderer>().sprite = this.sprite_2;
                myText.text = "Storing Wood.";
            }
            if (i > (actionTime / 10) * 2 && i < (actionTime / 10) * 3)
            {
                gameObject.GetComponent<SpriteRenderer>().sprite = this.sprite_3;
                myText.text = "Storing Wood..";
            }
            if (i > (actionTime / 10) * 3 && i < (actionTime / 10) * 4)
            {
                gameObject.GetComponent<SpriteRenderer>().sprite = this.sprite_2;
                myText.text = "Storing Wood...";
            }
            if (i > (actionTime / 10) * 4 && i < (actionTime / 10) * 5)
            {
                gameObject.GetComponent<SpriteRenderer>().sprite = this.sprite_1;
                myText.text = "Storing Wood";
            }
            if (i > (actionTime / 10) * 5 && i < (actionTime / 10) * 6)
            {
                gameObject.GetComponent<SpriteRenderer>().sprite = this.sprite_1;
                myText.text = "Storing Wood";
            }
            if (i > (actionTime / 10) * 6 && i < (actionTime / 10) * 7)
            {
                gameObject.GetComponent<SpriteRenderer>().sprite = this.sprite_2;
                myText.text = "Storing Wood.";
            }
            if (i > (actionTime / 10) * 7 && i < (actionTime / 10) * 8)
            {
                gameObject.GetComponent<SpriteRenderer>().sprite = this.sprite_3;
                myText.text = "Storing Wood..";
            }
            if (i > (actionTime / 10) * 8 && i < (actionTime / 10) * 9)
            {
                gameObject.GetComponent<SpriteRenderer>().sprite = this.sprite_2;
                myText.text = "Storing Wood...";
            }
            if (i > (actionTime / 10) * 9 && i < (actionTime / 10) * 10)
            {
                gameObject.GetComponent<SpriteRenderer>().sprite = this.sprite_1;
                myText.text = "Storing Wood";
            }
            yield return null;
        }

        //Actual effect
        this.woodAmount--;
		this.batteryAmount -= batteryDecrease;

        //Reset Animation and Destroy the text
        gameObject.GetComponent<SpriteRenderer>().sprite = this.normalSprite;
        Destroy(newGO);

        //TODO - UpdateBeliefs
    }

	public void StoreStone()
	{
		StartCoroutine(StoreStone(GameManager.GetTileSize(), GameManager.GetBatteryDecrease("store-stone")));
	}

	public IEnumerator StoreStone(float tileSize, int batteryDecrease)
    {
        //Adding text
        GameObject newGO = new GameObject("myTextGO");
        newGO.transform.SetParent(this.transform);
        newGO.transform.position = new Vector2(gameObject.transform.position.x, gameObject.transform.position.y + (tileSize / 1.5f));
        TextMeshPro myText = newGO.AddComponent<TextMeshPro>();
        myText.autoSizeTextContainer = true;
        myText.fontSize = 4.5f;
        myText.text = "Storing Stone...";

        int actionTime = 120;
        //Animation
        for (int i = 0; i < actionTime; i++)
        {

            if (i > 0 && i < actionTime / 10)
            {
                gameObject.GetComponent<SpriteRenderer>().sprite = this.sprite_1;
                myText.text = "Storing Stone";
            }
            if (i > (actionTime / 10) && i < (actionTime / 10) * 2)
            {
                gameObject.GetComponent<SpriteRenderer>().sprite = this.sprite_2;
                myText.text = "Storing Stone.";
            }
            if (i > (actionTime / 10) * 2 && i < (actionTime / 10) * 3)
            {
                gameObject.GetComponent<SpriteRenderer>().sprite = this.sprite_3;
                myText.text = "Storing Stone..";
            }
            if (i > (actionTime / 10) * 3 && i < (actionTime / 10) * 4)
            {
                gameObject.GetComponent<SpriteRenderer>().sprite = this.sprite_2;
                myText.text = "Storing Stone...";
            }
            if (i > (actionTime / 10) * 4 && i < (actionTime / 10) * 5)
            {
                gameObject.GetComponent<SpriteRenderer>().sprite = this.sprite_1;
                myText.text = "Storing Stone";
            }
            if (i > (actionTime / 10) * 5 && i < (actionTime / 10) * 6)
            {
                gameObject.GetComponent<SpriteRenderer>().sprite = this.sprite_1;
                myText.text = "Storing Stone";
            }
            if (i > (actionTime / 10) * 6 && i < (actionTime / 10) * 7)
            {
                gameObject.GetComponent<SpriteRenderer>().sprite = this.sprite_2;
                myText.text = "Storing Stone.";
            }
            if (i > (actionTime / 10) * 7 && i < (actionTime / 10) * 8)
            {
                gameObject.GetComponent<SpriteRenderer>().sprite = this.sprite_3;
                myText.text = "Storing Stone..";
            }
            if (i > (actionTime / 10) * 8 && i < (actionTime / 10) * 9)
            {
                gameObject.GetComponent<SpriteRenderer>().sprite = this.sprite_2;
                myText.text = "Storing Stone...";
            }
            if (i > (actionTime / 10) * 9 && i < (actionTime / 10) * 10)
            {
                gameObject.GetComponent<SpriteRenderer>().sprite = this.sprite_1;
                myText.text = "Storing Stone";
            }
            yield return null;
        }
        //Actual effect
        this.stoneAmount--;
		this.batteryAmount -= batteryDecrease;

        //Reset Animation and Destroy the text
        gameObject.GetComponent<SpriteRenderer>().sprite = this.normalSprite;
        Destroy(newGO);

        //TODO - UpdateBeliefs
    }

	public void GiveWood()
	{
		StartCoroutine(GiveWood(GameManager.GetTileSize(), GameManager.GetBatteryDecrease("retrieve-wood")));
	}

	//NO text, since Producer already showing
	public IEnumerator GiveWood(float tileSize, int batteryDecrease)
    {

        int actionTime = 120;
        //Animation
        for (int i = 0; i < actionTime; i++)
        {

            if (i > 0 && i < actionTime / 10)
            {
                gameObject.GetComponent<SpriteRenderer>().sprite = this.sprite_1;
            }
            if (i > (actionTime / 10) && i < (actionTime / 10) * 2)
            {
                gameObject.GetComponent<SpriteRenderer>().sprite = this.sprite_2;
            }
            if (i > (actionTime / 10) * 2 && i < (actionTime / 10) * 3)
            {
                gameObject.GetComponent<SpriteRenderer>().sprite = this.sprite_3;
            }
            if (i > (actionTime / 10) * 3 && i < (actionTime / 10) * 4)
            {
                gameObject.GetComponent<SpriteRenderer>().sprite = this.sprite_2;
            }
            if (i > (actionTime / 10) * 4 && i < (actionTime / 10) * 5)
            {
                gameObject.GetComponent<SpriteRenderer>().sprite = this.sprite_1;
            }
            if (i > (actionTime / 10) * 5 && i < (actionTime / 10) * 6)
            {
                gameObject.GetComponent<SpriteRenderer>().sprite = this.sprite_1;
            }
            if (i > (actionTime / 10) * 6 && i < (actionTime / 10) * 7)
            {
                gameObject.GetComponent<SpriteRenderer>().sprite = this.sprite_2;
            }
            if (i > (actionTime / 10) * 7 && i < (actionTime / 10) * 8)
            {
                gameObject.GetComponent<SpriteRenderer>().sprite = this.sprite_3;
            }
            if (i > (actionTime / 10) * 8 && i < (actionTime / 10) * 9)
            {
                gameObject.GetComponent<SpriteRenderer>().sprite = this.sprite_2;
            }
            if (i > (actionTime / 10) * 9 && i < (actionTime / 10) * 10)
            {
                gameObject.GetComponent<SpriteRenderer>().sprite = this.sprite_1;
            }
            yield return null;
        }
        //Actual effect
        this.woodAmount--;
		this.batteryAmount -= batteryDecrease;

		//Reset Animation and Destroy the text
		gameObject.GetComponent<SpriteRenderer>().sprite = this.normalSprite;

        //TODO - UpdateBeliefs
    }

	public void GiveStone()
	{
		StartCoroutine(GiveStone(GameManager.GetTileSize(), GameManager.GetBatteryDecrease("retrieve-stone")));
	}

	//NO text, since Producer already showing
	public IEnumerator GiveStone(float tileSize, int batteryDecrease)
    {

        int actionTime = 120;
        //Animation
        for (int i = 0; i < actionTime; i++)
        {

            if (i > 0 && i < actionTime / 10)
            {
                gameObject.GetComponent<SpriteRenderer>().sprite = this.sprite_1;
            }
            if (i > (actionTime / 10) && i < (actionTime / 10) * 2)
            {
                gameObject.GetComponent<SpriteRenderer>().sprite = this.sprite_2;
            }
            if (i > (actionTime / 10) * 2 && i < (actionTime / 10) * 3)
            {
                gameObject.GetComponent<SpriteRenderer>().sprite = this.sprite_3;
            }
            if (i > (actionTime / 10) * 3 && i < (actionTime / 10) * 4)
            {
                gameObject.GetComponent<SpriteRenderer>().sprite = this.sprite_2;
            }
            if (i > (actionTime / 10) * 4 && i < (actionTime / 10) * 5)
            {
                gameObject.GetComponent<SpriteRenderer>().sprite = this.sprite_1;
            }
            if (i > (actionTime / 10) * 5 && i < (actionTime / 10) * 6)
            {
                gameObject.GetComponent<SpriteRenderer>().sprite = this.sprite_1;
            }
            if (i > (actionTime / 10) * 6 && i < (actionTime / 10) * 7)
            {
                gameObject.GetComponent<SpriteRenderer>().sprite = this.sprite_2;
            }
            if (i > (actionTime / 10) * 7 && i < (actionTime / 10) * 8)
            {
                gameObject.GetComponent<SpriteRenderer>().sprite = this.sprite_3;
            }
            if (i > (actionTime / 10) * 8 && i < (actionTime / 10) * 9)
            {
                gameObject.GetComponent<SpriteRenderer>().sprite = this.sprite_2;
            }
            if (i > (actionTime / 10) * 9 && i < (actionTime / 10) * 10)
            {
                gameObject.GetComponent<SpriteRenderer>().sprite = this.sprite_1;
            }
            yield return null;
        }
        //Actual effect
        this.stoneAmount--;
		this.batteryAmount -= batteryDecrease;

		//Reset Animation and Destroy the text
		gameObject.GetComponent<SpriteRenderer>().sprite = this.normalSprite;

        //TODO - UpdateBeliefs
    }

	public void InterruptActions()
	{
		StopAllCoroutines();
	}

	public void ResetPosition()
	{
		this.transform.position = new Vector2(0, 0);
	}

	void OnMouseDown()
	{
		InterruptActions();
		gameObject.GetComponent<SpriteRenderer>().color = Color.gray;
		distance = Vector3.Distance(transform.position, Camera.main.transform.position);
		dragging = true;
	}

	void OnMouseUp()
	{
		gameObject.GetComponent<SpriteRenderer>().color = Color.white;
		underneathTile.GetComponent<SpriteRenderer>().color = Color.white;

		this.posX = (int)(underneathTile.transform.position.x - GameManager.GetGridPosition().x);
		this.posY = (int)(underneathTile.transform.position.y - GameManager.GetGridPosition().y);

		ResetPosition();
		MoveToDestination(GameManager.GetTileSize(), GameManager.GetGridPosition());

		Dictionary<string, int> toUpdate = new Dictionary<string, int>();
		toUpdate.Add("posX_" + this.name, this.posX);
		toUpdate.Add("posY_" + this.name, this.posY);
		Parser.UpdateSensors(toUpdate, "SET", GameManager.GetFrame());

		dragging = false;
	}
}
