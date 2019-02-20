using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Network : MonoBehaviour {

    public void CheckInternetConnection(Action hasConnected)
    {
        base.StartCoroutine(this.InternetChecking(hasConnected));
    }

    private IEnumerator InternetChecking(Action hasConnected)
    {
        WWW www = new WWW("https://www.google.com");
        yield return www;
        if (www.error != null)
        {
            Debug.Log("No internet connection.");
            //Notify.instance.ShowNotification("No internet connection.");
            //Sound.instance.Play("Notification");
        }
        else
        {
            hasConnected();
        }
        yield break;
    }
}
