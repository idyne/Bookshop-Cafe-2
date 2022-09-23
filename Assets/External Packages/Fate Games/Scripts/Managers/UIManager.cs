using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace FateGames
{
    public class UIManager : MonoBehaviour
    {
        private static UIWinScreen winScreen;
        private static UILoseScreen loseScreen;
        private static UILevelText levelText;
        private static UIStartText startText;
        private static UILoadingScreen loadingScreen;
        public static UIWinScreen WinScreen { get => winScreen; }
        public static UILoseScreen LoseScreen { get => loseScreen; }
        public static UILevelText LevelText { get => levelText; }
        public static UIStartText StartText { get => startText; }
        public static UILoadingScreen LoadingScreen { get => loadingScreen; }

        public static void CreateUILevelText()
        {
            levelText = Instantiate(PrefabManager.Instance.GetPrefab("UILevelText")).GetComponent<UILevelText>();
        }
        public static void CreateUIWinScreen()
        {
            winScreen = Instantiate(PrefabManager.Instance.GetPrefab("UIWinScreen")).GetComponent<UIWinScreen>();
        }
        public static void CreateUILoseScreen()
        {
            loseScreen = Instantiate(PrefabManager.Instance.GetPrefab("UILoseScreen")).GetComponent<UILoseScreen>();
        }
        public static void CreateUIStartText()
        {
            startText = Instantiate(PrefabManager.Instance.GetPrefab("UIStartText")).GetComponent<UIStartText>();
        }
        public static void CreateUILoadingScreen()
        {
            loadingScreen = Instantiate(PrefabManager.Instance.GetPrefab("UILoadingScreen")).GetComponent<UILoadingScreen>();
        }
    }
}
