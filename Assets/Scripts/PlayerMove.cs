using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.EventSystems;

public class PlayerMove : MonoBehaviour
{
    public Joystick Joystick;
    public float speed = 1f;
    public float rotationSpeed;
    public RectTransform swipeArea;

    private Rigidbody2D rb;
    private Vector2 smoothedMovementInput;
    private Vector2 smoothedMovementInputVelocity;

    private TouchInput touchInputActions;
    private Vector2 swipeInput;
    private Vector2 touchPosition;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        touchInputActions = new TouchInput();
    }

    private void OnEnable()
    {
        touchInputActions.Enable();
        touchInputActions.TouchControls.PrimaryTouch.performed += OnTouchPerformed;
        touchInputActions.TouchControls.Swipe.performed += OnSwipePerformed;
    }

    private void OnDisable()
    {
        touchInputActions.Disable();
        touchInputActions.TouchControls.PrimaryTouch.performed -= OnTouchPerformed;
        touchInputActions.TouchControls.Swipe.performed -= OnSwipePerformed;
    }

    

    private void Update()
    {
        SetPlayerVelocity();
        RotatePlayer();
    }

    private void SetPlayerVelocity()
    {
        Vector2 movementInput = Joystick.Direction.normalized;

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
            // Calculate the target angle based on swipe input
            float targetAngle = Mathf.Atan2(swipeInput.y, swipeInput.x) * Mathf.Rad2Deg;

            // Define the desired rotation based on the target angle
            Quaternion targetRotation = Quaternion.Euler(0, 0, targetAngle);

            // Smoothly interpolate towards the target rotation based on rotationSpeed
            transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
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
                null,
                out Vector2 localPoint))
        {
            return swipeArea.rect.Contains(localPoint);
        }
        return false;
    }
}