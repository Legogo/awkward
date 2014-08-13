using UnityEngine;
using System.Collections;

/// <summary>
/// Texture swap shader.
/// Written by Romain Péchot ( http://opotable.tumblr.com/ )
/// </summary>

[ExecuteInEditMode]
public class TextureSwapShader : MonoBehaviour
{
	public Material _M_mat;
	public Transform _T_pivot;
	private Vector4 _V4_pivot;
	private float _f_radius;
	
	
	private void LateUpdate()
	{
		if(_T_pivot != null && _M_mat != null && _M_mat.HasProperty("_Pivot") && _M_mat.HasProperty("_Radius"))
		{
			// set pivot
			_V4_pivot.x = _T_pivot.position.x;
			_V4_pivot.y = _T_pivot.position.y;
			_V4_pivot.z = _T_pivot.position.z;
			_M_mat.SetVector("_Pivot", _V4_pivot);
			
			// set radius
			_f_radius = _T_pivot.localScale.x;
			_M_mat.SetFloat("_Radius", _f_radius);
			
			// offset
			if(Application.isPlaying) _M_mat.SetTextureOffset("_AlphaTex", Vector2.one * (Time.time % 5f) * 0.1f);
		}
		
	}/*LateUpdate*/
	
	private void OnDrawGizmos()
	{
		if(_T_pivot != null)
		{
			Gizmos.color = Color.green;
			Gizmos.DrawWireSphere(_T_pivot.position, _T_pivot.localScale.x);
		}

	}/*OnDrawGizmos()*/
	
}/*SetSwapTexturePivot*/