using UnityEngine;

public class NPCSpawner : MonoBehaviour
{
    public int npcCount = 15;
    public GameObject[] npcPrefabs;
    public Transform[] spawnPoints;
    public Transform waypoints;

    private void Awake()
    {
        Spawn(npcCount);
    }

    public void Spawn(int count)
    {
        for (int i = 0; i < count; i++)
        {
            var spawn = spawnPoints[Random.Range(0, spawnPoints.Length - 1)];
            var npc = Instantiate(npcPrefabs[UnityEngine.Random.Range(0, npcPrefabs.Length - 1)], spawn.position, Quaternion.identity);
            var ai = npc.GetComponent<AI>();
            ai.waypointSet = waypoints;
        }
    }
}
