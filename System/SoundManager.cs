using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/*
 * v0.14
 * 
 * 2012-10-18
 * # added return src
 * # added playMusic()
 * # added playSound(loop)
 * # added intègre directement le dossier Resources/Sound/
 * 
 * 2012-10-15
 * # added stopAll()
 * # added stopSound(name)
 * */

public class SoundManager {
	
  static public List<AudioClip> clips;
	
	static private GameObject snd;
	
	static public void init() {
		
    snd = GameObject.Find("[SOUND]");
    if(snd == null){
      snd = new GameObject("[SOUND]");
    }
		
		Object[] allSound = Resources.LoadAll("Sound");
		
    clips = new List<AudioClip>();
		//declare all clips
		for(int i = 0; i < allSound.Length; i++){
			clips.Add((AudioClip)(allSound[i]));
		}
		
		//Debug.Log("SoundM has "+clips.Count+" sounds");
		//printContent();
	}
	
	static public void printContent(){
    if(!isReady()) SoundManager.init();

		foreach(AudioClip clip in clips){
			Debug.Log(clip.name);
		}
	}
	
	static public int getSoundId(string name){
    if(!isReady()) SoundManager.init();

		for(int i = 0; i < clips.Count; i++){
			if(name == clips[i].name){
				return i;
			}
		}
		
		Debug.Log("Couldn't find sound of name : "+name);
		return -1;
	}
	
	static public AudioSource playMusic(string name){
		return playSound(getSoundId(name), true);
	}
	
	static public AudioSource playSound(string name){
		return playSound(getSoundId(name), false);
	}
	
	static public AudioSource playSound(int id, bool loop){
    if(!isReady()) SoundManager.init();
		
		AudioSource src = getAvailable();
		
		if(id >= clips.Count){
			Debug.LogError("Id is too high for sounds clips array");
			return null;
		}
		
		if(clips[id] == null){
			Debug.LogError("No sound clip ref for id "+id);
			return null;
		}
		
		if(src == null){
			Debug.LogError("No audio source ?");
      return null;
		}
		
		src.clip = clips[id];
		src.loop = loop;
		src.volume = 2f;
		src.Play();
		
		return src;
	}
	
	static public void stopSound(string name){
    if(!isReady()) SoundManager.init();

		foreach(AudioSource src in snd.transform.GetComponents<AudioSource>()){
			if(src.clip != null){
				if(src.clip.name == name){
					src.Stop();
				}
			}
			
		}
	}
	
  static public bool isReady(){ return snd != null; }

	static public AudioSource addSource(){
		return snd.AddComponent<AudioSource>();
	}
	
	static public AudioSource getAvailable(){
		foreach(AudioSource src in snd.transform.GetComponents<AudioSource>()){
			if(!src.isPlaying) return src;
		}
		AudioSource newSource = addSource();
		return newSource;
	}
	
	static public void stopAll(){
		foreach(AudioSource src in snd.transform.GetComponents<AudioSource>()){
			src.Stop();
		}
	}
}
