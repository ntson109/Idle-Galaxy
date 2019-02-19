using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    public long gold;
    public long diamond;
	void Start () {
		
	}

	void Update () {
		
	}

    public void AddGold(long _value)
    {
        gold += _value;
        if (gold <= 0)
            gold = 0;

        UIManager.Instance.txtGold.text = UIManager.Instance.ToLongString(gold);
    }

    public void AddDiamond(long _value)
    {
        diamond += _value;
        if (diamond <= 0)
            diamond = 0;

        UIManager.Instance.txtDiamond.text = UIManager.Instance.ToLongString(diamond);
    }
}
