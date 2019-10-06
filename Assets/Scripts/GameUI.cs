using UnityEngine;
using TMPro;
using System.Text;

public class GameUI : MonoBehaviour
{
    [SerializeField] private TMP_Text checklist;

    private void Awake()
    {
        GameManager.itemsChanged += ItemsChanged;
    }

    private void ItemsChanged()
    {
        var builder = new StringBuilder();
        foreach (var name in GameManager.allItems)
        {
            if (!GameManager.itemsToCollect.Contains(name))
            {
                builder.AppendLine($"<s>{name}</s>");
            }
            else
            {
                builder.AppendLine($"{name}");
            }
        }

        checklist.text = builder.ToString();
    }
}
