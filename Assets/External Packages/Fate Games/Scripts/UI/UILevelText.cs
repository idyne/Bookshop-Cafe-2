using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace FateGames
{
    public class UILevelText : UIElement
    {
        [SerializeField] private TextMeshProUGUI levelText;

        private void Awake()
        {
            levelText.text = "Level " + PlayerProgression.CurrentLevel;
        }

    }
}