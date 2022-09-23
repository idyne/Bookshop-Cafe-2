using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace FateGames
{
    public class UIMoney : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI moneyText;
        [SerializeField] private RectTransform moneyImageTransform;

        public RectTransform MoneyImageTransform { get => moneyImageTransform; }

        private void Awake()
        {
            GlobalEventDispatcher.Register("UPDATE_MONEY", UpdateMoneyText);
        }

        private void UpdateMoneyText()
        {
            int money = PlayerProgression.MONEY;
            string text;
            if (money > 1000000) text = (money / 1000000f).ToString("0.00").Replace(',', '.') + "M";
            else if (money > 10000) text = (money / 1000f).ToString("0.0").Replace(',', '.') + "K";
            else if (money > 1000) text = (money / 1000f).ToString("0.00").Replace(',', '.') + "K";
            else text = money.ToString();
            moneyText.text = text;
        }
    }

}
