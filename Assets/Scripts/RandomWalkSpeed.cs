using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterMover))]
public class RandomWalkSpeed : MonoBehaviour
{
    public float minWalkSpeed = 0.8f;
    public float maxWalkSpeed = 1.3f;

    private CharacterMover mover;

    // Start is called before the first frame update
    void Start()
    {
        mover = GetComponent<CharacterMover>();
        mover.walkSpeed = Random.Range(minWalkSpeed, maxWalkSpeed);
    }
}
