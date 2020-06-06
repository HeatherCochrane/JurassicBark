using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DayNightCycle : MonoBehaviour
{
	[SerializeField]
	Light mainLight;
	//https://gamedev.stackexchange.com/questions/118305/how-do-i-lerp-text-color-over-time
	IEnumerator UpdateLightColor(Color32 start, Color32 end)
	{
		float t = 0;
		while (t < 1)
		{
			// Now the loop will execute on every end of frame until the condition is true
			mainLight.color = Color.Lerp(start, end, t);
			t += Time.deltaTime / 2;
			yield return new WaitForEndOfFrame(); // So that I return something at least.
		}
	}


	IEnumerator dayTimer()
	{
		float dayLength = 0;
		float totalTime = 20;

		while(dayLength <= totalTime)
		{
			dayLength += Time.deltaTime;

			yield return null;
		}

		nightCycle();
	}

	[SerializeField]
	GameObject skip;

	[SerializeField]
	VisitorHandler visitorHandler;
	// Start is called before the first frame update
	void Start()
    {
		skip.SetActive(false);
	}

    // Update is called once per frame
    void Update()
    {
        
    }

	public void dayCycle()
	{
		StopAllCoroutines();
		visitorHandler.parkOpen();
		StartCoroutine(UpdateLightColor(new Color32(56, 24, 77, 1), new Color32(255, 255, 255, 1)));
		StartCoroutine("dayTimer");
		skip.SetActive(false);
	}

	public void nightCycle()
	{
		StopAllCoroutines();
		visitorHandler.parkClosed();
		StartCoroutine(UpdateLightColor(new Color32(255, 255, 255, 1), new Color32(56, 24, 77, 1)));
		Invoke("showSkipButton", 5);
	}

	public void showSkipButton()
	{
		skip.SetActive(true);
	}

}
