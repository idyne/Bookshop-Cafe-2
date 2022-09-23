using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FateGames
{
    public class AutoSaver : MonoBehaviour
    {
        [SerializeField] private float period = 10;
        private void Start()
        {
            StartCoroutine(AutoSaveCoroutine());
        }

        private IEnumerator AutoSaveCoroutine()
        {
            yield return new WaitForSeconds(period);
            SaveManager.Save(PlayerProgression.PlayerData);
            StartCoroutine(AutoSaveCoroutine());
        }
    }

}
