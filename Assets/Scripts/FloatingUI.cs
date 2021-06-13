using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FloatingUI : MonoBehaviour
{
	public enum Direction {Up, Down, Left, Right};
	public Direction floatDirection = Direction.Up;

	private RectTransform controlBar;

	private float outPosition;

    private void Start() {
		controlBar = GetComponent<RectTransform>();
    }

    private void Update() {

    }
}
