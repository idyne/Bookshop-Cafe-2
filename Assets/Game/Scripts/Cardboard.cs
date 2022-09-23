using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cardboard : MonoBehaviour
{
    private Transform _transform;

    public Transform Transform { get => _transform; }

    private void Awake()
    {
        _transform = transform;
    }
}
