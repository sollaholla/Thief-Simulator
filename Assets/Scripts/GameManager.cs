using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using TMPro;

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

        if(itemsToCollect.Count == 0)
        {
            EndGame(true);
        }
    }

    public static void OnCaught()
    {
        EndGame(false);
    }

    public static void EndGame(bool playerWon)
    {
        var GameOverScreen = GameObject.FindGameObjectWithTag("GameOverScreen");
        var GameOverTitle = GameObject.FindGameObjectWithTag("EndGameTitle");

        if (playerWon)
        {
            GameOverTitle.GetComponent<TextMeshPro>().text = "Passed";
            GameOverTitle.GetComponent<TextMeshPro>().color = Color.green;
        }

        GameOverScreen.SetActive(true);
    }
}
