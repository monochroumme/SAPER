using UnityEngine;
using UnityEngine.UI;

public class Timer : MonoBehaviour
{
	Text time;
	float countdown;

	void Start()
	{
		time = GetComponent<Text>();
	}

	void Update ()
	{
		if (GameManager.instance.isStarted && GameManager.instance.isPlaying)
		{
			countdown += Time.deltaTime;
			time.text = string.Format("{00:00:00}", countdown);
		}
	}
}