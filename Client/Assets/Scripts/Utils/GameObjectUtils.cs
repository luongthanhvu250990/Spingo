﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.1
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------
using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public static class GameObjectUtils
{
	public static void ClearAllChild(GameObject parent)
	{
		var children = new List<GameObject>();
		
		foreach (Transform child in parent.transform) children.Add(child.gameObject);
		
		parent.transform.DetachChildren();
		
		if (Application.isEditor ) 
		{
			
			children.ForEach(child => GameObject.DestroyImmediate(child));
		}
		else
		{
			children.ForEach(child => GameObject.Destroy(child));
		}
		
		Resources.UnloadUnusedAssets();
	}
	
	public static void SetActiveAllChild(GameObject parent, bool active)
	{
		//		Debug.LogWarning("SetActiveAllChild " + parent.name + ":" + active);
		foreach (Transform child in parent.transform)
		{
			//	Debug.LogWarning("child " + child.name + ":" + active);
			child.gameObject.SetActive(active);
		}
	}
	
	public static GameObject AddChild(GameObject parent, string name = "NewGO")
	{
		GameObject c = new GameObject();
		c.name = name;
		c.transform.parent = parent.transform;
		c.transform.localPosition = Vector3.zero;
		c.transform.localScale = Vector3.one;
		c.transform.localEulerAngles = Vector3.zero;
		
		return c;
	}
	
	public static Component FindComponentInParent(Transform child, Type type)
	{
		Transform p = child.parent;
		if(p == null)
		{
			return null;
		}
		
		if(p.GetComponent(type) != null)
			return p.GetComponent(type);
		
		return FindComponentInParent(p, type);
	}
	
	public static Texture LoadTexture(string path)
	{
		//Debug.Log("LoadTexture " + path);
		try
		{
			return Resources.Load(path, typeof(Texture)) as Texture;
		}
		catch(Exception e)
		{
			Debug.Log("Cannot load texture " + path + "with error " + e.Message);
		}
		return null;
	}
	
	public static Texture2D LoadSprite(string path)
	{
		//Debug.Log("LoadTexture " + path);
		try
		{
			return Resources.Load(path, typeof(Texture2D)) as Texture2D;
		}
		catch(Exception e)
		{
			Debug.Log("Cannot load Sprite " + path + "with error " + e.Message);
		}
		return null;
	}
	
	public static TextAsset LoadTextAsset(string path)
	{
		//Debug.Log("LoadTextAsset " + path);
		try
		{
			return Resources.Load(path, typeof(TextAsset)) as TextAsset;
		}
		catch(Exception e)
		{
			Debug.Log("Cannot load text asset " + path + "with error " + e.Message);
		}
		return null;
	}
	
	public static GameObject LoadPrefab(string path)
	{
		try
		{
			Debug.Log("LoadPrefab " + path);
			GameObject ret = Resources.Load(path, typeof(GameObject)) as GameObject;
			if(ret == null)
				Debug.LogError("Cannot load prefab " + path);
			return ret;
		}
		catch(Exception e)
		{
			Debug.LogError("Cannot load prefab " + path + "with error " + e.Message);
		}
		return null;
	}
	

	public static GameObject LoadGameObject(string path)
	{
		return LoadGameObject(null, path);
	}
	
	public static GameObject LoadGameObject(GameObject prefab)
	{
		return LoadGameObject(null, prefab);
	}
	
	public static GameObject LoadGameObject(Transform parent, string path)
	{
		GameObject prefab = LoadPrefab(path);
		if(prefab == null) return null;
		
		GameObject go =(GameObject) GameObject.Instantiate(prefab, Vector3.zero, Quaternion.identity);
		go.transform.SetParent(parent);
		go.transform.localPosition = Vector3.zero;
		go.transform.localRotation = Quaternion.identity;
		go.transform.localScale = Vector3.one;
		
		return go;
		
	}
	
	public static GameObject LoadGameObject(Transform parent, GameObject prefab)
	{
		if (prefab == null) return null;
		
		GameObject go = (GameObject)GameObject.Instantiate(prefab, Vector3.zero, Quaternion.identity);
		go.transform.parent = parent;
		go.transform.localPosition = Vector3.zero;
		go.transform.localRotation = Quaternion.identity;
		go.transform.localScale = Vector3.one;
		//Debug.LogError(go.name);
		return go;
		
	}
	
	public static void SetLayerRecursively(GameObject go, int layer)
	{
		go.layer = layer;
		int count = go.transform.childCount;
		for(int i = 0; i < count; i++)
		{
			SetLayerRecursively(go.transform.GetChild(i).gameObject, layer);
		}        
	}
	
	public static void SetLayerRecursively(GameObject obj, int layer, int addSortingOrder,  int sortingLayerID = 0)
	{
		obj.layer = layer;		
		var renderer = obj.GetComponent<SpriteRenderer>();
		if (renderer != null) {
			renderer.sortingLayerID = sortingLayerID;
			renderer.sortingOrder += addSortingOrder;
		}
		foreach (Transform child in obj.transform) {
			SetLayerRecursively(child.gameObject, layer, addSortingOrder, sortingLayerID);
		}
	}
	
	
	public static Vector2 GetRandomPointInsidePolygon(Vector2 A,Vector2 B, Vector2 C){//A----B
		//|    |
		//C----D
		float u = UnityEngine.Random.Range (0f, 1.0f);
		float v = UnityEngine.Random.Range (0f, 1.0f);
		Vector2 p = A + u * (new Vector2 (B.x - A.x, B.y - A.y)) + v * (new Vector2 (C.x - A.x, C.y - A.y));
		return p;
	}
	
	
	
	#if UNITY_EDITOR
	public static GameObject LoadPrefabInAsset(Transform parent, string name)
	{
		GameObject prefab = UnityEditor.AssetDatabase.LoadAssetAtPath(name, typeof(GameObject)) as GameObject;
		if(prefab == null)
		{
			Debug.LogError("Cannot load asset at path " + name);
			return null;
		}
		GameObject go =(GameObject) GameObject.Instantiate(prefab, Vector3.zero, Quaternion.identity);
		go.transform.parent = parent;
		go.transform.localPosition = Vector3.zero;
		go.transform.localRotation = Quaternion.identity;
		go.transform.localScale = Vector3.one;
		return go;
	}
	
	public static Sprite LoadSpriteInAsset(string path)
	{
		//Debug.Log(path);
		Sprite tex = UnityEditor.AssetDatabase.LoadAssetAtPath<Sprite>(path);
		if(tex == null)
			Debug.LogError("Khong tim thay " + path);
		return tex;
	}
	
	public static TextAsset LoadTextInAsset(string path)
	{
		//Debug.Log(path);
		TextAsset ret = UnityEditor.AssetDatabase.LoadAssetAtPath<TextAsset>(path);
		if(ret == null)
			Debug.LogError("Khong tim thay " + path);
		return ret;
	}
	#endif
	
	
	
}

