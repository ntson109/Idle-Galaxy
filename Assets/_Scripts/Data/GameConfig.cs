using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using com.shephertz.app42.paas.sdk.csharp;
using com.shephertz.app42.paas.sdk.csharp.storage;

[RequireComponent(typeof(LoadDataJson))]
public class GameConfig : MonoBehaviour {
    public static GameConfig Instance;
    public static string id = "";
    void Awake()
    {
        if (Instance != null)
        {
            return;
        }
        Instance = this;
    }

    [System.Serializable]
    public struct PropertiesMap
    {       
        public int ID;
        public string Name;
        //public long BuyAI;
        public int Unlock_condition;
        public int Unlock_time;
        public List<int> miningTime;
        public long[] Unlock_cost;
        public List<long> Unlock_reward;
        public int MoreMine_cost_2;
        public int MoreMine_cost_1;
        public float MoreMine_cost_3;
        public List<int> Upgrade_time;
        public List<int> Upgrade_condition;
        public List<int> Productivity;
        public List<long> Upgrade_cost;
        public List<double> Unit_Price;
        public List<MineShaft.UpgradeSpecial> Upgrade_Special;
    }

    #region === GAME CONFIG ===
    [Header("GAME CONFIG")]
    public List<PropertiesMap> lstPropertiesMap = new List<PropertiesMap>();   
    public long GoldStart;
    public long CoinStart;
    public float TimeAd;
    public string ID_UnityAds_ios;
    public string ID_Inter_android;
    public string ID_Inter_ios;
    public string ID_Banner_ios;
    public string link_ios;
    public string link_android;
    public string string_Share;
    public List<string> lstIntroduction = new List<string>();
    public string kProductID50 = "consumable";
    public string kProductID300 = "consumable";
    public string kProductID5000 = "consumable";
    public List<long> lstCostTransporter = new List<long>();
    public string NameTransport;
    public int TimeTransport;
    public int Capacity_1;
    public float Capacity_2;
    public float Capacity_3;
    public float UFO_speed;
    public int UFO_time;
    public float[] UFO_rate_gold;
    public float[] UFO_rate_coin;
    public List<float> lstRewardSpin_Gold = new List<float>();
    public List<int> lstRewardSpin_Time = new List<int>();
    public List<int> lstRewardSpin_Coin = new List<int>();
    #endregion

    #region === APP42 ===
    string app42_apiKey = "41b8289bb02efae4f37f1c9d891b09bb43f6f801bdbbf17a557bc4598ddf836b";
    string app42_secretKey = "35d9a321b8d4cfc3b375b5f212f15ffab98bb2b53e4b9da20d22881fc01a0efa";
    void Start()
    {
        //if (id == "")
        //{
        //    App42API.Initialize(app42_apiKey, app42_secretKey);
        //    //GameCenterPlatform.ShowDefaultAchievementCompletionBanner(true);
        //    Social.localUser.Authenticate(success =>
        //    {
        //        if (success)
        //        {
        //            id = Social.localUser.id;
        //            StorageService storageService = App42API.BuildStorageService();
        //            storageService.FindDocumentByKeyValue("Db", "Data", "id", id, new UnityCallBack1());
        //        }
        //        else
        //            Debug.Log("Failed to authenticate");
        //    });
        //}
    }

    public void RestoreProgess()
    {
        StorageService storageService = App42API.BuildStorageService();
        storageService.FindDocumentByKeyValue("Db", "Data", "id", GameConfig.id, new UnityCallBack3());
        //UIManager.Instance.panelSetting.SetActive(false);
        //UIManager.Instance.PushGiveGold("Waiting ...");
    }
    #endregion
}

public class SaveGold
{
    public string id;
    public int gold;
    public SaveGold(string id, int gold)
    {
        this.id = id;
        this.gold = gold;
    }
}

