using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using TMPro;
using Visualization;

[RequireComponent(typeof(PlayerStateMachine))]
public class PlayerDebug : MonoBehaviour
{
	private PlayerStateMachine _player;

	public Transform debugCanvas;
	public TextMeshProUGUI playerStateText;

	private VisualDrawer drawer;
	private ArrowVisual arrow;

	private void Start()
	{
		_player = GetComponent<PlayerStateMachine>();

		VisualDrawer drawer = new VisualDrawer();
		arrow = drawer.CreateVisual<ArrowVisual>("Vel");
		arrow.DrawVisual(Vector2.zero, Vector2.zero, Color.white);
	}

	private void LateUpdate()
	{
		string stateText = _player.StateMachine.CurrentState.ToString();
		stateText = stateText.Remove(0, 6);
		stateText = stateText.Remove(stateText.Length - 5, 5);
		playerStateText.text = stateText;

		debugCanvas.transform.position = transform.position;

		arrow.MoveVisual(_player.transform.position, (Vector2)_player.transform.position + _player.RB.velocity * 0.2f);
	}

    private void OnDrawGizmos()
    {
		/*
		string stateText = _player.StateMachine.CurrentState.ToString();
		stateText = stateText.Remove(0, 6);
		stateText = stateText.Remove(stateText.Length - 5, 5);

		Gizmos.DrawLine(transform.position, transform.position + (Vector3)_player.RB.velocity * 0.2f);
		*/
	}
}
