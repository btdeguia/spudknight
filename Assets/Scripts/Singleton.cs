using UnityEngine;

public class Singleton<T> : MonoBehaviour where T : MonoBehaviour {

	private static T instance;

	public static T Instance { 
		get {return instance;}
	}

	protected virtual void Awake(){
		if (instance == null){
			Debug.Log("instance is null");
			instance = FindObjectOfType<T>();
		} else {
			Debug.Log("Destroying other instance");
			Destroy(FindObjectOfType<T>());
		}
		DontDestroyOnLoad(FindObjectOfType<T>());
	}
}