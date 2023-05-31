using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlashMaterial : MonoBehaviour
{
    // Start is called before the first frame update
    private Color32 color1;//= new Color(255, 255, 255);
    private Color32 color2;//= new Color(251f, 197f, 49f);
    private Color32 originalColor;
    Coroutine lastRoutine = null;
    [SerializeField] private float speed = 10;
    [SerializeField] private bool diasableMeshRendererNotFlashing = false;
    private void Start()
    {
        if (diasableMeshRendererNotFlashing)
        {
            GetRenderer().enabled = false;
        }
        color1 = new Color32((byte)255, (byte)255, (byte)255, (byte)255);
        color2 = new Color32((byte)251, (byte)197, (byte)49, (byte)255);
        originalColor = GetRenderer().material.color;
    }
    IEnumerator FlashObject()
    {
        while (true)
        {
            GetRenderer().material.color = Color.Lerp(color1, color2, Mathf.Sin(speed * Time.time));
            // GetRenderer().material.SetColor("_BaseColor", Color.Lerp(color1, color2, Mathf.Sin(speed * Time.time)));
            yield return null;
        }
        // this.GetRenderer().material.color = color2;
    }
    public void FlashOn()
    {
        if (diasableMeshRendererNotFlashing)
        {
            GetRenderer().enabled = true;
        }
        lastRoutine = StartCoroutine(FlashObject());

    }
    public void FlashOff()
    {
        if (diasableMeshRendererNotFlashing)
        {
            GetRenderer().enabled = false;
        }
        StopCoroutine(lastRoutine);
        GetRenderer().material.color = originalColor;
    }

    private MeshRenderer GetRenderer()
    {
        if (this.GetComponent<MeshRenderer>() != null)
        {
            return this.GetComponent<MeshRenderer>();
        }
        else
        {
            return this.GetComponentInChildren<MeshRenderer>();
        }
    }

}
