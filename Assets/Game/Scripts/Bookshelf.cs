using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FateGames;
using UnityEngine.Events;

public class Bookshelf : MonoBehaviour
{
    [SerializeField] private CustomItemStack stack;
    [SerializeField] private CustomItemStack moneyStack;
    private Customer.BookTask task;
    private int numberOfAvaliableBooks = 0;
    private int numberOfBooksOfNextTask = 1;
    private UnityEvent<Customer.BookTask> onNewTask = new UnityEvent<Customer.BookTask>();
    [SerializeField] private Transform interactionPoint;

    private void Awake()
    {
        stack.OnAdd.AddListener((item) =>
        {
            numberOfAvaliableBooks++;
            CreateTask();
        });
    }

    private void Start()
    {
        SetNumberOfBooksOfNextTask();
    }

    private void CreateTask()
    {
        if (task != null || numberOfAvaliableBooks < numberOfBooksOfNextTask) return;
        task = new Customer.BookTask(this, numberOfBooksOfNextTask);
        numberOfAvaliableBooks -= numberOfBooksOfNextTask;
        SetNumberOfBooksOfNextTask();
        task.OnComplete.AddListener(() =>
        {
            task = null;
            CreateTask();
        });
        onNewTask.Invoke(task);
    }

    private void SetNumberOfBooksOfNextTask()
    {
        numberOfBooksOfNextTask = Random.Range(1, 4);
    }

    public CustomItemStack Stack { get => stack; }
    public UnityEvent<Customer.BookTask> OnNewTask { get => onNewTask; }
    public Transform InteractionPoint { get => interactionPoint; }
    public CustomItemStack MoneyStack { get => moneyStack; }
}
