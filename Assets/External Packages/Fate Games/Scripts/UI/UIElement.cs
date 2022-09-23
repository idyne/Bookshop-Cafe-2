using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
namespace FateGames
{
    public abstract class UIElement : MonoBehaviour
    {
        public void Show()
        {
            gameObject.SetActive(true);
        }

        public void Hide()
        {
            gameObject.SetActive(false);
        }
    }
}