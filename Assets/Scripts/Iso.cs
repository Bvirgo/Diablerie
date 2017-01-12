﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[ExecuteInEditMode]
[RequireComponent (typeof(SpriteRenderer))]
public class Iso : MonoBehaviour {

	static public float tileSize = 0.2f;
    static public float tileSizeY = tileSize / 2;
    public Vector2 pos;
	public Vector2 tilePos;
	SpriteRenderer spriteRenderer;
    public int macroTileOrder;

	static public Vector3 MapToWorld(Vector3 iso) {
		return new Vector3(iso.x - iso.y, (iso.x + iso.y) / 2) * tileSize;
	}

	static public Vector3 MapToIso(Vector3 world) {
		return new Vector3(world.y + world.x / 2, world.y - world.x / 2) / tileSize;
	}

	static public void DebugDrawTile(Vector3 pos, Color color, float margin = 0) {
		pos = Iso.MapToWorld(pos);
		float d = 0.5f - margin;
		Debug.DrawLine(pos + Iso.MapToWorld(new Vector2(d, d)), pos + Iso.MapToWorld(new Vector2(d, -d)), color);
		Debug.DrawLine(pos + Iso.MapToWorld(new Vector2(-d, -d)), pos + Iso.MapToWorld(new Vector2(-d, d)), color);
		Debug.DrawLine(pos + Iso.MapToWorld(new Vector2(d, d)), pos + Iso.MapToWorld(new Vector2(-d, d)), color);
		Debug.DrawLine(pos + Iso.MapToWorld(new Vector2(d, -d)), pos + Iso.MapToWorld(new Vector2(-d, -d)), color);
	}
    static public void GizmosDrawTile(Vector3 pos, float size = 1.0f)
    {
        pos = Iso.MapToWorld(pos);
        float d = 0.5f * size;
        Gizmos.DrawLine(pos + Iso.MapToWorld(new Vector2(d, d)), pos + Iso.MapToWorld(new Vector2(d, -d)));
        Gizmos.DrawLine(pos + Iso.MapToWorld(new Vector2(-d, -d)), pos + Iso.MapToWorld(new Vector2(-d, d)));
        Gizmos.DrawLine(pos + Iso.MapToWorld(new Vector2(d, d)), pos + Iso.MapToWorld(new Vector2(-d, d)));
        Gizmos.DrawLine(pos + Iso.MapToWorld(new Vector2(d, -d)), pos + Iso.MapToWorld(new Vector2(-d, -d)));
    }

    static public void DebugDrawTile(Vector3 pos, float margin = 0) {
		DebugDrawTile(pos, Color.white, margin);
	}

	static public Vector3 MouseTile() {
		Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
		mousePos.z = 0;
		return Snap(MapToIso(mousePos));
	}

	static public Vector3 Snap(Vector3 pos) {
		pos.x = Mathf.Round(pos.x);
		pos.y = Mathf.Round(pos.y);
		return pos;
	}

    static public Vector3 MacroTile(Vector3 pos)
    {
        var macroTile = pos;
        macroTile.x = Mathf.Round(pos.x / 5);
        macroTile.y = Mathf.Round(pos.y / 5);
        return macroTile;
    }

	void Awake() {
		pos = MapToIso(transform.position);
		tilePos = Snap(pos);
		spriteRenderer = GetComponent<SpriteRenderer>();
	}

	void Start () {
		
	}
    static float fmod(float a, float b)
    {
        return a - b * Mathf.Round(a / b);
    }

    void Update () {
        if (Application.isPlaying)
        {
            transform.position = MapToWorld(pos);
        }
        else
        {
            transform.position = MapToWorld(Snap(MapToIso(transform.position)));
            pos = MapToIso(transform.position);
        }
		spriteRenderer.sortingOrder = -Mathf.RoundToInt(transform.position.y / tileSizeY);
        var macroTile = MacroTile(pos);
        macroTileOrder = -Mathf.RoundToInt((MapToWorld(macroTile)).y / tileSizeY);
        spriteRenderer.sortingOrder += macroTileOrder * 1000;
    }
}
