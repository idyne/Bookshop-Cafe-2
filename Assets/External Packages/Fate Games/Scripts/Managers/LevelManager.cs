using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace FateGames
{
    public class LevelManager : MonoBehaviour
    {
        [SerializeField] private UnityEvent onStart, onSuccess, onFail;
        public void StartLevel()
        {
            onStart.Invoke();
        }
        public void FinishLevel(bool success)
        {
            GameManager.Instance.UpdateGameState(GameState.FINISHED);
            if (success) onSuccess.Invoke();
            else onFail.Invoke();
            SceneManager.FinishLevel(success);
        }
    }
}