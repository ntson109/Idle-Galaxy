using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using EventDispatcher;
using System;

public class DataPlayer : MonoBehaviour
{
    public static DataPlayer Instance;
    private DateTime dateNowPlayer;
    void Awake()
    {
        if (Instance != null)
        {
            return;
        }
        if (!PlayerPrefs.HasKey(KeyPrefs.TIME_QUIT_GAME))
        {
            PlayerPrefs.SetString(KeyPrefs.TIME_QUIT_GAME, DateTime.Now.ToString());
        }
        Instance = this;
    }
    [HideInInspector]
    public long gold;

    [HideInInspector]
    public long coin;

    [HideInInspector]
    public long freeGold1s;

    [HideInInspector]
    public List<MapJSON> lstMap;

    [HideInInspector]
    public BoostJSON boost;

    [HideInInspector]
    public int countSpin;

    [HideInInspector]
    public List<MineShaftJSON> lsMineShaft;

    public void SaveDataPlayer()
    {
        DataPlayer data = new DataPlayer();
        data.gold = GameManager.Instance.GOLD;
        data.coin = GameManager.Instance.COIN;
        data.freeGold1s = GetFreeGoldPerSecond();
        data.boost = new BoostJSON();
        if (GameManager.Instance.boost.type == TypeBoost.NONE)
        {
            data.boost.type = 1;
        }
        else if (GameManager.Instance.boost.type == TypeBoost.GOLD)
        {
            data.boost.type = 2;
        }
        else if (GameManager.Instance.boost.type == TypeBoost.SPEED)
        {
            data.boost.type = 3;
        }
        data.boost.time = GameManager.Instance.timeBoost;
        data.countSpin = GameManager.Instance.countSpin;

        data.lstMap = new List<MapJSON>();
        for (int i = 0; i < GameManager.Instance.lstMap.Count; i++)
        {
            MapJSON m = new MapJSON();
            m.transporter = new TransporterJSON();
            if (GameManager.Instance.lstMap[i].type == TypeMap.MOON)
            {
                m.typeMap = 1;
            }
            else if (GameManager.Instance.lstMap[i].type == TypeMap.EARTH)
            {
                m.typeMap = 2;
            }
            m.totalAmount = GameManager.Instance.lstMap[i].totalAmount;
            m.totalMoney = GameManager.Instance.lstMap[i].totalMoney;
            m.transporter.level = GameManager.Instance.lstMap[i].transporter.level;
            m.transporter.capacity = GameManager.Instance.lstMap[i].transporter.capacity;
            data.lstMap.Add(m);
        }

        data.lsMineShaft = new List<MineShaftJSON>();
        for (int i = 0; i < GameManager.Instance.lstMap.Count; i++)
        {
            for (int j = 0; j < GameManager.Instance.lstMap[i].lstMineShaft.Count; j++)
            {
                MineShaftJSON m = new MineShaftJSON();
                if (GameManager.Instance.lstMap[i].type == TypeMap.MOON)
                {
                    m.typeMap = 0;
                }
                else if (GameManager.Instance.lstMap[i].type == TypeMap.EARTH)
                {
                    m.typeMap = 1;
                }
                m.ID = GameManager.Instance.lstMap[i].lstMineShaft[j].ID;
                m.level = GameManager.Instance.lstMap[i].lstMineShaft[j].properties.level;
                m.timeCurrent = GameManager.Instance.lstMap[i].lstMineShaft[j].timer;
                m.numberMine = GameManager.Instance.lstMap[i].lstMineShaft[j].numberMine;
                m.buyMoreMinePrice = GameManager.Instance.lstMap[i].lstMineShaft[j].properties.buyMoreMinePrice;
                m.miningTime = GameManager.Instance.lstMap[i].lstMineShaft[j].properties.miningTime;
                m.capacity = GameManager.Instance.lstMap[i].lstMineShaft[j].properties.capacity;
                if (GameManager.Instance.lstMap[i].lstMineShaft[j].state == MineShaft.StateMineShaft.LOCK)
                {
                    m.state = 1;
                }
                else if (GameManager.Instance.lstMap[i].lstMineShaft[j].state == MineShaft.StateMineShaft.UNLOCKING)
                {
                    m.state = 2;
                }
                else if (GameManager.Instance.lstMap[i].lstMineShaft[j].state == MineShaft.StateMineShaft.IDLE)
                {
                    m.state = 3;
                }
                else if (GameManager.Instance.lstMap[i].lstMineShaft[j].state == MineShaft.StateMineShaft.WORKING)
                {
                    m.state = 4;
                }
                else if (GameManager.Instance.lstMap[i].lstMineShaft[j].state == MineShaft.StateMineShaft.UPGRADING)
                {
                    m.state = 5;
                }

                m.timeUpgradeSpecial = new List<float>();
                for (int k = 0; k < GameManager.Instance.lstMap[i].lstMineShaft[j].timeUpgradeSpecial.Count; k++)
                {
                    m.timeUpgradeSpecial.Add(GameManager.Instance.lstMap[i].lstMineShaft[j].timeUpgradeSpecial[k]);
                }

                m.stateUpgradeSpecial = new List<int>();
                for (int k = 0; k < GameManager.Instance.lstMap[i].lstMineShaft[j].typeUpgradeSpecial.Count; k++)
                {
                    if (GameManager.Instance.lstMap[i].lstMineShaft[j].typeUpgradeSpecial[k] == UpgradeObj_Special.Type.NONE)
                        m.stateUpgradeSpecial.Add(1);
                    else if (GameManager.Instance.lstMap[i].lstMineShaft[j].typeUpgradeSpecial[k] == UpgradeObj_Special.Type.UPGRADING)
                        m.stateUpgradeSpecial.Add(2);
                    else if (GameManager.Instance.lstMap[i].lstMineShaft[j].typeUpgradeSpecial[k] == UpgradeObj_Special.Type.UPGRADED)
                        m.stateUpgradeSpecial.Add(3);
                }

                m.isAutoWorking = GameManager.Instance.lstMap[i].lstMineShaft[j].isAutoWorking;
                m.timeUnlocking = GameManager.Instance.lstMap[i].lstMineShaft[j].timeUnlocking;
                m.timeUpgradeLevel = GameManager.Instance.lstMap[i].lstMineShaft[j].timeUpgradeLevel;

                data.lsMineShaft.Add(m);
            }
        }

        string _path = Path.Combine(Application.persistentDataPath, "DataPlayer.json");
        File.WriteAllText(_path, JsonUtility.ToJson(data, true));
        File.ReadAllText(_path);
        PlayerPrefs.SetInt(KeyPrefs.IS_CONTINUE, 1);

        Debug.Log(SimpleJSON_DatDz.JSON.Parse(File.ReadAllText(_path)));
    }

    public void LoadDataPlayer()
    {
        dateNowPlayer = DateTime.Now;
        string _path = Path.Combine(Application.persistentDataPath, "DataPlayer.json");
        string dataAsJson = File.ReadAllText(_path);
        var objJson = SimpleJSON_DatDz.JSON.Parse(dataAsJson);
        Debug.Log(objJson);
        if (objJson != null)
        {
            StartCoroutine(IE_LoadDataPlayer(objJson));
        }
    }

    IEnumerator IE_LoadDataPlayer(SimpleJSON_DatDz.JSONNode objJson)
    {

        GameManager.Instance.AddGold(objJson["gold"].AsLong);
        GameManager.Instance.AddCoin(objJson["coin"].AsLong);

        for (int i = 0; i < objJson["lstMap"].Count; i++)
        {
            if (objJson["lstMap"][i]["typeMap"] == 1)
            {
                GameManager.Instance.lstMap[i].type = TypeMap.MOON;
            }
            else if (objJson["lstMap"][i]["typeMap"] == 2)
            {
                GameManager.Instance.lstMap[i].type = TypeMap.MOON;
            }
            GameManager.Instance.lstMap[i].totalAmount = objJson["lstMap"][i]["totalAmount"].AsLong;
            GameManager.Instance.lstMap[i].totalMoney = objJson["lstMap"][i]["totalMoney"].AsLong;
            GameManager.Instance.lstMap[i].transporter.level = objJson["lstMap"][i]["transporter"]["level"].AsInt;
            GameManager.Instance.lstMap[i].transporter.capacity = objJson["lstMap"][i]["transporter"]["capacity"].AsLong;
        }

        if (objJson["boost"]["type"] == 1)
        {
            GameManager.Instance.boost.type = TypeBoost.NONE;
        }
        else if (objJson["boost"]["type"] == 2)
        {
            GameManager.Instance.boost.type = TypeBoost.GOLD;
        }
        else if (objJson["boost"]["type"] == 3)
        {
            GameManager.Instance.boost.type = TypeBoost.SPEED;
        }
        GameManager.Instance.timeBoost = objJson["boost"]["time"].AsFloat;

        GameManager.Instance.countSpin = objJson["boost"]["countSpin"].AsInt;

        for (int i = 0; i < objJson["lsMineShaft"].Count; i++)
        {
            if (i <= GameManager.Instance.lstMap[0].lstMineShaft.Count)
            {
                GameManager.Instance.lstMap[0].lstMineShaft[i].ID = objJson["lsMineShaft"][i]["ID"].AsInt;
                GameManager.Instance.lstMap[0].lstMineShaft[i].properties.level = objJson["lsMineShaft"][i]["level"].AsInt;
                GameManager.Instance.lstMap[0].lstMineShaft[i].timer = objJson["lsMineShaft"][i]["timeCurrent"].AsInt;
                GameManager.Instance.lstMap[0].lstMineShaft[i].numberMine = objJson["lsMineShaft"][i]["numberMine"].AsInt;
                GameManager.Instance.lstMap[0].lstMineShaft[i].properties.buyMoreMinePrice = objJson["lsMineShaft"][i]["buyMoreMinePrice"].AsLong;
                GameManager.Instance.lstMap[0].lstMineShaft[i].properties.capacity = objJson["lsMineShaft"][i]["capacity"].AsInt;
                GameManager.Instance.lstMap[0].lstMineShaft[i].properties.miningTime = objJson["lsMineShaft"][i]["miningTime"].AsInt;

                if (objJson["lsMineShaft"][i]["state"].AsInt == 1)
                    GameManager.Instance.lstMap[0].lstMineShaft[i].state = MineShaft.StateMineShaft.LOCK;
                else if (objJson["lsMineShaft"][i]["state"].AsInt == 2)
                    GameManager.Instance.lstMap[0].lstMineShaft[i].state = MineShaft.StateMineShaft.UNLOCKING;
                else if (objJson["lsMineShaft"][i]["state"].AsInt == 3)
                    GameManager.Instance.lstMap[0].lstMineShaft[i].state = MineShaft.StateMineShaft.IDLE;
                else if (objJson["lsMineShaft"][i]["state"].AsInt == 4)
                    GameManager.Instance.lstMap[0].lstMineShaft[i].state = MineShaft.StateMineShaft.WORKING;
                else if (objJson["lsMineShaft"][i]["state"].AsInt == 5)
                    GameManager.Instance.lstMap[0].lstMineShaft[i].state = MineShaft.StateMineShaft.UPGRADING;

                for (int j = 0; j < objJson["lsMineShaft"][i]["timeUpgradeSpecial"].Count; j++)
                {
                    GameManager.Instance.lstMap[0].lstMineShaft[i].timeUpgradeSpecial.Add(objJson["lsMineShaft"][i]["timeUpgradeSpecial"][j].AsFloat);
                }

                for (int j = 0; j < objJson["lsMineShaft"][i]["stateUpgradeSpecial"].Count; j++)
                {
                    if (objJson["lsMineShaft"][i]["stateUpgradeSpecial"][j].AsInt == 1)
                    {
                        GameManager.Instance.lstMap[0].lstMineShaft[i].typeUpgradeSpecial.Add(UpgradeObj_Special.Type.NONE);
                    }
                    else if (objJson["lsMineShaft"][i]["stateUpgradeSpecial"][j].AsInt == 2)
                    {
                        GameManager.Instance.lstMap[0].lstMineShaft[i].typeUpgradeSpecial.Add(UpgradeObj_Special.Type.UPGRADING);
                    }
                    else if (objJson["lsMineShaft"][i]["stateUpgradeSpecial"][j].AsInt == 3)
                    {
                        GameManager.Instance.lstMap[0].lstMineShaft[i].typeUpgradeSpecial.Add(UpgradeObj_Special.Type.UPGRADED);
                    }
                }

                GameManager.Instance.lstMap[0].lstMineShaft[i].isAutoWorking = objJson["lsMineShaft"][i]["isAutoWorking"].AsBool;
                GameManager.Instance.lstMap[0].lstMineShaft[i].timeUnlocking = objJson["lsMineShaft"][i]["timeUnlocking"].AsFloat;
                GameManager.Instance.lstMap[0].lstMineShaft[i].timeUpgradeLevel = objJson["lsMineShaft"][i]["timeUpgradeLevel"].AsFloat;
            }
        }

        yield return new WaitForEndOfFrame();
        ScenesManager.Instance.isNextScene = true;
        this.PostEvent(EventID.START_GAME);
        UIManager.Instance.timeOffline = UIManager.Instance.GetOfflineTime(PlayerPrefs.GetString(KeyPrefs.TIME_QUIT_GAME));
        UIManager.Instance.goldOffline = objJson["freeGold1s"].AsLong * UIManager.Instance.timeOffline * 60;
        UIManager.Instance.ShowPanelOffline();
    }

    long GetFreeGoldPerSecond()
    {
        long _gold = 0;
        for (int i = 0; i < GameManager.Instance.lstMap.Count; i++)
        {
            for (int j = 0; j < GameManager.Instance.lstMap[i].lstMineShaft.Count; j++)
            {
                if (GameManager.Instance.lstMap[i].lstMineShaft[j].state != MineShaft.StateMineShaft.LOCK && GameManager.Instance.lstMap[i].lstMineShaft[j].state != MineShaft.StateMineShaft.UNLOCKING)
                {
                    _gold += (long)(((GameManager.Instance.lstMap[i].lstMineShaft[j].numberMine * GameManager.Instance.lstMap[i].lstMineShaft[j].properties.capacity) / GameManager.Instance.lstMap[i].lstMineShaft[j].properties.miningTime) * GameManager.Instance.lstMap[i].lstMineShaft[j].properties.unitPrice);
                }
            }
        }
        return _gold;
    }

    private void OnDestroy()
    {
        SaveDataPlayer();

        PlayerPrefs.SetString(KeyPrefs.TIME_QUIT_GAME, DateTime.Now.ToString());
    }

    private void OnApplicationPause(bool pause)
    {
        if (pause == true)
        {
            SaveDataPlayer();

            PlayerPrefs.SetString(KeyPrefs.TIME_QUIT_GAME, DateTime.Now.ToString());
        }
        else
        {
            dateNowPlayer = DateTime.Now;
        }
    }
}
