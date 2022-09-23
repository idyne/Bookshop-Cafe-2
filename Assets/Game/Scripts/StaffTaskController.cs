using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StaffTaskController : MonoBehaviour
{
    public int numberOfEmptyBookSlots { get; private set; } = 0;
    [SerializeField] private Bookshop bookshop;

    public List<Bookshelf> bookshelves { get => bookshop.bookshelves; }
    public List<Chair> chairs { get => bookshop.chairs; }

    private List<StaffAI> staffs;

    private void Awake()
    {
        staffs = new List<StaffAI>(FindObjectsOfType<StaffAI>());
        foreach (Bookshelf bookshelf in bookshelves)
        {
            numberOfEmptyBookSlots += bookshelf.Stack.Limit;
            bookshelf.Stack.OnAdd.AddListener((book) =>
            {
                numberOfEmptyBookSlots++;
            });
            bookshelf.Stack.OnRemove.AddListener((book) =>
            {
                numberOfEmptyBookSlots--;
            });
        }
    }

    private void Start()
    {
        //TODO 
        /*
         burada stafflarý tetikle

         
         
         
         
         
         
         
         */
    }

}
