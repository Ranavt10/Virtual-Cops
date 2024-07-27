using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwipeRotate : MonoBehaviour
{
    // Rotation speed of the player
    public float rotationSpeed = 5f;

    // Minimum swipe distance to detect
    public float minSwipeDistance = 50f;

    // Factor to control the smoothness of rotation (higher values for smoother rotation)
    public float rotationSmoothness = 5f;

    // Direction of the swipe
    private Vector2 startTouchPosition;

    // Target rotation angle
    private float targetRotation;

    // Update is called once per frame
    void Update()
    {
        // Check for swipe input
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);

            switch (touch.phase)
            {
                case TouchPhase.Began:
                    // Store the starting position of the touch
                    startTouchPosition = touch.position;
                    break;

                case TouchPhase.Ended:
                    // Calculate the swipe direction based on the difference between the starting and ending touch positions
                    Vector2 swipeDirection = touch.position - startTouchPosition;

                    // Check if the swipe distance is greater than the minimum distance
                    if (swipeDirection.magnitude > minSwipeDistance)
                    {
                        // Calculate the angle of rotation based on the swipe direction
                        float angle = Mathf.Atan2(swipeDirection.x, swipeDirection.y) * Mathf.Rad2Deg;

                        // Calculate the target rotation by adding the angle to the current rotation
                        targetRotation = transform.eulerAngles.y - angle;
                    }
                    break;
            }
        }

        // Smoothly rotate the player towards the target rotation
        float currentRotation = Mathf.LerpAngle(transform.eulerAngles.y, targetRotation, Time.deltaTime * rotationSmoothness);
        transform.rotation = Quaternion.Euler(0f, currentRotation, 0f);
    }
}
