using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.IO;
using UltimateXR.Manipulation;
using SG;
using UltimateXR.Core;
using UltimateXR.Avatar;
using UltimateXR.Devices;
// using UnityEngine.Windows.Speech;
// using ReadSpeaker;
// using System.Speech.Synthesis;
using Interhaptics.Utils;

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

public class NewMain : MonoBehaviour
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
    // private SpeechSynthesizer speechSynthesizer;
    // private KeywordRecognizer keywordRecognizer;

    private string filepath, ExerciseName, temp, playmode1;
    private Transform[] vertical, wingNut, distMeasure, horizontalGrab, cups, verticalFx, wingNutFx, measureSphere;
    private Transform[,] horizontalGrabsPairs, movableCup, horizontal,refPlanes;
    private Collider[] verticalBound = new Collider[4];
    private string recentlyGrabbed, recentlyreleased, controllerName;
    public GameObject holder;
    public GameObject box;
    public TextMeshProUGUI heightPanel;
    public Digital_MT digital_MT;
    [SerializeField] private AudioHapticSource successHaptics;
    [SerializeField] private AudioHapticSource failureHaptics;
    private float[] measuredHeight = new float[4];
    private string[] measuredText = new string[4];
    // public TMPro.TextMeshProUGUI text2;
    private bool buttonPressed = false;
    private float tapeLength;
    // private Coroutine myCoroutine;
    private UxrGrabbableObject holderGrabObj => holder.GetComponent<UxrGrabbableObject>();
    private UxrGrabbableObject boxGrabObj => box.GetComponent<UxrGrabbableObject>();

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
            // StartCoroutine(SetupExercise0());
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
        refPlanes=new Transform[4, 2];
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
            refPlanes[i,0]=scaffoldingSet[i].transform.Find("Base/wing_nut/Plane 1"); //Top Plane
            refPlanes[i,1]=scaffoldingSet[i].transform.Find("Base/Plane 2");//Bottom Plane

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
        ListenManipulationEvents(box.transform);
        ListenManipulationEvents(holder.transform);
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
            Debug.Log($"Grabbed {obj.name} with UxR");
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
    private bool IsButtonPressed(int i)
    {
        //write on panel t
        // tapeLength = Vector3.Distance(holder.transform.position, box.transform.position);
        // float holderPositionY = holder.transform.position.y;
        // float boxPositionY = box.transform.position.y;

        // Debug.Log("Holder position: " + holderPositionY);
        // Debug.Log("Box position: " + boxPositionY);
        // Debug.Log("Tape length: " + tapeLength);

        buttonPressed = UxrAvatar.LocalAvatarInput.GetButtonsEvent(UxrHandSide.Left, UxrInputButtons.Trigger, UxrButtonEventType.PressDown);
        // if (0.75f <= holderPositionY && holderPositionY <= 0.85f && 0.95f <= boxPositionY && boxPositionY <= 1.05 && buttonPressed)
        if (buttonPressed)
        {
            // text.text = "Tape length: " + tapeLength.ToString();
            measuredHeight[i] = digital_MT.measuredDist;
            // boxGrabObj.IsLockedInPlace = true;
            Debug.Log("Trigger Pressed");
            string temp = "";
            for (int j = 0; j <= 3; j++)
            {
                measuredText[j] = "Height " + (j + 1) + ": " + measuredHeight[j] + "\n";
                temp += measuredText[j];
            }
            heightPanel.text = temp;
        }
        return buttonPressed;
    }
    // make another is playerinside collider 2 script that checks if the player is inside the collider or not

    // private bool IsPlayerInsideCollider2(Collider col)
    // {
    //     // Collider col = verticalBound[i];
    //     bool result = true;
    //     if (col.bounds.Contains(MainCamera.position))
    //     {
    //         print($"{playerName} Reached the {col.name}");
    //         result = true;
    //     }
    //     else
    //     {
    //         result = false;
    //     }
    //     return result;
    //     // return col.bounds.Contains(MainCamera.position);

    // }

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
    // make courouting setupexercise8 which will setup the exercise and then call the couroutine for the next exercise




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
            // measureSphere[i].GetComponent<Renderer>().enabled = false;
        }
        scaffoldingSet[0].transform.parent.transform.position=new Vector3(0,0.3f,0);
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
        // for (int i = 0; i < 4; i++)
        // {
        //     WriteOnPanel($"Go to {i + 1} and turn on the measurment");
        //     yield return new WaitUntil(() => distMeasure[i].gameObject.activeSelf);

        // }
        Debug.Log("Coroutine started");
        WriteOnPanel("Go near the table and grab the measuring tape");
        //wait for the player to grab the measuring tape
        yield return new WaitUntil(() => CheckRecentlyGrabbed("Box"));
        for (int i = 0; i <= 3; i++)

        {

            //write on panel to go near the i th vertical and grab the measuring tape
            WriteOnPanel($"Go near the {i + 1} th vertical");
            //wait for the player to go near the i th vertical
            verticalFx[i].gameObject.SetActive(true);
            yield return new WaitUntil(() => IsPlayerInsideCollider(verticalBound[i]));
            verticalFx[i].gameObject.SetActive(false);

            //write on panel to grab the measuring tape
            // WriteOnPanel("Take the measuring tape");
            // //wait for the player to grab the measuring tape
            // yield return new WaitUntil(() => CheckRecentlyGrabbed("Box"));
            //write on panel to measure the distance between base and wingnut
            WriteOnPanel("Keep the tape on the top plane");
            yield return new WaitUntil(() => CheckCollisionWithBox(refPlanes[i,0]));
            // boxGrabObj.IsLockedInPlace=true;
            WriteOnPanel("Pull the tape to the bottom plane");
            yield return new WaitUntil(() => CheckCollisionWithHolder(refPlanes[i, 1]));
            successHaptics.Play();
            WriteOnPanel("After Noting the Measurement, Release the tape");
             yield return new WaitUntil(() => CheckRecentlyReleased("Holder"));
            WriteOnPanel("Take the measuring tape");
            yield return new WaitUntil(() => CheckRecentlyGrabbed("Box"));

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


    // public bool checkNearWINGnut()
    // {
    //     while (true)
    //     {
    //         buttonPressed = UxrAvatar.LocalAvatarInput.GetButtonsEvent(UxrHandSide.Left, UxrInputButtons.Trigger, UxrButtonEventType.PressDown);
    //         tapeLength = Vector3.Distance(holder.transform.position, box.transform.position);
    //         float holderPositionY = holder.transform.position.y;
    //         float boxPositionY = box.transform.position.y;

    //         // Debug.Log("Holder position: " + holderPositionY);
    //         // Debug.Log("Box position: " + boxPositionY);
    //         // Debug.Log("Tape length: " + tapeLength);
    //         // Set 1: Plane1 is at (0.00, 0.00, -0.13) and Plane2 is at (0.00, 0.32, -0.13)
    //         if ((holderPositionY >= 0.00f && holderPositionY <= 0.32f) || (boxPositionY >= 0.00f && boxPositionY <= 0.32f))
    //         {
    //             return true;
    //         }



    //         // if ((0.75f <= holderPositionY && holderPositionY <= 0.85f) || 0.95f <= boxPositionY && boxPositionY <= 1.05 && buttonPressed)
    //         // {
    //         //     text.text = "Tape length: " + tapeLength.ToString();
    //         //     grabObj1.IsLockedInPlace = true;
    //         //     Debug.Log("Holder Locked");

    //         //     // Set the flag to true
    //         // }

    //         // if (buttonPressed)
    //         // {
    //         //     text2.text = tapeLength.ToString();

    //         //     // Call the next coroutine
    //         // }

    //     }
    // }
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
    private bool CheckCollisionWithBox(Transform obj)
    {
        return obj.GetComponent<CollisionCheckBox>().collidedWithBox;
    }
    private bool CheckCollisionWithHolder(Transform obj)
    {
        return obj.GetComponent<CollisionCheckHolder>().collidedWithHolder;
    }


    private void TurnOnSphere()
    {
        for (int i = 0; i < 4; i++)
        {

            measureSphere[i].GetComponent<Renderer>().enabled = true;

        }
    }

    // private bool InRangeY(Vector3 position, Vector3 minRange, Vector3 maxRange)
    // {
    //     // return position.x >= minRange.x && position.x <= maxRange.x
    //     //     || position.y >= minRange.y && position.y <= maxRange.y
    //     //     || position.z >= minRange.z && position.z <= maxRange.z;
    //     return position.y>=minRange.y && position.y <= maxRange.y;
    // }

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