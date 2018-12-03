using UnityEngine;
using System.Collections;

public class HoldCharacter : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        other.gameObject.transform.SetParent(transform);
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        other.gameObject.transform.SetParent(null);
    }
}
