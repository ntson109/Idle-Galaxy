using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Transporter : MonoBehaviour
{
    public long capacity;
    public float speed;
    public RectTransform posBeginTransport;
    public RectTransform posEndTransport;
    private Button thisButton;
    public Map mapParent;
    public bool isTransporting;

    void Start()
    {
        //this.speed = 1f;
        thisButton = GetComponent<Button>();
        thisButton.onClick.AddListener(() => ShowUpgrade());
    }

    void Update()
    {

    }

    public void Transport()
    {
        if (!isTransporting)
        {
            StartCoroutine(TurnGo());
            isTransporting = true;
        }
    }

    IEnumerator TurnGo()
    {
        while (this.gameObject.GetComponent<RectTransform>().transform.position.x > this.posEndTransport.position.x)
        {
            //this.gameObject.GetComponent<RectTransform>().transform.position = Vector3.MoveTowards(this.gameObject.GetComponent<RectTransform>().transform.position, this.posEndTransport.position, Time.deltaTime * this.speed);
            this.gameObject.GetComponent<RectTransform>().transform.Translate(Vector3.left * Time.deltaTime * this.speed);
            yield return null;
        }
        yield return new WaitForSeconds(0.75f);
        mapParent.CompleteTransport();
        yield return new WaitForSeconds(0.5f);
        this.gameObject.GetComponent<RectTransform>().transform.localScale = new Vector3(1, 1, 1);
        StartCoroutine(TurnBack());
    }

    IEnumerator TurnBack()
    {
        while (this.gameObject.GetComponent<RectTransform>().transform.position.x < this.posBeginTransport.position.x)
        {
            //this.gameObject.GetComponent<RectTransform>().transform.position = Vector3.MoveTowards(this.gameObject.GetComponent<RectTransform>().transform.position, this.posBeginTransport.position, Time.deltaTime * this.speed);
            this.gameObject.GetComponent<RectTransform>().transform.Translate(Vector3.right * Time.deltaTime * this.speed);
            yield return null;
        }
        yield return new WaitForEndOfFrame();
        this.gameObject.GetComponent<RectTransform>().transform.localScale = new Vector3(-1, 1, 1);
        yield return new WaitForSeconds(0.5f);
        isTransporting = false;
    }

    void ShowUpgrade()
    {
        //Transport();
    }
}
