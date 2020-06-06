using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControl : MonoBehaviour
{

	Environment map;
	//Camera
	public Camera mainCam;
	Game game;


	Vector3 pointerPosition;

	float speed = 50.0f;

	bool moveCamera = false;


	//Camera Bounds
	float minz = -30;
	float maxz = 30;

	float camFOVmax = 30;
	float camFOVmin = 10;

	float minx = 0;
	float maxx = 0;
	float miny = 0;
	float maxy = 0;


	// Start is called before the first frame update
	void Start()
	{
		game = GameObject.Find("Game").GetComponent<Game>();
		map = GameObject.Find("Environment").GetComponent<Environment>();

		minx = -map.getMapSize().x * 5;
		maxx = map.getMapSize().x * 5;

		miny = -map.getMapSize().y * 5;
		maxy = map.getMapSize().y * 5;

		minz = -map.getMapSize().x * 7;
		maxz = -map.getMapSize().x + 100;

		moveCamera = false;

		mainCam.fieldOfView = 30;
	}

	// Update is called once per frame
	void Update()
	{
		if (moveCamera)
		{
			if (Input.mousePosition.x >= Screen.width - 10)
			{
				mainCam.transform.position += new Vector3(speed * Time.deltaTime, 0, 0);
			}
			else if (Input.mousePosition.x <= 0)
			{
				mainCam.transform.position -= new Vector3(speed * Time.deltaTime, 0, 0);
			}

			if (Input.mousePosition.y >= Screen.height - 10)
			{
				mainCam.transform.position += new Vector3(0, 0, speed * Time.deltaTime);
			}
			else if (Input.mousePosition.y <= 0)
			{
				mainCam.transform.position -= new Vector3(0, 0, speed * Time.deltaTime);
			}

			mainCam.fieldOfView += Input.mouseScrollDelta.y * -0.5f;

			mainCam.fieldOfView = Mathf.Clamp(mainCam.fieldOfView, camFOVmin, camFOVmax);
			mainCam.transform.position = new Vector3(Mathf.Clamp(mainCam.transform.position.x, minx, maxx), Mathf.Clamp(mainCam.transform.position.y, miny, maxy), Mathf.Clamp(mainCam.transform.position.z, minz, maxz));
		}
	}

	public void setCamera(bool set)
	{
		moveCamera = set;
	}
}
