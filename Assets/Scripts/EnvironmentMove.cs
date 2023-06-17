using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnvironmentMove : MonoBehaviour
{
    private float xPos, yPos;
    public GameObject myTail;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(-EnvironmentController.Instance.environmentSpeed * EnvironmentController.Instance.movementMultiplier * Time.deltaTime, 0f, 0f);
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if(col.gameObject == EnvironmentController.Instance.destroyPoint)
        {
            if (gameObject.tag == "Pipe")
            {
                xPos = myTail.transform.position.x + EnvironmentController.Instance.pipeSpacing;
                yPos = Random.Range(-EnvironmentController.Instance.pipeVertRange, EnvironmentController.Instance.pipeVertRange);
            }
            else
            {
                xPos = myTail.transform.position.x + EnvironmentController.Instance.groundSpacing;
                yPos = transform.position.y;
            }

            transform.position = new Vector2(xPos, yPos);
        }
    }
}
