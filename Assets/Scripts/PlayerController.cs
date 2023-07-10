using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float speed = 2f;
    private Vector3 startPoint;
    private bool isMoving = false;
    private Vector3 rotation;
    private float playerScale = 1.0f;

    //ABSTRACTION

    public float GetScale()
    {
        return playerScale;
    }

    public void SetScale(float scale)
    {
        playerScale = scale;
        transform.localScale = new Vector3(scale, scale, scale);
    }

    private void Start()
    {
        isMoving = false;
    }

    private void OnMouseDrag()
    {
        if (IsClickedOnPlayer())
        {
            isMoving = true;
            startPoint = Input.mousePosition;
            rotation = GetComponent<Rigidbody>().angularVelocity;
            GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
        }
    }

    private void LateUpdate()
    {
        if (Input.GetMouseButtonUp(0))
        {
            if (isMoving)
            {
                isMoving = false;
                GetComponent<Rigidbody>().angularVelocity = rotation * speed;
                GetComponent<Rigidbody>().velocity = new Vector3(GetComponent<Rigidbody>().velocity.x, GetComponent<Rigidbody>().velocity.y, 0);
            }
        }

        if (isMoving)
        {
            Vector3 direction = (Input.mousePosition - startPoint).normalized;
            float distance = Vector3.Distance(startPoint, Input.mousePosition) * 0.05f;
            Vector3 movement = new Vector3(direction.x, 0, direction.y) * distance * speed * Time.deltaTime;
            float newX = Mathf.Clamp(transform.position.x + movement.x, 730f, 810f);
            movement.x = newX - transform.position.x;
            float newZ = Mathf.Clamp(transform.position.z + movement.z, -24f, 20f);
            movement.z = newZ - transform.position.z;
            transform.position += movement;
            Vector3 rotationVector = new Vector3(-direction.y, direction.x, 0);
            GetComponent<Rigidbody>().AddTorque(rotationVector * speed);
        }
    }

    private bool IsClickedOnPlayer()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit))
        {
            if (hit.collider.gameObject == gameObject)
            {
                return true;
            }
        }

        return false;
    }
}