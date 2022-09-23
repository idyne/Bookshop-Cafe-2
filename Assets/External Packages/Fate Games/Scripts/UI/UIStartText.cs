using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
namespace FateGames
{
    public class UIStartText : UIElement
    {
        [SerializeField] private ControlTutorial[] tutorials;
        private Dictionary<ControlType, GameObject> tutorialDictionary = new Dictionary<ControlType, GameObject>();
        [System.Serializable]
        private class ControlTutorial
        {
            [SerializeField] private ControlType type;
            [SerializeField] private GameObject tutorial;

            public GameObject Tutorial { get => tutorial; }
            public ControlType Type { get => type; }
        }

        private void Start()
        {
            for (int i = 0; i < tutorials.Length; i++)
            {
                ControlTutorial tutorial = tutorials[i];
                tutorialDictionary.Add(tutorial.Type, tutorial.Tutorial);
            }
            tutorialDictionary[GameManager.Instance.ControlType].SetActive(true);
        }

    }
}