using UnityEngine;

[RequireComponent(typeof(CharacterMover))]
[RequireComponent(typeof(DoorOpener))]
public class Player : MonoBehaviour
{
    private CharacterMover mover;
    private DoorOpener opener;
    private new Camera camera;

    private void Awake()
    {
        mover = GetComponent<CharacterMover>();
        opener = GetComponent<DoorOpener>();
        camera = Camera.main;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            opener.TryOpenDoor();
        }
    }

    private void FixedUpdate()
    {
        var inputDir = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical"));
        var relativeInput = Quaternion.Euler(0, camera.transform.eulerAngles.y, 0) * inputDir;
        mover.Move(relativeInput.x, relativeInput.z, Input.GetButton("Sprint"), Input.GetButton("Crouch"));

        if (relativeInput.sqrMagnitude > 0)
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(relativeInput), Time.fixedDeltaTime * 5f);
        }
    }
}
