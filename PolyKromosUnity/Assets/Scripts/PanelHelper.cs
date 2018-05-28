using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PanelHelper : MonoBehaviour {
    public void TurnOffOtherRightPanels() {
        UIController.singleton.TurnOffOtherRightPanels(this.gameObject.name);
    }
}
