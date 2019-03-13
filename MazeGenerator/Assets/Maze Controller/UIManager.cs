using System.Collections;
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
	}
	

	private void RegenerateHandler() {
		manager.CancelAnimation ();
		manager.GenerateMaze ();
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

}
