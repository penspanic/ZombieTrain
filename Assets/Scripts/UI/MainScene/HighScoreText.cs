using UnityEngine;
using UnityEngine.UI;
using System.Collections;

namespace UI.MainScene
{
    public class HighScoreText : MonoBehaviour
    {
        [SerializeField]
        private Text _highScoreText;

        private void Awake()
        {
            if(PlayerPrefs.HasKey("HighScore") == true)
            {
                DynamicInfo.DynamicInfo.HighScore = PlayerPrefs.GetInt("HighScore");
            }

            _highScoreText.text = "HIGHSCORE:" + DynamicInfo.DynamicInfo.HighScore;
        }
    }
}