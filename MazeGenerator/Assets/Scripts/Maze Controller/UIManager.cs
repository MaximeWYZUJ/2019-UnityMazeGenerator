﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour {

	public GameObject managerObj;
	public Button regenerate;
	public Dropdown generatorSelector;
	public Toggle animationToggle;
	public InputField gridFormatLineField;
	public InputField gridFormatColumnField;
	public Button cancelAnimation;
	public Button switchShape;
	public Toggle teleportersToggle;
	public Button hideButton;
	public GameObject leftMenu;


	private Manager manager;


	void Start () {
		manager = managerObj.GetComponent<Manager> ();

		// Add the regenerate handler
		regenerate.onClick.AddListener (RegenerateHandler);

		// Add the selection handler
		generatorSelector.onValueChanged.AddListener (delegate {
			GeneratorSelectorHandler();
		});

		// Add the toggle animation handler
		animationToggle.onValueChanged.AddListener (delegate {
			ToggleAnimationHandler ();
		});

		// Change the grid format
		gridFormatLineField.contentType = InputField.ContentType.IntegerNumber;
		gridFormatColumnField.contentType = InputField.ContentType.IntegerNumber;

		gridFormatLineField.onEndEdit.AddListener (delegate {
			GridFormatHandler(true);
		});
		gridFormatColumnField.onEndEdit.AddListener (delegate {
			GridFormatHandler (false);
		});

		// Cancel animation
		cancelAnimation.onClick.AddListener (CancelHandler);

		// Allow teleporters
		teleportersToggle.onValueChanged.AddListener (delegate {
			ToggleTeleportersHandler ();
		});

		// Switch between squares and haxagons
		switchShape.onClick.AddListener(SwitchShapeHandler);

		// Hide/Show the left menu
		hideButton.onClick.AddListener(HideButtonHandler);
	}
	

	private void RegenerateHandler() {
		manager.CancelAnimation ();
		manager.GenerateMaze ();
	}

	private void HideButtonHandler() {
		// If the menu is shown, we hide it
		if (leftMenu.activeInHierarchy) {
			leftMenu.SetActive (false);
			hideButton.GetComponentInChildren<Text> ().text = "Show";
		} else {
			leftMenu.SetActive (true);
			hideButton.GetComponentInChildren<Text> ().text = "Hide";
		}
	}

	private void CancelHandler() {
		manager.CancelAnimation ();
	}

	private void GridFormatHandler(bool line) {
		if (line) {
			manager.nbLines = int.Parse (gridFormatLineField.text);
		} else {
			manager.nbColumns = int.Parse (gridFormatColumnField.text);
		}
	}

	private void ToggleAnimationHandler() {
		manager.animated = animationToggle.isOn;
	}

	private void GeneratorSelectorHandler() {
		GeneratorType g = GeneratorType.DFS;

		switch (generatorSelector.value) {
		case 0:
			{
				g = GeneratorType.DFS;
				break;
			}
		case 1:
			{
				g = GeneratorType.Prim;
				break;
			}
		case 2:
			{
				g = GeneratorType.Wilson;
				break;
			}
		}

		manager.genType = g;
	}

	private void ToggleTeleportersHandler() {
		manager.allowTeleporters = teleportersToggle.isOn;
	}

	private void SwitchShapeHandler() {
		if (manager.nbBorders == 4) {
			manager.nbBorders = 6;
			switchShape.GetComponentInChildren<Text> ().text = "Hexagons";
		} else {
			manager.nbBorders = 4;
			switchShape.GetComponentInChildren<Text> ().text = "Squares";
		}
	}

}