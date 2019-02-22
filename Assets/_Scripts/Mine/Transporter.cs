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
    // Use this for initialization
    void Start()
    {
        this.speed = 1f;
        thisButton = GetComponent<Button>();
        thisButton.onClick.AddListener(() => Upgrade());
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void Transport()
    {
        StartCoroutine(TurnGo());
    }

    IEnumerator TurnGo()
    {
        while (this.gameObject.GetComponent<RectTransform>().transform.position.x > this.posEndTransport.position.x)
        {
            this.gameObject.GetComponent<RectTransform>().transform.position = Vector3.MoveTowards(this.gameObject.GetComponent<RectTransform>().transform.position, this.posEndTransport.position, Time.deltaTime * this.speed);
            yield return null;
        }
        yield return new WaitForSeconds(1f);
        mapParent.CompleteTransport();
        yield return new WaitForSeconds(0.5f);
        this.gameObject.GetComponent<RectTransform>().transform.localScale = new Vector3(1, 1, 1);
        StartCoroutine(TurnBack());
    }

    IEnumerator TurnBack()
    {
        while (this.gameObject.GetComponent<RectTransform>().transform.position.x < this.posBeginTransport.position.x)
        {
            this.gameObject.GetComponent<RectTransform>().transform.position = Vector3.MoveTowards(this.gameObject.GetComponent<RectTransform>().transform.position, this.posBeginTransport.position, Time.deltaTime * this.speed);
            yield return null;
        }
        yield return new WaitForEndOfFrame();
        this.gameObject.GetComponent<RectTransform>().transform.localScale = new Vector3(-1, 1, 1);
    }

    void Upgrade()
    {
        Transport();
    }
}
