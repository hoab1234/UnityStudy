using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerExample : MonoBehaviour
{
    Animator animator;

    SpriteRenderer sprite;

    InputAction moveAction;
    InputAction jumpAction;

    float speed = 6f;

    public GameObject jumpText;

    private readonly int IsWalkingHash = Animator.StringToHash("IsWalking");

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        sprite = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();

        moveAction = InputSystem.actions.FindAction("Move");
        jumpAction = InputSystem.actions.FindAction("Jump");

        jumpText.SetActive(false);
    }

    void Update()
    {
        Vector2 moveValue = moveAction.ReadValue<Vector2>();
        Debug.Log($"move Value = {moveValue}");

        bool isWalking = moveValue.x != 0;

        if(isWalking)
            sprite.flipX = moveValue.x > 0;

        animator.SetBool(IsWalkingHash, isWalking);

        Vector3 move = moveValue * speed * Time.deltaTime;
        
        transform.position += move;

        if(jumpAction.IsPressed())
        {
            StartCoroutine(SetActiveJumpText());
        }
    }

    IEnumerator SetActiveJumpText()
    {
        jumpText.SetActive(true);
        yield return new WaitForSeconds(1f);
        jumpText.SetActive(false);
    }
}
