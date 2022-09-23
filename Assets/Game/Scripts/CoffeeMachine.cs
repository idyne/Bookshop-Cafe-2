using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using FateGames;
using FateGames.ArcadeIdle;

public class CoffeeMachine : Machine
{
    protected override IEnumerator DoSomethingWithInput(Stackable stackable)
    {
        stackable.gameObject.SetActive(false);
        yield return null;
    }

    protected override IEnumerator DoSomethingWithOutput(Stackable stackable)
    {
        outcomeStack.Push(stackable);
        yield return null;
    }

}
