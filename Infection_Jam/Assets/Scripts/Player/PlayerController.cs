using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(PlayerStats))]
public class PlayerController : MonoBehaviour
{
    [SerializeField] InputActionAsset actions;

    [Range(0.01f, 1.0f)] [SerializeField] float deceleration;
    [SerializeField] float acceleration;
    [SerializeField] float moveSpeed;

    [SerializeField] GameObject hitboxPrefab;

    Rigidbody2D rb;
    PlayerStats playerStats;

    Vector2 moveInput;
    int inputComboCount = 0;

    int lookDirection = 1; //1 for right, -1 for left

    enum PlayerStates { Idle, Attacking}

    PlayerStates playerState = PlayerStates.Idle;
    

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        playerStats = GetComponent<PlayerStats>();

        Application.targetFrameRate = -1;
        //Application.targetFrameRate = 15;

    }

    // Update is called once per frame
    void Update()
    {
        if ( playerState != PlayerStates.Attacking)
        {
            moveInput = actions.FindActionMap("Standard").FindAction("Move").ReadValue<Vector2>().normalized;
        }
        else
        {
            moveInput = Vector2.zero;
        }

        if (actions.FindActionMap("Standard").FindAction("Attack").triggered)
        {
            OnAttack();
        }
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
                //Cap Movement Speed
                if (rb.velocity.magnitude >= moveSpeed)
                {
                    if (Vector2.Dot(rb.velocity, moveInput) > 0)
                    {
                        rb.velocity = moveInput * moveSpeed;
                    }
                    else
                    {
                        rb.velocity += moveInput * acceleration;
                    }


                }
                //Standard movement acceleration
                else
                {
                    rb.velocity += moveInput * acceleration;
                }

                if (rb.velocity.x != 0) { lookDirection = Math.Sign(rb.velocity.x); }
            }
    }

    private void OnAttack()
    {
        //Debug.Log("On Attack");
        playerState = PlayerStates.Attacking;
        inputComboCount = (inputComboCount >= 3) ? inputComboCount : inputComboCount + 1;
        if (inputComboCount == 1)
        {
            StartCoroutine(DoAttack());
        }
        
    }

    private IEnumerator DoAttack()
    {
        int currentCombo = 0;

        while (inputComboCount > 0)
        {
            currentCombo++;
            Debug.Log("Attack, " + lookDirection + ", " + currentCombo);
            GameObject newBox = createHitbox(Vector2.right * lookDirection * 2, new Vector2(1, 2));

            yield return new WaitForSeconds(1);

            Destroy(newBox);
            inputComboCount--;
        }

        playerState = PlayerStates.Idle;
        inputComboCount = 0;
    }

    private GameObject createHitbox(Vector2 positionOffset, Vector2 size)
    {
        GameObject newHitbox = Instantiate(hitboxPrefab, transform);

        newHitbox.GetComponent<Transform>().position = transform.position + new Vector3(positionOffset.x, positionOffset.y, 0);
        newHitbox.GetComponent<BoxCollider2D>().size = size;

        return newHitbox;
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
