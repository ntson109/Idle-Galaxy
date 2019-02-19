using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class HandMove : MonoBehaviour
{

    public Animator anim;
    public Transform hand;
    public Transform tfBegin;
    public Transform tfEnd;
    public bool isPath;
    public Transform[] tfWay;
    private Vector3[] v3Way;

    private bool isAction;
    private float timeAction;
    private bool isOne;

    public void Start()
    {
        if (isPath)
        {
            v3Way = new Vector3[tfWay.Length];
            for (int i = 0; i < tfWay.Length; i++)
            {
                v3Way[i] = tfWay[i].position;
            }
        }
    }

    public void OnEnable()
    {
        if (!isPath)
        {
            if (tfBegin != null)
            {
                hand.position = tfBegin.position;
            }
        }
        else
        {
            isOne = true;
            hand.position = tfWay[0].position;
        }
    }

    public void Update()
    {
        if (!isPath)
        {
            if (tfBegin != null && tfEnd != null)
            {
                if (isAction)
                {
                    hand.position = Vector3.MoveTowards(hand.position, tfEnd.position, 1.5f * Time.deltaTime);
                    if (hand.position == tfEnd.position)
                    {
                        hand.position = tfBegin.position;
                        anim.enabled = true;
                        isAction = false;
                        timeAction = 0;
                    }
                }
                else
                {
                    timeAction += Time.deltaTime;
                    if (timeAction >= (1f/3f))
                    {
                        anim.Rebind();
                        anim.enabled = false;
                        isAction = true;
                        timeAction = 0;
                    }
                }
            }
        }
        else
        {
            if (isOne)
            {
                isOne = false;
                hand.DOPath(v3Way, 3f).OnComplete(() =>
                {
                    hand.position = tfWay[0].position;
                    isOne = true;
                });
            }
        }
    }
}
