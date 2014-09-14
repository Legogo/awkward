
using UnityEngine;
using System.Collections;

/// <summary>
/// Video player.
/// André BERLEMONT
/// 
/// Will setup everything to play a movie on a geometric object
/// Just add the script to a renderable object and reference the movie in the public var
/// 
/// You might need to add unlit/texture to the resources folder for standalone compiling (issue with shader.find)
/// </summary>

public class VideoPlayer : MonoBehaviour {

  public bool loop = false;
  public MovieTexture movie;

  void Start () {
    if(movie == null) return;

    AudioSource src = GetComponent<AudioSource>();
    if(src == null) src = gameObject.AddComponent<AudioSource>();


    Material mat = renderer.material;
    if(mat == null){
      mat = new Material(Shader.Find("Unlit/Texture"));
      renderer.material = mat;
    }

    mat.mainTexture = movie;

    src.loop = loop;
    src.clip = movie.audioClip;

    src.Play ();
    movie.Play();
  }
  
}