using UnityEngine;
using System.ComponentModel;
using SRDebugger;

public class SettingsButton : MonoBehaviour {

  public void OpenSettings() {
    SRDebug.Instance.ShowDebugPanel(DefaultTabs.Options, false);
  }

}
