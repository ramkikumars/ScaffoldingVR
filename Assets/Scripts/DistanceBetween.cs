using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using TMPro.Examples;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class DistanceBetween : MonoBehaviour
{
    [SerializeField] private Transform minDistance;
    [SerializeField] private GameObject additionalTape;

    [FormerlySerializedAs("currentCorrectPosition")]
    [SerializeField]
    private Transform currentLastPosition;

    [SerializeField] private Texture[] texture = new Texture[9];

    private GameObject[] _createdObjects = new GameObject[10];

    private int tapePosition = 0;
    private int texturePosition = 0;

    private float currentMaxDistance = 0.10f;

    private void Awake()
    {
        _createdObjects[tapePosition++] = currentLastPosition.gameObject;
        // Debug.Log("tapePosition: " + tapePosition);
    }

    // Update is called once per frame
    void Update()
    {
        float dist = Vector3.Distance(minDistance.position, transform.position);
        // Debug.Log("minDis: " + dist);

        if (dist > 1)
        {
            transform.position =
                new Vector3(transform.position.x, transform.position.y, transform.position.z - (dist - 1));
        }
        else if (dist > currentMaxDistance)
        {
            currentMaxDistance += 0.1f;
            // Debug.Log("Increased with dist: " + dist);
            // Debug.Log("New Max distance: " + currentMaxDistance);

            var newTape = Instantiate(additionalTape, currentLastPosition.position, currentLastPosition.rotation,
                transform);
            var pos = newTape.transform.position;
            newTape.transform.position = pos - newTape.transform.right * 0.1f;

            var meshRenderer = newTape.GetComponent<MeshRenderer>();
            meshRenderer.material.mainTexture = texture[texturePosition++];
            _createdObjects[tapePosition] = newTape;
            // Debug.Log("_createdObjects[tapePosition] : " + _createdObjects[tapePosition]);
            tapePosition++;
            // Debug.Log("tapePosition: " + tapePosition);

            currentLastPosition = newTape.transform;
        }
        else if (dist < 0.02)
        {
            transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.z + 0.02f);
        }
        else if (dist < currentMaxDistance - 0.1)
        {
            currentMaxDistance -= 0.1f;
            Debug.Log("Decreased with dist: " + dist);
            Debug.Log("New Max distance: " + currentMaxDistance);
            Destroy(currentLastPosition.gameObject);
            Debug.Log("tapePosition: " + (--tapePosition));
            texturePosition--;
            currentLastPosition = _createdObjects[tapePosition - 1].transform;
        }
    }
}