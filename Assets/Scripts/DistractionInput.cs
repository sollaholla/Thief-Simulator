using System.Linq;
using UnityEngine;

public class DistractionInput : MonoBehaviour
{
    private bool input;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            input = true;
        }
    }

    private void FixedUpdate()
    {
        if (!input)
        {
            return;
        }

        var distraction = Physics.OverlapSphere(transform.position, 1.5f)
            .Select(x => x.GetComponentInParent<Distraction>())
            .Where(x => x != null)
            .OrderBy(x => Vector3.Distance(x.transform.position, transform.position))
            .FirstOrDefault();

        if (distraction != null)
        {
            distraction.Trigger();
        }

        input = false;
    }
}
