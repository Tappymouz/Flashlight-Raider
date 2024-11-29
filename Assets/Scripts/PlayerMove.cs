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
    private Vector2 swipeInput1, swipeInput2;
    private Vector2 touchPosition1, touchPosition2;

    private string currentTileSound;
    private float stepDelay = 1f;  // Delay between footstep sounds
    private float stepTimer = 0.5f;    // Timer to track footstep intervals
    private bool isWalking = false;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        touchInputActions = new TouchInput();
    }

    private void OnEnable()
    {
        touchInputActions.Enable();
        touchInputActions.TouchControls.PrimaryTouch1.performed += OnTouchPerformed1;
        touchInputActions.TouchControls.PrimaryTouch2.performed += OnTouchPerformed2;
        touchInputActions.TouchControls.Swipe1.performed += OnSwipePerformed1;
        touchInputActions.TouchControls.Swipe2.performed += OnSwipePerformed2;
    }

    private void OnDisable()
    {
        touchInputActions.Disable();
        touchInputActions.TouchControls.PrimaryTouch1.performed -= OnTouchPerformed1;
        touchInputActions.TouchControls.PrimaryTouch2.performed -= OnTouchPerformed2;
        touchInputActions.TouchControls.Swipe1.performed -= OnSwipePerformed1;
        touchInputActions.TouchControls.Swipe2.performed -= OnSwipePerformed2;
    }



    private void Update()
    {
        SetPlayerVelocity();
        // Debug logging to understand input states
        if (IsTouchInSwipeArea(touchPosition1))
        {
            RotatePlayer(swipeInput1);
        }
        else if (IsTouchInSwipeArea(touchPosition2))
        {
            RotatePlayer(swipeInput2);
        }

        if (rb.velocity.magnitude > 0.1f)  // Player is moving
        {
            isWalking = true;
        }
        else  // Player stopped moving
        {
            isWalking = false;
        }

        if (isWalking)
        {
            stepTimer += Time.deltaTime;
            if (stepTimer >= stepDelay)
            {
                stepTimer = 0f;
                DetectTileType();  // Keep checking tilemap area
            }
        }


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

    private void RotatePlayer(Vector2 swipeInput)
    {
        if (swipeInput != Vector2.zero)
        {
            // Calculate the target angle based on swipe input
            float targetAngle = Mathf.Atan2(swipeInput.y, swipeInput.x) * Mathf.Rad2Deg;

            // Define the desired rotation based on the target angle
            Quaternion targetRotation = Quaternion.Euler(0, 0, targetAngle);

            // Smoothly interpolate towards the target rotation based on rotationSpeed
            transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        }
    }



    private void OnSwipePerformed1(InputAction.CallbackContext context)
    {
        swipeInput1 = context.ReadValue<Vector2>();
    }

    private void OnSwipePerformed2(InputAction.CallbackContext context)
    {
        swipeInput2 = context.ReadValue<Vector2>();
    }

    private void OnTouchPerformed1(InputAction.CallbackContext context)
    {
        touchPosition1 = context.ReadValue<Vector2>();
    }

    private void OnTouchPerformed2(InputAction.CallbackContext context)
    {
        touchPosition2 = context.ReadValue<Vector2>();
    }

    private bool IsTouchInSwipeArea(Vector2 touchPosition)
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


    private void DetectTileType()
    {
        // Perform an overlap circle to detect the tilemap area the player is standing on
        Collider2D[] hitTiles = Physics2D.OverlapCircleAll(transform.position, 0.1f); // Larger detection area

        bool soundPlayed = false;  // To track if a sound has been played

        foreach (var hit in hitTiles)
        {
            TileType tileTypeComponent = hit.GetComponent<TileType>();
            if (tileTypeComponent != null)
            {
                // Only play sound if tile type is different or we haven't played the sound yet
                if (tileTypeComponent.tileType.ToString() != currentTileSound || !soundPlayed)
                {
                    PlaySoundForTile(tileTypeComponent.tileType);
                    currentTileSound = tileTypeComponent.tileType.ToString();  // Update the current sound
                    soundPlayed = true;  // Mark that the sound has been played
                    break;  // Stop after playing the sound for the detected tilemap area
                }
            }
        }
    }

    private void PlaySoundForTile(TileType.TileTypeEnum tileType)
    {
        // Always play the sound for the tile as long as the player is walking on it
        Debug.Log("Playing footstep sound for: " + tileType);

        AudioManager.Instance.PlaySFX(tileType.ToString());
    }
}