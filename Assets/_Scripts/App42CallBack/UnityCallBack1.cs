using com.shephertz.app42.paas.sdk.csharp;
using com.shephertz.app42.paas.sdk.csharp.storage;
using System;
using UnityEngine;

public class UnityCallBack1 : App42CallBack
{
    public void OnSuccess(object response)
    {
        Debug.Log(response);
    }
    public void OnException(Exception e)
    {
        StorageService storageService = App42API.BuildStorageService();
        storageService.InsertJSONDocument("Db", "Data", UnityEngine.JsonUtility.ToJson(new SaveGold(GameConfig.id, PlayerPrefs.GetInt("Gold", 10))), new UnityCallBack2());
    }
}
