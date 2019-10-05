using UnityEngine;

public class Loot : MonoBehaviour
{
    public float weight = 5f;
    public BoxCollider[] colliders;
    public Rigidbody[] bodies;

    public void OnCollected()
    {
        TogglePhysics(false);
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
}
