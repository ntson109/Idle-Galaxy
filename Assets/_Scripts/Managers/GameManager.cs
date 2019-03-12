using System.Collections;
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
    public UpgradeObj_Level upgradeLevel;

    [Header("UFO")]
    public UFO UFO;
    public float timeFlyingUFO;

    [Header("Spin")]
    public int countSpin = 0;

    long test = 5;
    long test_2;
    void Start()
    {
        //boost.SetDefault();
        //for (int i = 1; i <= 500; i++)
        //{
        //    if (i <= 100)
        //    {
        //        test_2 = (long)(test * 0.05f);
        //    }
        //    else
        //    {
        //        test_2 = (long)(test * 0.015f);
        //    }
        //    if (test_2 <= 2)
        //        test_2 = 2;
        //    test += test_2;
        //    if (i == 10)
        //    {
        //        Debug.Log(test);
        //    }

        //    if (i == 50)
        //    {
        //        Debug.Log(test);
        //    }

        //    if (i == 100)
        //    {
        //        Debug.Log(test);
        //    }

        //    if (i == 200)
        //    {
        //        Debug.Log(test);
        //    }

        //    if (i == 500)
        //    {
        //        Debug.Log(test);
        //    }
        //}
        
    }

    void Update()
    {
        if (stateGame == StateGame.PLAYING)
        {
            if (boost.type != TypeBoost.NONE)
            {
                if (timeBoost <= 0)
                {
                    boost.type = TypeBoost.NONE;
                    timeBoost = 0;
                    UIManager.Instance.txtTimeBoost.text = "";
                    UIManager.Instance.txtBoost.text = "";
                    boost.SetDefault();
                }
                timeBoost -= Time.deltaTime;
                UIManager.Instance.txtTimeBoost.text = UIManager.Instance.transformToTime(timeBoost);
            }

            if (!UFO.isOpening)
            {
                if (timeFlyingUFO >= GameConfig.Instance.UFO_time)
                {
                    UFO.Move();
                    timeFlyingUFO = 0;
                }
                else
                {
                    timeFlyingUFO += Time.deltaTime;
                }
            }
        }
    }

    public void AddGold(long _value)
    {
        if (boost.type == TypeBoost.GOLD)
        {
            if (_value >= 0)
                GOLD = GOLD + (_value * 2);
            else
                GOLD = GOLD + (_value);
        }
        else
        {
            GOLD = GOLD + (_value);
        }

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

        UIManager.Instance.txtCoin.text = UIManager.Instance.ToLongString(COIN);
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
    SPEED
}

public struct Boost
{
    public TypeBoost type;
    public int time;

    public void SetDefault()
    {
        this.type = TypeBoost.NONE;
        this.time = 0;
    }

    public void SetBoost(TypeBoost _type, int _value, int _time)
    {
        this.type = _type;
        this.time = _time;
    }
}
