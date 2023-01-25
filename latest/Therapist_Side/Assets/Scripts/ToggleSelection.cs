using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class ToggleSelection : MonoBehaviour
{
    public UnityEngine.UI.ToggleGroup ToggleGroup; // Drag & drop the desired ToggleGroup in the inspector
    Toggle selectedToggle;

    private void Start()
    {
        if (ToggleGroup == null) ToggleGroup = GetComponent<ToggleGroup>();
    }

    public void LogSelectedToggle()
    {
        // May have several selected toggles
        foreach (Toggle toggle in ToggleGroup.ActiveToggles())
        {
            Debug.Log(toggle, toggle);
        }

        // OR

        selectedToggle = ToggleGroup.ActiveToggles().First();
        if (selectedToggle != null)
            Debug.Log(selectedToggle);
    }

    public void Update()
    {
        Debug.Log(selectedToggle);
    }
}
