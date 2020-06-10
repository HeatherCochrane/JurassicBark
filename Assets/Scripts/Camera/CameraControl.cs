using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControl : MonoBehaviour
{
	public static CameraControl instance;

	public Transform followTransform;

	[SerializeField]
	Transform cameraTransform;

	public float normalSpeed;
	public float fastSpeed;
	public float movementSpeed;
	public float movementTime;
	public float rotationAmount;
	public Vector3 zoomAmount;

	[SerializeField]
	Vector3 newPosition;
	[SerializeField]
	Quaternion newRotation;
	[SerializeField]
	Vector3 newZoom;


	//Environment map;
	Game game;
	bool moveCamera = false;
	// Start is called before the first frame update
	void Start()
	{
		instance = this;

		game = GameObject.Find("Game").GetComponent<Game>();
		//map = GameObject.Find("Environment").GetComponent<Environment>();

		newPosition = transform.position;
		newRotation = transform.rotation;
		newZoom = cameraTransform.localPosition;
	}

	// Update is called once per frame
	void Update()
	{
		if (moveCamera)
		{
			if (followTransform != null)
			{
				transform.position = followTransform.position;
			}
			else
			{
				HandleMovementInput();
			}

			if (Input.GetKeyDown(KeyCode.Escape))
			{
				followTransform = null;
			}

			HandleZoom();
		}
		
	}

	void HandleZoom()
	{

		newZoom += Input.mouseScrollDelta.y * zoomAmount;

		newZoom = new Vector3(newZoom.x, Mathf.Clamp(newZoom.y, 60, 660), Mathf.Clamp(newZoom.z, -660, -60));
		cameraTransform.localPosition = Vector3.Lerp(cameraTransform.localPosition, newZoom, Time.deltaTime * movementTime);
	}
	void HandleMovementInput()
	{
		if(Input.GetKey(KeyCode.LeftShift))
		{
			movementSpeed = fastSpeed;
		}
		else
		{
			movementSpeed = normalSpeed;
		}

		if (Input.mousePosition.y >= Screen.height - 10)
		{
			newPosition += (transform.forward * movementSpeed);
		}
		else if (Input.mousePosition.y <= 0)
		{
			newPosition += (transform.forward * -movementSpeed);
		}
		if (Input.mousePosition.x >= Screen.width - 10)
		{
			newPosition += (transform.right * movementSpeed);
		}
		else if (Input.mousePosition.x <= -0)
		{
			newPosition += (transform.right * -movementSpeed);
		}

		transform.position = Vector3.Lerp(transform.position, newPosition, Time.deltaTime * movementTime);

		//if(Input.GetKey(KeyCode.Q))
		//{
		//	newRotation *= Quaternion.Euler(Vector3.up * rotationAmount);
		//}
		//if(Input.GetKey(KeyCode.E))
		//{
		//	newRotation *= Quaternion.Euler(Vector3.up * -rotationAmount);
		//}

		//transform.rotation = Quaternion.Lerp(transform.rotation, newRotation, Time.deltaTime * movementTime);

	}

	void rotatingCamera()
	{

	}
	public void setCamera(bool set)
	{
		moveCamera = set;
		game.setMoveCamera(set);
	}

	public void setClampValues(float miX, float miZ, float maX, float maZ)
	{
		//minx = miX;
		//minz = miZ;
		//maxx = maX;
		//maxz = maZ;

	}

	public void setFollowTransform()
	{
		followTransform = null;
	}
}
