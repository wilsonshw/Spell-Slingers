using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using PDollarGestureRecognizer;
using System.IO;

using UnityEngine.Events;

public class GestureRecognizer : MonoBehaviour
{
    public string whichHand;
    public GameObject summonSpawnerObj;
    SummonSpawner summonScript;
    public XRNode inputSource;
    public InputHelpers.Button inputButton;
    public float inputThreshold = 0.1f;
    public Transform movementSource;

    [SerializeField]
    private string[] gestureClass = { "Meteor", "MeteorR", "MeteorL", "Fireball", "DragonWall", "EarthWall" };

    public float newPosThresholdDistance = 0.05f;
    public GameObject debugCubePrefab;
    public bool creationMode = true; //create or recognize gesture
    public string newGestureName; //if in create mode

    public float recognitionThreshold = 0.9f;

    [System.Serializable]
    public class UnityStringEvent : UnityEvent<string> { }
    public UnityStringEvent OnRecognizedProjectile;
    public UnityStringEvent OnRecognizedStatic;
    public UnityStringEvent OnRecognizedSummon;

    private List<Gesture> trainingSet = new List<Gesture>();
    private bool isMoving = false;
    private List<Vector3> positionsList = new List<Vector3>();

    public GameObject projectileSpawnerObj;
    public GameObject staticSpawnerObj;
    // Start is called before the first frame update

    RaycastHit hit;
    bool hitGround;
    Vector3 groundHitPosition;

    void Start()
    {
        summonScript = summonSpawnerObj.GetComponent<SummonSpawner>();
        string[] gestureFiles = Directory.GetFiles(Application.persistentDataPath, "*.xml");
        foreach (var item in gestureFiles)
        {
            trainingSet.Add(GestureIO.ReadGestureFromFile(item));
        }
    }

    // Update is called once per frame
    void Update()
    {
        InputHelpers.IsPressed(InputDevices.GetDeviceAtXRNode(inputSource), inputButton, out bool isPressed, inputThreshold);

        //start movement
        if (!isMoving && isPressed)
        {
            if (Physics.Raycast(movementSource.position, movementSource.forward, out hit, 50))
            {
                if (hit.transform.tag == "Ground")
                {
                    //Debug.Log("hitting ground!!!");
                    if (!hitGround)
                    {
                        hitGround = true;
                        groundHitPosition = hit.point;
                    }
                }
            }

            StartMovement();

        }
        else if (isMoving && !isPressed) //ending movement
        {
            EndMovement();
        }
        else if (isMoving && isPressed) //updating movement
        {
            UpdateMovement();
        }

    }

    void StartMovement()
    {
        if (!hitGround)
        {
            projectileSpawnerObj.GetComponent<ProjectileSpawner>().whichHand = movementSource;
        }
        else
        {
            staticSpawnerObj.GetComponent<StaticSpawner>().spawnPosition = groundHitPosition;
        }

        isMoving = true;
        positionsList.Clear();
        positionsList.Add(movementSource.position);

        if (debugCubePrefab)
            Destroy(Instantiate(debugCubePrefab, movementSource.position, Quaternion.identity), 3);
    }

    void EndMovement()
    {
        isMoving = false;

        //Create gesture from position list
        Point[] pointArray = new Point[positionsList.Count];

        for (int i = 0; i < positionsList.Count; i++)
        {
            Vector2 screenPoint = Camera.main.WorldToScreenPoint(positionsList[i]);
            pointArray[i] = new Point(screenPoint.x, screenPoint.y, 0);
        }

        Gesture newGesture = new Gesture(pointArray);

        //Add new gesture to training set
        if (creationMode)
        {
            newGesture.Name = newGestureName;
            trainingSet.Add(newGesture);

            string fileName = Application.persistentDataPath + "/" + newGestureName + ".xml";
            GestureIO.WriteGesture(pointArray, newGestureName, fileName);

            CastTheSpell(newGesture);
        }
        else
        {
            CastTheSpell(newGesture);
        }
    }

    void CastTheSpell(Gesture newGesture)
    {

        Result result = PointCloudRecognizer.Classify(newGesture, trainingSet.ToArray());
        Debug.Log(result.GestureClass + result.Score);

        //Meteor = 0
        //MetorR = 1
        //MeteorL = 2
        //Fireball = 3
        //Dragon = 4
        //Earthwall = 5

        if (result.Score > recognitionThreshold)
        {
            if (result.GestureClass != gestureClass[5] && result.GestureClass != gestureClass[0])
            {
                if (result.GestureClass == gestureClass[1])
                {
                    if (whichHand == "right")
                    {
                        //Debug.Log("rightdone");
                        summonScript.rightHandDone = true;
                        if (summonScript.timer <= 0)
                        {
                            summonScript.timer = 0.5f;
                        }
                    }

                    if (summonScript.rightHandDone && summonScript.leftHandDone && summonScript.timer > 0)
                    {
                        OnRecognizedSummon.Invoke(result.GestureClass);
                        summonScript.rightHandDone = false;
                        summonScript.leftHandDone = false;
                        summonScript.timer = 0;
                    }
                }
                else if (result.GestureClass == gestureClass[2])
                {
                    if (whichHand == "left")
                    {
                        //Debug.Log("leftdone");
                        summonScript.leftHandDone = true;
                        if (summonScript.timer <= 0)
                        {
                            summonScript.timer = 0.5f;
                        }
                    }

                    if (summonScript.rightHandDone && summonScript.leftHandDone && summonScript.timer > 0)
                    {
                        //Debug.Log("aaaaa");
                        OnRecognizedSummon.Invoke(result.GestureClass);
                        summonScript.rightHandDone = false;
                        summonScript.leftHandDone = false;
                        summonScript.timer = 0;
                    }
                }
                else if (result.GestureClass == gestureClass[3])
                {
                    OnRecognizedProjectile.Invoke(result.GestureClass);
                    Debug.Log("Cast Fireball");
                }
                else if (result.GestureClass == gestureClass[4])
                {
                    if (hitGround)
                    {
                        Debug.Log("cast dragon");
                        OnRecognizedStatic.Invoke(result.GestureClass);
                        hitGround = false;
                    }
                }
            }
            else if (result.GestureClass == gestureClass[5])
            {
                if (hitGround)
                {
                    //Debug.Log("cast earth");
                    OnRecognizedStatic.Invoke(result.GestureClass);
                    hitGround = false;
                }
            }
            else if (result.GestureClass == gestureClass[4])
            {
                if (hitGround)
                {
                    Debug.Log("cast dragon");
                    OnRecognizedStatic.Invoke(result.GestureClass);
                    hitGround = false;
                }
            }

        }
    }


    void UpdateMovement()
    {
        Vector3 lastPosition = positionsList[positionsList.Count - 1];
        if (Vector3.Distance(movementSource.position, lastPosition) > newPosThresholdDistance)
        {
            positionsList.Add(movementSource.position);
            if (debugCubePrefab)
                Destroy(Instantiate(debugCubePrefab, movementSource.position, Quaternion.identity), 3);
        }
    }
}
