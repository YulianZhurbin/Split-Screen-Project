using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class CamRotator : MonoBehaviour
{
    [SerializeField] Camera cam;
    [SerializeField] Transform target;
    [SerializeField] float rotationSpeed = 40000f;

    private Vector3 direction;
    private Vector3 previousPosition;
    private readonly float camOffset = 10.0f;

    void Update()
    {
        if(IsMouseInScope())
        {
            if (Input.GetMouseButtonDown(0))
            {
                previousPosition = cam.ScreenToViewportPoint(Input.mousePosition);
            }

            if (Input.GetMouseButton(0))
            {
                direction = previousPosition - cam.ScreenToViewportPoint(Input.mousePosition);

                cam.transform.position = target.position;

                cam.transform.Rotate(new Vector3(1, 0, 0), direction.y * rotationSpeed * Time.deltaTime);
                cam.transform.Rotate(new Vector3(0, 1, 0), -direction.x * rotationSpeed * Time.deltaTime, Space.World);

                cam.transform.Translate(0, 0, -camOffset);

                previousPosition = cam.ScreenToViewportPoint(Input.mousePosition);
            }
        }

        if (cam.transform.position.y < 0)
        {
            AlignCamera();
        }

        CheckEscapeRequest();
    }

    private bool IsMouseInScope()
    {
        Vector3 mousePos = cam.ScreenToViewportPoint(Input.mousePosition);

        bool isMouseInScope = mousePos.x > 1;

        return isMouseInScope;
    }

    private void AlignCamera()
    {
        Debug.Log("Cam Y coordinate: " + cam.transform.position.y);

        cam.transform.position = target.position;
        cam.transform.Rotate(new Vector3(1, 0, 0), -direction.y * rotationSpeed * Time.deltaTime);
        cam.transform.Translate(0, 0, -camOffset);
    }

    private void CheckEscapeRequest()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
#if UNITY_EDITOR
            EditorApplication.ExitPlaymode();
#else
            Application.Quit();
#endif
        }
    }
}
