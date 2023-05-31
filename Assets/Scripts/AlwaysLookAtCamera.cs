using UnityEngine;

public class AlwaysLookAtCamera : MonoBehaviour
{
    // Start is called before the first frame update
    private Camera _camera;

    void Start()
    {
        _camera = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {

        transform.LookAt(_camera.transform.position);
        transform.Rotate(0, 180f, 0);
    }
}
