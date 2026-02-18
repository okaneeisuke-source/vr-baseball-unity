using UnityEngine;

public class AvatarThrowController : MonoBehaviour
{
    public Animator animator;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            animator.SetTrigger("Throw");
        }
    }
}