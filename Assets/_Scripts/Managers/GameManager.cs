﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EventDispatcher;

public class GameManager : MonoBehaviour
{
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
    public StateGame stateGame = StateGame.NONE;
    public List<Map> lstMap = new List<Map>();
    public Boost boost;
    public float timeBoost;
    public List<UpgradeObj_Special> lstUpgradeSpecial;

    void Start()
    {
        boost.SetDefault();
    }

    void Update()
    {
        if (stateGame == StateGame.PLAYING)
        {
            if (boost.type != TypeBoost.NONE)
            {
                if(timeBoost <= 0)
                {
                    boost.type = TypeBoost.NONE;
                    timeBoost = 0;
                    UIManager.Instance.txtTimeBoost.text = "";
                    UIManager.Instance.txtBoost.text = "";
                    boost.SetDefault();
                }
                timeBoost -= Time.deltaTime;
                UIManager.Instance.txtTimeBoost.text = transformToTime(timeBoost);
            }
        }
    }

    string transformToTime(float time = 0)
    {
        int minutes = Mathf.FloorToInt(time / 60F);
        int seconds = Mathf.FloorToInt(time - minutes * 60);
        return string.Format("{0:00}:{1:00}", minutes, seconds);
    }

    public void AddGold(long _value)
    {
        GOLD = GOLD + (_value * boost.value);
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

[System.Serializable]
public enum StateGame
{
    NONE,
    PLAYING
}

[System.Serializable]
public enum TypeBoost
{
    NONE,
    GOLD,
    CAPACITY,
    SPEED,
    TIME
}

public struct Boost
{
    public TypeBoost type;
    public int value;
    public int time;

    public void SetDefault()
    {
        this.type = TypeBoost.NONE;
        this.value = 1;
        this.time = 0;
    }

    public void SetBoost(TypeBoost _type,int _value, int _time)
    {
        this.type = _type;
        this.value = _value;
        this.time = _time;
    }
}
