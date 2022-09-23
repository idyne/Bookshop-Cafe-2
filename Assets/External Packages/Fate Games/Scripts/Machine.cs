using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace FateGames.ArcadeIdle
{
    public abstract class Machine : MonoBehaviour
    {
        [SerializeField] protected string outcomeProductTag = "";
        [SerializeField] protected float productionPeriod = 1f;
        [SerializeField] protected Ingredient[] ingredients;
        [SerializeField] protected ItemStack outcomeStack;
        protected Dictionary<string, Ingredient> ingredientDictionary = new Dictionary<string, Ingredient>();
        protected UnityEvent<Stackable> onProduct = new UnityEvent<Stackable>();
        protected bool isProducing = false;

        public ItemStack OutcomeStack { get => outcomeStack; }

        public bool IsValidIngredient(string tag)
        {
            bool result = false;
            for (int i = 0; i < ingredients.Length; i++)
            {
                result = ingredients[i].Tag == tag;
                if (result)
                    break;
            }
            return result;
        }

        [System.Serializable]
        public class Ingredient
        {
            [SerializeField] private string tag;
            [SerializeField] private ItemStack stack;
            [SerializeField] private int requiredQuantity;

            public string Tag { get => tag; }
            public ItemStack Stack { get => stack; }
            public int RequiredQuantity { get => requiredQuantity; }
        }

        private void InitializeDictionary()
        {
            for (int i = 0; i < ingredients.Length; i++)
            {
                Ingredient ingredient = ingredients[i];
                ingredientDictionary.Add(ingredient.Tag, ingredient);
            }
        }

        protected void Awake()
        {
            InitializeDictionary();
            foreach (Ingredient ingredient in ingredientDictionary.Values)
                ingredient.Stack.OnAdd.AddListener((item) => { CheckAndProduce(); });
        }

        protected IEnumerator CheckAndProduceCoroutine()
        {
            if (CanProduce())
            {
                isProducing = true;
                yield return Produce();
                yield return new WaitForSeconds(productionPeriod);
                isProducing = false;
                CheckAndProduce();
            }
        }

        protected void CheckAndProduce()
        {
            StartCoroutine(CheckAndProduceCoroutine());
        }

        private bool CanProduce()
        {
            if (!outcomeStack.CanPush() || isProducing) return false;
            bool result = true;
            foreach (Ingredient ingredient in ingredientDictionary.Values)
            {
                result = ingredient.Stack.Size >= ingredient.RequiredQuantity;
                if (!result)
                    break;
            }
            return result;
        }

        public void TransferIngredient(string tag, IItemStack itemStack, bool audio = false, bool overrideWave = false)
        {
            itemStack.Transfer(ingredientDictionary[tag].Stack, audio, overrideWave);
        }

        protected IEnumerator Produce()
        {
            foreach (Ingredient ingredient in ingredientDictionary.Values)
            {
                for (int i = 0; i < ingredient.RequiredQuantity; i++)
                {
                    yield return DoSomethingWithInput(ingredient.Stack.Pop());
                }
            }
            Stackable product = ObjectPooler.Instance.SpawnFromPool(outcomeProductTag, outcomeStack.Transform.position, Quaternion.identity).GetComponent<Stackable>();
            yield return DoSomethingWithOutput(product);
            onProduct.Invoke(product);
        }

        protected abstract IEnumerator DoSomethingWithInput(Stackable stackable);
        protected abstract IEnumerator DoSomethingWithOutput(Stackable stackable);


    }
}
