using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.IO;
using UltimateXR.Manipulation;
using SG;
using UltimateXR.Core;
using UltimateXR.Avatar;
/*
Note:
Naming Convention in Scene View:
    movable_cup12 ==> 1--> Vertical 1 | 2 --> Cup 2
    movable_cup34 ==> 3--> Vertical 3 | 4 --> Cup 4
    [4]---[3]
     |      |
    [1]---[2]
    BH_Grab_43    ==> 4--> lifts the Vertical 4 | 3 --> Bottom horizontal number
*/

public class MainTraining : MonoBehaviour
{
    // Start is called before the first frame update
    enum controllers
    {
        VR,
        SG,
    }
    enum _playMode
    {
        Training,
        Evaluation,
    }
    [Header("----------------General Settings--------------")]
    [SerializeField] private string playerName;
    // [SerializeField] private controllers controllerUsed = controllers.SG;
    [SerializeField] private _playMode playMode = _playMode.Training;
    [SerializeField] private Transform MainCamera;
    [SerializeField] private bool resetLocation;

    [SerializeField] private Transform playerPos;
    [SerializeField] private TextMeshProUGUI panelText;
    [SerializeField] private GameObject[] scaffoldingSet = new GameObject[4];
    [SerializeField] bool playExercise1 = false;
    [SerializeField] private bool playExercise2 = false;
    // [SerializeField] private bool EvaluationMode;
    [Header("----------------Exercise 1 Settings--------------")]
    [SerializeField] private float targetHeight;
    [SerializeField] private float[] verticalHeight = new float[4];

    [Header("----------------Exercise 2 Settings--------------")]
    [SerializeField] private GameObject hammer;
    private List<int> changedVerticals = new List<int>();
    private string filepath, ExerciseName, temp, playmode1;
    private Transform[] vertical, wingNut, distMeasure, horizontalGrab, cups, verticalFx, wingNutFx, measureSphere;
    private Transform[,] horizontalGrabsPairs, movableCup, horizontal;
    private Collider[] verticalBound = new Collider[4];
    private string recentlyGrabbed, recentlyreleased, controllerName;
    TextWriter tw;
    void Start()
    {
        // playerName += "_" + controllerUsed.ToString();
        // UxrManager.Instance.MoveAvatarTo(UxrAvatar.LocalAvatar, playerPos.position);

        PlayerDataFile(playerName);
        GetObjects();
        AddManipulationEvents();
        playmode1 = playMode.ToString();
        if (playExercise1)
        {
            // playerName += "Ex2";
            // StartCoroutine(Exercise1());
            StartCoroutine(SetupExercsise1());
        }
        else if (playExercise2)
        {
            // playerName += "Ex2";
            SetupExercise2();
        }

    }

    // Update is called once per frame
    // private void Awake()
    // {
    //     Vector3 pos = UxrAvatar.transform.position;
    //     pos.x = 0.6f;
    //     pos.z = -0.6f;
    //     UxrAvatar.transform.position = pos;
    //     // UxrAvatar.transform.rotation = Quaternion.identity;
    // }
    void Update()
    {
        if (resetLocation)
        {
            UxrManager.Instance.TeleportLocalAvatar(playerPos.position, Quaternion.identity);
            resetLocation = false;
        }
    }

    public void PlayerDataFile(string playerName)
    {
        filepath = Application.dataPath + "/Player Data/PlayerLog.csv";
        // filepath += playerName + ".csv";
        tw = new StreamWriter(filepath, true);
        // tw.WriteLine("Time,Player Name,Exercise, Object Name, Action");
        // tw.Close();
        // tw = new StreamWriter(filepath, true);
    }

    private void GetObjects()
    {
        int n = 4;
        vertical = new Transform[n];
        horizontal = new Transform[4, 4];
        horizontalGrab = new Transform[n];
        wingNut = new Transform[n];
        distMeasure = new Transform[n];
        horizontalGrabsPairs = new Transform[4, 2];
        movableCup = new Transform[4, 4];
        cups = new Transform[n];
        verticalFx = new Transform[n];
        wingNutFx = new Transform[n];
        measureSphere = new Transform[n];
        for (int i = 0; i < 4; i++)
        {
            vertical[i] = scaffoldingSet[i].transform.Find("Ledgers/Vertical");
            // horizontal[i] = set[i].transform.Find("Bottom Horizontal/Horizontal");
            horizontalGrab[i] = scaffoldingSet[i].transform.Find("Ledgers/Horizontal Grabs");
            cups[i] = scaffoldingSet[i].transform.Find("Ledgers/Vertical/Cups");
            wingNut[i] = scaffoldingSet[i].transform.Find("Base/wing_nut/nut");
            distMeasure[i] = scaffoldingSet[i].transform.Find("Distance Measurer");
            measureSphere[i] = scaffoldingSet[i].transform.Find("Distance Measurer/Sphere");
            verticalBound[i] = scaffoldingSet[i].transform.Find("Vertical Highlight").GetComponent<Collider>();
            verticalFx[i] = scaffoldingSet[i].transform.Find("Vertical Highlight");
            wingNutFx[i] = scaffoldingSet[i].transform.Find("Base/wing_nut/curved arrow");
            horizontalGrabsPairs[i, 0] = horizontalGrab[i].GetChild(0);
            horizontalGrabsPairs[i, 1] = horizontalGrab[i].GetChild(1);
            distMeasure[i].GetComponent<MeasureDistance>().targetDist = targetHeight;

            for (int k = 0; k < 4; k++)
            {
                movableCup[i, k] = cups[i].GetChild(k).GetChild(0);
                horizontal[i, k] = scaffoldingSet[i].transform.Find("Ledgers/Horizontals").GetChild(k);

            }

        }
    }

    private void AddManipulationEvents()
    {
        for (int i = 0; i < 4; i++)
        {
            ListenManipulationEvents(horizontalGrabsPairs[i, 0]);
            ListenManipulationEvents(horizontalGrabsPairs[i, 1]);
            ListenManipulationEvents(wingNut[i]);
            for (int k = 0; k < 4; k++)
            {
                ListenManipulationEvents(movableCup[i, k]);
            }
        }
        ListenManipulationEvents(hammer.transform);
    }


    private void ListenManipulationEvents(Transform obj)
    {
        obj.GetComponent<UxrGrabbableObject>().Grabbed += ObjGrabbed;
        obj.GetComponent<UxrGrabbableObject>().Released += ObjReleased;
        if (obj.GetComponent<SG_SimpleDrawer>() != null)
        {
            obj.GetComponent<SG_SimpleDrawer>().ObjectGrabbed.AddListener(ObjGrabbed);
            obj.GetComponent<SG_SimpleDrawer>().ObjectReleased.AddListener(ObjReleased);
        }
        else if (obj.GetComponent<SG_Rotater>() != null)
        {
            obj.GetComponent<SG_Rotater>().ObjectGrabbed.AddListener(ObjGrabbed);
            obj.GetComponent<SG_Rotater>().ObjectReleased.AddListener(ObjReleased);
        }
        else if (obj.GetComponent<SG_Grabable>() != null)
        {
            obj.GetComponent<SG_Grabable>().ObjectGrabbed.AddListener(ObjGrabbed);
            obj.GetComponent<SG_Grabable>().ObjectReleased.AddListener(ObjReleased);
        }
    }


    private void ObjGrabbed(object obj1, object obj2)
    {
        // Debug.Log(obj1.GetType());
        Object obj = new Object();
        if (obj1.GetType() == typeof(UxrGrabbableObject))
        {
            obj = obj1 as UxrGrabbableObject;
            // Debug.Log($"Grabbed {obj.name} with UxR");
            controllerName = "VR";
        }
        else if (obj1.GetType() == typeof(SG_SimpleDrawer))
        {
            obj = obj1 as SG_SimpleDrawer;
            // Debug.Log($"Grabbed {obj.name} with SG");
            controllerName = "SG";
        }
        else if (obj1.GetType() == typeof(SG_Rotater))
        {
            obj = obj1 as SG_Rotater;
            // Debug.Log($"Grabbed {obj.name} with SG");
            controllerName = "SG";
        }
        else if (obj1.GetType() == typeof(SG_Grabable))
        {
            obj = obj1 as SG_Grabable;
            // Debug.Log($"Grabbed {obj.name} with SG");
            controllerName = "SG";
        }
        recentlyGrabbed = obj.name;
        recentlyreleased = "";
        temp = $"{System.DateTime.Now.ToString("HH:mm:ss")},{controllerName},{playerName},{playmode1},{ExerciseName},{recentlyGrabbed},Grabbed";
        tw.WriteLine(temp);
    }


    private void ObjReleased(object obj1, object obj2)
    {
        // Debug.Log(obj1.GetType());
        Object obj = new Object();
        if (obj1.GetType() == typeof(UxrGrabbableObject))
        {
            obj = obj1 as UxrGrabbableObject;
            // Debug.Log($"Released {obj.name} with UxR");
        }
        else if (obj1.GetType() == typeof(SG_SimpleDrawer))
        {
            obj = obj1 as SG_SimpleDrawer;
            // Debug.Log($"Released {obj.name} with SG");
        }
        else if (obj1.GetType() == typeof(SG_Rotater))
        {
            obj = obj1 as SG_Rotater;
            // Debug.Log($"Released {obj.name} with SG");
        }
        else if (obj1.GetType() == typeof(SG_Grabable))
        {
            obj = obj1 as SG_Grabable;
            // Debug.Log($"Grabbed {obj.name} with SG");
            // controllerName = "SG";
        }
        recentlyreleased = obj.name;
        recentlyGrabbed = "";
        temp = $"{System.DateTime.Now.ToString("HH:mm:ss")},{controllerName},{playerName},{playmode1},{ExerciseName},{recentlyreleased},Released";
        tw.WriteLine(temp);
    }

    private bool CheckRecentlyGrabbed(string objName)
    {
        return objName == recentlyGrabbed ? true : false;
    }
    private bool CheckRecentlyReleased(string objName)
    {
        return objName == recentlyreleased ? true : false;
    }

    private bool IsPlayerInsideCollider(Collider col)
    {
        // Collider col = verticalBound[i];
        bool result = true;
        if (col.bounds.Contains(MainCamera.position))
        {
            print($"{playerName} Reached the {col.name}");
            result = true;
        }
        else
        {
            result = false;
        }
        return result;
        // return col.bounds.Contains(MainCamera.position);

    }

    private void WriteOnPanel(string _text)
    {
        panelText.text = _text;
    }

    private bool CheckCupAngle(Transform cup)
    {
        return cup.eulerAngles.y == 0f ? true : false;
    }

    private bool IsCupLocked(Transform cup)
    {
        bool reachedLimit = cup.GetComponentInParent<RotateObjOnTriggerEnter>().reachedLimit;
        return reachedLimit;
    }




    IEnumerator SetupExercsise1()
    {
        ExerciseName = "Ex1";
        Transform wingNut, Ledgers;
        Vector3 pos, eulerRot;
        eulerRot = new Vector3(0, -90f, 0);
        for (int i = 0; i < 4; i++)
        {
            movableCup[i, 0].transform.Rotate(eulerRot, Space.Self);
            if (verticalHeight[i] != targetHeight)
            {
                changedVerticals.Add(i);
            }

            movableCup[i, 0].GetComponentInParent<Rigidbody>().isKinematic = true;
            wingNut = scaffoldingSet[i].transform.Find("Base/wing_nut");
            pos = wingNut.transform.position;
            pos.y = verticalHeight[i];
            wingNut.transform.position = pos;
            Ledgers = scaffoldingSet[i].transform.Find("Ledgers/Vertical");
            pos = Ledgers.transform.position;
            pos.y = verticalHeight[i];
            Ledgers.transform.position = pos;
        }
        yield return new WaitForSeconds(10f);
        for (int i = 0; i < 4; i++)
        {
            movableCup[i, 0].GetComponentInParent<Rigidbody>().isKinematic = false;
        }
        yield return new WaitForSeconds(5f);
        eulerRot = new Vector3(0, 90, 0);
        for (int i = 0; i < 4; i++)
        {
            movableCup[i, 0].GetComponentInParent<Rigidbody>().isKinematic = true;
            // horizontal[i, 0].GetComponentInParent<Rigidbody>().isKinematic = true;
            movableCup[i, 0].Rotate(eulerRot, Space.Self);
            measureSphere[i].GetComponent<Renderer>().enabled = false;
        }
        if (playmode1 == "Training")
        {
            // playerName += "Ex2";
            StartCoroutine(Exercise1());
            // StartCoroutine(SetupExercsise1());
        }
        else
        {
            WriteOnPanel("Start");
        }

        // StartCoroutine(Exercise1());
    }

    IEnumerator Exercise1()
    {
        // StartCoroutine(SetupExercsise1());
        string grabName = "";
        for (int i = 0; i < 4; i++)
        {
            WriteOnPanel($"Go to {i + 1} and turn on the measurment");
            yield return new WaitUntil(() => distMeasure[i].gameObject.activeSelf);

        }
        // yield return new WaitUntil(IsAllMeasurementOn);
        foreach (int i in changedVerticals)
        {
            WriteOnPanel($"Go to the {i + 1} Vertical to adjust the height");
            verticalFx[i].gameObject.SetActive(true);
            yield return new WaitUntil(() => IsPlayerInsideCollider(verticalBound[i]));
            verticalFx[i].gameObject.SetActive(false);
            movableCup[i, 0].GetComponentInParent<Rigidbody>().isKinematic = true;
            WriteOnPanel("Grab the horizontal ledger");
            horizontalGrabsPairs[i, 0].GetComponent<FlashMaterial>().FlashOn();
            grabName = horizontalGrabsPairs[i, 0].name;
            yield return new WaitUntil(() => CheckRecentlyGrabbed(grabName));
            horizontalGrabsPairs[i, 0].GetComponent<FlashMaterial>().FlashOff();
            WriteOnPanel("Grab the wingnut and rotate");
            StartCoroutine(WingNutCue(i, "snap 1"));
            yield return new WaitUntil(() => distMeasure[i].GetComponent<MeasureDistance>().reachedTarget);

        }
        WriteOnPanel("You have successfully Completed the First Exercise");
        if (playExercise1 && playExercise2)
        {
            StartCoroutine(Exercise2());
        }
    }
    private void SetupExercise2()
    {
        // Vector3 eulerRot;
        ExerciseName = "Ex2";
        // eulerRot = new Vector3(0, 0, 0);
        // Quaternion cupRot = new Quaternion();
        // cupRot.eulerAngles = eulerRot;
        for (int i = 0; i < 4; i++)
        {

            // cups[i].transform.rotation = cupRot;
            horizontal[i, 1].gameObject.SetActive(true);
            horizontal[i, 2].gameObject.SetActive(true);
            horizontal[i, 1].GetComponentInParent<Rigidbody>().isKinematic = true;
            horizontal[i, 2].GetComponentInParent<Rigidbody>().isKinematic = true;


        }
        if (playmode1 == "Training")
        {
            StartCoroutine(Exercise2());
        }
        else
        {
            WriteOnPanel("Start");
        }

    }
    IEnumerator Exercise2()
    {
        string heading = "<size=80%><align=\"center\"><u>Exercise-2: Locking of Cups</align></u><size=60%>\n";
        string temp = "";
        string grabName = "";
        Transform cup;
        // float angle = 0f;
        temp = "<align=\"left\">Tasks:\n<indent=15%> 1.Initial tightening with hands\n2.Locking with hammer</indent>";
        WriteOnPanel(heading + temp);
        yield return new WaitForSeconds(10);
        for (int i = 0; i < 4; i++)
        {
            WriteOnPanel($"Go to Vertical {i + 1}");
            verticalFx[i].gameObject.SetActive(true);
            yield return new WaitUntil(() => IsPlayerInsideCollider(verticalBound[i]));
            verticalFx[i].gameObject.SetActive(false);
            for (int j = 1; j < 3; j++)
            {
                cup = movableCup[i, j];
                WriteOnPanel($"Grab and Rotate the Movable Cup {j + 1}");
                cup.GetComponent<FlashMaterial>().FlashOn();
                grabName = cup.name;
                yield return new WaitUntil(() => CheckRecentlyGrabbed(grabName));
                cup.GetComponent<FlashMaterial>().FlashOff();
                WriteOnPanel("Rotate the Movable Cup");
                // yield return new WaitUntil(() => CheckCupAngle(cup));
                yield return new WaitUntil(() => CheckRecentlyReleased(grabName));
                WriteOnPanel("Grab the Hammer");
                hammer.transform.position = scaffoldingSet[i].transform.TransformPoint(-0.20f, 0.99f, 0);
                grabName = hammer.name;
                yield return new WaitUntil(() => CheckRecentlyGrabbed(grabName));
                WriteOnPanel("Hit the Cup to tighten");
                yield return new WaitUntil(() => (IsCupLocked(cup)));
                WriteOnPanel("Good Job, Cup Locked");
                yield return new WaitForSeconds(2f);
            }
        }
    }


    private IEnumerator WingNutCue(int i, string snap)
    {
        bool reachedTarget = false;
        while (!reachedTarget)
        {
            wingNut[i].Find(snap).GetComponent<FlashMaterial>().FlashOn();
            wingNutFx[i].gameObject.SetActive(true);
            string grabName = wingNut[i].name;
            yield return new WaitUntil(() => CheckRecentlyGrabbed(grabName));
            recentlyGrabbed = "";
            wingNut[i].Find(snap).GetComponent<FlashMaterial>().FlashOff();
            wingNutFx[i].gameObject.SetActive(false);
            yield return new WaitUntil(() => CheckAllowChange(wingNut[i]));
            snap = snap == "snap 1" ? "snap 2" : "snap 1";
            reachedTarget = distMeasure[i].GetComponent<MeasureDistance>().reachedTarget;
        }
    }


    private bool CheckAllowChange(Transform obj)
    {
        return obj.GetComponent<RotationConstraint>().allowChange;
    }


    private void TurnOnSphere()
    {
        for (int i = 0; i < 4; i++)
        {

            measureSphere[i].GetComponent<Renderer>().enabled = true;

        }
    }
    private void TurnOffSphere()
    {
        for (int i = 0; i < 4; i++)
        {

            measureSphere[i].GetComponent<Renderer>().enabled = false;

        }
    }

    void OnDestroy()
    {
        if (Application.isPlaying)
        {
            Debug.Log("Destroyed in play mode");
            tw.Close();
        }
    }

}
