using System.Collections;
using UnityEngine;

public class Door : MonoBehaviour
{
    public float openDuration = 3f;
    public bool isLocked;

    private bool isOpen;
    private Quaternion startRotation;
    private Collider[] colliders;

    private void Awake()
    {
        startRotation = transform.rotation;
        colliders = GetComponentsInChildren<Collider>();
    }

    public void Open(bool overrideLock)
    {
        if (isLocked && !overrideLock)
        {
            return;
        }

        if (isOpen)
        {
            return;
        }

        isOpen = true;
        StopAllCoroutines();
        StartCoroutine(OpenAnim());
    }

    public void Close()
    {
        if (!isOpen)
        {
            return;
        }

        isOpen = false;
        StopAllCoroutines();
        StartCoroutine(CloseAnim());
    }

    private IEnumerator CloseAnim()
    {
        yield return DoRot(startRotation);
    }

    private IEnumerator OpenAnim()
    {
        yield return DoRot(startRotation * Quaternion.Euler(0, 90, 0));
        yield return new WaitForSeconds(openDuration);
        Close();
    }

    private IEnumerator DoRot(Quaternion target)
    {
        var targetRot = target;
        SetCollision(false);

        while (transform.rotation != targetRot)
        {
            transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRot, Time.deltaTime * 100f);
            yield return null;
        }

        SetCollision(true);
    }

    private void SetCollision(bool enabled)
    {
        foreach (var collider in colliders)
        {
            collider.enabled = enabled;
        }
    }
}
