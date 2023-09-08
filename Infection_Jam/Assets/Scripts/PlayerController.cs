using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [SerializeField] InputActionAsset actions;

    [Range(0.01f, 1.0f)] [SerializeField] float deceleration;
    [SerializeField] float moveSpeed;

    Rigidbody2D rb;

    Vector2 moveInput;


    void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        Application.targetFrameRate = -1;
        //Application.targetFrameRate = 15;

    }

    // Update is called once per frame
    void Update()
    {
        moveInput = actions.FindActionMap("Standard").FindAction("Move").ReadValue<Vector2>().normalized;

    }

    private void FixedUpdate()
    {
        //Deceleration
        if (moveInput.magnitude <= 0)
        {
            rb.velocity -= rb.velocity * deceleration;

            if (rb.velocity.magnitude < 0.1f) 
            {
                rb.velocity = Vector2.zero;
            }
        }
        //Standard movement
        else
        {
            rb.velocity = moveInput * moveSpeed;
        }

        Debug.Log("Fixed: " + rb.velocity.magnitude);
    }

    private void OnEnable()
    {
        actions.FindActionMap("Standard").Enable();
    }

    private void OnDisable()
    {
        actions.FindActionMap("Standard").Disable();
    }
}
