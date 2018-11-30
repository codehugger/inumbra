using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public float speed;

    private Rigidbody2D rb;
    private Vector2 moveVelocity;
    private Vector2 rotationVelocity;

    // Use this for initialization
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        // Move with keyboard WASD
        Vector2 moveInput = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
        moveVelocity = moveInput.normalized * speed;

        // Rotate to face mouse
        //Vector2 direction = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        //float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        //Quaternion rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        //transform.rotation = Quaternion.Slerp(transform.rotation, rotation, 10f * Time.deltaTime);

        var dir = Input.mousePosition - Camera.main.WorldToScreenPoint(transform.position);
        var angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
    }

    void FixedUpdate()
    {
        rb.MovePosition(rb.position + moveVelocity * Time.fixedDeltaTime);
    }
}
