using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TakeAwayPoint : MonoBehaviour
{
    [SerializeField] private WaitingQueue takeawayQueue;
    [SerializeField] private Transform interactionPoint;
    public Customer.TakeawayTask task { get; private set; } = null;

    private void Start()
    {
        CreateTask();
    }

    private void CreateTask()
    {
        Customer.TakeawayTask task = new Customer.TakeawayTask(interactionPoint, Random.value > 0.5f ? "Coffee" : "Book", Random.Range(1, 6));
        this.task = task;
        task.OnComplete.AddOneTimeListener(CreateTask);
        takeawayQueue.AddTask(task);
    }
}
