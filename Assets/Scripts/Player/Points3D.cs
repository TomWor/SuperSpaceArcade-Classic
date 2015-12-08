using UnityEngine;
using System.Collections;

public class Points3D : MonoBehaviour {

  //private GameObject letter;
  //private GameObject letter2;

  private Vector3 originalPosition;


  public void Awake(){
    //originalPosition = transform.position;
  }

  public void SetPoints(int points){

    if (points > 0){

      GetComponent<TextMesh>().text = points.ToString();

/*
      GameObject value = new GameObject("Value");

      letter = Instantiate(Resources.Load("Alphabet/Test"), new Vector3(originalPosition.x, originalPosition.y+7, originalPosition.z), Quaternion.identity) as GameObject;
      letter.transform.parent = value.transform;
      letter2 = Instantiate(Resources.Load("Alphabet/Test"), new Vector3(originalPosition.x + 3, originalPosition.y+7, originalPosition.z), Quaternion.identity) as GameObject;
      letter2.transform.parent = value.transform;

      value.transform.parent = transform;

    } else {
      letter = Instantiate(Resources.Load("Alphabet/Test"), new Vector3(originalPosition.x, originalPosition.y+7, originalPosition.z), Quaternion.identity) as GameObject;
      letter.transform.parent = transform;
*/
    }

  }

  public void FixedUpdate(){
    /*
    letter.GetComponent<Transform>().localPosition += new Vector3(0,0.2f,1);
    letter2.GetComponent<Transform>().localPosition += new Vector3(0,0.2f,1);
    */

    transform.localPosition += new Vector3(0,0.4f,1.5f);

    Color color = GetComponent<MeshRenderer>().material.color;
    color.a -= 0.01f;
    GetComponent<MeshRenderer>().material.color = color;

  }
}


