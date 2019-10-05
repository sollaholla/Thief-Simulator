using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public List<Loot> inventory = new List<Loot>();

    public void Add(Loot loot)
    {
        inventory.Add(loot);
        loot.OnCollected();
    }

    public void Drop(Loot loot)
    {
        inventory.Remove(loot);
        loot.OnDropped();
    }
}
