using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class PlaneOnclick : MonoBehaviour
{
    public GameObject imgGive;
    public GameObject give;
    public Transform posIns;

    private bool isOnclick;

    public void BtnOnClick()
    {
        if (isOnclick)
            return;
        imgGive.SetActive(false);
        give.SetActive(true);
        give.transform.position = posIns.position;
        give.GetComponent<Rigidbody2D>().gravityScale = 0.15f;
        Invoke("OpenPopupAds", 0.5f);
        isOnclick = true;
    }

    public void OpenPopupAds()
    {
        Invoke("ActiveGive", 5f);
        give.GetComponent<Rigidbody2D>().gravityScale = 0f;
        give.SetActive(false);
        
    }

    void ActiveGive()
    {
        imgGive.SetActive(true);
        isOnclick = false;
    }
}
