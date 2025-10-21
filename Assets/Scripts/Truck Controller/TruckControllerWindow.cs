using UnityEngine;
using UnityEditor;

public class TruckControllerWindow : EditorWindow
{
    private GameObject truckPrefab;

    [Header("Engine Settings")]
    private float engineIdleRPM = 800f;
    private float engineMaxRPM = 2500f;
    private float torque = 400f;
    private float brakeTorque = 5000f;
    private bool useAutomatic = true;

    [Header("Suspension Settings")]
    private float suspensionDistance = 0.3f;
    private float suspensionSpring = 35000f;
    private float suspensionDamper = 4500f;
    private float wheelMass = 100f;

    [MenuItem("Tools/Truck Controller Setup")]
    public static void ShowWindow()
    {
        GetWindow<TruckControllerWindow>("Truck Controller");
    }

    private void OnGUI()
    {
        GUILayout.Label("Truck Controller Setup", EditorStyles.boldLabel);

        truckPrefab = (GameObject)EditorGUILayout.ObjectField("Truck Prefab", truckPrefab, typeof(GameObject), true);

        GUILayout.Space(10);
        GUILayout.Label("Engine Settings", EditorStyles.boldLabel);
        engineIdleRPM = EditorGUILayout.FloatField("Idle RPM", engineIdleRPM);
        engineMaxRPM = EditorGUILayout.FloatField("Max RPM", engineMaxRPM);
        torque = EditorGUILayout.FloatField("Torque", torque);
        brakeTorque = EditorGUILayout.FloatField("Brake Torque", brakeTorque);
        useAutomatic = EditorGUILayout.Toggle("Automatic Transmission", useAutomatic);

        GUILayout.Space(10);
        GUILayout.Label("Suspension Settings", EditorStyles.boldLabel);
        suspensionDistance = EditorGUILayout.FloatField("Suspension Distance", suspensionDistance);
        suspensionSpring = EditorGUILayout.FloatField("Spring Force", suspensionSpring);
        suspensionDamper = EditorGUILayout.FloatField("Damper Force", suspensionDamper);
        wheelMass = EditorGUILayout.FloatField("Wheel Mass", wheelMass);

        GUILayout.Space(10);
        if (GUILayout.Button("Setup Truck Controller"))
        {
            if (truckPrefab != null)
            {
                SetupTruckController(truckPrefab);
            }
            else
            {
                EditorUtility.DisplayDialog("Error", "Please assign a truck prefab first.", "OK");
            }
        }
    }

    private void SetupTruckController(GameObject truck)
    {
        if (truck.GetComponent<TruckController>() == null)
        {
            TruckController controller = truck.AddComponent<TruckController>();
            controller.engineIdleRPM = engineIdleRPM;
            controller.engineMaxRPM = engineMaxRPM;
            controller.torque = torque;
            controller.brakeTorque = brakeTorque;
            controller.useAutomatic = useAutomatic;

            controller.suspensionDistance = suspensionDistance;
            controller.suspensionSpring = suspensionSpring;
            controller.suspensionDamper = suspensionDamper;
            controller.wheelMass = wheelMass;

            EditorUtility.SetDirty(truck);
            Debug.Log("TruckController added and configured!");
        }
        else
        {
            Debug.LogWarning("Truck already has a TruckController.");
        }
    }
}
