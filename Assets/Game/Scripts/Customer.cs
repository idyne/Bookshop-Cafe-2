using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;
using FateGames;
using System;


public class Customer : Agent
{
    private Task _task = null;
    [SerializeField] private ItemStack stack;
    [SerializeField] private CoffeeStack coffeeStack;
    [SerializeField] private Transform hand;
    public ItemStack Stack { get => stack; }
    public Task task { get => _task; }
    public CoffeeStack CoffeeStack { get => coffeeStack; }

    public void GetBook(Bookshelf bookshelf)
    {
        bookshelf.Stack.Transfer(stack);
        Pay(bookshelf.MoneyStack);
    }

    public void Pay(CustomItemStack moneyStack)
    {
        if (moneyStack.IsFull) return;
        Stackable money = ObjectPooler.Instance.SpawnFromPool("Money", _transform.position, Quaternion.identity).GetComponent<Stackable>();
        moneyStack.Push(money);
    }

    public void Drink()
    {
        animator.SetTrigger("Drink");
    }

    public void Deactivate()
    {
        gameObject.SetActive(false);
        stack.Clear();
        stack.ClearEvents();
        coffeeStack.Clear();
        coffeeStack.ClearEvents();
    }

    public void SetTask(Task task)
    {
        _task = task;
        task.SetCustomer(this);
        task.Start();
    }

    #region Task

    public abstract class Task
    {
        public Customer customer { get; protected set; } = null;
        protected UnityEvent onComplete = new UnityEvent();
        public UnityEvent OnComplete { get => onComplete; }
        public void SetCustomer(Customer customer) => this.customer = customer;
        public abstract void Start();
        public abstract void Complete();
    }

    public class TakeawayTask : Task
    {
        public Transform interactionPoint;
        public string productTag;
        public int numberOfProducts = 1;
        public bool isReached { get; private set; } = false;

        public TakeawayTask(Transform interactionPoint, string productTag, int numberOfProducts)
        {
            print("Take-away customer: " + numberOfProducts + " " + productTag);
            this.interactionPoint = interactionPoint;
            this.productTag = productTag;
            this.numberOfProducts = numberOfProducts;
        }

        public override void Complete()
        {
            //TODO doldur
            onComplete.Invoke();
            customer.SetDestination(Vector3.zero);
            customer.onReached.AddOneTimeListener(customer.Deactivate);
        }

        public override void Start()
        {
            if (productTag == "Coffee")
            {
                customer.coffeeStack.OnAdd.AddConditionalOneTimeListener((item) =>
                {
                    if (customer.coffeeStack.Size >= numberOfProducts)
                        Complete();
                }, () =>
                {
                    return customer.coffeeStack.Size >= numberOfProducts;
                });
            }
            else
            {
                customer.stack.ValidItemTags.Add(productTag);
                customer.stack.OnAdd.AddConditionalOneTimeListener((item) =>
                {
                    if (customer.stack.Size >= numberOfProducts)
                        Complete();
                }, () =>
                {
                    return customer.stack.Size >= numberOfProducts;
                });
            }
            customer.SetDestination(interactionPoint);
            customer.onReached.AddOneTimeListener(() =>
            {
                isReached = true;
            });

        }
    }
    public class BookTask : Task
    {
        private Bookshelf bookshelf;
        private int numberOfBooks;

        public BookTask(Bookshelf bookshelf, int numberOfBooks)
        {
            this.bookshelf = bookshelf;
            this.numberOfBooks = numberOfBooks;
        }

        public Bookshelf Bookshelf { get => bookshelf; }
        public int NumberOfBooks { get => numberOfBooks; }

        public override void Start()
        {
            customer.SetDestination(bookshelf.InteractionPoint);
            customer.onReached.AddOneTimeListener(() =>
            {
                customer.StartCoroutine(GetBook(bookshelf, numberOfBooks, Complete));
            });
        }

        private IEnumerator GetBook(Bookshelf bookshelf, int numberOfBooks, Action onComplete)
        {
            if (numberOfBooks == 0) onComplete();
            else
            {
                yield return new WaitUntil(customer.Stack.CanPush);
                customer.GetBook(bookshelf);
                customer.StartCoroutine(GetBook(bookshelf, numberOfBooks - 1, onComplete));
            }
        }

        public override void Complete()
        {
            customer.SetDestination(new Vector3(10, 0, 15));
            onComplete.Invoke();
            customer.onReached.AddListener(customer.Deactivate);
        }
    }

    public class CoffeeTask : Task
    {
        private Chair chair;
        private UnityEvent onWaitingCoffee = new UnityEvent(), onCoffeeServed = new UnityEvent();

        public CoffeeTask(Chair chair)
        {
            this.chair = chair;
        }

        public UnityEvent OnWaitingCoffee { get => onWaitingCoffee; }
        public UnityEvent OnCoffeeServed { get => onCoffeeServed; }

        public override void Start()
        {
            customer.SetDestination(chair.transform);
            customer.onReached.AddOneTimeListener(() =>
            {
                onWaitingCoffee.Invoke();
            });
        }

        public void Serve(GameObject coffee)
        {
            coffee.transform.ParabolicMove(customer.hand.position, 0.2f, () =>
            {
                coffee.transform.parent = customer.hand;
                customer.Drink();
            });
            onCoffeeServed.Invoke();
        }

        public override void Complete()
        {
            customer.hand.Find("Coffee(Clone)").gameObject.SetActive(false);
            customer.Pay(chair.Table.MoneyStack);
            chair.CreateTask();
            customer.SetDestination(new Vector3(10, 0, 15));
            customer.onReached.AddOneTimeListener(() =>
            {
                onComplete.Invoke();
                customer.Deactivate();
            });
        }
    }
    #endregion
}
