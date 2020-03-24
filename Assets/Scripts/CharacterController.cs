using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterController : MonoBehaviour
{
    [SerializeField] MovementType movementType;
    // [SerializeField]
    // [Range(0, 1f)] float movementDelay;


    [SerializeField]                            
    [Range(0.001f, 2f)] float timeToCompleteMove;

   // [SerializeField]
   // [Range(0.001f, 2f)] 
    float movementSpeed; // tiles per second
    

    [SerializeField]
    [Range(0, 10)] int maxNumberOfQueuedMoves;
    Queue<Vector2> moveQueue = new Queue<Vector2>();

    [SerializeField]
    [Range(0,0.51f)]private float snapToGridTolerance;



    KeyCode upKey = KeyCode.W;
    KeyCode leftKey = KeyCode.A;
    KeyCode downKey = KeyCode.S;
    KeyCode rightKey = KeyCode.D;



    Vector2? direction;
    Vector3? target;



    // Start is called before the first frame update
    void Start()
    {
        direction = (Vector2?)null;
        target = (Vector3?)null;
    }

    // Update is called once per frame
    void Update()
    {
        GetMovementInput();
        DoMovement();


        //float xPos = Mathf.Floor(transform.position.x) +0.5f;
        //float zPos = Mathf.Floor(transform.position.z) +0.5f;
        //transform.position = new Vector3(xPos, 0, zPos);
    }

    private void DoMovement()
    {
        if(!direction.HasValue && !target.HasValue)
        {
            return;
        }

        if(!target.HasValue)
        {
            Vector3Int targetDirection = new Vector3Int((int)direction?.x, (int)direction?.y, 0);
            target = transform.position + targetDirection;
            
            if(targetDirection.magnitude == 1 && GameManager.Instance.TilemapManager.GetTileInfo((Vector3Int.FloorToInt((Vector3)target))).IsSolid)  // solid orthographic
            {
                target = null;
                return;
            }
            else // moving diagonally
            {
                Vector3Int upDown = new Vector3Int((int)transform.position.x, (int)(float)target?.y, 0);
                Vector3Int leftRight = new Vector3Int( (int)(float)target?.x,(int)transform.position.y, 0);
                if(GameManager.Instance.TilemapManager.GetTileInfo(upDown).IsSolid || GameManager.Instance.TilemapManager.GetTileInfo(leftRight).IsSolid)
                { // one of orthographic is solid
                    target = null;
                    return;
                }

            }
            
            direction = null;
        }

        Vector3 vecToTarget = (Vector3)target - transform.position;

        if (Mathf.Abs(vecToTarget.magnitude) < snapToGridTolerance)
        {
            transform.position = new Vector3(Mathf.Round(transform.position.x),Mathf.Round(transform.position.y),0);
            target = null;
        }
        else
        {
            movementSpeed = 1f / timeToCompleteMove;
            transform.position = Vector3.Lerp(transform.position, (Vector3)target, movementSpeed * Time.deltaTime);
        }

    }

    private void GetMovementInput()
    {
        switch (movementType)
        {
            case MovementType.Queued:
                GetDirectionQueued();
                break;
            case MovementType.Single:
                break;
            case MovementType.Interrupt:
                throw new NotImplementedException();
                break;
            default:
                break;
        }

        




    }

    private void GetDirectionQueued()
    {
       
        Vector2? newDirection = new Vector2();
        if (moveQueue.Count == 0 && !target.HasValue)
        {
            newDirection = GetHeldDirectionFromInput();
            if (newDirection != null)
            { moveQueue.Enqueue((Vector2)newDirection); }
        }
        else if (/*moveQueue.Count > 0 && */ moveQueue.Count < maxNumberOfQueuedMoves)
        {
            newDirection = (Vector2?)GetNewDirectionFromInput();
            if (newDirection.HasValue)
            { moveQueue.Enqueue((Vector2)newDirection); }
        }
      
        

        if (!target.HasValue) { // stopped moving
             direction = (moveQueue.Count > 0) ? (Vector2?)moveQueue.Dequeue() : (Vector2?)null;
        }


        
      
       
        
    }

    private Vector2? GetHeldDirectionFromInput()
    {
        Vector2 direction = new Vector2(Input.GetKey(rightKey) ? 1f : Input.GetKey(leftKey) ? -1f : 0f, Input.GetKey(upKey) ? 1f : Input.GetKey(downKey) ? -1f : 0f);
        //direction.Normalize();
        return direction.magnitude > 0.05f ? direction : (Vector2?)null;
    }

    Vector2? GetNewDirectionFromInput()
    {
        Vector2 direction = new Vector2(Input.GetKeyDown(rightKey) ? 1f: Input.GetKeyDown(leftKey) ? -1f: 0f, Input.GetKeyDown(upKey) ? 1f : Input.GetKeyDown(downKey) ? -1f : 0f);  
        //direction.Normalize();
        return direction.magnitude > 0.05f ? direction : (Vector2?)null;
    }

    enum MovementType
    {
        Queued,
        Single,
        Interrupt,
    }

}
