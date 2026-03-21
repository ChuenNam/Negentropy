/* using UnityEngine;

public class PlayerAnimation : Move
{
    protected virtual void OnIdle()
    {
        animator.SetBool("isWalk", false);
        Debug.Log("OnIdle");
    }
    
    protected override void OnWalk()
    {
        animator.SetBool("isWalk", true);
        Debug.Log("OnWalk");
    }

    protected override void OnJumpStart()
    {
        animator.SetBool("isJump", true);
    }

    protected override void OnLand()
    {
        animator.SetBool("isJump", false);
    }
} */