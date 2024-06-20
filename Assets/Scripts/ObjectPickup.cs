using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;

public class ObjectPickup : MonoBehaviour
{
    public InputActionAsset inputActions;
    private InputAction takeAction;
    private GameObject pickedObject;
    private Vector3 objectOffset;
    private bool isHolding = false;
    public float pickupDistance = 0.5f;

    private void Awake()
    {
        takeAction = inputActions.FindAction("Take");
        takeAction.performed += ctx => TogglePickup();
        takeAction.Enable();
    }

    private void OnDestroy()
    {
        takeAction.Disable();
    }

    private void TogglePickup()
    {
        if (!isHolding)
        {
            PickupObject();
        }
        else
        {
            DropObject();
        }
    }
    private void PickupObject()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, pickupDistance);

        foreach (Collider collider in colliders)
        {
            if (collider.gameObject != gameObject && collider.CompareTag("cube"))
            {
                pickedObject = collider.gameObject;
                objectOffset = pickedObject.transform.position - transform.position;
                pickedObject.transform.SetParent(transform);
                isHolding = true;
                break;
            }
        }
    }


    private void DropObject()
    {
        float nearestDistance = Mathf.Infinity;
        GameObject nearestObject = null;
        GameObject secondZoneCell = null;

        if (pickedObject != null)
        {
            pickedObject.transform.SetParent(null);

            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    secondZoneCell = TagComponent.GetObjectByTag($"c{i + 10}r{j + 10}");

                    float distance = Vector3.Distance(pickedObject.transform.position, secondZoneCell.transform.position);

                    if (distance < nearestDistance && distance <= 3)
                    {
                        nearestObject = secondZoneCell;
                        nearestDistance = distance;
                    }
                }
            }
        }

        pickedObject.transform.position = nearestObject.transform.position;
        pickedObject.transform.SetParent(nearestObject.transform);
        pickedObject.transform.rotation = Quaternion.identity;

        pickedObject = null;
        isHolding = false;
    }

    private void Update()
    {
        if (isHolding && pickedObject != null)
        {
            Vector3 newPosition = transform.position + transform.forward * objectOffset.magnitude;
            newPosition.y = 1f;
            pickedObject.transform.position = newPosition;
            pickedObject.transform.rotation = transform.rotation;
        }
    }
}