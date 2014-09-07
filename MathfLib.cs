using UnityEngine;
using System;
using System.Collections;

/*
	Author : Romain Pechot
*/

public static class MathfLib
{
	// pour le Planecast
	private static Plane plane;
	private static Ray ray;
	
	// Lerp non clampé
	public static float Lerp(float start, float end, float val)
	{
		return start + (end - start) * val;
		
	}/*Lerp()*/
	
	
	// InverseLerp non clampé
	public static float InverseLerp(float start, float end, float val)
	{
		return (val - start) / (end - start);
		
	}/*InverseLerp()*/
	
	
	
	// renvois le point de croisement entre deux segments
	// ATTENTION : ceci ne marche que sur le plant X/Z
	// ATTENTION : pour des soucis de simplicité nous ne testons pas
	// si les segments se croisent. Le résultat sera toujours "bon"
	// meme si les segments ne se croisent pas : l'intersection sera
	// en dehors de ces derniers.
	public static Vector3 IntersectionLinesXZ(Vector3 A, Vector3 B, Vector3 C, Vector3 D)
	{
		Vector3 v0 = B - A;
		
		Vector3 v1 = D - C;
		
		float ratio = -(-v0.x * A.z + v0.x * C.z + v0.z * A.x - v0.z * C.x)/(v0.x * v1.z - v0.z * v1.x);
		
		return C + ratio * v1;
		
	}/*IntersectionLinesXZ*/
	
	
	// renvois false s'il n'y a aucuns contacts, true s'il y a contact
	// si il a bien croisement, la première intersection est stockée dans result
	//
	// pour comprendre l'algorithme :
	// http://gamedev.stackexchange.com/questions/18333/circle-line-collision-detection-problem/18339#18339
	//
	// implémentation de l'équation quadratic tiré d'ici :
	// http://www.cprogramdevelop.com/2745559/
	public static bool IntersectionSegmentCircleXZ(Vector3 segStart, Vector3 segEnd, Vector3 circleCenter, float circleRadius, ref Vector3 result)
	{
		// radius pow2
		circleRadius *= circleRadius;
		
		float fDis = Mathf.Sqrt((segEnd.x - segStart.x) * (segEnd.x - segStart.x) + (segEnd.z - segStart.z) * (segEnd.z - segStart.z));
		
		Vector3 d;
		d.x = (segEnd.x - segStart.x) / fDis;
		d.z = (segEnd.z - segStart.z) / fDis;
		
		Vector3 e;
		e.x = circleCenter.x - segStart.x;
		e.z = circleCenter.z - segStart.z;
		
		float a = e.x * d.x + e.z * d.z;
		float a2 = a * a;
		float e2 = e.x * e.x + e.z * e.z;
		
		// ne prend pas en compte les tangent
		if((circleRadius - e2 + a2) <= 0f)
		{
			return false;
		}
		else
		{
			float f = Mathf.Sqrt(circleRadius - e2 + a2);
			float t = a - f;
			
			result.x = segStart.x + t * d.x;
			result.z = segStart.z + t * d.z;
			
			return true;
		}
		
	}/*IntersectionSegmentCircleXZ()*/
	
	
	// permet de savoir si deux transforms sont alignés dans les
	// meme directions mais pas forcément sur les meme axes
	public static bool IsTransformsAligned(Transform a, Transform b, float threshold)
	{
		// les bon résultat sont : 0, 90 ou 180 (des multiples de 90)
		float z = Mathf.Abs(Vector3.Angle(a.forward, b.forward));
		
		// on fait le modulo du résultat
		// si nous sommes à 0/90/180 cela va renvoyer <= 0.0001
		z %= 90f;
		
		float x = Mathf.Abs(Vector3.Angle(a.right, b.right));
		x %= 90f;
		
		return (z <= threshold) && (x <= threshold);
		
	}/*IsTransformsAligned*/
	
	// overload
	public static bool IsTransformsAligned(Transform a, Transform b)
	{
		return IsTransformsAligned(a, b, 0f);
		
	}/*IsTransformsAligned*/
	
	
	
	public static bool IsMoreSpaced(float a, float b, float delta)
	{
		return Mathf.Abs(b-a) > Mathf.Abs(delta);
		
	}/*IsMoreSpaced()*/
	
	
	// renvois vrai si la valeur "val" est entre min et max
	// le dernier paramètre permet de choisir si "min" et "max" peuvent etre inclusife :
	// 0 == aucuns des deux ne sont inclusifs
	// 1 == "min" est inclusif / "max" est exclusif
	// 2 == "min" est exlusif / "max" est inclusif
	// 3 == les deux sont inclusif
	public static bool IsInside(float min, float max, float val, int inclusive)
	{
		switch(inclusive)
		{

		case 0 : return (val > min && val < max);

		case 1 : return (val >= min && val < max);

		case 2 : return (val > min && val <= max);

		default : return (val >= min && val <= max);

		}//switch(inclusive)
		
	}/*IsMoreSpaced()*/
	
	
	// overload
	public static bool IsInside(float min, float max, float val)
	{
		return IsInside(min, max, val, 3);
		
	}/*IsMoreSpaced()*/
	
	
	
	public static float GetPointRatioPosition(Transform origin, float angle, Vector3 worldPoint)
	{
		// get point position to world space into origin space
		worldPoint = origin.InverseTransformPoint(worldPoint);
		
		// normalize position into vector
		worldPoint.Normalize();
		
		// dot this shit out !
		float dot = Vector3.Angle(Vector3.forward, worldPoint) / 180f;
		
		// sign the dot
		dot *= Mathf.Sign(worldPoint.x);
		
		// convert this angle to dot result
		angle /= 360f;
		
		// convert the dot result into ratio result
		dot = MathfLib.InverseLerp(-angle, angle, dot);
		
		return dot;
		
	}/*GetPointRatioPosition()*/
	
	
	public static Vector3 Abs(Vector3 val)
	{
		val.x = Mathf.Abs(val.x);
		val.y = Mathf.Abs(val.y);
		val.z = Mathf.Abs(val.z);
		
		return val;
		
	}/*Abs()*/
	
	
	// clamp "val" sur une grille de taille "scale" avec un "offset".
	// "mul" sert de controleur : si ce dernier est à 0 ClampToObj() renvois
	// une valeur complètement clampé. sinon elle renvoie la valeur par défaut.
	public static float ClampToObj(float val, float mul, float offset, float scale)
	{
		return Mathf.Lerp(Mathf.Floor((val + offset) / scale) * scale, val, mul);
		
	}/*ClampToObj()*/
	
	
	// ref. float ClampToObj(float)
	public static Vector3 ClampToObj(Vector3 val, Vector3 mul, Vector3 offset, Vector3 scale)
	{
		val.x = ClampToObj(val.x, Mathf.Abs(mul.x), offset.x, scale.x);
		val.y = ClampToObj(val.y, Mathf.Abs(mul.y), offset.y, scale.y);
		val.z = ClampToObj(val.z, Mathf.Abs(mul.z), offset.z, scale.z);
		
		return val;
		
	}/*ClampToObj()*/
	
	
	
	public static Vector3 TrimVector3(Vector3 val)
	{
		val.x = TrimFloat(val.x);
		val.y = TrimFloat(val.y);
		val.z = TrimFloat(val.z);
		
		return val;
		
	}/*TrimVector3*/
	
	
	
	public static float TrimFloat(float val)
	{
		if(Mathf.Abs(val) > 0.5f) return Mathf.Sign(val);
		
		return 0f;
		
	}/*TrimFloat()*/
	
	
	
	// renvois l'axe principal le plus proche par rapport au vecteur de direction en entré
	public static Vector3 FindNearest(Vector3 dir, Vector3[] vectorDirs)
	{
		float[] dotDirs = new float[vectorDirs.Length];
		
		dir.Normalize();
		
		for(int i = 0; i < dotDirs.Length; i++)
		{
			dotDirs[i] = Mathf.Abs(Vector3.Dot(dir, vectorDirs[i]));

		}//for()
		
		int nearest = MathfLib.FindNearest(dotDirs, 1f);
		
		if(nearest >= 0) return vectorDirs[nearest];
		
		return dir;
		
	}/*FindNearest*/
	
	// overload
	public static Vector3 FindNearest(Vector3 dir)
	{
		return FindNearest(dir, new Vector3[3]{Vector3.right, Vector3.up, Vector3.forward});
		
	}/*FindNearest*/
	
	
	
	// génère un vector3 représentant un angle sur le cercle trigonométrique
	public static Vector3 AngleToVectorXZ(float angle, float offset)
	{
		return Vector3.forward * Mathf.Sin((angle + offset) * Mathf.Deg2Rad) + Vector3.right * Mathf.Cos((angle + offset) * Mathf.Deg2Rad);
		
	}/*AngleToVectorXZ()*/
	
	
	
	public static Vector3 AngleToVectorXZ(float angle)
	{
		return AngleToVectorXZ(angle, 0f);
		
	}/*AngleToVectorXZ()*/
	
	
	// renvoi un angle entre -180° et 180° :
	//     _0°_
	//    /    \
	// -90|    |+90
	//    \____/
	// -180 | 180
	public static float VectorToAngleXZ(Vector3 localVector)
	{
		localVector.Normalize();
		
		return (180f - (Mathf.Asin(localVector.z) * Mathf.Rad2Deg + 90f)) * Mathf.Sign(Mathf.Asin(localVector.x));
		
	}/*VectorToAngleXZ()*/
	
	
	
	// fonction faisant boucler un index autour d'un tableau
	// si l'index dépasse dans un sens ou dans l'autre 
	// on le fait passer de l'autre coté
	public static int GetIndex(int indexStart, int arraySize, int indexStep)
	{
		indexStart += indexStep;
		
		if(indexStart < 0) indexStart += arraySize;
		else if(indexStart >= arraySize) indexStart -= arraySize;
		
		return indexStart;
		
	}/*GetIndex()*/
	
	// custom overload
	public static int GetIndexNext(int indexStart, int arraySize)
	{
		return GetIndex(indexStart, arraySize, 1);
		
	}/*GetIndexNext()*/
	
	// custom overload
	public static int GetIndexPrevious(int indexStart, int arraySize)
	{
		return GetIndex(indexStart, arraySize, -1);
		
	}/*GetIndexPrevious()*/
	
	
	
	// renvoi un CrossResult (voir enum eCrossResult)
	public static eCrossResult CrossLines(float A1, float A2, float B1, float B2, float threshold)
	{
		// A est-il devant B ?
		switch(Smallest(A1, B1, threshold))
		{
			// A1 est plus petit que B1
			case -1 :
			{
				if(A2 < B1)
				{
					return eCrossResult.AExludeB;
				}
				else
				{
					int d = Smallest(A2, B2, threshold);
					if(d < 0) return eCrossResult.ACrossB;
					else if(d > 0) return eCrossResult.AContainsB;
					return eCrossResult.ACrossB;
				}
			}
			
			// A1 et B1 sont équivalent
			case 0 :
			{
				int d = Smallest(A2, B2, threshold);
				if(d < 0) return eCrossResult.BContainsA;
				else if(d > 0) return eCrossResult.AContainsB;
				return eCrossResult.AEqualsB;
			}
			
			// B1 est plus petit A1
			case 1 :
			{
				if(B2 < A1)
				{
					return eCrossResult.BExcludeA;
				}
				else
				{
					int d = Smallest(A2, B2, threshold);
					if(d < 0) return eCrossResult.BContainsA;
					else if(d > 0) return eCrossResult.BCrossA;
					return eCrossResult.BContainsA;
				}
			}
			
			default: return eCrossResult.Error;
			
		}//switch()
		
	}/*CrossLines()*/
	
	
	
	public static eCrossResult CrossLines(float A1, float A2, float B1, float B2)
	{
		return CrossLines(A1, A2, B1, B2, 0f);
		
	}/*CrossLines()*/
	
	
	
	// renvoi :
	// -1 si a < b
	//  1 si b < a
	//  0 si a == b (en prenant en compte le seuil)
	public static int Smallest(float a, float b, float threshold)
	{
		if(Mathf.Abs(b - a) <= Mathf.Abs(threshold)) return 0;
		else if(a < b) return -1;
		else return 1;
		
	}/*Smallest()*/
	
	
	
	public static int Smallest(float a, float b)
	{
		return Smallest(a, b, 0f);
		
	}/*Smallest()*/
	
	
	
	public static int FindSmallest(params float[] floats)
	{
		int index = -1;
		
		for(int i = 0; i < floats.Length; i++)
		{
			if(index < 0) index = i;
			else if(floats[i] < floats[index]) index = i;
			
		}//for()
		
		return index;
		
	}/*FindSmallest()*/
	
	
	// trouve l'index d'un des éléments du tableau
	// qui se rapproche le plus de la valeur unique
	public static int FindNearest(float[] values, float val)
	{
		int index = -1;
		
		if(values != null)
		{
			for(int i = 0; i < values.Length; i++)
			{
				if(index < 0) index = i;
				else if(Mathf.Abs(val - values[index]) > Mathf.Abs(val - values[i])) index = i;

			}//for()
		}
		
		return index;
			
	}/*FindNearest()*/
	
	public enum eCrossResult
	{
		AContainsB = 0,// 	A contient B
		BContainsA = 1,// 	B contient A
		AEqualsB = 2,// 	A et B ont leur point de départ et d'arrivé équivalent
		ACrossB = 3,// 		A commence avant B mais fini après le début de B (se croisent)
		BCrossA = 4,// 		B commence avant A mais fini après le début de A (se croisent)
		AExludeB = 5,//		A est complètement derrière B (ne se croisent jamais)
		BExcludeA = 6,//	B est complètement derrière A (ne se croisent jamais)
		Error = 7,//		Erreur... ?
		
	}/*eCrossResult*/
	
	
	// fonction simplifiant l'appel pour savoir si un Ray peut toucher un plan dans l'espace
	// la booléen isPlaneSided permet de simuler le fait que le plan n'ai qu'un seul sens de pénétration.
	public static bool Planecast(Vector3 planeOrigin, Vector3 planeDirection, bool isPlaneSided, Vector3 rayOrigin, Vector3 rayDirection, out Vector3 hitPoint)
	{
		float hit = 0f;
		
		// setup plane
		plane.SetNormalAndPosition(planeDirection, planeOrigin);
		
		// setup ray
		ray.origin = rayOrigin;
		ray.direction = rayDirection;
		
		// si isPlaneSided est à true, on vérifie que notre ray est bien du bon coté de notre plane
		if(isPlaneSided && !plane.GetSide(rayOrigin))
		{
			// pas bon ! on sort !
			hitPoint = Vector3.zero;
			
			return false;
		}
		
		// intersect ?
		if(plane.Raycast(ray, out hit))
		{
			// compute world space result
			hitPoint = ray.GetPoint(hit);
			
			return true;
		}
		else
		{
			hitPoint = Vector3.zero;
			
			return false;
		}
		
	}/*Planecast()*/
	
	// overload
	public static bool Planecast(Transform plane, bool isPlaneSided, Transform ray, out Vector3 hitPoint)
	{
		return Planecast(plane.position, plane.forward, isPlaneSided, ray.position, ray.forward, out hitPoint);
		
	}/*Planecast()*/
	
	
	// cette version améliorée du Planecast rajoute des bordures pour notre Plane, le transformant en Carré
	public static bool Squarecast(Transform plane, bool isPlaneSided, Transform ray, Vector2 limits)
	{
		Vector3 hit;
		
		// on vérifie d'abord que notre ray touche bien notre plane
		if(Planecast(plane, isPlaneSided, ray, out hit))
		{
			// ok, on transforme maintenant notre point monde en point local
			hit = plane.InverseTransformPoint(hit);
			
			// est-ce qu'on est à l'intérieur de nos limites ?
			return (IsInside(-limits.x, limits.x, hit.x) && IsInside(-limits.y, limits.y, hit.y));
		}
		else
		{
			// pas bon, on sort !
			return false;
		}
		
	}/*Squarecast()*/
	
}/*MathfLib*/


// petite classe contenant une durée prédéterminée
// renvoie true si nous avons dépassée cette durée.
public class StopWatchBuffer
{
	private float time = 1f;
	private float buffer = 0f;
	
	public StopWatchBuffer(float time)
	{
		this.time = time;
		this.buffer = 0f;
		
	}/*StopWatchBuffer()*/
	
	public void Reset()
	{
		buffer = 0f;
		
	}/*Reset()*/
	
	public bool Update(float deltaTime)
	{
		buffer += Time.deltaTime;
		
		return IsOverflowed();
		
	}/*Update()*/
	
	public bool IsOverflowed()
	{
		return buffer > time;
		
	}/*IsOverflowed()*/
	
}/*StopWatchBuffer*/