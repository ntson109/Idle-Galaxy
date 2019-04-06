using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Unlock : MonoBehaviour {

    public UnityEvent method;

    public void CallEvent()
    {
        this.GetComponent<Animator>().enabled = false;
        method.Invoke();
    }
}
