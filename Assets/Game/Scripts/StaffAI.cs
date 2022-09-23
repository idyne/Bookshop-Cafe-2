using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Staff))]
public class StaffAI : Agent
{
    private Staff staff;

    protected override void Awake()
    {
        base.Awake();
        staff = GetComponent<Staff>();
    }
}
