using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Text))]
public class RandomTextOnEnable : MonoBehaviour
{
	public string[] texts;

	void OnEnable()
	{
		GetComponent<Text>().text = texts[Random.Range(0, texts.Length)];
	}
}
