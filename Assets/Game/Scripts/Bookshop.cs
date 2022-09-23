using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FateGames;

public class Bookshop : MonoBehaviour
{
    [SerializeField] private WaitingQueue waitingQueue;
    public List<Chair> chairs { get; private set; }
    public List<Bookshelf> bookshelves { get; private set; }
    [SerializeField] private Transform exitPoint;


    private void Awake()
    {
        chairs = new List<Chair>(GetComponentsInChildren<Chair>());
        bookshelves = new List<Bookshelf>(GetComponentsInChildren<Bookshelf>());

        foreach (Chair chair in chairs)
            chair.OnNewTask.AddListener(waitingQueue.AddTask);
        foreach (Bookshelf bookshelf in bookshelves)
            bookshelf.OnNewTask.AddListener(waitingQueue.AddTask);
    }

}
