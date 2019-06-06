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
        LoadGameConfig();
    }
    private string gameConfig = "GameConfig";

    void Start()
    {
        
        /*Ads.Instance.RequestAd();
        Ads.Instance.RequestBanner();
        if (PlayerPrefs.GetInt(KeyPrefs.TUTORIAL_DONE) == 1)
        {
            Ads.Instance.ShowBanner();
        }
#if UNITY_ADS
        Advertisement.Initialize(GameConfig.Instance.ID_UnityAds_ios, true);
#endif*/
        //Purchaser.Instance.Init();
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
            GameConfig.Instance.AdmobID_Interstitial_android = objJson["AdmobID_Interstitial_android"];
            GameConfig.Instance.AdmobID_Interstitial_ios = objJson["AdmobID_Interstitial_ios"];
            GameConfig.Instance.AdmobID_Banner_ios = objJson["AdmobID_Banner_ios"];
            GameConfig.Instance.AdmobID_VideoReward_ios = objJson["AdmobID_VideoReward_ios"];
            GameConfig.Instance.kProductID50 = objJson["kProductID50"];
            GameConfig.Instance.kProductID300 = objJson["kProductID300"];
            GameConfig.Instance.kProductID5000 = objJson["kProductID5000"];
            GameConfig.Instance.link_ios = objJson["link_ios"];
            GameConfig.Instance.link_android = objJson["link_android"];
            GameConfig.Instance.string_Share = objJson["string_Share"];
            GameConfig.Instance.UFO_speed = objJson["UFO_speed"].AsFloat;
            GameConfig.Instance.UFO_time = objJson["UFO_time"].AsInt;
            GameConfig.Instance.UFO_rate_gold = new float[2];
            GameConfig.Instance.UFO_rate_gold[0] = objJson["UFO_rate_Gold"][0].AsFloat;
            GameConfig.Instance.UFO_rate_gold[1] = objJson["UFO_rate_Gold"][1].AsFloat;
            GameConfig.Instance.UFO_rate_coin = new float[2];
            GameConfig.Instance.UFO_rate_coin[0] = objJson["UFO_rate_Coin"][0].AsFloat;
            GameConfig.Instance.UFO_rate_coin[1] = objJson["UFO_rate_Coin"][1].AsFloat;

            for (int i = 0; i < objJson["introduction"].Count; i++)
            {
                GameConfig.Instance.lstIntroduction.Add(objJson["introduction"][i]);
            }
            for (int i = 0; i < objJson["Tutorial"].Count; i++)
            {
                GameConfig.Instance.lstTutorial.Add(objJson["Tutorial"][i]);
            }

            for (int i = 0; i < objJson["Map_Moon"].Count; i++)
            {
                for (int j = 0; j < objJson["Map_Moon"][i].Count; j++)
                {
                    pM = new GameConfig.PropertiesMap();
                    pM.ID = i;
                    pM.Name = objJson["Map_Moon"][i]["Name"];
                    pM.BuyAI = objJson["Map_Moon"][i]["BuyAI"].AsLong;
                    pM.Unlock_condition = objJson["Map_Moon"][i]["Unlock_condition"].AsInt;
                    pM.Unlock_time = objJson["Map_Moon"][i]["Unlock_time"].AsInt * 60;
                    pM.miningTime = objJson["Map_Moon"][i]["miningTime"].AsFloat;

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

                    pM.MoreMine_cost_1 = objJson["Map_Moon"][i]["MoreMine_cost_1"].AsInt;
                    pM.MoreMine_cost_2 = objJson["Map_Moon"][i]["MoreMine_cost_2"].AsInt;
                    pM.MoreMine_cost_3 = objJson["Map_Moon"][i]["MoreMine_cost_3"].AsFloat;

                    pM.Upgrade_time = new List<int>();
                    for (int k = 0; k < objJson["Map_Moon"][i]["Upgrade_time"].Count; k++)
                    {
                        pM.Upgrade_time.Add(objJson["Map_Moon"][i]["Upgrade_time"][k].AsInt * 60);
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

                    pM.Productivity = objJson["Map_Moon"][i]["Productivity"].AsInt;

                    pM.Upgrade_Special = new List<MineShaft.UpgradeSpecial>();
                    for (int k = 0; k < objJson["Map_Moon"][i]["Upgrade_Special"].Count; k++)
                    {
                        MineShaft.UpgradeSpecial temp = new MineShaft.UpgradeSpecial();
                        temp.name = objJson["Map_Moon"][i]["Upgrade_Special"][k]["name"];
                        temp.description = objJson["Map_Moon"][i]["Upgrade_Special"][k]["description"];
                        temp.price = objJson["Map_Moon"][i]["Upgrade_Special"][k]["price"].AsLong;
                        temp.time = objJson["Map_Moon"][i]["Upgrade_Special"][k]["time"].AsFloat * 60;
                        temp.coinMax = objJson["Map_Moon"][i]["Upgrade_Special"][k]["coinMax"].AsInt;
                        pM.Upgrade_Special.Add(temp);
                    }

                    pM.Unit_Price = objJson["Map_Moon"][i]["Unit_Price"].AsInt;

                    pM.Store_Cost_1 = objJson["Map_Moon"][i]["Store_Cost_1"].AsInt;
                    pM.Store_Cost_2 = objJson["Map_Moon"][i]["Store_Cost_2"].AsFloat;
                    pM.Store_Cost_3 = objJson["Map_Moon"][i]["Store_Cost_3"].AsInt;
                    pM.Store_Capacity_1 = objJson["Map_Moon"][i]["Store_Capacity_1"].AsInt;
                    pM.Store_Capacity_2 = objJson["Map_Moon"][i]["Store_Capacity_2"].AsInt;
                }
                GameConfig.Instance.lstPropertiesMap.Add(pM);
            }
            GameConfig.Instance.TimeTransport = objJson["Transporter"]["TimeTransport"].AsInt;
            GameConfig.Instance.NameTransport = objJson["Transporter"]["NameTransport"];
            GameConfig.Instance.Trans_Cost_1 = objJson["Transporter"]["Cost_1"].AsInt;
            GameConfig.Instance.Trans_Capacity_1 = objJson["Transporter"]["Capacity_1"].AsInt;

            GameConfig.Instance.Trans_Cost_2 = new float[objJson["Transporter"]["Cost_2"].Count];
            for (int j = 0; j < GameConfig.Instance.Trans_Cost_2.Length; j++)
            {
                GameConfig.Instance.Trans_Cost_2[j] = objJson["Transporter"]["Cost_2"][j].AsFloat;
            }

            GameConfig.Instance.Trans_Capacity_2 = new int[objJson["Transporter"]["Capacity_2"].Count];
            for (int j = 0; j < GameConfig.Instance.Trans_Capacity_2.Length; j++)
            {
                GameConfig.Instance.Trans_Capacity_2[j] = objJson["Transporter"]["Capacity_2"][j].AsInt;
            }

            for (int i = 0; i < objJson["Transporter"]["Cost"].Count; i++)
            {
                GameConfig.Instance.lstCostTransporter.Add(objJson["Transporter"]["Cost"][i].AsLong);
            }
            GameManager.Instance.UFO.speed = GameConfig.Instance.UFO_speed;


            for (int j = 0; j < objJson["Spin"]["Gold"].Count; j++)
            {
                GameConfig.Instance.lstRewardSpin_Gold.Add(objJson["Spin"]["Gold"][j].AsFloat);
            }
            for (int j = 0; j < objJson["Spin"]["TimeSkip"].Count; j++)
            {
                GameConfig.Instance.lstRewardSpin_Time.Add(objJson["Spin"]["TimeSkip"][j].AsInt);
            }
            for (int j = 0; j < objJson["Spin"]["Coin"].Count; j++)
            {
                GameConfig.Instance.lstRewardSpin_Coin.Add(objJson["Spin"]["Coin"][j].AsInt);
            }

            GameConfig.Instance.listCoinPacks = new List<CoinPack>();
            for (int j = 0; j < objJson["coinPacks"].Count; j++)
            {
                var coin_pack = new CoinPack();
                coin_pack.productID = objJson["coinPacks"][j]["productID"];
                coin_pack.price = objJson["coinPacks"][j]["price"];
                coin_pack.value = objJson["coinPacks"][j]["value"].AsInt;
                GameConfig.Instance.listCoinPacks.Add(coin_pack);
            }

            GameConfig.Instance.listGoldPacks = new List<GoldPack>();
            for (int j = 0; j < objJson["goldPacks"].Count; j++)
            {
                var gold_pack = new GoldPack();
                gold_pack.price = objJson["goldPacks"][j]["price"].AsInt;
                gold_pack.value = objJson["goldPacks"][j]["value"].AsLong;
                GameConfig.Instance.listGoldPacks.Add(gold_pack);
            }
        }
    }

    string loadJson(string _nameJson)
    {
        TextAsset _text = Resources.Load(_nameJson) as TextAsset;
        return _text.text;
    }
}
