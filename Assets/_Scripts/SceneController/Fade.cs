using UnityEngine;
using DG.Tweening;

public class Fade : MonoBehaviour {

    public static Fade Instance;

    public enum FadeState
    {
        None, FadeInDone, FadeOutDone
    }

    public FadeState state;

    void Awake()
    {
        Instance = this;
    }

    public void StartFade()
    {
        transform.localScale = new Vector3(1f, 1f, 1f);
        FadeInDone();
    }

    public void EndFade()
    {
        transform.localScale = new Vector3(0f, 0f, 0f);
        state = FadeState.None;
    }

    public void FadeInDone()
    {
        state = FadeState.FadeInDone;
    }

    public void FadeOutDone()
    {
        state = FadeState.FadeOutDone;
    }

}
