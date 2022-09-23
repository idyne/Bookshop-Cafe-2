using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FateGames
{
    public class UIInGame : MonoBehaviour
    {
        private static UIInGame instance;
        [SerializeField] private UIMoney uiMoney;
        public static UIInGame Instance { get => instance; }
        public UIMoney UiMoney { get => uiMoney; }

        private void Awake()
        {
            if (instance)
            {
                DestroyImmediate(gameObject);
                return;
            }
            instance = this;
        }
    }

}
