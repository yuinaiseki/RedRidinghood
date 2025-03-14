using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Dialogue : MonoBehaviour
{
    public Animator animator;
    private static TextMeshProUGUI textComponent;
    private TextMeshProUGUI hitEnter;
    public float textSpeed;
    public List<DialogueObject> dialogueScript = new List<DialogueObject>();
    //private static List<bool>beenTriggered = new List<bool>();
    private int index;
    private int currentScript;
    private bool isToggled;
    public string currentChapter;

    private int corruptionLevel;

    // inventory for flower count
    public PlayerInventory inv;


    //for the choices mechanic
    private DisplayChoices choiceFunction;
    // Start is called before the first frame update

    public GameObject choicesPanel;
    private bool choicesToggled;
    void Start()
    {
        corruptionLevel = 0;

        //get the choices box
        Transform parentTransform = this.transform;
        choicesPanel = parentTransform.GetChild(3).gameObject;
        choiceFunction = choicesPanel.GetComponent<DisplayChoices>();


        isToggled = true;
        currentScript = 0;

        for (int i = 0; i < dialogueScript.Count; i++) {
            //beenTriggered.Add(false);
        }

        //beenTriggered[0] = true;

        textComponent = GetComponentInChildren<TextMeshProUGUI>();

        textComponent.text = string.Empty;

        choicesToggled = false;

        if (dialogueScript.Count > 0) {
            GameManager.Instance.SetCutsceneTrigger(true);
            StartDialogue();
        } else {
            isToggled = false;
            ToggleChildren(false);
            GameManager.Instance.SetCutsceneTrigger(false);
        }
        GameManager.Instance.SetCutsceneTrigger(false);
        //Debug.Log(GameManager.Instance.GetCutsceneTrigger());

    }

    // Update is called once per frame
    void Update()
    {
        if (dialogueScript.Count > 0) {


            //display choices if dialogue reaches the last box

            if (textComponent.text == dialogueScript[currentScript].Lines[0] && (dialogueScript[currentScript].myChoices != null) && choicesToggled == false) {

                choiceFunction.leftText = dialogueScript[currentScript].myChoices.leftChoice;
                choiceFunction.rightText = dialogueScript[currentScript].myChoices.rightChoice;

                choiceFunction.currentLeftID = dialogueScript[currentScript].myChoices.leftID;
                choiceFunction.currentRightID = dialogueScript[currentScript].myChoices.rightID;
                choiceFunction.ActivateChoice(true);
                choicesToggled = true;

            }


            // if(choicesToggled == true && choiceFunction.choicePressed == false){
            //     //display the pressed choices dialogue lines 
            // }

            // when choice button is pressed

            // Debug.Log(choicesToggled);
            if (Input.GetKeyDown(KeyCode.Return) && isToggled == true && choicesToggled == false)
            {
                if (textComponent.text == dialogueScript[currentScript].Lines[index]) {
                    NextLine();
                } else {
                    StopAllCoroutines();
                    textComponent.text = dialogueScript[currentScript].Lines[index];
                }


            }

            if (currentChapter == "1") {
                Chapter1Triggers();
            } else if (currentChapter == "2") {
                chapter2Triggers();
            } else if (currentChapter == "3") {
                chapter3Triggers();
            } else if (currentChapter == "4") {
                chapter4Triggers();
            } else if (currentChapter == "5.8") {
                chapter5Triggers();
            } else if (currentChapter == "GE") {
                chapterENDTriggers();
            }
            else if (currentChapter == "BE") {
                if (animator != null && (isToggled == false)) {
                    Debug.Log("Huh");
                    // Trigger animation using the provided trigger name
                    animator.SetTrigger("BadEnding");
                }
            }
        }


        // //this is for testing purposes
        // if (Input.GetKeyDown(KeyCode.W)){
        //     isToggled = true;
        //     ToggleChildren(true);
        //     textComponent.text = string.Empty;
        //     StartDialogue();

        // }

    }

    void StartDialogue() {
        index = 0;
        GameManager.Instance.SetCutsceneTrigger(true);
        StartCoroutine(TypeLine());
    }

    void NextLine() {
        Debug.Log("display next line: ");
        Debug.Log("index: " + index + "< dialogueScript[currentScript].Lines.Count: " + dialogueScript[currentScript].Lines.Count);
        Debug.Log("currentScript is: " + currentScript);
        Debug.Log(dialogueScript[currentScript]);

        if (index < dialogueScript[currentScript].Lines.Count - 1) {
            Debug.Log("Onto next dialogue");
            index++;
            textComponent.text = string.Empty;
            StartCoroutine(TypeLine());
        } else {
            Debug.Log("Dialogue finished");
            //all dialogues are finished
            isToggled = false;
            ToggleChildren(false);
            GameManager.Instance.SetCutsceneTrigger(false);

            // next script in dialogue.Script
            if (currentScript < dialogueScript.Count - 1) {
                currentScript++;
                Debug.Log("currentScript is now: " + currentScript);
            }

            if (currentScript == dialogueScript.Count - 1) {
                isToggled = false;
                ToggleChildren(false);
                GameManager.Instance.SetCutsceneTrigger(false);
            }
        }
    }

    IEnumerator TypeLine() {

        foreach (char c in dialogueScript[currentScript].Lines[index].ToCharArray()) {
            textComponent.text += c;

            if ((c == ',') || (c == '?')) {
                yield return new WaitForSeconds(textSpeed + 0.5f);

            } else if ((c == '.')) {
                yield return new WaitForSeconds(textSpeed + 0.5f);

            }
            else {
                yield return new WaitForSeconds(textSpeed);
            }
        }
    }


    void ToggleChildren(bool activeState)
    {
        foreach (Transform child in transform)
        {
            child.gameObject.SetActive(activeState);
        }
    }

    void Chapter1Triggers() {
        for (int i = 1; i <= 4; i++) {
            TriggerScriptLine(1, i);
        }
    }

    void chapter2Triggers() {

        TriggerScriptLine(2, 1);
        TriggerScriptLine(2, 2);
        TriggerScriptLine(2, 3);
        //TriggerScriptLine(2,4);
        

        if (choicesToggled == true && choiceFunction.choicePressed == false) {
            //display the pressed choices dialogue lines
            if (textComponent.text == dialogueScript[1].Lines[index]) { //maybe this will work?
                if (GameManager.Instance.GetChoiceValue(200)) {
                    dialogueScript[1].Lines[1] = dialogueScript[1].myChoices.GetLines(1, 0);

                } else if (GameManager.Instance.GetChoiceValue(201)) {
                    dialogueScript[1].Lines[1] = dialogueScript[1].myChoices.GetLines(2, 0);
                }
                choicesToggled = false;
                NextLine();
                GameManager.Instance.SetTrigger(2, 2, true);

            } else if (textComponent.text == dialogueScript[2].Lines[index]) {
                if (GameManager.Instance.GetChoiceValue(202)) {
                    dialogueScript[2].Lines[1] = dialogueScript[2].myChoices.GetLines(1, 0);

                } else if (GameManager.Instance.GetChoiceValue(203)) {
                    dialogueScript[2].Lines[1] = dialogueScript[2].myChoices.GetLines(2, 0);
                }
                choicesToggled = false;
                NextLine();
                GameManager.Instance.SetTrigger(2, 3, true);

            } else if (textComponent.text == dialogueScript[3].Lines[index]) {
                if (GameManager.Instance.GetChoiceValue(204)) {
                    dialogueScript[3].Lines[1] = dialogueScript[3].myChoices.GetLines(1, 0);

                } else if (GameManager.Instance.GetChoiceValue(205)) {
                    dialogueScript[3].Lines[1] = dialogueScript[3].myChoices.GetLines(2, 0);
                }
                choicesToggled = false;
                NextLine();
                //GameManager.Instance.SetTrigger(2,4,true);
            }

        }

    }

    void chapter4Triggers(){
        TriggerScriptLine(4, 1);

        if (choicesToggled == true && choiceFunction.choicePressed == false) {
            //display the pressed choices dialogue lines
            if (textComponent.text == dialogueScript[1].Lines[index]) { //maybe this will work?
                if (GameManager.Instance.GetChoiceValue(400)) {
                    dialogueScript[1].Lines[1] = dialogueScript[1].myChoices.GetLines(1, 0);

                } else if (GameManager.Instance.GetChoiceValue(401)) {
                    dialogueScript[1].Lines[1] = dialogueScript[1].myChoices.GetLines(2, 0);
                }
                choicesToggled = false;
                NextLine();
                GameManager.Instance.SetTrigger(4, 2, true);
            }
        }
    }

    void chapter3Triggers()
    {

        TriggerScriptLine(3, 1);
        TriggerScriptLine(3, 2);
        TriggerScriptLine(3, 3);
        TriggerScriptLine(3, 4);
        TriggerScriptLine(3, 5);
        TriggerScriptLine(3, 6);
        TriggerScriptLine(3, 7);
        TriggerScriptLine(3, 8);
        TriggerScriptLine(3, 9);

        if (choicesToggled == true && choiceFunction.choicePressed == false)
        {
            //display the pressed choices dialogue lines

            if (textComponent.text == dialogueScript[1].Lines[index])
            {
                if (GameManager.Instance.GetChoiceValue(300))
                {
                    dialogueScript[1].Lines[1] = dialogueScript[1].myChoices.GetLines(1, 0);
                    corruptionLevel -= 1;
                }
                else if (GameManager.Instance.GetChoiceValue(301))
                {
                    dialogueScript[1].Lines[1] = dialogueScript[1].myChoices.GetLines(2, 0);
                    corruptionLevel += 1;
                }
                Debug.Log("corruptionLevel: " + corruptionLevel);
                choicesToggled = false;
                NextLine();
                GameManager.Instance.SetTrigger(3, 2, true);

            }
            else if (textComponent.text == dialogueScript[2].Lines[index])
            {
                if (GameManager.Instance.GetChoiceValue(302))
                {
                    dialogueScript[2].Lines[1] = dialogueScript[2].myChoices.GetLines(1, 0);
                    corruptionLevel += 1;
                }
                else if (GameManager.Instance.GetChoiceValue(303))
                {
                    dialogueScript[2].Lines[1] = dialogueScript[2].myChoices.GetLines(2, 0);
                    corruptionLevel -= 1;
                }
                Debug.Log("corruptionLevel: " + corruptionLevel);
                choicesToggled = false;
                NextLine();
                GameManager.Instance.SetTrigger(3, 3, true);
            }

            else if (textComponent.text == dialogueScript[3].Lines[index])
            {
                // Debug.Log("textComponent.text == dialogueScript[3].Lines[index]");
                if (GameManager.Instance.GetChoiceValue(304))
                {
                    dialogueScript[3].Lines[1] = dialogueScript[4].myChoices.GetLines(1, 0);

                }
                else if (GameManager.Instance.GetChoiceValue(305))
                {
                    dialogueScript[3].Lines[1] = dialogueScript[4].myChoices.GetLines(2, 0);
                }
                choicesToggled = false;
                NextLine();
            }

            else if (textComponent.text == dialogueScript[4].Lines[index])
            {
                choicesToggled = false;
                NextLine();
                if (choiceFunction.leftBP)
                {
                    GameManager.Instance.SetTrigger(3, 5, true);
                    Debug.Log("Ready");
                    GameManager.Instance.SetTrigger(3, 6, true);
                } else if (choiceFunction.rightBP)
                {
                    Debug.Log("Not ready");
                    GameManager.Instance.SetTrigger(3, 4, false);
                    currentScript = 4;
                }
            }

            // Tulips or black roses?
            else if (textComponent.text == dialogueScript[6].Lines[index])
            {
                if (GameManager.Instance.GetChoiceValue(306)) {
                    dialogueScript[6].Lines[1] = dialogueScript[6].myChoices.GetLines(1, 0);
                    if (inv.tulipAmount < 3)
                    {
                        dialogueScript[6].Lines[1] = "But you didn't pick enough...";
                        corruptionLevel -= 3;
                    }
                    corruptionLevel += 3;
                }
                else if (GameManager.Instance.GetChoiceValue(307)) {
                    dialogueScript[6].Lines[1] = dialogueScript[6].myChoices.GetLines(2, 0);
                    corruptionLevel -= 3;
                }
                Debug.Log("corruptionLevel: " + corruptionLevel);
                inv.ResetTulipBlackrose();
                choicesToggled = false;
                NextLine();
                GameManager.Instance.SetTrigger(3, 7, true);
            }

            // Lavenders or Sunflowers?
            else if (textComponent.text == dialogueScript[7].Lines[index])
            {
                if (GameManager.Instance.GetChoiceValue(308))
                {
                    dialogueScript[7].Lines[1] = dialogueScript[7].myChoices.GetLines(1, 0);
                    if (inv.lavenderAmount < 3 || inv.sunflowerAmount <3)
                    {
                        dialogueScript[6].Lines[1] = "But you didn't pick enough...";
                        corruptionLevel -= 3;
                    }
                    corruptionLevel += 3;
                }
                else if (GameManager.Instance.GetChoiceValue(309))
                {
                    dialogueScript[7].Lines[1] = dialogueScript[7].myChoices.GetLines(2, 0);
                    corruptionLevel -= 3;
                }
                Debug.Log("corruptionLevel: " + corruptionLevel);
                inv.ResetLavenderSunflower();
                choicesToggled = false;
                NextLine();
                GameManager.Instance.SetTrigger(3, 8, true);
            }

            //And finally, Lily of the vallies or Lilies?
            else if (textComponent.text == dialogueScript[8].Lines[index])
            {
                if (GameManager.Instance.GetChoiceValue(310))
                {
                    dialogueScript[8].Lines[1] = dialogueScript[8].myChoices.GetLines(1, 0);
                    if (inv.lovAmount < 3)
                    {
                        dialogueScript[6].Lines[1] = "But you didn't pick enough...";
                        corruptionLevel -= 3;
                    }
                    corruptionLevel += 3;
                }
                else if (GameManager.Instance.GetChoiceValue(311))
                {
                    dialogueScript[8].Lines[1] = dialogueScript[8].myChoices.GetLines(2, 0);
                    corruptionLevel -= 3;
                }
                Debug.Log("corruptionLevel: " + corruptionLevel);
                inv.ResetLovLily();
                choicesToggled = false;
                NextLine();
                GameManager.Instance.SetTrigger(3, 9, true);
            }
        }
    }

    async void chapter5Triggers() {
        TriggerScriptLine(5, 1);
        TriggerScriptLine(5, 2);
        TriggerScriptLine(5, 3);
        TriggerScriptLine(5, 4);
        TriggerScriptLine(5, 5);
        TriggerScriptLine(5, 6);
        //TriggerScriptLine(5,6);

        if (dialogueScript[currentScript].Lines[index] == dialogueScript[1].Lines[0]) {
            if (animator != null) { 
                // Trigger animation using the provided trigger name
                animator.SetTrigger("PFP1");
                }
            }else if(dialogueScript[currentScript].Lines[index] == dialogueScript[2].Lines[0]){
                if (animator != null) { 
                // Trigger animation using the provided trigger name
                animator.SetTrigger("PFP2");
                }

            }else if(dialogueScript[currentScript].Lines[index] == dialogueScript[3].Lines[1]){
                if (animator != null) { 
                // Trigger animation using the provided trigger name
                animator.SetTrigger("PFP3");
                }

            }

        if (choicesToggled == true && choiceFunction.choicePressed == false) {

            //display the pressed choices dialogue lines
            //Ch5.8.1
            if (textComponent.text == dialogueScript[1].Lines[index]) { //maybe this will work?

            
                if (GameManager.Instance.GetChoiceValue(502)) {
                    dialogueScript[1].Lines[1] = dialogueScript[1].myChoices.GetLines(1, 0);
                    corruptionLevel -= 1;

                } else if (GameManager.Instance.GetChoiceValue(503)) {
                    dialogueScript[1].Lines[1] = dialogueScript[1].myChoices.GetLines(2, 0);
                    corruptionLevel += 1;
                }
                choicesToggled = false;
                NextLine();
                GameManager.Instance.SetTrigger(5, 2, true);
                GameManager.Instance.SetTrigger(5, 3, true);


                //Ch5.8.3
            } else if (textComponent.text == dialogueScript[3].Lines[index]) {
                    
                if (GameManager.Instance.GetChoiceValue(504)) {
                    dialogueScript[3].Lines[1] = dialogueScript[3].myChoices.GetLines(1, 0);
                    corruptionLevel -= 1;

                } else if (GameManager.Instance.GetChoiceValue(505)) {
                    dialogueScript[3].Lines[1] = dialogueScript[3].myChoices.GetLines(2, 0);
                    corruptionLevel += 1;
                }
                choicesToggled = false;
                NextLine();
                GameManager.Instance.SetTrigger(5, 4, true);
                GameManager.Instance.SetTrigger(5, 5, true);

                //Ch5.8.5
            } else if (textComponent.text == dialogueScript[5].Lines[index]) {
                if (GameManager.Instance.GetChoiceValue(506)) {
                    dialogueScript[5].Lines[1] = dialogueScript[5].myChoices.GetLines(1, 0);
                    corruptionLevel -= 1;

                } else if (GameManager.Instance.GetChoiceValue(507)) {
                    dialogueScript[5].Lines[1] = dialogueScript[5].myChoices.GetLines(2, 0);
                    corruptionLevel += 1;
                }
                choicesToggled = false;
                NextLine();
                GameManager.Instance.SetTrigger(5, 6, true);

            } else if (textComponent.text == dialogueScript[6].Lines[index]) {
                if (GameManager.Instance.GetChoiceValue(508)) {
                    dialogueScript[6].Lines[1] = dialogueScript[6].myChoices.GetLines(1, 0);
                    corruptionLevel -= 1;
                    //Call good or bad ending 1
                    bool allglow = false;

                    for(int i =0; i<5; i++){
                        if(GameManager.Instance.GetUnlockOrb(i)){
                            allglow = true;
                        }else{
                            allglow = true;
                            break;

                        }
                    }
                    if (corruptionLevel <= 0 && allglow) { //Add all orbs have been found
                    isToggled = false;
                    ToggleChildren(false);
                        GameManager.Instance.OpenScene("GoodEnding1");
                    } else {
                        isToggled = false;
                        ToggleChildren(false);
                        GameManager.Instance.OpenScene("BadEnding1");
                    }

                } else if (GameManager.Instance.GetChoiceValue(509)) {
                    dialogueScript[6].Lines[1] = dialogueScript[6].myChoices.GetLines(2, 0);
                    corruptionLevel += 1;
                    //Call bad ending 2
                    isToggled = false;
                    ToggleChildren(false);

                    GameManager.Instance.OpenScene("BadEnding2");

                }
                choicesToggled = false;
                NextLine();
                //GameManager.Instance.SetTrigger(5,6,true);
            }
        }
    }

    void chapterENDTriggers() {
        TriggerScriptLine(6, 1);

        if (choicesToggled == true && choiceFunction.choicePressed == false) {

            //display the pressed choices dialogue lines

            if (textComponent.text == dialogueScript[0].Lines[index]) { //maybe this will work?
                if (GameManager.Instance.GetChoiceValue(510)) {
                    dialogueScript[0].Lines[1] = dialogueScript[0].myChoices.GetLines(1, 0);

                } else if (GameManager.Instance.GetChoiceValue(511)) {
                    dialogueScript[0].Lines[1] = dialogueScript[0].myChoices.GetLines(2, 0);
                }
                choicesToggled = false;
                NextLine();

                //play animation?
                if (animator != null && (currentScript == 1 || index == 1)) {
                    // Trigger animation using the provided trigger name
                    animator.SetTrigger("GoodEnding");
                }

            }
        }
    }

    private void TriggerScriptLine(int chap, int curIndx) {
        // Debug.Log("in TriggerScriptLine " + "chap: "+ chap + ", curIndx: " + curIndx + ", isToggled: " + isToggled + ", currentScript: " + curIndx);

        if ((GameManager.Instance.GetTrigger(chap, curIndx)) && (isToggled == false) && currentScript == curIndx) {
            isToggled = true;
            ToggleChildren(true);
            GameManager.Instance.SetCutsceneTrigger(true);
            textComponent.text = string.Empty;
            StartDialogue();
            GameManager.Instance.SetTrigger(chap, curIndx, false);
        }

    }
}

