using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VectionScroll : MonoBehaviour
{
    public float scrollSpeed = 0.5F;
	public Renderer rend;
	public bool up;
	void Start() {
		rend = GetComponent<Renderer>();
	}

	void FixedUpdate() {
		float offset = Time.time * scrollSpeed;
		if (!up) {
			offset*=-1;
		}
		rend.material.SetTextureOffset("_MainTex", new Vector2(0, offset));
	}
}
