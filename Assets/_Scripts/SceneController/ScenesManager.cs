using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ScenesManager : MonoBehaviour
{

    public enum TypeScene
    {
        Home, Main
    }

    [System.Serializable]
    public struct Scenes
    {
        public string name;
        public TypeScene type;
        public GameObject objects;
    }

    public static ScenesManager Instance;

    void Awake()
    {
        Instance = this;
    }

    public Scenes[] secenes;
    public bool isNextScene;

    public void GoToScene(TypeScene typeScene, UnityAction actionLoadScenesDone = null, UnityAction actionLoadScenesLast = null)
    {
        StartCoroutine(GoToSceneHandel(typeScene, actionLoadScenesDone, actionLoadScenesLast));
    }

    private IEnumerator GoToSceneHandel(TypeScene typeScene, UnityAction actionLoadScenesDone = null, UnityAction actionLoadScenesLast = null)
    {
        Fade.Instance.StartFade();
        yield return new WaitUntil(() => Fade.Instance.state == Fade.FadeState.FadeInDone);

        secenes[0].objects.SetActive(false);
        if (actionLoadScenesDone != null)
            actionLoadScenesDone();
        yield return new WaitForSeconds(1.5f);
        //Hazz
        yield return new WaitUntil(() => isNextScene = true);
        if (actionLoadScenesLast != null)
            actionLoadScenesLast();
        Fade.Instance.EndFade();
    }

}
