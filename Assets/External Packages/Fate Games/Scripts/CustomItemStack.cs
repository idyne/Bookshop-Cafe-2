using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.Events;

namespace FateGames
{
    public class CustomItemStack : MonoBehaviour, IItemStack
    {
        protected List<Transform> slots;
        protected Transform _transform;
        protected List<Stackable> collection = new List<Stackable>();
        protected int limit = -1;
        [SerializeField] protected string validStackableTag = "";
        [SerializeField] protected float itemAdjustingSpeed = 40;
        [SerializeField] protected float itemAdjustingRotatingSpeed = 45;
        private AnimationCurve curve = new AnimationCurve(new Keyframe(0, 0), new Keyframe(0.5f, 1.5f), new Keyframe(1, 0));
        [SerializeField] protected bool immobile = true;
        [SerializeField] protected float pushPeriod = 0.2f;
        protected UnityEvent<Stackable> onAdd = new UnityEvent<Stackable>();
        protected UnityEvent<Stackable> onRemove = new UnityEvent<Stackable>();
        protected Queue<PushAction> pushQueue = new Queue<PushAction>();
        [SerializeField] protected int maxNumberOfParallelPushes = 10;
        protected int numberOfRunningPushes = 0;
        public bool CanPush() => numberOfRunningPushes < maxNumberOfParallelPushes && !IsFull;

        public int Limit { get => limit; }
        public int Size { get => collection.Count; }

        public bool IsFull { get => Limit >= 0 && Size + numberOfRunningPushes >= Limit; }
        public bool IsEmpty { get => Size == 0; }
        public UnityEvent<Stackable> OnAdd { get => onAdd; }
        public UnityEvent<Stackable> OnRemove { get => onRemove; }


        protected void Awake()
        {
            _transform = transform;
            slots = GetChildrenWithTag(_transform, "Item Slot");
            limit = slots.Count;
        }

        protected virtual void FixedUpdate()
        {
            if (immobile) return;
            if (collection.Count == 0) return;
            for (int i = 0; i < collection.Count; i++)
            {
                Stackable currentItem = collection[i];
                Transform slot = slots[i];
                currentItem.Transform.SetPositionAndRotation(
                    Vector3.Lerp(currentItem.Transform.position, slot.position, Time.fixedDeltaTime * itemAdjustingSpeed),
                    Quaternion.Lerp(currentItem.Transform.rotation, slot.rotation, Time.fixedDeltaTime * itemAdjustingRotatingSpeed)
                    );

            }
        }
        #region Curve

        protected void SetCurve()
        {
            Keyframe newKey = curve.keys[1];
            newKey.value = 1.5f + collection.Count * 0.2f;
            curve.MoveKey(1, newKey);
        }
        #endregion
        protected List<Transform> GetChildrenWithTag(Transform parent, string tag)
        {
            List<Transform> result = new List<Transform>();
            for (int i = 0; i < parent.childCount; i++)
            {
                Transform child = parent.GetChild(i);
                if (child.CompareTag(tag)) result.Add(child);
            }
            return result;
        }

        public struct PushAction
        {
            public readonly Stackable item;
            public readonly bool audio;
            public readonly bool overrideWave;

            public PushAction(Stackable item, bool audio, bool overrideWave) : this()
            {
                this.item = item;
                this.audio = audio;
                this.overrideWave = overrideWave;
            }


        }

        // Adds the item to the end of the stack.
        public void Push(Stackable newItem, bool audio = false, bool overrideWave = false)
        {
            if (Size >= slots.Count)
            {
                Debug.LogError("Stack is full!", this);
                pushQueue.Clear();
                return;
            }
            if (!CanPush())
            {
                pushQueue.Enqueue(new PushAction(newItem, audio, overrideWave));
                return;
            }
            numberOfRunningPushes++;
            OnPushStart();
            //SetCurve();
            Transform slot = slots[Size + numberOfRunningPushes - 1];
            Vector3 start = newItem.Transform.position;
            Vector3 end = slot.position;
            DOTween.To((val) =>
            {
                Vector3 pos = Vector3.Lerp(start, end, val);
                pos.y += curve.Evaluate(val);
                newItem.Transform.position = pos;
                newItem.Transform.rotation = Quaternion.Lerp(newItem.Transform.rotation, slot.rotation, Time.fixedDeltaTime * itemAdjustingRotatingSpeed);
            }, 0, 1, pushPeriod).OnComplete(() =>
            {
                numberOfRunningPushes--;
                OnPushComplete(newItem);
            });
        }

        protected virtual void OnPushStart()
        {

        }

        protected void OnPushComplete(Stackable newItem)
        {
            collection.Add(newItem);
            onAdd.Invoke(newItem);
            if (pushQueue.Count > 0)
            {
                PushAction pushAction = pushQueue.Dequeue();
                Push(pushAction.item, pushAction.audio, pushAction.overrideWave);
            }
        }

        protected Stackable RemoveAt(int index)
        {
            if (index < 0 || index >= collection.Count)
            {
                //Debug.LogError("Cannot remove from empty stack!", this);
                return null;
            }
            Stackable objectToPop = collection[index];
            collection.RemoveAt(index);
            onRemove.Invoke(objectToPop);
            return objectToPop;
        }

        public Stackable Pop()
        {
            return RemoveAt(collection.Count - 1);
        }

        public bool Transfer(IItemStack otherStack, bool audio = false, bool overrideWave = false)
        {
            if (!otherStack.CanPush() || !otherStack.IsTagValid(validStackableTag)) return false;
            bool result = false;
            Stackable item = Pop();
            if (item)
            {
                otherStack.Push(item, audio, overrideWave);
                result = true;
            }
            return result;
        }

        public bool Transfer(IItemStack otherStack, out Stackable item, bool audio = false, bool overrideWave = false)
        {
            item = null;
            if (!otherStack.CanPush() || !otherStack.IsTagValid(validStackableTag)) return false;
            bool result = false;
            item = Pop();
            if (item)
            {
                otherStack.Push(item, audio, overrideWave);
                result = true;
            }
            return result;
        }



        public bool IsTagValid(string stackableTag)
        {
            return stackableTag == validStackableTag;
        }

        public virtual void Clear()
        {
            for (int i = 0; i < collection.Count; i++)
                collection[i].gameObject.SetActive(false);
            collection.Clear();
        }

        public virtual void ClearEvents()
        {
            onAdd.RemoveAllListeners();
            onRemove.RemoveAllListeners();
        }
    }

}
