using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MEASURINGTAPE : MonoBehaviour
{
    public GameObject plane;
    public GameObject box;
    // make an array to store plane1 plane 2...
    public GameObject[] planes;
    float maxDistance = 73f;
    float minValue = 0f;
    float maxValue = 73f;
    int minIndex = 0;
    int maxIndex = 9;
    void Start()
    {

    }


    void Update()
    {

        // Debug.Log("Distance");
        // Debug.Log(Distance(plane, box));
        // Debug.Log("Global Distance");
        // Debug.Log(GlobalDistance(plane, box));

        for (int i = 0; i < planes.Length; i++)
        {
            // print(planes[0].transform.position);

            // when the planes global posiition is ahead of the box then make the plane active

            // when global position of planes x y and z 
            // if (planes[i].transform.position.x > box.transform.position.x)
            int mappedIndex = MapValueToIndex((Distance(box, plane)), minValue, maxValue, minIndex, maxIndex);

            if (GlobalDistance(planes[i], box) < 0.1f || i < mappedIndex)
            {
                planes[i].SetActive(true);
            }
            else
            {
                planes[i].SetActive(false);
            }

        }
    }
    public float Distance(GameObject plane, GameObject box)
    {
        return Vector3.Distance(plane.transform.localPosition, box.transform.localPosition);
    }

    public float GlobalDistance(GameObject plane, GameObject box)
    {
        return Vector3.Distance(plane.transform.position, box.transform.position);
    }
    public int MapValueToIndex(float value, float minValue, float maxValue, int minIndex, int maxIndex)
    {
        // Calculate the normalized value within the range [0, 1]
        float normalizedValue = Mathf.Clamp01((value - minValue) / (maxValue - minValue));

        // Map the normalized value to the desired index range
        int mappedIndex = Mathf.RoundToInt(normalizedValue * (maxIndex - minIndex)) + minIndex;

        return mappedIndex;
    }
}
