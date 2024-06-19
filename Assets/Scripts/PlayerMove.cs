using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMove : MonoBehaviour
{
    public float moveSpeed = 3f;

    public InputActionAsset inputActions;

    InputAction moveAction;

    Camera mainCamera;

    Vector3 targetPosition;

    bool isMoving = false;

    void Start()
    {
        moveAction = inputActions.FindAction("Move");

        moveAction.Enable();

        mainCamera = Camera.main;

    }
    void Update()
    {
        if (moveAction.WasPressedThisFrame())
        {
            Vector2 mousePosition = Mouse.current.position.ReadValue();

            Ray ray = mainCamera.ScreenPointToRay(mousePosition);

            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                targetPosition = hit.point;

                isMoving = true;
            }
        }

        if (isMoving)
        {
            Vector3 direction = (targetPosition - transform.position).normalized;

            transform.rotation = Quaternion.LookRotation(direction);

            transform.position = Vector3.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);

            if (transform.position == targetPosition)
            {
                isMoving = false;

            }
        }
    }
}
