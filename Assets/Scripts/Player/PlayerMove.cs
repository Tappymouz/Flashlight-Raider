using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.EventSystems;

public class PlayerMove : MonoBehaviour
{
    public Joystick Joystick;
    public float speed = 10f;
    public float rotationSpeed = 1440f;
    public RectTransform swipeArea;
    public Transform Aim; // Define Aim
    public Camera mainCamera;  // Assign this manually in the Inspector

    private Rigidbody2D rb;
    private Vector2 smoothedMovementInput;
    private Vector2 smoothedMovementInputVelocity;
    private Vector2 movementInput;
    private Vector2 swipeInput;
    private Vector2 touchPosition;

    // Single Awake method
    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        if (mainCamera == null)
        {
            mainCamera = Camera.main;  // Fallback to main camera if not set
        }
    }

    private void Update()
    {
        SetPlayerVelocity();
        RotatePlayer();
    }

    private void FixedUpdate()
    {
        // Corrected movement without Time.fixedDeltaTime
        rb.velocity = movementInput * speed;

        if (movementInput != Vector2.zero)
        {
            // Aiming with correct input
            Vector2 vector2 = Vector2.left * movementInput.x + Vector2.down * movementInput.y;
            Aim.rotation = Quaternion.LookRotation(Vector3.forward, vector2);
        }
    }

    private void SetPlayerVelocity()
    {
        // Get movement input from Joystick
        movementInput = Joystick.Direction.normalized;

        smoothedMovementInput = Vector2.SmoothDamp(
            smoothedMovementInput,
            movementInput,
            ref smoothedMovementInputVelocity,
            0.1f);

        rb.velocity = smoothedMovementInput * speed;
    }

    private void RotatePlayer()
    {
        if (swipeInput != Vector2.zero && IsTouchInSwipeArea())
        {
            float targetAngle = Mathf.Atan2(swipeInput.y, swipeInput.x) * Mathf.Rad2Deg;
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.z, targetAngle, ref rotationSpeed, 0.1f);
            rb.MoveRotation(angle);
        }
        else if (swipeInput == Vector2.zero && IsTouchInSwipeArea())
        {
            // Use the correct input for aiming
            Vector2 vector2 = Vector2.left * movementInput.x + Vector2.down * movementInput.y;
            Aim.rotation = Quaternion.LookRotation(Vector3.forward, vector2);

            Debug.Log("Player is holding the touch, applying aim rotation.");
        }
        else if (!IsTouchInSwipeArea())
        {
            Debug.Log("Touch is outside the swipe area, no rotation allowed.");
        }
    }

    private void OnSwipePerformed(InputAction.CallbackContext context)
    {
        swipeInput = context.ReadValue<Vector2>();
    }

    private void OnTouchPerformed(InputAction.CallbackContext context)
    {
        touchPosition = context.ReadValue<Vector2>();
    }

    private bool IsTouchInSwipeArea()
    {
        if (RectTransformUtility.ScreenPointToLocalPointInRectangle(
                swipeArea,
                touchPosition,
                mainCamera,  // Use main camera here
                out Vector2 localPoint))
        {
            return swipeArea.rect.Contains(localPoint);
        }
        return false;
    }
}
