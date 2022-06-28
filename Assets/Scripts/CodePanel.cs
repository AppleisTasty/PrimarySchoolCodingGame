using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

public class CodePanel : MonoBehaviour, IDropHandler
{
    private PlayerController playerController;
    public GameObject draggableItem;
    public Sprite MoveRight;
    public Sprite MoveLeft;
    public Sprite JumpRight; 
    public Sprite JumpLeft;
    public Sprite HeightReduce;
    public Sprite MoveRightExecution;
    public Sprite MoveLeftExecution;
    public Sprite JumpRightExecution;
    public Sprite JumpLeftExecution; 
    public Sprite HeightReduceExecution;
    public Sprite JumpIncrease;
    public Sprite JumpIncreaseExecution;
    public Sprite RepeatOneTime;
    public Sprite EndClosure;
    private float executionDelaySeconds = 1.5f; //s
    public Button GoButton;
    public TextMeshProUGUI GoButtonText;
    public RectTransform CodePanelRectTransform;
    public RectTransform DraggableTemplate;
    public ScrollRect ScrollBar;
    public TextMeshProUGUI LevelLabel;
    public AudioSource DraggableDropAudio;
    private int DraggableTemplateNumber = 2;


    //record player's initial place
    private Transform playerInitialPosition;
    //public Transform transformParent;

    //temporary variables
    Stack<int> repeatStartClosure = new Stack<int>();
    Stack<int> endClosure = new Stack<int>();

    void Awake()
    {
        //find the playerController script
        playerController = GameObject.FindObjectOfType<PlayerController>();
        //transform.GetChild(0).gameObject.active = false; //inactive 0 object in vertical layout
        playerInitialPosition = playerController.gameObject.transform; //reset player position
        LevelLabel.text = SceneManager.GetActiveScene().name;//initiate level label

    }

    public void OnDrop(PointerEventData eventData)
    {
        DraggableDropAudio.Play();// play the sound
        Debug.Log("OnDrop");
        //eventData.pointerDrag is the aim object that is being currently dragges

        if( eventData.pointerDrag != null && eventData.pointerDrag.gameObject.tag == "Undropped" && GoButtonText.text == "Go!")
        {
            //make the item snap into the position as we dropped
            //eventData.pointerDrag.GetComponent<RectTransform>().anchoredPosition = GetComponent<RectTransform>().anchoredPosition;

            // replicate from existing item (if it is not replicating there will be problem for sizes);
            
            

            if(eventData.pointerDrag.gameObject.name == "MoveRight")
            {
                InitializeNewDraggableItemfromTemplate(0, eventData).GetComponent<Image>().sprite = MoveRight;
            }
            else if (eventData.pointerDrag.gameObject.name == "MoveLeft")
            {
                InitializeNewDraggableItemfromTemplate(0, eventData).GetComponent<Image>().sprite = MoveLeft;
            }
            else if (eventData.pointerDrag.gameObject.name == "JumpRight")
            {
                InitializeNewDraggableItemfromTemplate(0, eventData).GetComponent<Image>().sprite = JumpRight;
            }
            else if (eventData.pointerDrag.gameObject.name == "JumpLeft")
            {
                InitializeNewDraggableItemfromTemplate(0, eventData).GetComponent<Image>().sprite = JumpLeft;
            }
            else if (eventData.pointerDrag.gameObject.name == "HeightReduce")
            {
                InitializeNewDraggableItemfromTemplate(1, eventData).GetComponent<Image>().sprite = HeightReduce;
                Destroy(eventData.pointerDrag.gameObject);
            }
            else if (eventData.pointerDrag.gameObject.name == "JumpIncrease")
            {
                InitializeNewDraggableItemfromTemplate(1, eventData).GetComponent<Image>().sprite = JumpIncrease;
                Destroy(eventData.pointerDrag.gameObject);
            }
            else if (eventData.pointerDrag.gameObject.name == "RepeatOneTime")
            {
                InitializeNewDraggableItemfromTemplate(1, eventData).GetComponent<Image>().sprite = RepeatOneTime;
            }
            else if (eventData.pointerDrag.gameObject.name == "EndClosure")
            {
                InitializeNewDraggableItemfromTemplate(0, eventData).GetComponent<Image>().sprite = EndClosure;
            }
            // add length to the height of the panel to fulfill scroll, set anchor as top-center
            if (transform.childCount > 9)
            {
                CodePanelRectTransform.sizeDelta = new Vector2(CodePanelRectTransform.sizeDelta.x, CodePanelRectTransform.sizeDelta.y + DraggableTemplate.sizeDelta.y + 1.2f); //2 is the spacing offset
                ScrollBar.normalizedPosition = new Vector2(0, 0); //0,0 for bottom, 0,1 for top

            }



            //access pointerdragged object's name
            //if(eventData.pointerDrag.gameObject.name == "DraggableItemMoveRight")
            //{

            //}
            //player move right
        }
    }

    private GameObject InitializeNewDraggableItemfromTemplate(int itemNumber, PointerEventData eventData)
    {
        GameObject g1 = Instantiate(transform.GetChild(itemNumber).gameObject, transform);
        g1.name = eventData.pointerDrag.gameObject.name;
        g1.SetActive(true);
        return g1;
    }

    public void ExecuteCodeGroup() 
    {
        GoButton.interactable = false;
        GoButtonText.text = "Running";
        StartCoroutine(ExecutionInDelay());
    }

    private IEnumerator ExecutionInDelay()
    {
        for (int i = 1; i < transform.childCount; i++)
        {
            if (transform.GetChild(i).name == "EndClosure")
            {
                if(endClosure.Count <= 0 && repeatStartClosure.Count <= 0)
                {
                    continue;
                }
                else if (endClosure.Count <= 0)
                {
                    endClosure.Push(i);
                    i = repeatStartClosure.Pop() - 1;
                }
                else
                {
                    if (endClosure.Peek() != i)
                    {
                        //fist time push and loop back
                        if (repeatStartClosure.Count > 0)
                        {
                            //if repeatStart is not empty, start from the index
                            endClosure.Push(i);
                            i = repeatStartClosure.Pop() - 1;
                        }
                        else
                        {
                            continue;
                        }
                    }
                    else
                    {
                        //second time pop
                        endClosure.Pop();
                    }
                }
                
                
                //to tell if it needs to loop, if not then continue;
            }
            else if (transform.GetChild(i).name == "RepeatOneTime")
            {
                //set the sign for loop, and jump into next line
                repeatStartClosure.Push(i + 1); // add into the stack
                continue;
            }
            else if (transform.GetChild(i).name == "MoveRight")
            {
                transform.GetChild(i).GetComponent<Image>().sprite = MoveRightExecution;
                playerController.move = PlayerController.Move.moveRight;
                yield return new WaitForSeconds(executionDelaySeconds);
                transform.GetChild(i).GetComponent<Image>().sprite = MoveRight;
            }
            else if (transform.GetChild(i).name == "MoveLeft")
            {
                transform.GetChild(i).GetComponent<Image>().sprite = MoveLeftExecution;
                playerController.move = PlayerController.Move.moveLeft;
                yield return new WaitForSeconds(executionDelaySeconds);
                transform.GetChild(i).GetComponent<Image>().sprite = MoveLeft;
            }
            else if (transform.GetChild(i).name == "JumpRight")
            {
                transform.GetChild(i).GetComponent<Image>().sprite = JumpRightExecution;
                playerController.move = PlayerController.Move.jumpRight;
                yield return new WaitForSeconds(executionDelaySeconds);
                transform.GetChild(i).GetComponent<Image>().sprite = JumpRight;
            }
            else if (transform.GetChild(i).name == "JumpLeft")
            {
                transform.GetChild(i).GetComponent<Image>().sprite = JumpLeftExecution;
                playerController.move = PlayerController.Move.jumpLeft;
                yield return new WaitForSeconds(executionDelaySeconds);
                transform.GetChild(i).GetComponent<Image>().sprite = JumpLeft;
            }
            else if (transform.GetChild(i).name == "HeightReduce")
            {
                transform.GetChild(i).GetComponent<Image>().sprite = HeightReduceExecution;
                playerController.move = PlayerController.Move.heightReduce;
                yield return new WaitForSeconds(executionDelaySeconds);
                transform.GetChild(i).GetComponent<Image>().sprite = HeightReduce;
            }
            else if (transform.GetChild(i).name == "JumpIncrease")
            {
                transform.GetChild(i).GetComponent<Image>().sprite = JumpIncreaseExecution;
                playerController.move = PlayerController.Move.jumpIncrease;
                yield return new WaitForSeconds(executionDelaySeconds);
                transform.GetChild(i).GetComponent<Image>().sprite = JumpIncrease;
            }
            

        }
        yield return new WaitForSeconds(executionDelaySeconds);
        ClearCodeGroup();

    }

    public void ClearCodeGroup()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        //for (int i = transform.childCount-1; i > 0; i--)
        //{
        //    transform.GetChild(i).parent = null;
        //}
        //PlayerInitialize();
    }

    private void PlayerInitialize()
    {
        playerController.gameObject.transform.position = new Vector2(-6.65f, 2.72f);
        playerController.gameObject.transform.localScale = new Vector2(1, 1);
    }
}
