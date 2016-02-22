using UnityEngine;
using System.Collections;

public class MenuLogo : MonoBehaviour
{
	void Update()
	{
		if (Input.GetMouseButtonDown(0)) { // if left button pressed...

			Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
			RaycastHit hit;
			int layerMask = 1 << 15;

			if (Physics.Raycast(ray, out hit, 1000f, layerMask)) { // if something hit...

				Transform selected = hit.transform;
				selected.gameObject.SetActive(false);
				//this.mainMenu.SetActive(true);
			}
		}
	}

}
