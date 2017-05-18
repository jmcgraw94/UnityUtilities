#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;

//Shortcut Commands: https://docs.unity3d.com/ScriptReference/MenuItem.html

public class EditorToolKit_Editor : ScriptableObject {
    [MenuItem ("Utilities/Deselect All &#d")]
    static void DoDeselect () {
        Selection.objects = new UnityEngine.Object [0];
    }
    #region Transforms
    [MenuItem ("Utilities/Snap Down &#s")]
    static void SnapToGround () {
        RaycastHit Hit;

        foreach (GameObject CurSel in Selection.objects)
            if (Physics.Raycast (CurSel.transform.position, Vector3.down, out Hit)) {
                if (Hit.collider != null) {
                    CurSel.transform.position = Hit.point;
                }
            }
    }
    [MenuItem ("Utilities/Reset Transfrom &#r")]
    static void ResetTransform () {
        foreach (GameObject CurSel in Selection.objects) {
            CurSel.transform.localEulerAngles = Vector3.zero;
            CurSel.transform.localPosition = Vector3.zero;
            CurSel.transform.localScale = Vector3.one;
        }
    }

    #region KeyboardMovement
    static void Move (Vector3 Displacement) {
        foreach (GameObject CurSel in Selection.objects) {
            CurSel.transform.position += Displacement;
        }
    }
    static float nudgeDistance = 0.1f;
    [MenuItem ("Utilities/Move/Nudge Left &LEFT")]
    static void NudgeLeft () {
        if (Camera.current) {
            Vector3 right = Vector3.Scale (Camera.current.transform.right, new Vector3 (1f, 0f, 1f)).normalized;
            Move (-nudgeDistance * right);
        }
    }
    [MenuItem ("Utilities/Move/Nudge Right &RIGHT")]
    static void NudgeRight () {
        if (Camera.current) {
            if (Camera.current) {
                Vector3 right = Vector3.Scale (Camera.current.transform.right, new Vector3 (1f, 0f, 1f)).normalized;
                Move (nudgeDistance * right);
            }
        }
    }
    [MenuItem ("Utilities/Move/Nudge Forward &UP")]
    static void NudgeForward () {
        if (Camera.current) {
            Vector3 forward = Vector3.Scale (Camera.current.transform.forward, new Vector3 (1f, 0f, 1f)).normalized;
            Move (nudgeDistance * forward);
        }
    }
    [MenuItem ("Utilities/Move/Nudge Backward &DOWN")]
    static void NudgeBackward () {
        if (Camera.current) {
            Vector3 forward = Vector3.Scale (Camera.current.transform.forward, new Vector3 (1f, 0f, 1f)).normalized;
            Move (-nudgeDistance * forward);
        }
    }
    static float shoveDistance = 2f;
    [MenuItem ("Utilities/Move/Shove Left &#LEFT")]
    static void ShoveLeft () {
        if (Camera.current) {
            Vector3 right = Vector3.Scale (Camera.current.transform.right, new Vector3 (1f, 0f, 1f)).normalized;
            Move (-shoveDistance * right);
        }
    }
    [MenuItem ("Utilities/Move/Shove Right &#RIGHT")]
    static void ShoveRight () {
        if (Camera.current) {
            if (Camera.current) {
                Vector3 right = Vector3.Scale (Camera.current.transform.right, new Vector3 (1f, 0f, 1f)).normalized;
                Move (shoveDistance * right);
            }
        }
    }
    [MenuItem ("Utilities/Move/Shove Forward &#UP")]
    static void ShoveForward () {
        if (Camera.current) {
            Vector3 forward = Vector3.Scale (Camera.current.transform.forward, new Vector3 (1f, 0f, 1f)).normalized;
            Move (shoveDistance * forward);
        }
    }
    [MenuItem ("Utilities/Move/Shove Backward &#DOWN")]
    static void ShoveBackward () {
        if (Camera.current) {
            Vector3 forward = Vector3.Scale (Camera.current.transform.forward, new Vector3 (1f, 0f, 1f)).normalized;
            Move (-shoveDistance * forward);
        }
    }
    #endregion KeyboardMovement
    #endregion Transforms

    #region CustomCreation
    static GameObject CreateEmptyGameObject (MenuCommand menuCommand) {
        GameObject go = new GameObject ("New GameObject");
        GameObjectUtility.SetParentAndAlign (go, menuCommand.context as GameObject);
        Undo.RegisterCreatedObjectUndo (go, "Create " + go.name);
        Selection.activeObject = go;
        return go;
    }

    [MenuItem ("GameObject/Triggers/Trigger Zone (Box)", false, 10)]
    static void CreateTriggerBox (MenuCommand menuCommand) {
        GameObject trigger = CreateEmptyGameObject (menuCommand);
        BoxCollider col = trigger.AddComponent<BoxCollider> ();
        col.isTrigger = true;
    }

    [MenuItem ("GameObject/Triggers/Trigger Zone (Capsule)", false, 10)]
    static void CreateTriggerCapsule (MenuCommand menuCommand) {
        GameObject trigger = CreateEmptyGameObject (menuCommand);
        CapsuleCollider col = trigger.AddComponent<CapsuleCollider> ();
        col.isTrigger = true;
    }
    #endregion CustomCreation

}
#endif
