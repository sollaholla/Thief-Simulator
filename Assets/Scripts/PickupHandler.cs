using System.Linq;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class PickupHandler : MonoBehaviour
{
    private bool input;
    private Animator animator;
    private Loot currentItem;

    public bool hasItem => currentItem != null;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
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

        var loot = Physics.OverlapSphere(transform.position, 1.5f)
            .Select(x => x.GetComponentInParent<Loot>())
            .Where(x => x != null)
            .OrderBy(x => Vector3.Distance(x.transform.position, transform.position))
            .FirstOrDefault();

        if (loot != null)
        {
            loot.transform.SetParent(transform);
            loot.OnPickup();

            animator.SetFloat("CarryType", loot.carryType);
            animator.SetBool("Carrying", true);
            currentItem = loot;
        }

        input = false;
    }

    public Loot Drop()
    {
        var item = currentItem;
        currentItem = null;
        item.OnDropped();
        animator.SetBool("Carrying", false);
        return item;
    }
}
