using UnityEngine;
using System.Collections;

public class HoldCharacter : MonoBehaviour
{
    private void OnCollisionEnter2D(Collision2D collision)
    {
        collision.transform.parent = gameObject.transform;
        Debug.Log("We have collision enter");
    }

    private void OnCollisionStay(Collision collision)
    {
        collision.transform.parent = gameObject.transform;
    }

    private void OnCollisionExit(Collision collision)
    {
        collision.transform.parent = null;
        Debug.Log("We have collision exit");
    }
}
