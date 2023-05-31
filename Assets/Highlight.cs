using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class Highlight : MonoBehaviour
{
    // Start is called before the first frame update
    public Color color1, color2;
    public float duration = 5f;
    private bool flashOn;
    public float speed = 1;
    void Start()
    {
        StartCoroutine(FlashObject());
    }

    // Update is called once per frame
    void Update()
    {
        // this.GetComponent<Renderer>().material.color = highlightColor;

    }

    IEnumerator FlashObject()
    {
        while (true)
        {
            this.GetComponent<Renderer>().material.color = Color.Lerp(color1, color2, Mathf.Sin(speed * Time.time));
            yield return null;
        }
        // this.GetComponent<Renderer>().material.color = color2;
    }
}
