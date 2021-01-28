using System.Collections;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(SnakeAnimController))]
public class Snake : MonoBehaviour
{
    [Range(0f, 1f)]
    [SerializeField] private float speedChangeFactor = 0.9f;
    [SerializeField] private float beanDelay = 0.2f;
    [SerializeField] private ScoreModel scoreModel = null;
    [SerializeField] private GameObject snakeBodyPrefab = null;
    [SerializeField] private GameObject particleEffect = null;
    [SerializeField] private LifetimeScoreUpdater scoreUpdater = null;
    [SerializeField] private float updateTick = 0.15f;
    [SerializeField] private List<GameObject> snakeBodyList = new List<GameObject>();
    private enum Direction {None, Up, Down, Left, Right};
    private Direction currentDirection = Direction.None;
    private bool isAlive = false;
    private Dictionary<Direction, Vector3> directionMap;
    private Rigidbody rigidBody;
    private float elapsedTime = 0f;
    private bool growOnNextMove = false;

    private void Awake()
    {
        isAlive = true;
        Debug.Assert(scoreModel != null, "Snake needs score model.");
        rigidBody = GetComponent<Rigidbody>();
        directionMap = new Dictionary<Direction, Vector3>()
        {
            {Direction.None, Vector3.zero },
            {Direction.Left, new Vector3(-1, 0, 0) },
            {Direction.Right, new Vector3(1, 0, 0) },
            {Direction.Up, new Vector3(0, 0, 1) },
            {Direction.Down, new Vector3(0, 0, -1) }
        };
        
    }
    private void Update()
    {
        if (isAlive)
        {
            HandleDirectionInput();
        }
        else
        {
            Reset();
        }
    }

    private void FixedUpdate()
    {
        if (isAlive) 
        {
            UpdatePosition();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("SnakeDeath"))
        {
            var beanPositions = snakeBodyList.Select(bean => bean.transform.position).ToList();  
            beanPositions.Insert(0, transform.position);
            StartCoroutine(ExplodeSnake(beanPositions));

            foreach (GameObject bean in snakeBodyList)
            {
                Destroy(bean);
            }
            GetComponent<Renderer>().enabled = false;
            currentDirection = Direction.None;
            scoreUpdater.enabled = false;
            isAlive = false;
        }

        var food = other.GetComponent<Food>();
        if (food != null)
        {
            food.Respawn();
            updateTick *= speedChangeFactor;
            scoreModel.Score += scoreModel.FoodTickAmount;
            StartCoroutine(SwallowFood());
            growOnNextMove = true;
            
            return;
        }
    }

    private void Reset()
    {
        if (Input.GetKeyDown(KeyCode.Space) && !isAlive)
        {
            isAlive = true;
            transform.position = Vector3.zero;
            GetComponent<Renderer>().enabled = true;
            snakeBodyList.Clear();
            scoreModel.Score = 0;
            scoreUpdater.enabled = true;
        }
    }
    private void HandleDirectionInput()
    {
        if (!isAlive)
        {
            currentDirection = Direction.None;
        }
        else
        {
            if (Input.GetKeyDown(KeyCode.W) && currentDirection != Direction.Down)
            {
                currentDirection = Direction.Up;
            }
            if (Input.GetKeyDown(KeyCode.S) && currentDirection != Direction.Up)
            {
                currentDirection = Direction.Down;
            }
            if (Input.GetKeyDown(KeyCode.A) && currentDirection != Direction.Right)
            {
                currentDirection = Direction.Left;
            }
            if (Input.GetKeyDown(KeyCode.D) && currentDirection != Direction.Left)
            {
                currentDirection = Direction.Right;
            }
        }
    }
    private void UpdatePosition()
    {
        elapsedTime += Time.fixedDeltaTime;
        if (elapsedTime > updateTick)
        {
            elapsedTime = 0f;
            if (currentDirection != Direction.None)
            {
                Vector3 headPosition = transform.position;
                transform.position += directionMap[currentDirection];
                AttemptGrowth();
                UpdateBodyPositions(headPosition);
            }
        }
    }
    private void AttemptGrowth()
    {
        if (!growOnNextMove)
        {
            return;
        }

        Vector3 bodyPosition = snakeBodyList.Count == 0 ? transform.position : snakeBodyList[snakeBodyList.Count - 1].transform.position;
        var body = Instantiate(
            original: snakeBodyPrefab,
            position: bodyPosition,
            rotation: Quaternion.identity
            );
        snakeBodyList.Add(body);
        growOnNextMove = false; 
    }

    private void UpdateBodyPositions(Vector3 headPosition)
    {
        if (snakeBodyList.Count == 0)
        {
            return;
        }

        for (int i = snakeBodyList.Count - 1; i > 0; i--)
        {
            snakeBodyList[i].transform.position = snakeBodyList[i - 1].transform.position;
        }
        snakeBodyList[0].transform.position = headPosition;
    }


    #region Coroutines
    private IEnumerator ExplodeSnake(List<Vector3> beanPositions)
    {
        foreach (var position in beanPositions)
        {
            var beanEffect = Instantiate(
                original: particleEffect,
                position: position,
                rotation: Quaternion.identity
                );
            yield return new WaitForSeconds(beanDelay);
        }
        yield return null;
    }

    private IEnumerator SwallowFood()
    {
        float animationDuration = 0.075f;
        GetComponent<SnakeAnimController>().PlaySwallowAnimation();

        yield return new WaitForSeconds(animationDuration);
        int i = 0;
        while (i < snakeBodyList.Count)
        {
            snakeBodyList[i].GetComponent<SnakeAnimController>().PlaySwallowAnimation();
            i += 1;
            yield return new WaitForSeconds(animationDuration);
        }
    }
    #endregion


}
