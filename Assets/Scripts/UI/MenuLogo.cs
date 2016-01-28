using UnityEngine;
using System.Collections;

public class MenuLogo : MonoBehaviour
{
	new Camera camera;
	GameObject mainMenu;

	void Start()
	{
		this.camera = GameObject.Find("FrontElementCamera").GetComponent<Camera>();
		this.mainMenu = GameObject.Find("MainMenu");
		this.mainMenu.SetActive(false);
	}

	void Update()
	{
		if (Input.GetMouseButtonDown(0)) { // if left button pressed...

			Ray ray = this.camera.ScreenPointToRay(Input.mousePosition);
			RaycastHit hit;
			int layerMask = 1 << 15;

			if (Physics.Raycast(ray, out hit, 1000f, layerMask)) { // if something hit...

				Transform selected = hit.transform;
				selected.gameObject.SetActive(false);
				this.mainMenu.SetActive(true);
			}
		}
	}

}
