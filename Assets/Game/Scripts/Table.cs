using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using FateGames;

public class Table : MonoBehaviour
{
    [SerializeField] private CustomItemStack moneyStack;
    private List<Chair> chairs;
    private List<Customer.CoffeeTask> tasksWaitingCoffee = new List<Customer.CoffeeTask>();

    public List<Customer.CoffeeTask> TasksWaitingCoffee { get => tasksWaitingCoffee.ToList(); }
    public List<Chair> Chairs { get => chairs; }
    public CustomItemStack MoneyStack { get => moneyStack; }

    private void Awake()
    {
        chairs = new List<Chair>(GetComponentsInChildren<Chair>());
        foreach (Chair chair in chairs)
        {
            chair.OnNewTask.AddListener((task) =>
            {
                task.OnWaitingCoffee.AddListener(() =>
                {
                    tasksWaitingCoffee.Add(task);
                });
                task.OnCoffeeServed.AddListener(() =>
                {
                    tasksWaitingCoffee.Remove(task);
                });
            });
        }
    }

}
