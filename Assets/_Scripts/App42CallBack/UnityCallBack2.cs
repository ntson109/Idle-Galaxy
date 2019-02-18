using com.shephertz.app42.paas.sdk.csharp;
using com.shephertz.app42.paas.sdk.csharp.storage;
using System;
using UnityEngine;

public class UnityCallBack2 : App42CallBack
{
    public void OnSuccess(object response)
    {
        Debug.Log(response);
        //if (UIManager.Instance.panelLoadingIAP != null)
        //    UIManager.Instance.panelLoadingIAP.SetActive(false);
    }
    public void OnException(Exception e)
    {
      
    }
}
