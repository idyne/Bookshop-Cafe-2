using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FateGames;
using FateGames.ArcadeIdle;
using BehaviorDesigner.Runtime;


[RequireComponent(typeof(TopDownCharacterMovement))]
[RequireComponent(typeof(Staff))]
public class Player : MonoBehaviour
{
    private Transform _transform;
    private Staff staff;
    [SerializeField] private BehaviorTree tree;
    private IEnumerator collectingMoneyRoutine = null;

    private void Awake()
    {
        _transform = transform;
        staff = GetComponent<Staff>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.H))
        {
            tree.SendEvent("Enter");
        }
    }

    private IEnumerator CollectMoneyCoroutine(CustomItemStack moneyStack, float time)
    {
        int k = Mathf.Clamp(10, 0, moneyStack.Size);
        for (int i = 0; i < k; i++)
        {
            Stackable money = moneyStack.Pop();
            money.Transform.ParabolicMove(_transform.position, time, () =>
            {
                money.gameObject.SetActive(false);
            });
        }
        yield return new WaitForSeconds(time);
        if (!moneyStack.IsEmpty)
        {
            collectingMoneyRoutine = CollectMoneyCoroutine(moneyStack, time);
            StartCoroutine(collectingMoneyRoutine);
        }
        else
            collectingMoneyRoutine = null;
    }


    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Supplier"))
        {
            Supplier supplier = other.GetComponent<Supplier>();
            staff.TakeItemFromSupplier(supplier);
        }
        else if (other.CompareTag("Machine"))
        {
            Machine machine = other.GetComponent<Machine>();
            staff.TransferIngredientToMachine(machine);
            staff.TakeItemFromMachine(machine);
        }
        else if (other.CompareTag("Bookshelf"))
        {
            Bookshelf bookshelf = other.GetComponent<Bookshelf>();
            staff.TransferItemOnStack(bookshelf.Stack);
        }
        else if (other.CompareTag("Table"))
        {
            Table table = other.GetComponent<Table>();
            if (table.TasksWaitingCoffee.Count > 0)
            {
                Stackable coffee = staff.PopCoffee();
                if (coffee)
                {
                    Customer.CoffeeTask task = table.TasksWaitingCoffee[0];
                    task.Serve(coffee.gameObject);
                }
            }
        }
        else if (other.CompareTag("Money Pile") && collectingMoneyRoutine == null)
        {
            CustomItemStack moneyStack = other.GetComponent<CustomItemStack>();
            if (!moneyStack.IsEmpty)
            {
                collectingMoneyRoutine = CollectMoneyCoroutine(moneyStack, Mathf.Clamp(2f / moneyStack.Size, 0.1f, 0.2f));
                StartCoroutine(collectingMoneyRoutine);
            }
        }
        else if (other.CompareTag("Take-away Point"))
        {
            TakeAwayPoint takeAwayPoint = other.GetComponent<TakeAwayPoint>();
            if (takeAwayPoint.task != null && takeAwayPoint.task.isReached && staff.CurrentStackableTag == takeAwayPoint.task.productTag)
                staff.TransferItemOnStack(staff.CurrentStackableTag == "Coffee" ? takeAwayPoint.task.customer.CoffeeStack : takeAwayPoint.task.customer.Stack);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Money Pile"))
        {
            if (collectingMoneyRoutine != null)
            {
                print("stop");
                StopCoroutine(collectingMoneyRoutine);
                collectingMoneyRoutine = null;
            }
        }
    }

}
