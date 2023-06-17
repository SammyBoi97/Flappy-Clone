using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnvironmentController : MonoBehaviour
{
    private static EnvironmentController _instance;
    public static EnvironmentController Instance { get { return _instance; } }

    public GameObject[] pipeGroup = new GameObject[5];
    public GameObject[] groundGroup = new GameObject[5];

    public Vector2 pipeOffset;
    public float pipeSpacing;
    public float pipeVertRange;

    [HideInInspector]
    public float groundSpacing;

    public float environmentSpeed;
    public float movementMultiplier = 1f;

    public GameObject destroyPoint;

    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            _instance = this;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        groundSpacing = groundGroup[0].GetComponent<SpriteRenderer>().bounds.size.x;
        
        for (int i = 0; i < pipeGroup.Length; i++)
        {
            pipeGroup[i].GetComponent<EnvironmentMove>().myTail = pipeGroup[(pipeGroup.Length - 1 + i) % pipeGroup.Length];
            groundGroup[i].GetComponent<EnvironmentMove>().myTail = groundGroup[(groundGroup.Length - 1 + i) % groundGroup.Length];
        }

        ResetEnvironment();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void ResetEnvironment()
    {
        for (int i = 0; i < pipeGroup.Length; i++)
        {
            pipeGroup[i].transform.position = new Vector2(pipeOffset.x + i * pipeSpacing,
                                                            pipeOffset.y + Random.Range(-pipeVertRange, pipeVertRange));
            groundGroup[i].transform.position = new Vector2(i * groundSpacing, groundGroup[i].transform.position.y);
        }
    }
}
