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
            string minutes = Mathf.Floor(countdown / 59).ToString("00");
            string seconds = (countdown % 59).ToString("00");
            time.text = minutes+":"+seconds;
		}
	}
}