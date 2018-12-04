using UnityEngine;
using System.Collections;

public class HoldCharacter : MonoBehaviour
{
    
    public GameObject roof;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player")
        {
        other.gameObject.transform.SetParent(transform);

        //hide roof
        roof.GetComponent<Renderer>().enabled = false;
        }
        
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player")
        {
        other.gameObject.transform.SetParent(null);

        //show roof
        roof.GetComponent<Renderer>().enabled = true;
        
        }
    }
}
