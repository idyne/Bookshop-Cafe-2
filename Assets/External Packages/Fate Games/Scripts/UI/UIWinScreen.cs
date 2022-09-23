using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
namespace FateGames
{
    public class UIWinScreen : MonoBehaviour
    {
        // Called by ContinueButton onClick
        public void Continue()
        {
            SceneManager.LoadCurrentLevel();
        }
    }
}