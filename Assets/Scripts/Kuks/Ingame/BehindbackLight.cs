using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class BehindbackLight : MonoBehaviour {

	[SerializeField]float _timeMinDelay = 3.0f;
	[SerializeField]float _timeMaxDelay = 5.0f;
	[SerializeField]float _lightDelay = 0.2f;
	[SerializeField]float _durationDelay = 0.9f;
	SpriteRenderer _backGround;
	// Use this for initialization
	/// <summary>
	/// Awake is called when the script instance is being loaded.
	/// </summary>
	void Awake()
	{
		_backGround = GetComponent<SpriteRenderer>();
	}
	void Start () {
		StartCoroutine(Lighting_Co());
	}
	
	IEnumerator Lighting_Co()
	{
		while (true)
		{
			float randomDelayTime = Random.Range(_timeMinDelay,_timeMaxDelay);
			yield return new WaitForSeconds(randomDelayTime);
			Debug.Log("yellow");
			yield return new WaitForSeconds(_lightDelay);
			_backGround.DOColor(new Vector4(255,255,0,1),_lightDelay);
			yield return new WaitForSeconds(_durationDelay);
			_backGround.DOColor(new Vector4(0,0,0,1),_lightDelay);
			Debug.Log("black");
			yield return new WaitForSeconds(_lightDelay);
		}
	}

}
