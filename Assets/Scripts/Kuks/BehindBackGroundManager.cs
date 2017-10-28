using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BehindBackGroundManager : MonoBehaviour {

	public static BehindBackGroundManager instance=null;

	[SerializeField]List<float> _lastHurdlesXPosition = new List<float>();
	[SerializeField]float _xMinLimit=0.0f;
	[SerializeField]float _xMaxLimit=1000.0f;
	[SerializeField]float _timeOfRegen=2.0f;
	[SerializeField]float _DistOfHurdles=5.0f;
	int _maxHurdles;

	[SerializeField]GameObject _hurdleObj;
	// Use this for initialization
	void Start () {
		_maxHurdles = (int)((_xMaxLimit-_xMinLimit)/(_DistOfHurdles*2));
		StartCoroutine(StartRegenHurdle_Co());
	}
	
	IEnumerator StartRegenHurdle_Co()
	{
		WaitForSeconds regenDelay = new WaitForSeconds(_timeOfRegen);
		while (true)
		{
			yield return regenDelay;
			RegenHurdle();
		}
	}
	void RegenHurdle()
	{
		float newXPosition = LoadXPositon();
		if (newXPosition>_xMaxLimit)
		{
			Debug.Log("Full Hurdle");
			return;
		}
		for (int i = 0; i < 1000; i++)
		{
			if (CheckNotClosedXposition(newXPosition))
			{
				CreateHurdle(newXPosition);
				return;
			}
		}
	}
	void CreateHurdle(float newXPosition)
	{
		_lastHurdlesXPosition.Add(newXPosition);
		Vector3 newPosition = new Vector3(newXPosition,0,0);
		GameObject obj = Instantiate(_hurdleObj,newPosition,Quaternion.identity) as GameObject;
	}
	float LoadXPositon()
	{
		if (IsMax())
		{
			return _xMaxLimit+1;
		}
		float returnFloat = CallRandomFloat();
		return CallRandomFloat();
	}
	float CallRandomFloat()
	{
		return Random.Range(_xMinLimit,_xMaxLimit);
	}
	bool CheckNotClosedXposition(float nowXPos)
	{
		foreach (float f in _lastHurdlesXPosition)
		{
			if (f+_DistOfHurdles>nowXPos&&f-_DistOfHurdles<nowXPos)
			{
				return false;
			}
		}
		return true;
	}
	bool IsMax()
	{
		if(_maxHurdles<_lastHurdlesXPosition.Count+2)
		{
			return true;
		}else
		{
			return false;
		}
	}
}
