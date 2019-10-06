using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class House : MonoBehaviour
{
    public List<Room> rooms;
    public HashSet<GameObject> occupants = new HashSet<GameObject>();

    private List<BoxCollider> roomColliders;
    private Dictionary<BoxCollider, int> colliderRooms = new Dictionary<BoxCollider, int>();

    private void Awake()
    {
        roomColliders = rooms.SelectMany(x => x.colliders).ToList();

        foreach (var collider in roomColliders)
        {
            colliderRooms[collider] = rooms.FindIndex(x => x.colliders.Contains(collider));
        }
    }

    private void OnTriggerStay(Collider other)
    {
        var roomHandler = other.transform.GetComponent<RoomHandler>();
        if (roomHandler != null)
        {
            var room = GetRoom(other.transform.position);
            if (room != -1)
            {
                roomHandler.room = room;
                if (!occupants.Contains(roomHandler.gameObject))
                {
                    occupants.Add(roomHandler.gameObject);
                }
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        var roomHandler = other.transform.GetComponent<RoomHandler>();
        if (roomHandler != null)
        {
            roomHandler.room = -1;
            occupants.Remove(roomHandler.gameObject);
        }
    }

    private int GetRoom(Vector3 worldPos)
    {
        foreach (var collider in roomColliders)
        {
            var localPos = collider.transform.InverseTransformPoint(worldPos) - collider.center;
            var halfExt = collider.bounds.size / 2;

            if (localPos.x < halfExt.x && localPos.x > -halfExt.x &&
                localPos.y < halfExt.y && localPos.y > -halfExt.y &&
                localPos.z < halfExt.z && localPos.z > -halfExt.z)
            {
                return colliderRooms[collider];
            }
        }

        return -1;
    }
}

[System.Serializable]
public class Room
{
    public List<BoxCollider> colliders;
}
