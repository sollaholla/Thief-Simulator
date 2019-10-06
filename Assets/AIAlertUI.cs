using TMPro;
using UnityEngine;

[RequireComponent(typeof(TMP_Text))]
public class AIAlertUI : MonoBehaviour
{
    private TMP_Text text;
    private AI ai;

    void Start()
    {
        ai = GetComponentInParent<AI>();
        text = GetComponent<TMP_Text>();
    }

    void Update()
    {
        switch (ai.state)
        {
            case AIState.AlertingCop:
                text.text = "!";
                break;
            case AIState.Patrolling:
                if (ai.investigating)
                {
                    text.text = "?";
                }
                else
                {
                    text.text = "";
                }
                break;
        }

        transform.rotation = Camera.main.transform.rotation;
    }
}
