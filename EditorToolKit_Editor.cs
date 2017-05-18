#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;

//Shortcut Commands: https://docs.unity3d.com/ScriptReference/MenuItem.html

public class EditorToolKit_Editor : ScriptableObject {
    [MenuItem("Utilities/Deselect All &#d")]
    static void DoDeselect() {
        Selection.objects = new UnityEngine.Object[0];
    }

    [MenuItem("Utilities/Snap Down &#s")]
    static void SnapToGround() {
        RaycastHit Hit;

        foreach (GameObject CurSel in Selection.objects)
            if (Physics.Raycast(CurSel.transform.position, Vector3.down, out Hit)) {
                if (Hit.collider != null) {
					CurSel.transform.position = Hit.point;
                }
            }
    }
	
	[MenuItem("Utilities/Toggle Active %e")]
    static void ToggleActive() {
        foreach (GameObject CurSel in Selection.objects) {
            CurSel.SetActive(!CurSel.activeSelf);
        }
    }
}
#endif