using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EventDispatcher;

public class GameManager : MonoBehaviour {
    public static GameManager Instance = new GameManager();
    void Awake()
    {
        if (Instance != null)
        {
            return;
        }
        Instance = this;
    }

    public long GOLD;
    public long COIN;
	void Start () {
		
	}

	void Update () {
		
	}

    public void AddGold(long _value)
    {
        GOLD += _value;
        if (GOLD <= 0)
            GOLD = 0;

        UIManager.Instance.txtGold.text = UIManager.Instance.ToLongString(GOLD);
        this.PostEvent(EventID.CHANGE_GOLD_COIN);
    }

    public void AddCoin(long _value)
    {
        COIN += _value;
        if (COIN <= 0)
            COIN = 0;

        UIManager.Instance.txtDiamond.text = UIManager.Instance.ToLongString(COIN);
        this.PostEvent(EventID.CHANGE_GOLD_COIN);
    }
}
