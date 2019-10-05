using UnityEngine;

[RequireComponent(typeof(CharacterMover))]
public class Player : MonoBehaviour
{
    private CharacterMover mover;
    private new Camera camera;

    private void Awake()
    {
        mover = GetComponent<CharacterMover>();
        camera = Camera.main;
    }

    private void Update()
    {
        var inputDir = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical"));
        var relativeInput = Quaternion.Euler(0, camera.transform.eulerAngles.y, 0) * inputDir;
        mover.Move(relativeInput.x, relativeInput.z, Input.GetButton("Sprint"), Input.GetButton("Crouch"));
    }
}
