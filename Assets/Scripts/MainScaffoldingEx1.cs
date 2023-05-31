using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.IO;
using UltimateXR.Manipulation;
using SG;
// [ExecuteInEditMode]

public class MainScaffoldingEx1 : MonoBehaviour
{

    [SerializeField] private string playerName;
    [SerializeField] private float targetHeight;
    [SerializeField] private float[] verticalHeight = new float[4];
    [SerializeField] private GameObject[] scaffoldingSets = new GameObject[4];
    [SerializeField] private TextMeshPro instructionText;
    [SerializeField] private Transform VRcamera;
    // [SerializeField] private Material _material;
    // [SerializeField] private GameObejct movablecupColliders;
    private List<int> changedVerticals = new List<int>();
    private string filepath;
    private Transform[] verticals, horizontals, wingNuts, distMeasures, horizontalGrabs, movableCups, verticalFx, wingNutFx;
    private Transform[,] horizontalGrabsPairs;
    private Collider[] verticalBounds = new Collider[4];
    private string recentlyGrabbed, recentlyreleased;
    TextWriter tw;
    // public bool check1;
    void Start()
    {
        PlayerDataFile(playerName);
        int n = scaffoldingSets.Length;
        verticals = new Transform[n];
        horizontals = new Transform[n];
        horizontalGrabs = new Transform[n];
        wingNuts = new Transform[n];
        distMeasures = new Transform[n];
        horizontalGrabsPairs = new Transform[4, 2];
        movableCups = new Transform[n];
        verticalFx = new Transform[n];
        wingNutFx = new Transform[n];
        GetObjects();
        // verticalLedgers[0].GetComponent<FlashMaterial>().FlashOn();
        StartCoroutine(SetVerticalHeight());
        StartCoroutine(Procedure());
        // WriteCSV();
    }

    // Update is called once per frame
    void Update()
    {
        // Debug.Log(scaffoldingSet1.transform.Find("Ledgers/Vertical Ledger").name);

    }
    private void GetObjects()
    {
        for (int i = 0; i < 4; i++)
        {
            verticals[i] = scaffoldingSets[i].transform.Find("Ledgers/Vertical");
            horizontals[i] = scaffoldingSets[i].transform.Find("Bottom Horizontal/Horizontal");
            horizontalGrabs[i] = scaffoldingSets[i].transform.Find("Ledgers/Horizontal Grabs");
            movableCups[i] = scaffoldingSets[i].transform.Find("Ledgers/Vertical/movable_cup");
            wingNuts[i] = scaffoldingSets[i].transform.Find("Base/wing_nut/nut");
            distMeasures[i] = scaffoldingSets[i].transform.Find("Distance Measurer");
            verticalBounds[i] = scaffoldingSets[i].transform.Find("Vertical Boundary").GetComponent<Collider>();
            verticalFx[i] = scaffoldingSets[i].transform.Find("Vertical Highlight");
            wingNutFx[i] = scaffoldingSets[i].transform.Find("Base/wing_nut/curved arrow");
            int j = 0;
            foreach (Transform item in horizontalGrabs[i])
            {
                horizontalGrabsPairs[i, j] = item;
                AddMainpulationEvents(horizontalGrabsPairs[i, j]);
                Debug.Log(horizontalGrabsPairs[i, j].name);
                j = 1;
            }
            distMeasures[i].GetComponent<MeasureDistance>().targetDist = targetHeight;
            // AddMainpulationEvents([i]);
            AddMainpulationEvents(wingNuts[i]);

        }
    }
    IEnumerator Procedure()
    {
        for (int i = 0; i < 4; i++)
        {
            instructionText.text = $"Go to {i + 1} and turn on the measurment";
            yield return new WaitUntil(() => distMeasures[i].gameObject.activeSelf);

        }
        // yield return new WaitUntil(IsAllMeasurementOn);
        foreach (int i in changedVerticals)
        {
            instructionText.text = $"Go to the {i + 1} Vertical to adjust the height";
            verticalFx[i].gameObject.SetActive(true);
            // verticals[i].GetComponent<FlashMaterial>().FlashOn();
            yield return new WaitUntil(() => CheckInside(i));
            verticalFx[i].gameObject.SetActive(false);
            // verticals[i].GetComponent<FlashMaterial>().FlashOff();
            movableCups[i].GetComponent<Rigidbody>().isKinematic = true;
            instructionText.text = "Grab the horizontal ledger";
            horizontalGrabsPairs[i, 0].GetComponent<FlashMaterial>().FlashOn();
            string grabName = horizontalGrabsPairs[i, 0].name;
            yield return new WaitUntil(() => CheckRecentlyGrabbed(grabName));
            horizontalGrabsPairs[i, 0].GetComponent<FlashMaterial>().FlashOff();
            instructionText.text = "Grab the wingnut and rotate";
            StartCoroutine(WingNutCue(i, "snap 1"));
            yield return new WaitUntil(() => distMeasures[i].GetComponent<MeasureDistance>().reachedTarget);
            // instructionText.text = "Adjust the Height";

        }
        instructionText.text = "You have successfully Completed the Exercise";
    }

    public void PlayerDataFile(string playerName)
    {
        filepath = Application.dataPath + "/Player Data/";
        filepath += playerName + ".csv";
        tw = new StreamWriter(filepath, false);
        tw.WriteLine("Time, Object Name, Action");
        tw.Close();
        tw = new StreamWriter(filepath, true);
    }
    private void ObjGrabbed(object obj1, object obj2)
    {
        // Debug.Log(obj1.GetType());
        Object obj = new Object();
        if (obj1.GetType() == typeof(UxrGrabbableObject))
        {
            obj = obj1 as UxrGrabbableObject;
            Debug.Log($"Grabbed {obj.name} with UxR");
        }
        else if (obj1.GetType() == typeof(SG_SimpleDrawer))
        {
            obj = obj1 as SG_SimpleDrawer;
            Debug.Log($"Grabbed {obj.name} with SG");
        }
        else if (obj1.GetType() == typeof(SG_Rotater))
        {
            obj = obj1 as SG_Rotater;
            Debug.Log($"Grabbed {obj.name} with SG");
        }
        recentlyGrabbed = obj.name;
        recentlyreleased = "";
        tw.WriteLine($"{System.DateTime.Now.ToString("HH:mm:ss")},{recentlyGrabbed},Grabbed");
    }
    private void ObjectReleased(object obj1, object obj2)
    {
        // Debug.Log(obj1.GetType());
        Object obj = new Object();
        if (obj1.GetType() == typeof(UxrGrabbableObject))
        {
            obj = obj1 as UxrGrabbableObject;
            Debug.Log($"Released {obj.name} with UxR");
        }
        else if (obj1.GetType() == typeof(SG_SimpleDrawer))
        {
            obj = obj1 as SG_SimpleDrawer;
            Debug.Log($"Released {obj.name} with SG");
        }
        else if (obj1.GetType() == typeof(SG_Rotater))
        {
            obj = obj1 as SG_Rotater;
            Debug.Log($"Released {obj.name} with SG");
        }
        recentlyreleased = obj.name;
        recentlyGrabbed = "";
        tw.WriteLine($"{System.DateTime.Now.ToString("HH:mm:ss")},{obj.name},Released");
    }
    IEnumerator SetVerticalHeight()
    {
        Transform wingNut, Ledgers;
        Vector3 pos;
        for (int i = 0; i < 4; i++)
        {
            if (verticalHeight[i] != targetHeight)
            {
                changedVerticals.Add(i);
            }

            movableCups[i].GetComponent<Rigidbody>().isKinematic = true;
            wingNut = scaffoldingSets[i].transform.Find("Base/wing_nut");
            pos = wingNut.transform.position;
            pos.y = verticalHeight[i];
            wingNut.transform.position = pos;
            Ledgers = scaffoldingSets[i].transform.Find("Ledgers");
            // pos = verticalLedgers[i].position;
            pos = Ledgers.transform.position;
            pos.y = verticalHeight[i];
            Ledgers.transform.position = pos;
        }
        // verticalLedgers[i].GetComponent<Rigidbody>().MovePosition(pos);
        // Ledgers.GetComponent<Rigidbody>().MovePosition(pos);
        // verticalLedgers[i].transform.position = pos;
        // verticalLedgers[i].Find("movable_cup/Colliders").gameObject.SetActive(true);
        yield return new WaitForSeconds(10f);
        // yield return null;
        for (int i = 0; i < 4; i++)
        {
            movableCups[i].GetComponent<Rigidbody>().isKinematic = false;
        }
        yield return new WaitForSeconds(10f);
        for (int i = 0; i < 4; i++)
        {
            movableCups[i].GetComponent<Rigidbody>().isKinematic = true;
        }
    }

    private bool IsAllMeasurementOn()
    {
        bool result = true;
        foreach (Transform o in distMeasures)
        {
            result = result && o.gameObject.activeSelf;
        }

        return result;
        // return check1;
    }

    private bool CheckInside(int i)
    {
        Collider col = verticalBounds[i];
        bool result = true;
        if (col.bounds.Contains(VRcamera.position))
        {
            print("point is inside collider");
            result = true;
        }
        else
        {
            result = false;
        }
        return result;

    }
    private bool CheckRecentlyGrabbed(string objName)
    {
        if (objName == recentlyGrabbed)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
    private void AddMainpulationEvents(Transform obj)
    {
        obj.GetComponent<UxrGrabbableObject>().Grabbed += ObjGrabbed;
        obj.GetComponent<UxrGrabbableObject>().Released += ObjectReleased;
        if (obj.GetComponent<SG_SimpleDrawer>() != null)
        {
            obj.GetComponent<SG_SimpleDrawer>().ObjectGrabbed.AddListener(ObjGrabbed);
            obj.GetComponent<SG_SimpleDrawer>().ObjectReleased.AddListener(ObjectReleased);
        }
        else if (obj.GetComponent<SG_Rotater>() != null)
        {
            obj.GetComponent<SG_Rotater>().ObjectGrabbed.AddListener(ObjGrabbed);
            obj.GetComponent<SG_Rotater>().ObjectReleased.AddListener(ObjectReleased);
        }
    }
    //
    void OnDestroy()
    {
        if (Application.isPlaying)
        {
            Debug.Log("Destroyed in play mode");
            tw.Close();
        }
    }
    private IEnumerator WingNutCue(int i, string snap)
    {
        bool reachedTarget = false;
        while (!reachedTarget)
        {
            wingNuts[i].Find(snap).GetComponent<FlashMaterial>().FlashOn();
            wingNutFx[i].gameObject.SetActive(true);
            string grabName = wingNuts[i].name;
            yield return new WaitUntil(() => CheckRecentlyGrabbed(grabName));
            recentlyGrabbed = "";
            wingNuts[i].Find(snap).GetComponent<FlashMaterial>().FlashOff();
            wingNutFx[i].gameObject.SetActive(false);
            yield return new WaitUntil(() => CheckAllowChange(wingNuts[i]));
            snap = snap == "snap 1" ? "snap 2" : "snap 1";
            reachedTarget = distMeasures[i].GetComponent<MeasureDistance>().reachedTarget;
        }
    }
    private bool CheckWingNut(int i)
    {
        bool reachedTarget = distMeasures[i].GetComponent<MeasureDistance>().reachedTarget;
        return reachedTarget;

    }
    private bool CheckAllowChange(Transform obj)
    {
        return obj.GetComponent<RotationConstraint>().allowChange;
    }
}
