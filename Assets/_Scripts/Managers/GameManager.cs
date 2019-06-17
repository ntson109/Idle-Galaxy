using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EventDispatcher;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance
    {
        get
        {
            if (_instace == null)
            {
                var go = GameObject.Find("GameManager");
                if (go != null) _instace = go.GetComponent<GameManager>();
            }
            return _instace;
        }
    }
    private static GameManager _instace;

    public long GOLD;
    public long COIN;
    public int MEDAL;
    public int SKIP_TIME;
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

    void Start()
    {
        DontDestroyOnLoad(this.gameObject);
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

    public void AddMedal(int _value)
    {
        MEDAL += _value;
        if (MEDAL <= 0)
            MEDAL = 0;

        UIManager.Instance.txtMedal.text = UIManager.Instance.ToLongString(MEDAL);
    }

    public void AddSkipTime(int _value)
    {
        SKIP_TIME += _value;
        if (SKIP_TIME <= 0)
            SKIP_TIME = 0;
        UIManager.Instance.txtSkipTime.text = UIManager.Instance.transformToTime(SKIP_TIME * 60);
        UIManager.Instance.txtSkipTimeInPanel.text = UIManager.Instance.transformToTime(SKIP_TIME * 60);
    }

    public void SaveExit()
    {
        Debug.Log("quit");
        Application.Quit();
    }

    private void OnDestroy()
    {
        if (this.stateGame == StateGame.PLAYING && DataPlayer.Instance)
        {
            DataPlayer.Instance.SaveDataPlayer();
            PlayerPrefs.SetString(KeyPrefs.TIME_QUIT_GAME, System.DateTime.Now.ToString());
        }
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
