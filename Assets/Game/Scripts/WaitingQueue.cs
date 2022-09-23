using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FateGames;
using UnityEngine.Events;

public class WaitingQueue : MonoBehaviour
{
    [Range(1, 15)]
    [SerializeField] private int maxSize = 5;
    [SerializeField] private Transform startPoint;
    [SerializeField] private Transform spawnPoint;
    private List<Customer> customers = new List<Customer>();
    private Transform _transform;
    private List<Customer.Task> tasks = new List<Customer.Task>();
    private UnityEvent<Customer.Task> onNewTask = new UnityEvent<Customer.Task>();

    public int Size { get => customers.Count; }

    private void Awake()
    {
        _transform = transform;
        onNewTask.AddListener((task) =>
        {
            if (DequeueCustomer(out Customer customer))
            {
                customer.SetTask(task);
                tasks.Remove(task);
            }
        });
    }

    private void Start()
    {
        StartCoroutine(SpawnCustomerCoroutine(1, 4));
    }

    public bool SpawnCustomer() => SpawnCustomer(out Customer customer);

    public bool SpawnCustomer(out Customer customer)
    {
        customer = null;
        if (Size >= maxSize) return false;
        customer = ObjectPooler.Instance.SpawnFromPool("Customer", spawnPoint.position, spawnPoint.rotation).GetComponent<Customer>();
        if (tasks.Count > 0)
        {
            customer.SetTask(tasks[0]);
            tasks.RemoveAt(0);
            return true;
        }
        Vector3 destination;
        if (Size > 0)
            destination = customers[Size - 1].Destination - _transform.forward * Random.Range(0.8f, 1.5f);
        else
            destination = startPoint.position;
        customers.Add(customer);
        customer.SetDestination(destination);
        return true;
    }

    public bool DequeueCustomer(out Customer customer)
    {
        customer = null;
        if (Size == 0) return false;
        customer = customers[0];
        customers.RemoveAt(0);
        RearrangePositions();
        return true;
    }

    private void RearrangePositions()
    {
        if (Size == 0) return;
        customers[0].SetDestination(startPoint);
        for (int i = 1; i < customers.Count; i++)
        {
            Customer customerInFront = customers[i - 1];
            Customer customer = customers[i];
            Vector3 offset = -_transform.forward * Random.Range(0.8f, 1.5f);
            customer.SetDestination(customerInFront.Destination + offset);
        }
    }

    public void AddTask(Customer.Task task)
    {
        tasks.Add(task);
        onNewTask.Invoke(task);
    }

    private IEnumerator SpawnCustomerCoroutine(float minTime, float maxTime)
    {
        SpawnCustomer();
        yield return new WaitForSeconds(Random.Range(minTime, maxTime));
        StartCoroutine(SpawnCustomerCoroutine(minTime, maxTime));
    }

}
