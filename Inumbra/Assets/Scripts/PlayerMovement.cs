﻿using UnityEngine;
using System.Collections;

public class PlayerMovement : MonoBehaviour
{

    public float speed = 1;
    public float legTurnSpeed = 5;
    public float deadAngle = 45;
    public Animator animator;

    public Camera sceneCam;

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
        float in_x = Input.GetAxis("Horizontal");
        float in_y = Input.GetAxis("Vertical");
        float pos_x = transform.position.x + (in_x * speed * Time.deltaTime);
        float pos_y = transform.position.y + (in_y * speed * Time.deltaTime);

        Vector3 new_pos = new Vector3(pos_x, pos_y, 0f);

        //Calculate length of movement for animation state change
        float length = Vector3.Magnitude(new_pos - transform.position);
        animator.SetFloat("Speed", length);
        transform.position = new_pos;

        Vector3 dir;

        if (GameObject.FindGameObjectWithTag("PixelCanvas") != null) {
            Vector3 canvas_pos = GameObject.FindGameObjectWithTag("PixelCanvas").GetComponent<Canvas>().transform.position;
            dir = Input.mousePosition - canvas_pos;
        }
        else {
            dir = Input.mousePosition - Camera.main.WorldToScreenPoint(transform.position);
        }

        var angH = Input.GetAxis("RightH");
        var angV = Input.GetAxis("RightV");
        float angleDiff;
        Quaternion legsRot = _legs.transform.rotation;
        Quaternion newRot = _body.transform.rotation;

        if (angH != 0 || angV != 0) {
            var ang = new Vector2(Input.GetAxis("RightH"), Input.GetAxis("RightV"));
            var angle = Vector2.SignedAngle(ang, Vector2.right);
            // _body.transform.Rotate(Vector3.left, 45 * Time.deltaTime * speed);
            // _body.transform.Rotate(0, 0, angle * Time.deltaTime * speed);
            _body.transform.rotation = Quaternion.Euler(0, 0, angle);

            newRot = Quaternion.AngleAxis(angle, Vector3.forward);

        } else if (mousePosition != Input.mousePosition) {
            float look_angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
            newRot = Quaternion.AngleAxis(look_angle, Vector3.forward);
            _body.transform.rotation = newRot;

            angleDiff = Quaternion.Angle(legsRot, newRot);
            // TODO: try to rotate legs towards input axis and not body
        }

        angleDiff = Quaternion.Angle(legsRot, newRot);

        if (angleDiff > deadAngle || System.Math.Abs(in_y) > 0 || System.Math.Abs(in_x) > 0)
        {
            _legs.transform.rotation = Quaternion.Lerp(legsRot, newRot, legTurnSpeed * Time.deltaTime);
        }

        mousePosition = Input.mousePosition;
    }
}
