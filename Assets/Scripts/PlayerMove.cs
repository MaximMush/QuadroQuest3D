using UnityEngine;
using UnityEngine.InputSystem;
using Unity.Netcode;
using System.Globalization;
using System;

public class PlayerMove : NetworkBehaviour
{
    public float moveSpeed = 3f;
    public InputActionAsset inputActions;

    InputAction moveAction;
    Camera mainCamera;
    Vector3 targetPosition;
    bool isMoving = false;

    void Start()
    {
        if (!IsOwner) return;

        moveAction = inputActions.FindAction("Move");
        moveAction.Enable();
        mainCamera = Camera.main;
    }

    void Update()
    {
        if (!IsOwner) return;

        if (moveAction.WasPressedThisFrame())
        {
            Vector2 mousePosition = Mouse.current.position.ReadValue();
            Ray ray = mainCamera.ScreenPointToRay(mousePosition);

            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                targetPosition = hit.point;
                targetPosition.y = transform.position.y;
                isMoving = true;
                MoveServerRpc(targetPosition);
            }
        }
    }

    [ServerRpc]
    void MoveServerRpc(Vector3 target)
    {
        targetPosition = target;
        isMoving = true;
    }

    void FixedUpdate()
    {
        if (!IsServer || !isMoving) return;

        Vector3 direction = (targetPosition - transform.position).normalized;
        direction.y = 0f;

        transform.rotation = Quaternion.Euler(0f, Quaternion.LookRotation(direction).eulerAngles.y, 0f);

        Vector3 newPosition = Vector3.MoveTowards(transform.position, targetPosition, moveSpeed * Time.fixedDeltaTime);
        newPosition.y = transform.position.y;

        transform.position = newPosition;

        if (transform.position == targetPosition)
        {
            isMoving = false;
        }
    }
}
