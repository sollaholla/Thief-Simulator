using System;
using System.Collections;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

public class Distraction : MonoBehaviour
{
    public UnityEvent onStartDistraction;
    public UnityEvent onStopDistraction;
    public House house;
    public float distractionDelay;
    public float range = 10f;

    private bool isActive;

    private void Awake()
    {
        if (house == null)
        {
            house = GetComponentInParent<House>();
        }
    }

    public void Trigger()
    {
        if (isActive)
        {
            return;
        }

        onStartDistraction?.Invoke();
        StartCoroutine(AwaitDistract());
        isActive = true;
    }

    private void TakeEffect()
    {
        var occupants = house.occupants
            .Where(x => Vector3.Distance(x.transform.position, transform.position) < range)
            .OrderBy(x => Vector3.Distance(transform.position, x.transform.position));

        foreach (var occ in occupants)
        {
            var ai = occ.GetComponent<AI>();
            if (ai != null)
            {
                ai.GoInvestigate(this, false);
                break;
            }
        }
    }

    private IEnumerator AwaitDistract()
    {
        yield return new WaitForSeconds(distractionDelay);
        while (true)
        {
            TakeEffect();
            yield return null;
        }
    }

    public void Stop()
    {
        if (!isActive)
        {
            return;
        }

        StopAllCoroutines();
        onStopDistraction?.Invoke();
        isActive = false;
    }
}
