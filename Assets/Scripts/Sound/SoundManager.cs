using UnityEngine;
using System.Collections;

public class SoundManager : MonoBehaviour {

  public AudioClip explodeSound;


  public void Explosion() {
    GetComponent<AudioSource>().PlayOneShot(explodeSound);
  }


  public void Play (AudioClip clip){
    GetComponent<AudioSource>().PlayOneShot(clip);
  }

}
