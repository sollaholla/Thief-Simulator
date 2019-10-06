using UnityEngine;

[RequireComponent(typeof(Inventory))]
public class Van : MonoBehaviour
{
    private Inventory inventory;

    private void Awake()
    {
        inventory = GetComponent<Inventory>();
    }

    private void OnTriggerEnter(Collider other)
    {
        var pickupHandler = other.GetComponent<PickupHandler>();
        if (pickupHandler && pickupHandler.hasItem)
        {
            var loot = pickupHandler.Drop();
            inventory.Add(loot);
            loot.transform.SetParent(transform, false);
            GameManager.OnCollectItem(loot.name);
        }
    }
}