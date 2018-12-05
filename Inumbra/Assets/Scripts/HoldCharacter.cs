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
        roof.SetActive(false);
       // roof.GetComponent<Renderer>().enabled = false;
        
       // roof.GetComponent<LightObstacleGenerator>().enabled = false;
        }
        
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player")
        {
        other.gameObject.transform.SetParent(null);

        //show roof
        roof.SetActive(true);
        }
    }
}
