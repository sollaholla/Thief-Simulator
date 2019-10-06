using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorOpener : MonoBehaviour
{
    public bool autoOpenDoors;

    private Door currentDoor;

    public void TryOpenDoor()
    {
        if (currentDoor == null)
        {
            return;
        }

        currentDoor.Open(false);
        currentDoor = null;
    }

    private void OnTriggerEnter(Collider other)
    {
        var door = other.GetComponentInParent<Door>();
        if (door != null)
        {
            if (autoOpenDoors)
            {
                door.Open(true);
            }
            else
            {
                currentDoor = door;
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (currentDoor == null)
        {
            return;
        }

        var door = other.GetComponentInParent<Door>();
        if (currentDoor == door)
        {
            currentDoor = null;
        }
    }
}
