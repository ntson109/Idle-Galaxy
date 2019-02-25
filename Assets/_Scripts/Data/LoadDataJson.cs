using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_ADS
using UnityEngine.Advertisements; // only compile Ads code on supported platforms
#endif

public class LoadDataJson : MonoBehaviour
{
    public static LoadDataJson Instance;
    public GameObject gameManager;
    public bool isReset;
    void Awake()
    {
        if (Instance != null)
        {
            return;
        }
        Instance = this;
        if (isReset)
        {
            PlayerPrefs.DeleteAll();
        }
    }
    private string gameConfig = "GameConfig";

    void Start()
    {
        LoadGameConfig();
        Ads.Instance.RequestAd();
        Ads.Instance.RequestBanner();
        if (PlayerPrefs.GetInt(KeyPrefs.IS_CONTINUE) == 1)
        {
            Ads.Instance.ShowBanner();
        }
#if UNITY_ADS
        Advertisement.Initialize(GameConfig.Instance.ID_UnityAds_ios, true);
#endif
        Purchaser.Instance.Init();
    }

    GameConfig.PropertiesMap pM;
    public void LoadGameConfig()
    {
        var objJson = SimpleJSON_DatDz.JSON.Parse(loadJson(gameConfig));
        //Debug.Log(objJson);
        Debug.Log("<color=yellow>Done: </color>LoadGameConfig !");
        if (objJson != null)
        {
            GameConfig.Instance.GoldStart = objJson["GoldStart"].AsLong;
            GameConfig.Instance.CoinStart = objJson["CoinStart"].AsLong;
            GameConfig.Instance.ID_UnityAds_ios = objJson["ID_UnityAds_ios"];
            GameConfig.Instance.ID_Inter_android = objJson["ID_Inter_android"];
            GameConfig.Instance.ID_Inter_ios = objJson["ID_Inter_ios"];
            GameConfig.Instance.ID_Banner_ios = objJson["ID_Banner_ios"];
            GameConfig.Instance.kProductID50 = objJson["kProductID50"];
            GameConfig.Instance.kProductID300 = objJson["kProductID300"];
            GameConfig.Instance.kProductID5000 = objJson["kProductID5000"];
            GameConfig.Instance.link_ios = objJson["link_ios"];
            GameConfig.Instance.link_android = objJson["link_android"];
            GameConfig.Instance.string_Share = objJson["string_Share"];
            for (int i = 0; i < objJson["introduction"].Count; i++)
            {
                GameConfig.Instance.lstIntroduction.Add(objJson["introduction"][i]);
            }
            for (int i = 0; i < objJson["Map_Moon"].Count; i++)
            {
                for (int j = 0; j < objJson["Map_Moon"][i].Count; j++)
                {
                    pM = new GameConfig.PropertiesMap();
                    pM.ID = i;
                    pM.Unlock_condition = objJson["Map_Moon"][i]["Unlock_condition"].AsInt;
                    pM.Unlock_time = objJson["Map_Moon"][i]["Unlock_time"].AsInt * 60;
                    pM.Unlock_cost = new long[2];
                    for (int k = 0; k < objJson["Map_Moon"][i]["Unlock_cost"].Count; k++)
                    {
                        pM.Unlock_cost[k] = objJson["Map_Moon"][i]["Unlock_cost"][k].AsLong;
                    }
                    pM.Unlock_reward = new List<long>();
                    for (int k = 0; k < objJson["Map_Moon"][i]["Unlock_reward"].Count; k++)
                    {
                        pM.Unlock_reward.Add(objJson["Map_Moon"][i]["Unlock_reward"][k].AsLong);
                    }
                    pM.BuyMine_cost = objJson["Map_Moon"][i]["BuyMine_cost"].AsInt;
                    pM.Upgrade_time = new List<int>();
                    for (int k = 0; k < objJson["Map_Moon"][i]["Upgrade_time"].Count; k++)
                    {
                        pM.Upgrade_time.Add(objJson["Map_Moon"][i]["Upgrade_time"][k].AsInt);
                    }
                    pM.Upgrade_condition = new List<int>();
                    for (int k = 0; k < objJson["Map_Moon"][i]["Upgrade_condition"].Count; k++)
                    {
                        pM.Upgrade_condition.Add(objJson["Map_Moon"][i]["Upgrade_condition"][k].AsInt);
                    }
                    pM.Upgrade_cost = new List<long>();
                    for (int k = 0; k < objJson["Map_Moon"][i]["Upgrade_cost"].Count; k++)
                    {
                        pM.Upgrade_cost.Add(objJson["Map_Moon"][i]["Upgrade_cost"][k].AsLong);
                    }
                    pM.Productivity = new List<int>();
                    for (int k = 0; k < objJson["Map_Moon"][i]["Productivity"].Count; k++)
                    {
                        pM.Productivity.Add(objJson["Map_Moon"][i]["Productivity"][k].AsInt);
                    }
                    pM.Unit_Price = objJson["Map_Moon"][i]["Unit_Price"].AsInt;
                }
                GameConfig.Instance.lstPropertiesMap.Add(pM);
            }
            for (int i = 0; i < objJson["CapTransporter"].Count; i++)
            {
                GameConfig.Instance.lstCapTransporter.Add(objJson["CapTransporter"][i].AsInt);
            }
            GameConfig.Instance.SpeedTransporter = objJson["SpeedTransporter"].AsFloat;
        }
    }

    string loadJson(string _nameJson)
    {
        TextAsset _text = Resources.Load(_nameJson) as TextAsset;
        return _text.text;
    }
}
