using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotation : MonoBehaviour
{
    public float rotationSensitivity = 5.0f;
    private bool isSwiping;

    // Reference to PlayerAttack to check attack state
    private PlayerAttack playerAttack;

    private void Start()
    {
        playerAttack = GetComponent<PlayerAttack>();
    }

    void Update()
    {
        // Check if the player is attacking, and skip rotation if true
        if (playerAttack != null && playerAttack.IsAttacking())
        {
            return; // Prevent rotation while attacking
        }

        // For touch input
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);

            if (touch.phase == TouchPhase.Began)
            {
                isSwiping = true;
            }
            else if (touch.phase == TouchPhase.Moved && isSwiping)
            {
                RotateTowardsTouchPosition(touch.position);
            }
            else if (touch.phase == TouchPhase.Ended)
            {
                isSwiping = false;
            }
        }
        // For mouse input
        else if (Input.GetMouseButtonDown(0))
        {
            isSwiping = true;
        }
        else if (Input.GetMouseButton(0) && isSwiping)
        {
            RotateTowardsTouchPosition(Input.mousePosition);
        }
        else if (Input.GetMouseButtonUp(0))
        {
            isSwiping = false;
        }
    }

    private void RotateTowardsTouchPosition(Vector2 touchPosition)
    {
        Vector3 worldPosition = Camera.main.ScreenToWorldPoint(touchPosition);
        worldPosition.z = transform.position.z;

        Vector3 direction = worldPosition - transform.position;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        Quaternion targetRotation = Quaternion.Euler(0, 0, angle - 90);
        transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, Time.deltaTime * rotationSensitivity);
    }
}

