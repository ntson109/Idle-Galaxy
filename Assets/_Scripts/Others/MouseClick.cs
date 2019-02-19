using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseClick : MonoBehaviour
{
    private bool touch;

    private float time;

    private List<GameObject> pool;

    [SerializeField]
    private float touchTime;

    [SerializeField]
    private GameObject effect;

    private void Start()
    {
        this.pool = new List<GameObject>();
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            this.touch = true;
        }
        if (this.touch)
        {
            this.time += Time.deltaTime;
        }
        if (Input.GetMouseButtonUp(0))
        {
            if (this.time < this.touchTime)
            {
                this.Spawn();
            }
            this.time = 0f;
            this.touch = false;
        }
    }

    private void Spawn()
    {
        Vector3 mousePosition = Input.mousePosition;
        mousePosition.z = 10f;
        Vector3 vector = Camera.main.ScreenToWorldPoint(mousePosition);
        vector.z = 0f;
        GameObject gameObject = this.pool.Find((GameObject target) => !target.activeInHierarchy);
        if (gameObject != null)
        {
            gameObject.transform.position = vector;
            gameObject.SetActive(true);
        }
        else
        {
            gameObject = Object.Instantiate<GameObject>(this.effect, vector, Quaternion.identity);
            this.pool.Add(gameObject);
        }
    }
}
