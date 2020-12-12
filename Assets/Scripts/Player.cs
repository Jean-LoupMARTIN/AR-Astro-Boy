using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public SmoothTranslate st;

    public Animator animator;

    public AnimationCurve jumpCurve;
    public float heightJump = 0.4f, jumpSpeed = 0.5f;

    [HideInInspector] public PlayerState state = PlayerState.WAIT;

    Cross target;
    Vector3 posStartJump, dirJump;
    float crtDistJump, distJump;

    void Update()
    {
        switch (state) {
            case PlayerState.WAIT:
                if (GameMan.inst.crosses.Count > 1)
                    StartJump();
                break;

            case PlayerState.JUMPING:
                UpdateJumping();
                break;
        }
    }

    void StartJump() {
        state = PlayerState.START_JUMP;
        animator.Play("Start Jump");

        posStartJump = transform.position;
        target = GameMan.inst.crosses[0];
        target.hidePoints();

        // rotation
        Vector3 targetProj = target.transform.position;
        targetProj.y = transform.position.y;
        Vector3 dirLook = Tool.Dir(this, targetProj, false);
        Quaternion rot = Quaternion.LookRotation(dirLook);
        transform.rotation = rot;
    }

    public void StartJumping() {
        state = PlayerState.JUMPING;

        dirJump = Tool.Dir(this, target, false);
        distJump = dirJump.magnitude;
        crtDistJump = 0;

        dirJump.Normalize();

        float tJumping = distJump / jumpSpeed;

        animator.SetFloat("jumpSpeed", 1 / tJumping);
    }

    public void EndJumping() {
        state = PlayerState.END_JUMP;
        transform.position = target.transform.position;
        Destroy(target.gameObject);
        GameMan.inst.crosses.RemoveAt(0);
    }

    public void StartWaiting() {
        state = PlayerState.WAIT;
    }


    void UpdateJumping()
    {
        crtDistJump += Time.deltaTime * jumpSpeed;

        float progress = Tool.Progress(crtDistJump, distJump);

        Vector3 pos = posStartJump;
        pos += crtDistJump * dirJump;
        pos += jumpCurve.Evaluate(progress) * distJump * heightJump * Vector3.up;

        transform.position = pos;
    }
}

public enum PlayerState { WAIT, START_JUMP, JUMPING, END_JUMP }