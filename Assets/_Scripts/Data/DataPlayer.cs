using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using EventDispatcher;
using System;

public class DataPlayer : MonoBehaviour
{
    public static DataPlayer Instance;
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
        //data.freeGold1s = GetFreeGoldPerSecond();
        //PlayerPrefs.SetString(KeyPrefs.GOLD_OFFLINE, GetFreeGoldPerSecond().ToString());
        PlayerPrefs.SetString(KeyPrefs.GOLD_OFFLINE, CapacityOffline().ToString());
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
            //m.totalAmount = GameManager.Instance.lstMap[i].totalAmount;
            m.totalMoney = GameManager.Instance.lstMap[i].totalMoney;
            m.transporter.level = GameManager.Instance.lstMap[i].transporter.level;
            m.transporter.capacity = GameManager.Instance.lstMap[i].transporter.capacity;
            m.transporter.price = GameManager.Instance.lstMap[i].transporter.price;
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
                m.unitPrice = GameManager.Instance.lstMap[i].lstMineShaft[j].properties.unitPrice;
                m.input = GameManager.Instance.lstMap[i].lstMineShaft[j].input;
                m.workBar = GameManager.Instance.lstMap[i].lstMineShaft[j].imgWorkBar.fillAmount;
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

                m.store = new StoreJSON();
                m.store.level = GameManager.Instance.lstMap[i].lstMineShaft[j].store.level;
                m.store.value = GameManager.Instance.lstMap[i].lstMineShaft[j].store.value;
                m.store.capacity = GameManager.Instance.lstMap[i].lstMineShaft[j].store.capacity;
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
            //GameManager.Instance.lstMap[i].totalAmount = objJson["lstMap"][i]["totalAmount"].AsLong;
            GameManager.Instance.lstMap[i].totalMoney = objJson["lstMap"][i]["totalMoney"].AsLong;
            //GameManager.Instance.lstMap[i].transporter.level = objJson["lstMap"][i]["transporter"]["level"].AsInt;
            //GameManager.Instance.lstMap[i].transporter.capacity = objJson["lstMap"][i]["transporter"]["capacity"].AsLong;
            GameManager.Instance.lstMap[i].transporter.SetInfo(objJson["lstMap"][i]["transporter"]["level"].AsInt, objJson["lstMap"][i]["transporter"]["capacity"].AsLong, objJson["lstMap"][i]["transporter"]["price"].AsLong);
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
                GameManager.Instance.lstMap[0].lstMineShaft[i].timer = objJson["lsMineShaft"][i]["timeCurrent"].AsFloat;
                GameManager.Instance.lstMap[0].lstMineShaft[i].numberMine = objJson["lsMineShaft"][i]["numberMine"].AsInt;
                GameManager.Instance.lstMap[0].lstMineShaft[i].properties.buyMoreMinePrice = objJson["lsMineShaft"][i]["buyMoreMinePrice"].AsLong;
                GameManager.Instance.lstMap[0].lstMineShaft[i].properties.capacity = objJson["lsMineShaft"][i]["capacity"].AsInt;
                GameManager.Instance.lstMap[0].lstMineShaft[i].properties.unitPrice = objJson["lsMineShaft"][i]["unitPrice"].AsInt;
                GameManager.Instance.lstMap[0].lstMineShaft[i].properties.miningTime = objJson["lsMineShaft"][i]["miningTime"].AsInt;
                GameManager.Instance.lstMap[0].lstMineShaft[i].input = objJson["lsMineShaft"][i]["input"].AsInt;
                GameManager.Instance.lstMap[0].lstMineShaft[i].imgWorkBar.fillAmount = objJson["lsMineShaft"][i]["workBar"].AsFloat;
                if (objJson["lsMineShaft"][i]["state"].AsInt == 1)
                    GameManager.Instance.lstMap[0].lstMineShaft[i].state = MineShaft.StateMineShaft.LOCK;
                else if (objJson["lsMineShaft"][i]["state"].AsInt == 2)
                    GameManager.Instance.lstMap[0].lstMineShaft[i].state = MineShaft.StateMineShaft.UNLOCKING;
                else if (objJson["lsMineShaft"][i]["state"].AsInt == 3)
                    GameManager.Instance.lstMap[0].lstMineShaft[i].state = MineShaft.StateMineShaft.IDLE;
                else if (objJson["lsMineShaft"][i]["state"].AsInt == 4)
                    GameManager.Instance.lstMap[0].lstMineShaft[i].state = MineShaft.StateMineShaft.WORKING;

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
                GameManager.Instance.lstMap[0].lstMineShaft[i].store.level = objJson["lsMineShaft"][i]["store"]["level"].AsInt;
                GameManager.Instance.lstMap[0].lstMineShaft[i].store.value = objJson["lsMineShaft"][i]["store"]["value"].AsInt;
                GameManager.Instance.lstMap[0].lstMineShaft[i].store.capacity = objJson["lsMineShaft"][i]["store"]["capacity"].AsInt;
            }
        }

        yield return new WaitForEndOfFrame();
        ScenesManager.Instance.isNextScene = true;
        this.PostEvent(EventID.START_GAME);
        UIManager.Instance.timeOffline = UIManager.Instance.GetOfflineTime(PlayerPrefs.GetString(KeyPrefs.TIME_QUIT_GAME));
        //if (UIManager.Instance.timeOffline <= 0)
        //    UIManager.Instance.timeOffline = 1;
        //UIManager.Instance.goldOffline = objJson["freeGold1s"].AsLong * UIManager.Instance.timeOffline * 60;
        if (UIManager.Instance.timeOffline >= 2)
        {
            UIManager.Instance.goldOffline = long.Parse(PlayerPrefs.GetString(KeyPrefs.GOLD_OFFLINE)) * UIManager.Instance.timeOffline * 60;
            if (UIManager.Instance.goldOffline < 100)
                UIManager.Instance.goldOffline = 100;
            UIManager.Instance.ShowPanelOffline();
        }
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
                    if (_gold >= long.MaxValue)
                        _gold = long.MaxValue / 2;
                    if (_gold <= 0)
                        _gold = 100000;
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
            if (GameManager.Instance.stateGame == StateGame.PLAYING)
            {
                UIManager.Instance.timeOffline = UIManager.Instance.GetOfflineTime(PlayerPrefs.GetString(KeyPrefs.TIME_QUIT_GAME));
                //if (UIManager.Instance.timeOffline <= 0)
                //    UIManager.Instance.timeOffline = 1;
                if (UIManager.Instance.timeOffline >= 2)
                {
                    UIManager.Instance.goldOffline = long.Parse(PlayerPrefs.GetString(KeyPrefs.GOLD_OFFLINE)) * UIManager.Instance.timeOffline * 60;
                    if (UIManager.Instance.goldOffline < 100)
                        UIManager.Instance.goldOffline = 100;
                    UIManager.Instance.ShowPanelOffline();
                }
            }
        }
    }


    int l;
    long CapacityOffline()
    {
        long _money = 0;
        int counter = 0;
        float time = 0;
        l = 0;
        int T_invalid = 5000;
        float[] c;
        int[] a;
        int[] b;
        int[] n_counter = new int[6] { 10, 100, 200, 400, 600, 800 };

        for (int i = 0; i < GameManager.Instance.lstMap[0].lstMineShaft.Count; i++)
        {
            if (GameManager.Instance.lstMap[0].lstMineShaft[i].state != MineShaft.StateMineShaft.LOCK && GameManager.Instance.lstMap[0].lstMineShaft[i].state != MineShaft.StateMineShaft.UNLOCKING)
            {
                l++;
            }
            else
                break;
        }

        c = new float[l];
        a = new int[l];
        b = new int[l];
        for (int i = 0; i < l; i++)
        {
            if (GameManager.Instance.lstMap[0].lstMineShaft[i].state != MineShaft.StateMineShaft.LOCK && GameManager.Instance.lstMap[0].lstMineShaft[i].state != MineShaft.StateMineShaft.UNLOCKING)
            {
                c[i] = GameManager.Instance.lstMap[0].lstMineShaft[i].properties.miningTime - GameManager.Instance.lstMap[0].lstMineShaft[i].timer;
                a[i] = GameManager.Instance.lstMap[0].lstMineShaft[i].input;
                if (i == 0)
                {
                    b[i] = a[i];
                }
                else
                {
                    b[i] = GameManager.Instance.lstMap[0].lstMineShaft[i - 1].store.value;
                }
            }
        }

        while (counter < n_counter[l - 1])
        {
            int j = 0;
            float temp = c[0];
            int sp = 0;

            for (int i = 1; i < l; i++)
            {
                if (c[i] < temp)
                {
                    temp = c[i];
                    j = i;
                }
            }

            sp = GameManager.Instance.lstMap[0].lstMineShaft[j].input;
            time = time + c[j];

            for (int i = 0; i < l; i++)
            {
                //neu nha may dang trong qua trinh san xuat thi update lai thoi gian
                if (c[i] != T_invalid)
                    c[i] = c[i] - temp;
            }

            if (b[j] > 0)
            {
                //Neu so san pham o nha chua j ma lon hon cong suat nha may j thi day so san pham len bang cong suat
                if (b[j] >= GameManager.Instance.lstMap[0].lstMineShaft[j].totalCapacity)
                {
                    //set so san pham dang xu ly cho nha chua j
                    a[j] = GameManager.Instance.lstMap[0].lstMineShaft[j].totalCapacity;
                    //set lai so luong trong nha chua j
                    if (j != 0)
                        b[j] = b[j] - a[j];// truong hop j = 0 thi nha chua dau luon co so sp bang cong suat nhu da noi o tren, k can update
                }
                else
                {
                    //set so san pham dang xu ly cho nha chua j
                    a[j] = b[j];
                    if (j != 0)
                    {
                        //set lai so luong trong nha chua j
                        b[j] = 0;
                    }
                }
                //set lai thoi gian cho nha j
                c[j] = GameManager.Instance.lstMap[0].lstMineShaft[j].properties.miningTime + 2;
            }
            else
            {
                a[j] = 0;			//khong co gi de xu ly
                c[j] = T_invalid;	//khong co thoi gian xu ly
            }

            if (j == l - 1)
            {
                // do nothing
            }
            else
            {
                //cac san pham nay se duoc day len nha chua j+1, con thua thi se duoc ban
                //kiem tra nha chua j+1, neu con kha nang chua duoc nhieu hon A[j] thi tat ca se duoc don len nha chua j+1
                //k co gi ban ra
                if (GameManager.Instance.lstMap[0].lstMineShaft[j].store.capacity - b[j + 1] >= sp)
                {
                    b[j + 1] = b[j + 1] + sp;
                    sp = 0;
                }
                else
                {
                    sp = sp - (GameManager.Instance.lstMap[0].lstMineShaft[j].store.capacity - b[j + 1]);
                    b[j + 1] = GameManager.Instance.lstMap[0].lstMineShaft[j].store.capacity;
                }

                //nếu nhà j+1 dang k san xuat (C[j+1] = T_invalid) thi set de nha j+1 san xuat
                if (c[j + 1] == T_invalid)
                {
                    //set A va B
                    //Neu so sp trong nha chua j+1 nhieu hon cong suat nha j+1
                    if (b[j + 1] >= GameManager.Instance.lstMap[0].lstMineShaft[j + 1].totalCapacity)
                    {
                        a[j + 1] = GameManager.Instance.lstMap[0].lstMineShaft[j + 1].totalCapacity;
                        b[j + 1] = b[j + 1] - a[j + 1];
                    }
                    else
                    {
                        a[j + 1] = b[j + 1];
                        b[j + 1] = 0;
                    }
                    //Set C
                    c[j + 1] = GameManager.Instance.lstMap[0].lstMineShaft[j + 1].properties.miningTime + 2;
                }
            }

            //tinh tien
            _money = _money + sp * GameManager.Instance.lstMap[0].lstMineShaft[j].properties.unitPrice;

            counter++;
        }

        Debug.Log("money " + _money);
        Debug.Log("cap " + _money / (int)time);
        Debug.Log("time " + time);
        if (_money / (int)time < 5)
        { 
            return 5; 
        }
        else
        {
            return (_money / (int)time);
        }
    }
}
