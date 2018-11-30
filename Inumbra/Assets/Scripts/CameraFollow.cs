using UnityEngine;
using System.Collections;

public class CameraFollow : MonoBehaviour
{
    private Transform playerPos;

    // Use this for initialization
    void Start()
    {
        playerPos = GameObject.Find("Player").transform;
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = new Vector3(playerPos.position.x, playerPos.position.y, transform.position.z);
    }
}
