using UnityEngine;
using System.Collections;

public class HoldCharacter : MonoBehaviour
{
    private void OnCollisionEnter2D(Collision2D collision)
    {
        collision.collider.transform.SetParent(transform);
        Debug.Log("We have collision enter");
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        collision.collider.transform.SetParent(null);
        Debug.Log("We have collision exit");
    }
}
