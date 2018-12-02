using UnityEngine;
using System.Collections;

public class PlayerMovement : MonoBehaviour
{

    public float speed = 1;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        //Debug.Log(Input.GetAxis("Horizontal") + " and " + Input.GetAxis("Vertical"));
        float in_x = Input.GetAxis("Horizontal");
        float in_y = Input.GetAxis("Vertical");
        float pos_x = transform.position.x + (in_x * speed * Time.deltaTime);
        float pos_y = transform.position.y + (in_y * speed * Time.deltaTime);
        Vector3 new_pos = new Vector3(pos_x, pos_y, 0f);
        Vector3 move_direction = new_pos - transform.position;
        transform.position = new_pos;
        if (System.Math.Abs(in_y) > 0.3 || System.Math.Abs(in_x) > 0.3)
        {
            float angle = Mathf.Atan2(move_direction.y, move_direction.x) * Mathf.Rad2Deg;
            GameObject.FindGameObjectWithTag("Legs").transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        }
    }
}
