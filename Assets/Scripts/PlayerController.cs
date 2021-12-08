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

    bool isInMovement = false;
    bool live = true;
    bool lBorder = false, rBorder = false;

    void Start()
    {
        animator = GetComponent<Animator>();
        character = GetComponent<CharacterController>();
    }

    void Update()
    {
        transform.position = new Vector3(Mathf.Clamp(transform.position.x, -3, 3), transform.position.y, transform.position.z);

        if (live)
        {
            float dir = Input.GetAxisRaw("Horizontal");

            if (!isInMovement && dir != 0)
            {
                isInMovement = true;
                currentDir = dir;
                currentDistanse = distance;
                if (dir > 0 && (!rBorder))
                {
                    PlayTriggerAnimation("Right");
                    time = AnimationLength(0);
                }
                if (dir < 0 && (!lBorder))
                {
                    Debug.Log("Turn Left");
                    PlayTriggerAnimation("Left");
                    time = AnimationLength(0);
                }
            }

            if (isInMovement)
                Move();
        }
    }

    void Move() 
    {
        if (currentDistanse <= 0)
        {
            isInMovement = false;
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
        if (other.CompareTag("Left Border"))
        {
            lBorder = true;
            Debug.Log($"lBorder = {lBorder}");
        }
        if (other.CompareTag("Right Border"))
        {
            rBorder = true;
            Debug.Log($"rBorder = {rBorder}");
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Left Border"))
        {
            lBorder = false;
            Debug.Log($"lBorder = {lBorder}");
        }
        if (other.CompareTag("Right Border"))
        {
            rBorder = false;
            Debug.Log($"rBorder = {rBorder}");
        }
    }

    float AnimationLength (int layerIndex)
    { 
        float length = animator.GetCurrentAnimatorStateInfo(layerIndex).length;
        return length;
    }
    void PlayTriggerAnimation(string name) => animator.SetTrigger(name);
}
