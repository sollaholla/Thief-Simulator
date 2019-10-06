using UnityEngine;

[RequireComponent(typeof(CharacterController))]
[RequireComponent(typeof(Animator))]
public class CharacterMover : MonoBehaviour
{
    public float crouchHeight = 1.25f;
    public float walkSpeed = 1.3f;
    public float crouchSpeed = 0.8f;
    public float runSpeed = 4;

    private CharacterController cc;
    private Animator animator;
    private float moveSpeedMultiplier = 1;

    private void Awake()
    {
        cc = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();
    }

    public void Move(float x, float y, bool running, bool crouching)
    {
        var velocity = new Vector3(x, 0, y).normalized * GetSpeed(running && !crouching, crouching);
        cc.SimpleMove(velocity);

        var relativeVel = transform.InverseTransformDirection(velocity);
        animator.SetFloat("VelocityX", relativeVel.x);
        animator.SetFloat("VelocityZ", relativeVel.z);

        if (crouching)
        {
            cc.height = crouchHeight;
            cc.center = new Vector3(0, crouchHeight / 2, 0);
        }

        animator.SetBool("Crouching", crouching);
    }

    private float GetSpeed(bool running, bool crouching)
    {
        return (running ? runSpeed : crouching ? crouchSpeed : walkSpeed) * moveSpeedMultiplier;
    }

    public void MultiplyMoveSpeed(float multiplier)
    {
        moveSpeedMultiplier = multiplier;
    }
}
