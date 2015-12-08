using UnityEngine;
using System.Collections;

public class MeshText : MonoBehaviour {

  public float letterSpacing = 4.0f;
  public Vector3 position;

  private GameObject[] letters = new GameObject[10];

  // Use this for initialization
  void Start () {
    setText("123456");
  }

  // Update is called once per frame
  public void setText(string text) {

    int counter = 0;
    foreach (char c in text){

      counter++;

      GameObject letterPrefab = Resources.Load("Alphabet/" + c) as GameObject;
      letterPrefab = (letterPrefab) ? letterPrefab : Resources.Load("Alphabet/Test") as GameObject;

      letters[counter] = Instantiate(letterPrefab, new Vector3(transform.position.x + (counter * letterSpacing), transform.position.y, transform.position.z), Quaternion.Euler(0,180,0)) as GameObject;
      letters[counter].transform.parent = transform;
    }

    transform.position -= Vector3.right * (counter * letterSpacing / 2);
  }

}
