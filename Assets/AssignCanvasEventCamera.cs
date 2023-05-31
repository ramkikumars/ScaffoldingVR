using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AssignCanvasEventCamera : MonoBehaviour
{
    // Start is called before the first frame update
    private Canvas canvas;
    private Camera _camera;
    private CanvasGroup canvasGroup;
    public bool fade = false;
    public float duration = 0;
    float time = 0;
    void Start()
    {
        canvas = GetComponent<Canvas>();
        _camera = Camera.main;
        canvasGroup = GetComponent<CanvasGroup>();
        if (canvas && canvas.worldCamera == null)
        {
            canvas.worldCamera = Camera.main;
        }
    }
    void Update()
    {
        this.transform.LookAt(_camera.transform.position);
        this.transform.Rotate(0, 180f, 0);
        if (fade)
        {
            FadeOut(duration);
        }
    }

    void FadeOut(float duration)
    {

        if (time <= duration)
        {

            canvasGroup.alpha = Mathf.Lerp(1f, 0f, time / duration);
            time += Time.deltaTime;
        }
    }
}
