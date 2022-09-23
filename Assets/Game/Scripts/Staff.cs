using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FateGames;
using FateGames.ArcadeIdle;

public class Staff : MonoBehaviour
{
    [SerializeField] private ItemStack stack;
    private CoffeeStack coffeeStack;
    private string currentStackableTag = null;
    public string CurrentStackableTag { get => currentStackableTag; }

    private void Awake()
    {
        coffeeStack = GetComponentInChildren<CoffeeStack>();
    }

    public Stackable PopItem()
    {
        Stackable item = null;
        if (currentStackableTag == "Coffee")
        {
            item = coffeeStack.Pop();
            if (coffeeStack.Size == 0)
                currentStackableTag = null;
        }
        else
        {
            item = stack.Pop();
            if (stack.Size == 0)
                currentStackableTag = null;
        }
        return item;
    }

    public Stackable PopCoffee()
    {
        Stackable coffee = null;
        if (currentStackableTag != "Coffee") return coffee;
        coffee = coffeeStack.Pop();
        if (coffeeStack.Size == 0)
            currentStackableTag = null;
        return coffee;
    }

    public void TransferItemOnStack(IItemStack otherStack)
    {
        if (currentStackableTag == "Coffee")
        {
            coffeeStack.Transfer(otherStack);
            if (coffeeStack.Size == 0) currentStackableTag = null;
        }
        else
        {
            stack.Transfer(otherStack);
            if (stack.Size == 0) currentStackableTag = null;
        }
    }

    public void TransferIngredientToMachine(Machine machine)
    {
        if (currentStackableTag == "Coffee")
        {
            if (coffeeStack.Size == 0 || !machine.IsValidIngredient(currentStackableTag)) return;
            machine.TransferIngredient(currentStackableTag, coffeeStack);
            if (coffeeStack.Size == 0) currentStackableTag = null;
        }
        else
        {
            if (stack.Size == 0 || !machine.IsValidIngredient(currentStackableTag)) return;
            machine.TransferIngredient(currentStackableTag, stack);
            if (stack.Size == 0) currentStackableTag = null;
        }
    }

    public void TakeItemFromStack(ItemStack otherStack)
    {
        if (currentStackableTag != null)
        {
            if (currentStackableTag == "Coffee")
                otherStack.Transfer(coffeeStack);
            else
                otherStack.Transfer(stack, currentStackableTag);
        }
        else
        {
            if (otherStack.Transfer(otherStack.TagOfLastItem == "Coffee" ? coffeeStack : stack, out Stackable item))
                currentStackableTag = item.StackableTag;
        }
    }

    public void TakeItemFromStack(CustomItemStack otherStack)
    {
        if (currentStackableTag != null)
        {
            if (currentStackableTag == "Coffee")
                otherStack.Transfer(coffeeStack);
            else
                otherStack.Transfer(stack);
        }
        else
        {
            if (otherStack.Transfer(otherStack.IsTagValid("Coffee") ? coffeeStack : stack, out Stackable item))
                currentStackableTag = item.StackableTag;
        }
    }

    public void TakeItemFromSupplier(Supplier supplier)
    {
        TakeItemFromStack(supplier.Stack);
    }

    public void TakeItemFromMachine(Machine machine)
    {
        TakeItemFromStack(machine.OutcomeStack);
    }

}
