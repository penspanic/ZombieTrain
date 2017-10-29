using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using DG.Tweening;

public class GameResultManager : MonoBehaviour {

	public static GameResultManager instance = null;

	[SerializeField]float _fadeTime=0.5f;
	[SerializeField]float _waitingTime=1.0f;
	[SerializeField]Text _gameOverText;

	void Awake()
	{
		instance = this;
	}
	// Use this for initialization
	void Start () {
		FadeFilter.instance.FadeIn(Color.black,_fadeTime);
		_gameOverText.text = "";
	}
	/// <summary>
	/// Update is called every frame, if the MonoBehaviour is enabled.
	/// </summary>
	void Update()
	{
		if (Input.GetMouseButton(0))
		{
			GameClear();
		}
	}
	public void GameOver()
	{
		StartCoroutine(GAMEEND_Co("GAME OVER"));
	}
	public void GameClear()
	{
		StartCoroutine(GAMEEND_Co("GAME CLEAR"));
	}
	IEnumerator GAMEEND_Co(string result)
	{
		FadeFilter.instance.FadeOut(Color.black,_fadeTime);
		yield return new WaitForSeconds(_fadeTime);
		_gameOverText.text = result;
		yield return new WaitForSeconds(_waitingTime);
		_gameOverText.DOFade(0,_fadeTime);
		SceneManager.LoadScene(0);

	}
}
