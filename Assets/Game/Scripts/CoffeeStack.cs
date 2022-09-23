using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FateGames;
using System.Linq;
public class CoffeeStack : CustomItemStack
{
    private List<Transform> cardboardSlots;
    private List<Cardboard> cardboards = new List<Cardboard>();


    private new void Awake()
    {
        base.Awake();
        cardboardSlots = GetChildrenWithTag(_transform, "Cardboard Slot");
        onRemove.AddListener((item) =>
        {
            if (Size % 4 == 0)
                cardboards[(Size + 1) / 4].gameObject.SetActive(false);
        });
        CreateCardboards();
    }

    private void CreateCardboards()
    {
        for (int i = 0; i < cardboardSlots.Count; i++)
        {
            Transform slot = cardboardSlots[i];
            Cardboard cardboard = Instantiate(PrefabManager.Instance.GetPrefab("Cardboard"), slot.position, slot.rotation).GetComponent<Cardboard>();
            cardboards.Add(cardboard);
            cardboard.gameObject.SetActive(false);
        }
    }
    protected override void OnPushStart()
    {
        base.OnPushStart();
        int index = Size / 4;
        Cardboard cardboard = cardboards[index];
        Transform slot = cardboardSlots[index];
        cardboard.Transform.SetPositionAndRotation(slot.position, slot.rotation);
        cardboard.gameObject.SetActive(true);
    }

    protected override void FixedUpdate()
    {
        base.FixedUpdate();
        if (immobile) return;
        if (collection.Count == 0) return;
        for (int i = 0; i < (Size / 4) + 1; i++)
        {
            Transform slot = cardboardSlots[i];
            Cardboard cardboard = cardboards[i];
            cardboard.Transform.SetPositionAndRotation(
                Vector3.Lerp(cardboard.Transform.position, slot.position, Time.fixedDeltaTime * itemAdjustingSpeed),
                Quaternion.Lerp(cardboard.Transform.rotation, slot.rotation, Time.fixedDeltaTime * itemAdjustingRotatingSpeed)
                );

        }
    }

    public override void Clear()
    {
        base.Clear();
        foreach (Cardboard cardboard in cardboards)
        {
            cardboard.gameObject.SetActive(false);
        }
    }


}
