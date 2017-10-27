using UnityEngine;
using UnityEngine.UI;
using System.Collections;

namespace UI.MainScene
{
    public class MainScene : MonoBehaviour
    {
        [SerializeField]
        private Button _startButton;
        [SerializeField]
        private Button _creditButton;

        private void Awake()
        {
            _startButton.onClick.AddListener(OnStartButtonDown);
            _creditButton.onClick.AddListener(OnCreditButtonDown);
        }

        private void OnStartButtonDown()
        {
            DynamicInfo.DynamicInfo.SelectedStageId = "Stage1";
            UnityEngine.SceneManagement.SceneManager.LoadScene("InGame");
        }

        private void OnCreditButtonDown()
        {

        }
    }
}