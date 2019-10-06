using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static List<string> itemsToCollect;
    public static List<string> allItems;
    public static event Action itemsChanged;

    private void Start()
    {
        allItems = FindObjectsOfType<Loot>().Select(x => x.name).ToList();
        itemsToCollect = allItems.ToList();
        itemsChanged?.Invoke();
    }

    public static void OnCollectItem(string name)
    {
        itemsToCollect.Remove(name);
        itemsChanged?.Invoke();
    }

    public void OnCaught()
    {
    }
}
