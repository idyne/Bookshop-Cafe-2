using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FateGames;

public class Supplier : MonoBehaviour
{
    [SerializeField] private ItemStack stack;
    [SerializeField] private string itemTag;
    private Truck truck;

    public ItemStack Stack { get => stack; }

    private void Awake()
    {
        truck = GetComponentInChildren<Truck>();
        stack.ValidItemTags.Add(itemTag);
        stack.OnFull.AddListener(() =>
        {
            CancelInvoke("AddItem");
            truck.Exit();
        });
        stack.OnEmpty.AddListener(() =>
        {
            truck.Enter();
        });
        truck.OnEnter.AddListener(() =>
        {
            InvokeRepeating("AddItem", 0, 0.5f);
        });
    }

    private void Start()
    {
        truck.Enter();
    }

    private void AddItem()
    {
        if (!stack.CanPush()) return;
        Stackable item = ObjectPooler.Instance.SpawnFromPool(itemTag, truck.UnloadingPoint.position, Quaternion.identity).GetComponent<Stackable>();
        stack.Push(item);
    }

    public void Transfer(ItemStack otherStack)
    {
        stack.Transfer(otherStack, itemTag);
    }
}
