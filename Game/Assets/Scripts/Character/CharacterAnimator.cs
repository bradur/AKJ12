using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterAnimator : MonoBehaviour
{
    public Transform Legs;
    public Transform UpperBody;
    public Animator LegsAnimator;

    private Rigidbody2D rb;

    private float MAX_FORWARD_ANGLE = 135;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        var moveDir = rb.velocity.normalized;

        if (rb.velocity.magnitude < 0.1f)
        {
            moveDir = UpperBody.up;
            LegsAnimator.SetBool("run", false);
            LegsAnimator.SetBool("backwards", false);
        }

        if (Vector2.Angle(moveDir, UpperBody.up) < MAX_FORWARD_ANGLE)
        {
            var angle = Vector2.SignedAngle(Legs.up, moveDir);
            Legs.Rotate(Vector3.forward, angle);

            if (rb.velocity.magnitude > 0.1f)
            {
                LegsAnimator.SetBool("run", true);
                LegsAnimator.SetBool("backwards", false);
            }
        }
        else
        {
            var angle = Vector2.SignedAngle(Legs.up, moveDir) + 180;
            Legs.Rotate(Vector3.forward, angle);
            LegsAnimator.SetBool("run", true);
            LegsAnimator.SetBool("backwards", true);
        }
    }
    
}
