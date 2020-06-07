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

	public float s = 3.5f;
	private float X;
	private float Y;


	// Start is called before the first frame update
	void Start()
	{
		game = GameObject.Find("Game").GetComponent<Game>();
		map = GameObject.Find("Environment").GetComponent<Environment>();

		moveCamera = false;

		mainCam.fieldOfView = 30;
	}

	// Update is called once per frame
	void Update()
	{
		if (moveCamera)
		{

			if (Input.GetMouseButton(0))
			{
				transform.Rotate(new Vector3(Input.GetAxis("Mouse Y") * s, -Input.GetAxis("Mouse X") * s, 0));
				X = transform.rotation.eulerAngles.x;
				Y = transform.rotation.eulerAngles.y;
				transform.rotation = Quaternion.Euler(X, Y, transform.rotation.z);
			}

			
			if (Input.GetAxis("Vertical") > 0)
			{
				mainCam.transform.position = transform.position + Camera.main.transform.forward * speed * Time.deltaTime;
				mainCam.transform.position = new Vector3(mainCam.transform.position.x, 110, mainCam.transform.position.z);
			}
			else if (Input.GetAxis("Vertical") < 0)
			{
				mainCam.transform.position = transform.position - Camera.main.transform.forward * speed * Time.deltaTime;
				mainCam.transform.position = new Vector3(mainCam.transform.position.x, 110, mainCam.transform.position.z);
			}

			if (Input.GetAxis("Horizontal") > 0)
			{
				mainCam.transform.position = transform.position + Camera.main.transform.right * speed * Time.deltaTime;
				mainCam.transform.position = new Vector3(mainCam.transform.position.x, 110, mainCam.transform.position.z);
			}
			else if (Input.GetAxis("Horizontal") < 0)
			{
				mainCam.transform.position = transform.position - Camera.main.transform.right * speed * Time.deltaTime;
				mainCam.transform.position = new Vector3(mainCam.transform.position.x, 110, mainCam.transform.position.z);
			}

			mainCam.fieldOfView += Input.mouseScrollDelta.y * -0.5f;

			mainCam.fieldOfView = Mathf.Clamp(mainCam.fieldOfView, camFOVmin, camFOVmax);
			mainCam.transform.position = new Vector3(Mathf.Clamp(mainCam.transform.position.x, minx, maxx), Mathf.Clamp(mainCam.transform.position.y, miny, maxy), Mathf.Clamp(mainCam.transform.position.z, minz, maxz));

			transform.position = new Vector3(transform.position.x, 110, transform.position.z);
		}
	}


	void rotatingCamera()
	{

	}
	public void setCamera(bool set)
	{
		moveCamera = set;
	}

	public void setClampValues(float miX, float miZ, float maX, float maZ)
	{
		minx = miX - 10;
		minz = miZ - 10;
		maxx = maX + 10;
		maxz = maZ + 10;

	}
}
