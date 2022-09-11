using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AndroidCamRotator : MonoBehaviour
{
    [SerializeField] Camera cam;
    [SerializeField] Transform target;
    [SerializeField] float rotationSpeed = 10000f;

    private Vector3 previousPosition;
    private Touch touch;
    private Vector3 direction;

    readonly float cameOffset = 10.0f;

    private void Update()
    {
        if (Input.touchCount > 0)
        {
            touch = Input.GetTouch(0);

            if (IsTouchInScope())
            {
                if (touch.phase == TouchPhase.Began)
                {
                    previousPosition = cam.ScreenToViewportPoint(touch.position);
                }

                if (touch.phase == TouchPhase.Moved)
                {
                    direction = previousPosition - cam.ScreenToViewportPoint(touch.position);

                    cam.transform.position = target.position;

                    cam.transform.Rotate(new Vector3(1, 0, 0), direction.y * rotationSpeed * Time.deltaTime);
                    cam.transform.Rotate(new Vector3(0, 1, 0), -direction.x * rotationSpeed * Time.deltaTime, Space.World);

                    cam.transform.Translate(0, 0, -cameOffset);

                    previousPosition = cam.ScreenToViewportPoint(touch.position);
                }
            }
        }

        if (cam.transform.position.y < 0)
        {
            AlignCamera();
        }
    }

    private bool IsTouchInScope()
    {
        Vector3 touchPos = cam.ScreenToViewportPoint(touch.position);

        bool isTouchInScope = touchPos.x > 1;

        return isTouchInScope;
    }

    private void AlignCamera()
    {
        cam.transform.position = target.position;
        cam.transform.Rotate(new Vector3(1, 0, 0), -direction.y * rotationSpeed * Time.deltaTime);
        cam.transform.Translate(0, 0, -cameOffset);
    }
}
