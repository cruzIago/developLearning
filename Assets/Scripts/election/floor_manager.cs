using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class floor_manager : MonoBehaviour
{
    public int MAX_FLOORS = 1;
    private int MAX_CHECKERS = 3;

    //List of all objects needed to manage
    public player_controller player;
    public List<GameObject> respawnPositions;
    public List<Text> correctAnswers;
    public List<GameObject> floors;
    public List<Text> questions;
    public List<answer_checker> checkers;
    public List<GameObject> pivots;

    //Current objects: The ones that are active in the floor
    private Vector3 currentRespawnPos;
    public int currentFloor;
    private Text currentCorrectAnswer;
    public Text chosenAnswer;
    private Text currentQuestion;
    private List<answer_checker> currentCheckers;
    private List<GameObject> currentPivots;

    private int mistakes;
    private float elapsed_time;
    public bool is_review_stage;
    public review_manager reviewer;

    // Start is called before the first frame update
    void Start()
    {
        elapsed_time = Time.time;
        currentFloor = 0;
        currentCheckers = new List<answer_checker>();
        currentPivots = new List<GameObject>();
        changeFloor();

    }

    // Update is called once per frame
    void Update()
    {

    }

    void changeFloor()
    {
        print("Current floor: " + currentFloor);
        //Set current Floor to active and hide the rest
        for (int i = 0; i < floors.Count; i++)
        {
            if (i != currentFloor)
            {
                floors[i].SetActive(false);
                questions[i].gameObject.SetActive(false);

            }
            else
            {
                floors[i].SetActive(true);
                currentQuestion = questions[i];
                currentQuestion.gameObject.SetActive(true);
            }
        }

        //Set current possible answers with their checkers and posible positions, and shuffle them
        clearAnswers();
        for (int i = 0; i < MAX_CHECKERS; i++)
        {
            currentCheckers.Add(checkers[currentFloor * MAX_CHECKERS + i]);
            currentPivots.Add(pivots[currentFloor * MAX_CHECKERS + i]);
            currentCheckers[i].resetCollision();
            print("Iteracion " + i);
            print("Checker: " + currentCheckers[i]);
            print("Pivot: " + currentPivots[i]);
        }
        shuffleAnswers();

        //Set current correct answer and current respawn position of the player
        currentCorrectAnswer = correctAnswers[currentFloor];
        currentRespawnPos = respawnPositions[currentFloor].transform.position;

        //Moves player to respawn position
        player.transform.position = currentRespawnPos;
    }

    //Checks if the answer given by the user is correct
    public void checkAnswer()
    {
        if (chosenAnswer == currentCorrectAnswer)
        {
            print("Acertaste");
            correctAnswer();
        }
        else
        {
            print("La has liao macho");
            wrongAnswer();
        }
    }

    private void correctAnswer()
    {
        currentFloor++;
        if (currentFloor <= MAX_FLOORS)
        {
            changeFloor();
        }
        else
        {
            gameOver();
        }

        if (currentFloor > MAX_FLOORS)
        {
            if (!is_review_stage)
            {
                int stars = 0;
                elapsed_time = Time.time - elapsed_time;
                if (mistakes <= 1)
                {
                    stars = 3;
                }
                else if (mistakes <= 4)
                {
                    stars = 2;
                }
                else
                {
                    stars = 1;
                }
                scene_manager.checkEndScreen(stars, elapsed_time, mistakes);
            }
            else {
                reviewer.nextReview(mistakes);
            }
        }
    }

    private void wrongAnswer()
    {
        currentFloor = 0;
        changeFloor();
    }

    private void gameOver()
    {
        mistakes += 1;
        clearAnswers();
        floors[currentFloor].SetActive(true);
        currentQuestion = questions[currentFloor];
        currentQuestion.gameObject.SetActive(true);
        questions[currentFloor - 1].gameObject.SetActive(false);
        currentRespawnPos = respawnPositions[currentFloor].transform.position;
        player.transform.position = currentRespawnPos;
    }

    private void shuffleAnswers()
    {
        int count = currentCheckers.Count;
        int last = count - 1;
        for (int i = 0; i < last; ++i)
        {
            var r = UnityEngine.Random.Range(i, count);
            var tempChecker = currentCheckers[i];
            currentCheckers[i] = currentCheckers[r];
            currentCheckers[r] = tempChecker;
        }

        for (int i = 0; i < currentCheckers.Count; i++)
        {
            currentCheckers[i].move(currentPivots[i].transform.position);
        }
    }

    private void clearAnswers()
    {
        currentCheckers.Clear();
        currentPivots.Clear();
    }


}
