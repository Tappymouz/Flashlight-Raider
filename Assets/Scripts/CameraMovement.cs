using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    public Transform[] targets;
    public Vector3 offset;
    public float smoothing;
    public Vector2 minPosition;
    public Vector2 maxPosition;

    private Transform activeTarget;

    // Update is called once per frame
    void LateUpdate()
    {
        activeTarget = GetActiveTarget();

        if (activeTarget != null && transform.position != activeTarget.position)
        {
            Vector3 targetPosition = new Vector3(activeTarget.position.x, activeTarget.position.y, transform.position.z);

            targetPosition.x = Mathf.Clamp(activeTarget.position.x, minPosition.x, maxPosition.x);
            targetPosition.y = Mathf.Clamp(activeTarget.position.y, minPosition.y, maxPosition.y);

            transform.position = Vector3.Lerp(transform.position, targetPosition, smoothing);
        }
    }
    private Transform GetActiveTarget()
    {
        foreach (Transform target in targets)
        {
            if (target != null && target.gameObject.activeInHierarchy)
            {
                return target;
            }
        }
        return null;
    }
}