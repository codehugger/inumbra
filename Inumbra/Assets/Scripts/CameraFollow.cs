using UnityEngine;
using System.Collections;

public class CameraFollow : MonoBehaviour
{
    public Camera assignedCamera;

    // Use this for initialization
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        assignedCamera.transform.position = transform.position;
    }
}
