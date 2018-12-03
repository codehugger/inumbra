using UnityEngine;
using System.Collections;

public class PlayerMovement : MonoBehaviour
{

    public float speed = 1;
    public float legTurnSpeed = 5;
    public float deadAngle = 45;

    private GameObject _body;
    private GameObject _legs;

    // Use this for initialization
    void Start()
    {
        _body = GameObject.FindGameObjectWithTag("Body");
        _legs = GameObject.FindGameObjectWithTag("Legs");
    }

    // Update is called once per frame
    void Update()
    {
        float in_x = Input.GetAxis("Horizontal");
        float in_y = Input.GetAxis("Vertical");
        float pos_x = transform.position.x + (in_x * speed * Time.deltaTime);
        float pos_y = transform.position.y + (in_y * speed * Time.deltaTime);
        Vector3 new_pos = new Vector3(pos_x, pos_y, 0f);
        transform.position = new_pos;

        Vector3 dir = Input.mousePosition - Camera.main.WorldToScreenPoint(transform.position);
        float look_angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        Quaternion new_rot = Quaternion.AngleAxis(look_angle, Vector3.forward);
        _body.transform.rotation = new_rot;

        Quaternion legs_rot = _legs.transform.rotation;
        float angle_diff = Quaternion.Angle(legs_rot, new_rot);

        if (angle_diff > deadAngle || System.Math.Abs(in_y) > 0 || System.Math.Abs(in_x) > 0)
        {
            _legs.transform.rotation = Quaternion.Lerp(legs_rot, new_rot, legTurnSpeed * Time.deltaTime);
        }
        // TODO: try to rotate legs towards input axis and not body
        //
    }
}
