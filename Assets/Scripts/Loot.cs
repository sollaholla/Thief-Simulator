using System;
using UnityEngine;

public class Loot : MonoBehaviour
{
    public float weight = 5f;
    public BoxCollider[] colliders;
    public Rigidbody[] bodies;
    public int carryType;
    public Vector3 carryOffset = new Vector3(0, 1, 0);
    public Vector3 rotationOffset = new Vector3();

    public void OnCollected()
    {
        gameObject.SetActive(false);
    }

    public void OnDropped()
    {
        TogglePhysics(true);
    }

    private void TogglePhysics(bool toggle)
    {
        foreach (var collider in colliders)
        {
            collider.enabled = toggle;
        }

        foreach (var rb in bodies)
        {
            rb.isKinematic = !toggle;
        }
    }

    public void OnPickup()
    {
        TogglePhysics(false);
        transform.localPosition = carryOffset;
        transform.localRotation = Quaternion.Euler(rotationOffset);
    }
}
