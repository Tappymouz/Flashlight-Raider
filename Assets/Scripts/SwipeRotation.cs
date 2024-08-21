using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwipeRotation : MonoBehaviour
{
    public Transform pointerTransform; // Reference to the pointer (black point) transform
    private Camera mainCamera;

    void Start()
    {
        mainCamera = Camera.main; // Cache the main camera reference
    }

    void Update()
    {
        // Check for touch input (for mobile devices)
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            RotateTowards(touch.position);
        }
        // Check for mouse input (for testing in the editor)
        else if (Input.GetMouseButton(0))
        {
            RotateTowards(Input.mousePosition);
        }
    }

    private void RotateTowards(Vector2 targetPosition)
    {
        // Convert screen position to world position
        Vector3 worldPosition = mainCamera.ScreenToWorldPoint(targetPosition);
        worldPosition.z = 0; // Ensure it's on the same plane as the capsule

        // Calculate direction from the capsule to the target position
        Vector2 direction = (worldPosition - transform.position).normalized;

        // Calculate the angle from the direction
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        // Rotate the capsule
        transform.rotation = Quaternion.Euler(0, 0, angle - 90); // Adjust by -90 to align with the capsule's orientation

        // Move the pointer to the tip of the capsule
        pointerTransform.position = transform.position + (Vector3)direction * transform.localScale.y / 2;
    }
}