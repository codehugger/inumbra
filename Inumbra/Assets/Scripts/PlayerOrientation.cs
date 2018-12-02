using UnityEngine;
using System.Collections;

public class PlayerOrientation : MonoBehaviour
{

    public float dead_angle = 45;
    public float leg_turn_speed = 5;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        Vector3 dir = Input.mousePosition - Camera.main.WorldToScreenPoint(transform.position);
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        Quaternion new_rot = Quaternion.AngleAxis(angle, Vector3.forward);
        GameObject legs = GameObject.FindGameObjectWithTag("Legs");
        Quaternion legs_rot = legs.transform.rotation;
        float angle_diff = Quaternion.Angle(legs_rot, new_rot);
        if (angle_diff > dead_angle)
        {
            legs.transform.rotation = Quaternion.Lerp(legs_rot, new_rot, leg_turn_speed * Time.deltaTime);

        }
        transform.rotation = new_rot;
    }
}
