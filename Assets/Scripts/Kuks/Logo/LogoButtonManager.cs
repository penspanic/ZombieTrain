using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LogoButtonManager : MonoBehaviour {

	[SerializeField]GameObject _creditObject;
	[SerializeField]Image _buttonImage;
	[SerializeField]Text _buttonText;
	[SerializeField]float _switchDelay = 0.3f;
	[SerializeField]float _fadeTime = 0.7f;
	[SerializeField]Sprite _switchSprite2;
	[SerializeField]Sprite _switchSprite3;
	bool _startClicked = false;
	
	/// <summary>
	/// Awake is called when the script instance is being loaded.
	/// </summary>
	void Awake()
	{

	}

	// Use this for initialization
	void Start () {
		FadeFilter.instance.FadeIn(Color.black,_fadeTime);
	}
	public void ButtonClick()
	{
		if(!_startClicked)
		StartCoroutine(ButtonClick_Co());
	}
	public void CreditsClick()
	{
		_creditObject.SetActive(true);
	}
	public void CreditsClose()
	{
		_creditObject.SetActive(false);
	}
	IEnumerator ButtonClick_Co()
	{
		_startClicked = true;
		_buttonText.text = "";
		yield return new WaitForSeconds(_switchDelay);
		_buttonImage.sprite = _switchSprite2;
		AppSound.instance.SE_button.Play();
		yield return new WaitForSeconds(_switchDelay);
		_buttonImage.sprite = _switchSprite3;
		AppSound.instance.SE_button.Play();
		yield return new WaitForSeconds(_switchDelay);
		FadeFilter.instance.FadeOut(Color.black,_fadeTime);
		yield return new WaitForSeconds(_fadeTime);
		SceneManager.LoadScene(1);
	}
	
}
