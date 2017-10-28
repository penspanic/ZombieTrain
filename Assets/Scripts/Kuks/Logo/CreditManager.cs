using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;


public class CreditManager : MonoBehaviour {

	[SerializeField]LogoButtonManager logoManager;
	[SerializeField]Image img;
	[SerializeField]GameObject button;
	[SerializeField]List<GameObject> texts = new List<GameObject>();
	Dictionary<string,Vector3> textDefaultPositions = new Dictionary<string,Vector3>();
	
	/// <summary>
	/// Awake is called when the script instance is being loaded.
	/// </summary>
	void Awake()
	{
		textDefaultPositions = texts.ToDictionary(a=>a.name,b=>b.transform.position);
	}
	// Use this for initialization
	void Start () {
	}
	/// <summary>
	/// This function is called when the object becomes enabled and active.
	/// </summary>
	void OnEnable()
	{
		Debug.Log("turn On");
		StartCoroutine(TurnOn_Co());
	}
	public void ButtonClick()
	{
		StartCoroutine(TurnOff_Co());
	}
	IEnumerator TurnOn_Co()
	{
		img.DOFade(1,0.5f);
		yield return new WaitForSeconds(0.2f);
		foreach (GameObject g in texts)
		{
			yield return new WaitForSeconds(0.2f);
			g.transform.DOMove(new Vector3(0,0,0),0.5f,false);
		}
		yield return new WaitForSeconds(0.3f);
		button.SetActive(true);
	}
	IEnumerator TurnOff_Co()
	{
		StopCoroutine("TurnOn_Co");
		foreach (GameObject g in texts)
		{
			g.transform.position = textDefaultPositions[g.name];
		}
		button.SetActive(false);
		img.DOFade(0,0.5f);
		yield return new WaitForSeconds(0.5f);
		Debug.Log("Turn Off");
		this.gameObject.SetActive(false);
	}
}
