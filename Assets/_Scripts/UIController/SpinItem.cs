using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpinItem : MonoBehaviour
{
    // UIs.
    public Image Border, RewardIcon;

    public void ShowBorder(bool show)
    {
        this.Border.gameObject.SetActive(show);
    }
}
