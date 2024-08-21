using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotation : MonoBehaviour
{
    public float rotationSensitivity = 5.0f; // Adjust the rotation speed

    private bool isSwiping;

    void Update()
    {
        // For touch input (on mobile devices)
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
        // For mouse input (for testing in the editor)
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
        // Convert touch/mouse position to world position
        Vector3 worldPosition = Camera.main.ScreenToWorldPoint(touchPosition);
        worldPosition.z = transform.position.z;

        // Calculate the direction vector from the player to the touch position
        Vector3 direction = worldPosition - transform.position;

        // Calculate the angle between the player's forward direction and the direction vector
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        // Smoothly rotate towards the calculated angle
        Quaternion targetRotation = Quaternion.Euler(0, 0, angle - 90); // Adjusting by -90 degrees if needed
        transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, Time.deltaTime * rotationSensitivity);
    }
}
