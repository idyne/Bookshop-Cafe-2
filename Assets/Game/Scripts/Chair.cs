using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Chair : MonoBehaviour
{
    [SerializeField] private Table table;
    private Customer.CoffeeTask task = null;
    private UnityEvent<Customer.CoffeeTask> onNewTask = new UnityEvent<Customer.CoffeeTask>();

    private void Start()
    {
        CreateTask();
    }


    public void CreateTask()
    {
        task = new Customer.CoffeeTask(this);
        onNewTask.Invoke(task);
    }

    public UnityEvent<Customer.CoffeeTask> OnNewTask { get => onNewTask; }
    public Table Table { get => table; }
}
