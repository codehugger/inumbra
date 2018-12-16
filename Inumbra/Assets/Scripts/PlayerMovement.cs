using UnityEngine;
using System.Collections;

public class PlayerMovement : MonoBehaviour
{

    public float speed = 1;
    public float legTurnSpeed = 5;
    public float deadAngle = 45;
    public Animator animator;

    public Camera sceneCam;

    [Range(1.0f, 5.0f)]
    public float sprintMultiplier = 1.5f;

    private GameObject _body;
    private GameObject _legs;

    Vector3 mousePosition;

    // Use this for initialization
    void Start()
    {
        _body = GameObject.FindGameObjectWithTag("Body");
        _legs = GameObject.FindGameObjectWithTag("Legs");
    }

    // Update is called once per frame
    void Update()
    {
        bool sprintEnabled = Input.GetKey(KeyCode.LeftShift) || Input.GetAxis("XBox L2") > 0.2 || Input.GetButton("Fire2");
        var currentSpeed = speed;

        // Debug.Log(string.Format("Sprint Enabled: {0}", sprintEnabled));

        if (sprintEnabled) {
            currentSpeed *= sprintMultiplier;
        }

        float in_x = Input.GetAxis("Horizontal");
        float in_y = Input.GetAxis("Vertical");
        float pos_x = transform.position.x + (in_x * currentSpeed * Time.deltaTime);
        float pos_y = transform.position.y + (in_y * currentSpeed * Time.deltaTime);

        Vector3 new_pos = new Vector3(pos_x, pos_y, 0f);

        // Calculate length of movement for animation state change
        float length = Vector3.Magnitude(new_pos - transform.position);
        // Debug.Log(length);
        animator.SetFloat("Speed", length);
        animator.SetBool("Running", sprintEnabled);
        transform.position = new_pos;

        Vector3 dir;

        if (GameObject.FindGameObjectWithTag("PixelCanvas") != null) {
            Vector3 canvas_pos = GameObject.FindGameObjectWithTag("PixelCanvas").GetComponent<Canvas>().transform.position;
            dir = Input.mousePosition - canvas_pos;
        }
        else {
            dir = Input.mousePosition - Camera.main.WorldToScreenPoint(transform.position);
        }

        var angH = 0.0f;
        var angV = 0.0f;

        if (Input.GetJoystickNames().Length > 0 && Input.GetJoystickNames()[0].Contains("Xbox")) {
            angH = Input.GetAxis("XBoxRightH");
            angV = Input.GetAxis("XBoxRightV");
        } else {
            angH = Input.GetAxis("PS4RightH");
            angV = Input.GetAxis("PS4RightV");
        }

        float angleDiff;
        Quaternion legsRot = _legs.transform.rotation;
        Quaternion newRot = _body.transform.rotation;

        if (angH != 0 || angV != 0) {
            var ang = new Vector2(angH, angV);
            var angle = Vector2.SignedAngle(ang, Vector2.right);
            _body.transform.rotation = Quaternion.Euler(0, 0, angle);
            newRot = Quaternion.AngleAxis(angle, Vector3.forward);
        } else if (mousePosition != Input.mousePosition) {
            float look_angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
            newRot = Quaternion.AngleAxis(look_angle, Vector3.forward);
            _body.transform.rotation = newRot;
            angleDiff = Quaternion.Angle(legsRot, newRot);
        }

        angleDiff = Quaternion.Angle(legsRot, newRot);

        // Make the legs move with the rotation of the body
        if (angleDiff > deadAngle || System.Math.Abs(in_y) > 0 || System.Math.Abs(in_x) > 0)
        {
            _legs.transform.rotation = Quaternion.Lerp(legsRot, newRot, legTurnSpeed * Time.deltaTime);
        }

        // Store mouse position so we can detect if it is moving or not
        mousePosition = Input.mousePosition;
    }
}
