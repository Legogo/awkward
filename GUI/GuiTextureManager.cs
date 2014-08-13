using UnityEngine;
using System.Collections;

/*
 * 2012-11-26
 * # Permet d'avoir une librairie de texture affichage en GUI
 * */

public class GuiTextureManager : MonoBehaviour {
	
	public Camera carrier;
	public Texture[] textureLibrary;
	protected GuiTexture[] texComponents;
	
	static public GuiTextureManager instance;
	
	void Awake(){
		instance = this;
	}
	
	void Start () {
		generateTextureComponents();
	}
	
	void generateTextureComponents(){
		
		if(textureLibrary.Length > 0){
			texComponents = new GuiTexture[textureLibrary.Length];
			
			for(int i = 0; i < textureLibrary.Length; i++){
				Texture tex = textureLibrary[i];
				GuiTexture guitex = carrier.gameObject.AddComponent<GuiTexture>();
				guitex.textureToDisplay = tex;
				texComponents[i] = guitex;
			}
		}
		
	}
	
	static public void closeAll(){
		//Debug.Log("CLOSE");
		foreach(GuiTexture tex in instance.texComponents){
			if(tex.isVisible())	tex.toggleVisible(false);
		}
	}
	
	static public GuiTexture getTextureComponent(string textureName){
		foreach(GuiTexture tex in instance.texComponents){
			if(tex.textureToDisplay.name == textureName)	return tex;
		}
		return null;
	}
	
	static public bool oneOpen(){
		foreach(GuiTexture tex in instance.texComponents){
			//Debug.Log(tex.textureToDisplay.name+" is "+tex.isVisible());
			if(tex.isVisible())	return true;
		}
		return false;
	}
	
}
