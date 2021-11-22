using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField]
    float distance = 3f;

    float time;
    float currentDistanse = 0f;
    float currentDir = 0f;

    Animator animator;    
    CharacterController character;

    bool isInMivement = false;
    bool live = true;

    void Start()
    {
        animator = GetComponent<Animator>();
        character = GetComponent<CharacterController>();
    }

    void Update()
    {

        if (live)
        {
            float dir = Input.GetAxisRaw("Horizontal");

            if (!isInMivement && dir != 0)
            {
                isInMivement = true;
                currentDir = dir;
                currentDistanse = distance;
                if (dir > 0)
                {
                    PlayTriggerAnimation("Right");
                    time = animator.GetCurrentAnimatorStateInfo(0).length;
                }
                if (dir < 0)
                { 
                    PlayTriggerAnimation("Left");
                    time = animator.GetCurrentAnimatorStateInfo(0).length;
                }
        }

            if (isInMivement)
            {
                Move();
            } 
        }
    }

    void Move() 
    {
        if (currentDistanse <= 0)
        {
            isInMivement = false;
            return;
        }
        float speed = distance / time;
        float tmpDist = Time.deltaTime * speed;
        character.Move(Vector3.right * currentDir * tmpDist);
        currentDistanse -= tmpDist;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Danger"))
        {
            PlayTriggerAnimation("Death");
            live = false;
        }
    }

    void PlayTriggerAnimation(string name) { animator.SetTrigger(name); }
}
