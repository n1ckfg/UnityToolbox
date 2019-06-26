using UnityEngine;
using System.Collections;
//using UnityEngine.Networking;

//[RequireComponent(typeof(NetworkIdentity))]
//public class BasicController2 : NetworkBehaviour {
public class BasicController2 : MonoBehaviour {

	[HideInInspector] public Vector3 cursorPos = Vector3.zero;
	[HideInInspector] public Vector3 lastHitPos = Vector3.one;
	
	private float zPos = 1f;
	private bool fixedZ = false;

	private void Awake() {
		if (rb == null) rb = GetComponent<Rigidbody>();
	}

	private void Start() {
		//if (!isLocalPlayer) return;

		currentSpeed = walkSpeed;
        Cursor.visible = showCursor;
		if (useTouch) Input.multiTouchEnabled = true;
		if (useMouse && rb != null) rb.freezeRotation = true;

		collisionStart();
	}

	private void Update() {
		//if (!isLocalPlayer) return;

		if (useKeyboard) wasdUpdate();

		if (useMouse) mouseUpdate();
		if (useButton) mouseButtonUpdate();
		if (useTouch) touchUpdate();	

		if (useRaycaster) rayUpdate();	
	}

    // ~ ~ ~ ~ ~ ~ ~ ~ 

    [Header("Keyboard")] 
    public bool useKeyboard = true;
    public bool useYAxis = false;
    public string yAxisName = "Vertical2";
	public float walkSpeed = 10f;
	public float runSpeed = 100f;
	public float accel = 0.01f;
	public Transform homePoint;

	private float currentSpeed;
	private Vector3 pos = Vector3.zero;
	private bool run = false;
	
	private void wasdUpdate() {
		if (Input.GetKeyDown(KeyCode.LeftShift)) {
			run = true;
		} else if (Input.GetKeyUp(KeyCode.LeftShift)) {
			run = false;
		}

		if (run && currentSpeed < runSpeed) {
			currentSpeed += accel;
			if (currentSpeed > runSpeed) currentSpeed = runSpeed;
		} else if (!run && currentSpeed > walkSpeed) {
			currentSpeed -= accel;
			if (currentSpeed < walkSpeed) currentSpeed = walkSpeed;
		}

		pos.x = Input.GetAxis("Horizontal") * Time.deltaTime * currentSpeed;
        if (useYAxis) {
			pos.y = Input.GetAxis(yAxisName) * Time.deltaTime * currentSpeed;
        }
        else {
			pos.y = 0f;
        }
		pos.z = Input.GetAxis("Vertical") * Time.deltaTime * currentSpeed;

		transform.Translate(pos);

		if (homePoint != null && Input.GetKeyDown(KeyCode.Home)){
			transform.position = homePoint.position;
			transform.rotation = homePoint.rotation;
			transform.localScale = homePoint.localScale;
		}
	}

	// ~ ~ ~ ~ ~ ~ ~ ~ 

	public enum RotationAxes { MouseXAndY, MouseX, MouseY, NONE };
    [Header("Mouse")]
    public bool useMouse = true;
	public bool useButton = true;
	public bool showCursor = false;
	public RotationAxes axes = RotationAxes.MouseXAndY;
	public float sensitivityX = 2f;
	public float sensitivityY = 2f;

	[HideInInspector] public bool mouseDown = false;
	[HideInInspector] public bool mousePressed = false;
	[HideInInspector] public bool mouseUp = false;

	private float minimumX = -360f;
	private float maximumX = 360f;
	private float minimumY = -60f;
	private float maximumY = 60f;
	private float rotationY = 0f;

	private void mouseUpdate() {
		if (axes == RotationAxes.MouseXAndY) {
			float rotationX = transform.localEulerAngles.y + Input.GetAxis("Mouse X") * sensitivityX;

			rotationY += Input.GetAxis("Mouse Y") * sensitivityY;
			rotationY = Mathf.Clamp(rotationY, minimumY, maximumY);

			transform.localEulerAngles = new Vector3(-rotationY, rotationX, 0f);
		} else if (axes == RotationAxes.MouseX) {
			transform.Rotate(0f, Input.GetAxis("Mouse X") * sensitivityX, 0f);
		} else if (axes == RotationAxes.MouseY) {
			rotationY += Input.GetAxis("Mouse Y") * sensitivityY;
			rotationY = Mathf.Clamp(rotationY, minimumY, maximumY);

			transform.localEulerAngles = new Vector3(-rotationY, transform.localEulerAngles.y, 0f);
		}

		// ~ ~ ~
	}

	private void mouseButtonUpdate() { 
		mouseDown = false;
		mouseUp = false;

		if (useButton) {
			if (Input.GetMouseButtonDown(0) && GUIUtility.hotControl == 0) {
				mouseDown = true;
				mousePressed = true;
			}

			if (Input.GetMouseButton(0) && GUIUtility.hotControl == 0) {
				if (!fixedZ) zPos = lastHitPos.z;
				cursorPos = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, zPos));
			}

			if (Input.GetMouseButtonUp(0)) {
				mousePressed = false;
				mouseUp = true;
			}
		}
	}

	// ~ ~ ~ ~ ~ ~ ~ ~ 

	[Header("Touch")]
	public bool useTouch = false;

	[HideInInspector] public bool touchPressed = false;
	[HideInInspector] public bool touchDown = false;
	[HideInInspector] public bool touchUp = false;
	[HideInInspector] public int touchCount = 0;

	void touchUpdate() {
		touchDown = false;
		touchUp = false;
		touchCount = Input.touchCount;

		if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began && GUIUtility.hotControl == 0) {
			touchPressed = true;
			touchDown = true;
		} else if (Input.touchCount < 1 || (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Ended) && GUIUtility.hotControl == 0) {
			touchPressed = false;
			touchUp = true;
		}

		if (touchPressed) {
			if (!fixedZ) zPos = lastHitPos.z;

			if (!useMouse && touchCount == 1) {
				Vector2 singlePos = Input.GetTouch(0).position;
				cursorPos = Camera.main.ScreenToWorldPoint(new Vector3(singlePos.x, singlePos.y, zPos));
			} else if (touchCount > 1) {
				Vector2 avgPos = Vector2.zero;

				for (int i = 0; i < touchCount; i++) {
					avgPos += Input.GetTouch(i).position;
				}

				avgPos /= (float)touchCount;
				Debug.Log(avgPos + " " + touchCount);

				cursorPos = Camera.main.ScreenToWorldPoint(new Vector3(avgPos.x, avgPos.y, zPos));
			}
		}
	}

	// ~ ~ ~ ~ ~ ~ ~ ~ 

	[Header("Raycaster")]
	public bool useRaycaster = true;
	public bool followCursor = true;
	public bool debugRaycaster = false;

	[HideInInspector] public bool isLooking = false;
	[HideInInspector] public string isLookingAt = "";
    [HideInInspector] public Collider isLookingCol;

	private float debugDrawTime = 0.3f;
	private float debugRayScale = 100f;

	void rayUpdate() {
		RaycastHit hit;
		Ray ray;

		if (followCursor) {
			ray = Camera.main.ScreenPointToRay(new Vector2(cursorPos.x, cursorPos.y));
		} else {
			ray = new Ray(transform.position, transform.forward);
		}

		if (Physics.Raycast(ray, out hit)) {
			isLooking = true;
			isLookingAt = hit.collider.name;
            isLookingCol = hit.collider;

			lastHitPos = hit.point;
		} else {
			isLooking = false;
			isLookingAt = "";
		}

		if (debugRaycaster) {
			if (followCursor) {
				Debug.DrawRay(Camera.main.ScreenToWorldPoint(cursorPos), transform.forward * debugRayScale, Color.red, debugDrawTime, false);
			} else {
				Debug.DrawRay(transform.position, transform.forward * debugRayScale, Color.red, debugDrawTime, false);
			}
			Debug.Log("isLooking: " + isLooking + " isLookingAt: " + isLookingAt + " lastHitPos: " + lastHitPos);
		}
	}

	// ~ ~ ~ ~ ~ ~ ~ ~ 

	[Header("Collisions")]
	public bool useCollisions = false;
	public Rigidbody rb;

	private void collisionStart() {
		if (rb != null) {
			if (useCollisions) {
				rb.useGravity = true;
				rb.constraints = RigidbodyConstraints.FreezePositionX | RigidbodyConstraints.FreezePositionZ | RigidbodyConstraints.FreezeRotation;
			} else {
				rb.useGravity = false;
				rb.constraints = RigidbodyConstraints.FreezePosition | RigidbodyConstraints.FreezeRotation;
			}
		}
	}

}
