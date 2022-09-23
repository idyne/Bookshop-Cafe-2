using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Truck : MonoBehaviour
{
    private Animator animator;
    private UnityEvent onEnter = new UnityEvent(), onExit = new UnityEvent();
    private bool canEnter = true, canExit = false;
    [SerializeField] private Transform unloadingPoint;

    public UnityEvent OnEnter { get => onEnter; }
    public UnityEvent OnExit { get => onExit; }
    public Transform UnloadingPoint { get => unloadingPoint; }

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    public void Enter()
    {
        if (!canEnter) return;
        canEnter = false;
        animator.SetTrigger("Enter");
    }

    public void Exit()
    {
        if (!canExit) return;
        canExit = false;
        animator.SetTrigger("Exit");
    }

    public void InvokeOnEnter()
    {
        canExit = true;
        canEnter = false;
        onEnter.Invoke();
    }

    public void InvokeOnExit()
    {
        canExit = false;
        canEnter = true;
        onExit.Invoke();
    }
}
