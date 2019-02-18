using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    #region === GAME CONFIG ===
    [Header("GAME CONFIG")]
    public float TimeAd;
    public string ID_UnityAds_ios;
    public string ID_Inter_android;
    public string ID_Inter_ios;
    public string ID_Banner_ios;
    public string link_ios;
    public string link_android;
    public string string_Share;
    public string kProductID50 = "consumable";
    public string kProductID300 = "consumable";
    public string kProductID5000 = "consumable";
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

