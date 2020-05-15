using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using UnityEngine;
using KModkit;

public class keywordsScript : MonoBehaviour
{
	public KMAudio Audio;
    public KMBombInfo Bomb;

    static int moduleIdCounter = 1;
    int moduleId;
    private bool moduleSolved;

    public KMSelectable[] buttons;
    public TextMesh[] buttonTexts;

    public TextMesh displayTXT;
    public TextMesh answerAtxt;
    public TextMesh answerBtxt;

    private string loggedAnswers;

    private int digitA1;
    private int digitB1;
    private int digitA2;
    private int digitB2;

    private string word1;
    private string word2;

    private string completeKey;
    private string displayKey;
    private string completeWordA;
    private string completedWordA;
    private string writtenWordA;
    private string answerA;
    private string completeWordB;
    private string completedWordB;
    private string writtenWordB;
    private string answerB;
    private string cleanA;
    private string cleanB;

    private string tempValuePossibility;

    public GameObject striketrhough1;
    public GameObject striketrhough2;

    public char[] displayTXTarray;

    public List<string> buttonValuePossibilities;
    public string[] buttonValues;

    private int currentDigit = 0;

    private int tempCompleteKeyForId;

    public List<int> digitList;
    public List<string> convowlsOrder = new List<string>() { "ccvv", "cvvc", "cvvc", "cvcv", "cvcv", "vcvc", "vcvc", "vccv", "vvcc" };

    private string selectedKeyOrder;

    private List<string> consonants = new List<string>() {"b", "c", "d", "f", "g", "h", "i", "j", "k", "l", "m", "n", "p", "q", "r", "s", "t", "v", "w", "x", "y", "z"};

    private string cons = "bcdfghjklmnpqrstvwxyz";
    private string vowls = "aeiou";

    private string tempWord;
    private int tempChar;

    private int tempPossibilityButton;

    void Awake()
    {
        moduleId = moduleIdCounter++;
        foreach (KMSelectable button in buttons)
        {
            KMSelectable pressedButton = button;
            button.OnInteract += delegate () { ButtonPress(pressedButton); return false; };
        }
    }

    void Start()
    {
        getDigits();
        getWords();
        wordKeyCompleter();
        changeText();
        getAnswers();
        Debug.Log("[Keywords #" + moduleId + "] The key is " + completeKey + " and the words are " + completedWordA + " and " + completedWordB + ", meaning the answers are " + answerA + " and " + answerB + ".");
        getButtonValues();
        setButtonValues();
    }

    void setButtonValues()
    {
        tempPossibilityButton = UnityEngine.Random.Range(0, 10);
        buttonTexts[10].text = buttonValuePossibilities[tempPossibilityButton].ToUpper();
        buttonValuePossibilities.RemoveAt(tempPossibilityButton);
        tempPossibilityButton = UnityEngine.Random.Range(0, 9);
        buttonTexts[11].text = buttonValuePossibilities[tempPossibilityButton].ToUpper();
        buttonValuePossibilities.RemoveAt(tempPossibilityButton);
        tempPossibilityButton = UnityEngine.Random.Range(0, 8);
        buttonTexts[12].text = buttonValuePossibilities[tempPossibilityButton].ToUpper();
        buttonValuePossibilities.RemoveAt(tempPossibilityButton);
        tempPossibilityButton = UnityEngine.Random.Range(0, 7);
        buttonTexts[13].text = buttonValuePossibilities[tempPossibilityButton].ToUpper();
        buttonValuePossibilities.RemoveAt(tempPossibilityButton);
        tempPossibilityButton = UnityEngine.Random.Range(0, 6);
        buttonTexts[14].text = buttonValuePossibilities[tempPossibilityButton].ToUpper();
        buttonValuePossibilities.RemoveAt(tempPossibilityButton);
        tempPossibilityButton = UnityEngine.Random.Range(0, 5);
        buttonTexts[15].text = buttonValuePossibilities[tempPossibilityButton].ToUpper();
        buttonValuePossibilities.RemoveAt(tempPossibilityButton);
        tempPossibilityButton = UnityEngine.Random.Range(0, 4);
        buttonTexts[16].text = buttonValuePossibilities[tempPossibilityButton].ToUpper();
        buttonValuePossibilities.RemoveAt(tempPossibilityButton);
        tempPossibilityButton = UnityEngine.Random.Range(0, 3);
        buttonTexts[17].text = buttonValuePossibilities[tempPossibilityButton].ToUpper();
        buttonValuePossibilities.RemoveAt(tempPossibilityButton);
        tempPossibilityButton = UnityEngine.Random.Range(0, 2);
        buttonTexts[18].text = buttonValuePossibilities[tempPossibilityButton].ToUpper();
        buttonValuePossibilities.RemoveAt(tempPossibilityButton);
        tempPossibilityButton = UnityEngine.Random.Range(0, 1);
        buttonTexts[19].text = buttonValuePossibilities[tempPossibilityButton].ToUpper();
        buttonValuePossibilities.RemoveAt(tempPossibilityButton);
    }

    void getButtonValues()
    {
        cleanA = new String(answerA.Where(c => c != '-' && (c < '0' || c > '9')).ToArray());
        cleanB = new String(answerB.Where(c => c != '-' && (c < '0' || c > '9')).ToArray());
        foreach (char answerValue in cleanA)
        {
            tempValuePossibility = Char.ToString(answerValue);
            buttonValuePossibilities.Add(tempValuePossibility);
        }
        foreach (char answerValue in cleanB)
        {
            tempValuePossibility = Char.ToString(answerValue);
            buttonValuePossibilities.Add(tempValuePossibility);
        }
        buttonValuePossibilities = buttonValuePossibilities.Distinct().ToList();
        while (buttonValuePossibilities.Count < 10)
        {
            buttonValuePossibilities = buttonValuePossibilities.Distinct().ToList();
            addLetter();
            buttonValuePossibilities = buttonValuePossibilities.Distinct().ToList();
        }
        Debug.Log("[Keywords #" + moduleId + "] The non-number buttons are the following: " + String.Join("", new List<string>(buttonValuePossibilities).ConvertAll(i => i.ToString()).ToArray()) + ".");
    }

    void addLetter()
    {
        if (!buttonValuePossibilities.Contains("e"))
        {
            buttonValuePossibilities.Add("e");
            return;
        }
        else if (!buttonValuePossibilities.Contains("a"))
        {
            buttonValuePossibilities.Add("a");
            return;
        }
        else if (!buttonValuePossibilities.Contains("o"))
        {
            buttonValuePossibilities.Add("o");
            return;
        }
        else if (!buttonValuePossibilities.Contains("i"))
        {
            buttonValuePossibilities.Add("i");
            return;
        }
        else if (!buttonValuePossibilities.Contains("u"))
        {
            buttonValuePossibilities.Add("u");
            return;
        }
        else
        {
            buttonValuePossibilities.Add(consonants.PickRandom());
            return;
        }
    }

    void getAnswers()
    {
        getAnswerTop();
        getAnswerBottom();
    }

    void getAnswerTop()
    {
        foreach (char wordCharacter in completedWordA)
        {
            tempCompleteKeyForId = 0;
            foreach (char keyCharacter in completeKey)
            {
                tempCompleteKeyForId += 1;
                if (wordCharacter == keyCharacter)
                {
                    answerA = answerA + tempCompleteKeyForId;
                    tempCompleteKeyForId = 9;
                }
                else if (tempCompleteKeyForId == 8)
                {
                    answerA = answerA + wordCharacter;
                }
            }
        }
    }

    void getAnswerBottom()
    {
        foreach (char wordCharacter in completedWordB)
        {
            tempCompleteKeyForId = 0;
            foreach (char keyCharacter in completeKey)
            {
                tempCompleteKeyForId += 1;
                if (wordCharacter == keyCharacter)
                {
                    answerB = answerB + tempCompleteKeyForId;
                    tempCompleteKeyForId = 9;
                }
                else if (tempCompleteKeyForId == 8)
                {
                    answerB = answerB + wordCharacter;
                }
            }
        }
    }

    void changeText()
    {
        displayTXT.text = displayKey;
        foreach (char letter in completeWordA.ToCharArray())
        {
            writtenWordA = writtenWordA + letter;
            writtenWordA = writtenWordA + " ";
        }
        writtenWordA.Remove(writtenWordA.Length - 1);
        foreach (char letter in completeWordB.ToCharArray())
        {
            writtenWordB = writtenWordB + letter;
            writtenWordB = writtenWordB + " ";
        }
        writtenWordA.Remove(writtenWordA.Length - 1);
        answerAtxt.text = "_ _ _ _ " + completeWordA;
        answerBtxt.text = "_ _ _ _ " + completeWordB;
    }

    void wordKeyCompleter()
    {
        completeTheKey();
        completeWordA = completeTheWords();
        completedWordA = word2 + completeWordA;
        completeWordB = completeTheWords();
        completedWordB = word2 + completeWordB;
    }

    void completeTheKey()
    {
        cons = "bcdfghjklmnpqrstvwxyz";
        vowls = "aeiou";
        completeKey = "";
        foreach (char character in word1.ToCharArray())
        {
            cons = String.Join("", cons.Split(character));
            vowls = String.Join("", vowls.Split(character));
        }
        selectedKeyOrder = convowlsOrder[UnityEngine.Random.Range(0, 9)];
        foreach (char character in selectedKeyOrder.ToCharArray())
        {
            if (character == 'c')
            {
                tempChar = UnityEngine.Random.Range(0, cons.Length);
                completeKey = completeKey + cons[tempChar];
                cons = String.Join("", cons.Split(cons[tempChar]));
            }
            else if (character == 'v')
            {
                tempChar = UnityEngine.Random.Range(0, vowls.Length);
                completeKey = completeKey + vowls[tempChar];
                vowls = String.Join("", vowls.Split(vowls[tempChar]));
            }
        }
        displayKey = completeKey + "----";
        completeKey = completeKey + word1;
    }

    string completeTheWords()
    {
        cons = "bcdfghjklmnpqrstvwxyz";
        vowls = "aeiou";
        tempWord = "";
        foreach (char character in word2.ToCharArray())
        {
            cons = String.Join("", cons.Split(character));
            vowls = String.Join("", vowls.Split(character));
        }
        selectedKeyOrder = convowlsOrder[UnityEngine.Random.Range(0, 9)];
        foreach (char character in selectedKeyOrder.ToCharArray())
        {
            if (character == 'c')
            {
                tempChar = UnityEngine.Random.Range(0, cons.Length);
                tempWord = tempWord + cons[tempChar];
                cons = String.Join("", cons.Split(cons[tempChar]));
            }
            else if (character == 'v')
            {
                tempChar = UnityEngine.Random.Range(0, vowls.Length);
                tempWord = tempWord + vowls[tempChar];
                vowls = String.Join("", vowls.Split(vowls[tempChar]));
            }
        }
        return tempWord;
    }

    void getWords()
    {
        getWord1();
        getWord2();
    }

    void getWord1()
    {
        if (digitA1 == 0)
        {
            if (digitB1 == 0)
            {
                word1 = "gaho";
            }
            else if (digitB1 == 1)
            {
                word1 = "wixo";
            }
            else if (digitB1 == 2)
            {
                word1 = "suap";
            }
            else if (digitB1 == 3)
            {
                word1 = "remi";
            }
            else if (digitB1 == 4)
            {
                word1 = "ascu";
            }
            else if (digitB1 == 5)
            {
                word1 = "acke";
            }
            else if (digitB1 == 6)
            {
                word1 = "agni";
            }
            else if (digitB1 == 7)
            {
                word1 = "oxfi";
            }
            else if (digitB1 == 8)
            {
                word1 = "orja";
            }
            else if (digitB1 == 9)
            {
                word1 = "loye";
            }
            else
            {
                word1 = "ERROR";
            }
        }
        if (digitA1 == 1)
        {
            if (digitB1 == 0)
            {
                word1 = "xied";
            }
            else if (digitB1 == 1)
            {
                word1 = "xida";
            }
            else if (digitB1 == 2)
            {
                word1 = "unge";
            }
            else if (digitB1 == 3)
            {
                word1 = "puor";
            }
            else if (digitB1 == 4)
            {
                word1 = "pero";
            }
            else if (digitB1 == 5)
            {
                word1 = "kelu";
            }
            else if (digitB1 == 6)
            {
                word1 = "frea";
            }
            else if (digitB1 == 7)
            {
                word1 = "joze";
            }
            else if (digitB1 == 8)
            {
                word1 = "inwo";
            }
            else if (digitB1 == 9)
            {
                word1 = "huli";
            }
            else
            {
                word1 = "ERROR";
            }
        }
        if (digitA1 == 2)
        {
            if (digitB1 == 0)
            {
                word1 = "gano";
            }
            else if (digitB1 == 1)
            {
                word1 = "lixo";
            }
            else if (digitB1 == 2)
            {
                word1 = "suay";
            }
            else if (digitB1 == 3)
            {
                word1 = "refi";
            }
            else if (digitB1 == 4)
            {
                word1 = "asku";
            }
            else if (digitB1 == 5)
            {
                word1 = "acse";
            }
            else if (digitB1 == 6)
            {
                word1 = "agti";
            }
            else if (digitB1 == 7)
            {
                word1 = "ovfi";
            }
            else if (digitB1 == 8)
            {
                word1 = "orva";
            }
            else if (digitB1 == 9)
            {
                word1 = "noye";
            }
            else
            {
                word1 = "ERROR";
            }
        }
        if (digitA1 == 3)
        {
            if (digitB1 == 0)
            {
                word1 = "gied";
            }
            else if (digitB1 == 1)
            {
                word1 = "kida";
            }
            else if (digitB1 == 2)
            {
                word1 = "unbe";
            }
            else if (digitB1 == 3)
            {
                word1 = "puob";
            }
            else if (digitB1 == 4)
            {
                word1 = "lero";
            }
            else if (digitB1 == 5)
            {
                word1 = "keru";
            }
            else if (digitB1 == 6)
            {
                word1 = "fyea";
            }
            else if (digitB1 == 7)
            {
                word1 = "roze";
            }
            else if (digitB1 == 8)
            {
                word1 = "inso";
            }
            else if (digitB1 == 9)
            {
                word1 = "suli";
            }
            else
            {
                word1 = "ERROR";
            }
        }
        if (digitA1 == 4)
        {
            if (digitB1 == 0)
            {
                word1 = "gaho";
            }
            else if (digitB1 == 1)
            {
                word1 = "wivo";
            }
            else if (digitB1 == 2)
            {
                word1 = "vuap";
            }
            else if (digitB1 == 3)
            {
                word1 = "reji";
            }
            else if (digitB1 == 4)
            {
                word1 = "akcu";
            }
            else if (digitB1 == 5)
            {
                word1 = "acbe";
            }
            else if (digitB1 == 6)
            {
                word1 = "abni";
            }
            else if (digitB1 == 7)
            {
                word1 = "ohfi";
            }
            else if (digitB1 == 8)
            {
                word1 = "ohja";
            }
            else if (digitB1 == 9)
            {
                word1 = "hoye";
            }
            else
            {
                word1 = "ERROR";
            }
        }
        if (digitA1 == 5)
        {
            if (digitB1 == 0)
            {
                word1 = "xieq";
            }
            else if (digitB1 == 1)
            {
                word1 = "xiba";
            }
            else if (digitB1 == 2)
            {
                word1 = "unfe";
            }
            else if (digitB1 == 3)
            {
                word1 = "buor";
            }
            else if (digitB1 == 4)
            {
                word1 = "peto";
            }
            else if (digitB1 == 5)
            {
                word1 = "kequ";
            }
            else if (digitB1 == 6)
            {
                word1 = "grea";
            }
            else if (digitB1 == 7)
            {
                word1 = "noze";
            }
            else if (digitB1 == 8)
            {
                word1 = "imwo";
            }
            else if (digitB1 == 9)
            {
                word1 = "husi";
            }
            else
            {
                word1 = "ERROR";
            }
        }
        if (digitA1 == 6)
        {
            if (digitB1 == 0)
            {
                word1 = "nago";
            }
            else if (digitB1 == 1)
            {
                word1 = "xilo";
            }
            else if (digitB1 == 2)
            {
                word1 = "yuas";
            }
            else if (digitB1 == 3)
            {
                word1 = "feri";
            }
            else if (digitB1 == 4)
            {
                word1 = "aksu";
            }
            else if (digitB1 == 5)
            {
                word1 = "asce";
            }
            else if (digitB1 == 6)
            {
                word1 = "atgi";
            }
            else if (digitB1 == 7)
            {
                word1 = "ofvi";
            }
            else if (digitB1 == 8)
            {
                word1 = "ovra";
            }
            else if (digitB1 == 9)
            {
                word1 = "yone";
            }
            else
            {
                word1 = "ERROR";
            }
        }
        if (digitA1 == 7)
        {
            if (digitB1 == 0)
            {
                word1 = "dieg";
            }
            else if (digitB1 == 1)
            {
                word1 = "dika";
            }
            else if (digitB1 == 2)
            {
                word1 = "ubne";
            }
            else if (digitB1 == 3)
            {
                word1 = "buop";
            }
            else if (digitB1 == 4)
            {
                word1 = "relo";
            }
            else if (digitB1 == 5)
            {
                word1 = "reku";
            }
            else if (digitB1 == 6)
            {
                word1 = "yfea";
            }
            else if (digitB1 == 7)
            {
                word1 = "zore";
            }
            else if (digitB1 == 8)
            {
                word1 = "isno";
            }
            else if (digitB1 == 9)
            {
                word1 = "lusi";
            }
            else
            {
                word1 = "ERROR";
            }
        }
        if (digitA1 == 8)
        {
            if (digitB1 == 0)
            {
                word1 = "hafo";
            }
            else if (digitB1 == 1)
            {
                word1 = "viwo";
            }
            else if (digitB1 == 2)
            {
                word1 = "puav";
            }
            else if (digitB1 == 3)
            {
                word1 = "jeri";
            }
            else if (digitB1 == 4)
            {
                word1 = "acku";
            }
            else if (digitB1 == 5)
            {
                word1 = "abce";
            }
            else if (digitB1 == 6)
            {
                word1 = "anbi";
            }
            else if (digitB1 == 7)
            {
                word1 = "ofhi";
            }
            else if (digitB1 == 8)
            {
                word1 = "ojha";
            }
            else if (digitB1 == 9)
            {
                word1 = "yohe";
            }
            else
            {
                word1 = "ERROR";
            }
        }
        if (digitA1 == 9)
        {
            if (digitB1 == 0)
            {
                word1 = "qiex";
            }
            else if (digitB1 == 1)
            {
                word1 = "bixa";
            }
            else if (digitB1 == 2)
            {
                word1 = "ufne";
            }
            else if (digitB1 == 3)
            {
                word1 = "ruob";
            }
            else if (digitB1 == 4)
            {
                word1 = "tepo";
            }
            else if (digitB1 == 5)
            {
                word1 = "qeku";
            }
            else if (digitB1 == 6)
            {
                word1 = "rgea";
            }
            else if (digitB1 == 7)
            {
                word1 = "zone";
            }
            else if (digitB1 == 8)
            {
                word1 = "iwmo";
            }
            else if (digitB1 == 9)
            {
                word1 = "suhi";
            }
            else
            {
                word1 = "ERROR";
            }
        }
    }

    void getWord2()
    {
        if (digitA2 == 0)
        {
            if (digitB2 == 0)
            {
                word2 = "gaho";
            }
            else if (digitB2 == 1)
            {
                word2 = "wixo";
            }
            else if (digitB2 == 2)
            {
                word2 = "suap";
            }
            else if (digitB2 == 3)
            {
                word2 = "remi";
            }
            else if (digitB2 == 4)
            {
                word2 = "ascu";
            }
            else if (digitB2 == 5)
            {
                word2 = "acke";
            }
            else if (digitB2 == 6)
            {
                word2 = "agni";
            }
            else if (digitB2 == 7)
            {
                word2 = "oxfi";
            }
            else if (digitB2 == 8)
            {
                word2 = "orja";
            }
            else if (digitB2 == 9)
            {
                word2 = "loye";
            }
            else
            {
                word2 = "ERROR";
            }
        }
        if (digitA2 == 1)
        {
            if (digitB2 == 0)
            {
                word2 = "xied";
            }
            else if (digitB2 == 1)
            {
                word2 = "xida";
            }
            else if (digitB2 == 2)
            {
                word2 = "unge";
            }
            else if (digitB2 == 3)
            {
                word2 = "puor";
            }
            else if (digitB2 == 4)
            {
                word2 = "pero";
            }
            else if (digitB2 == 5)
            {
                word2 = "kelu";
            }
            else if (digitB2 == 6)
            {
                word2 = "frea";
            }
            else if (digitB2 == 7)
            {
                word2 = "joze";
            }
            else if (digitB2 == 8)
            {
                word2 = "inwo";
            }
            else if (digitB2 == 9)
            {
                word2 = "huli";
            }
            else
            {
                word2 = "ERROR";
            }
        }
        if (digitA2 == 2)
        {
            if (digitB2 == 0)
            {
                word2 = "gano";
            }
            else if (digitB2 == 1)
            {
                word2 = "lixo";
            }
            else if (digitB2 == 2)
            {
                word2 = "suay";
            }
            else if (digitB2 == 3)
            {
                word2 = "refi";
            }
            else if (digitB2 == 4)
            {
                word2 = "asku";
            }
            else if (digitB2 == 5)
            {
                word2 = "acse";
            }
            else if (digitB2 == 6)
            {
                word2 = "agti";
            }
            else if (digitB2 == 7)
            {
                word2 = "ovfi";
            }
            else if (digitB2 == 8)
            {
                word2 = "orva";
            }
            else if (digitB2 == 9)
            {
                word2 = "noye";
            }
            else
            {
                word2 = "ERROR";
            }
        }
        if (digitA2 == 3)
        {
            if (digitB2 == 0)
            {
                word2 = "gied";
            }
            else if (digitB2 == 1)
            {
                word2 = "kida";
            }
            else if (digitB2 == 2)
            {
                word2 = "unbe";
            }
            else if (digitB2 == 3)
            {
                word2 = "puob";
            }
            else if (digitB2 == 4)
            {
                word2 = "lero";
            }
            else if (digitB2 == 5)
            {
                word2 = "keru";
            }
            else if (digitB2 == 6)
            {
                word2 = "fyea";
            }
            else if (digitB2 == 7)
            {
                word2 = "roze";
            }
            else if (digitB2 == 8)
            {
                word2 = "inso";
            }
            else if (digitB2 == 9)
            {
                word2 = "suli";
            }
            else
            {
                word2 = "ERROR";
            }
        }
        if (digitA2 == 4)
        {
            if (digitB2 == 0)
            {
                word2 = "gaho";
            }
            else if (digitB2 == 1)
            {
                word2 = "wivo";
            }
            else if (digitB2 == 2)
            {
                word2 = "vuap";
            }
            else if (digitB2 == 3)
            {
                word2 = "reji";
            }
            else if (digitB2 == 4)
            {
                word2 = "akcu";
            }
            else if (digitB2 == 5)
            {
                word2 = "acbe";
            }
            else if (digitB2 == 6)
            {
                word2 = "abni";
            }
            else if (digitB2 == 7)
            {
                word2 = "ohfi";
            }
            else if (digitB2 == 8)
            {
                word2 = "ohja";
            }
            else if (digitB2 == 9)
            {
                word2 = "hoye";
            }
            else
            {
                word2 = "ERROR";
            }
        }
        if (digitA2 == 5)
        {
            if (digitB2 == 0)
            {
                word2 = "xieq";
            }
            else if (digitB2 == 1)
            {
                word2 = "xiba";
            }
            else if (digitB2 == 2)
            {
                word2 = "unfe";
            }
            else if (digitB2 == 3)
            {
                word2 = "buor";
            }
            else if (digitB2 == 4)
            {
                word2 = "peto";
            }
            else if (digitB2 == 5)
            {
                word2 = "kequ";
            }
            else if (digitB2 == 6)
            {
                word2 = "grea";
            }
            else if (digitB2 == 7)
            {
                word2 = "noze";
            }
            else if (digitB2 == 8)
            {
                word2 = "imwo";
            }
            else if (digitB2 == 9)
            {
                word2 = "husi";
            }
            else
            {
                word2 = "ERROR";
            }
        }
        if (digitA2 == 6)
        {
            if (digitB2 == 0)
            {
                word2 = "nago";
            }
            else if (digitB2 == 1)
            {
                word2 = "xilo";
            }
            else if (digitB2 == 2)
            {
                word2 = "yuas";
            }
            else if (digitB2 == 3)
            {
                word2 = "feri";
            }
            else if (digitB2 == 4)
            {
                word2 = "aksu";
            }
            else if (digitB2 == 5)
            {
                word2 = "asce";
            }
            else if (digitB2 == 6)
            {
                word2 = "atgi";
            }
            else if (digitB2 == 7)
            {
                word2 = "ofvi";
            }
            else if (digitB2 == 8)
            {
                word2 = "ovra";
            }
            else if (digitB2 == 9)
            {
                word2 = "yone";
            }
            else
            {
                word2 = "ERROR";
            }
        }
        if (digitA2 == 7)
        {
            if (digitB2 == 0)
            {
                word2 = "dieg";
            }
            else if (digitB2 == 1)
            {
                word2 = "dika";
            }
            else if (digitB2 == 2)
            {
                word2 = "ubne";
            }
            else if (digitB2 == 3)
            {
                word2 = "buop";
            }
            else if (digitB2 == 4)
            {
                word2 = "relo";
            }
            else if (digitB2 == 5)
            {
                word2 = "reku";
            }
            else if (digitB2 == 6)
            {
                word2 = "yfea";
            }
            else if (digitB2 == 7)
            {
                word2 = "zore";
            }
            else if (digitB2 == 8)
            {
                word2 = "isno";
            }
            else if (digitB2 == 9)
            {
                word2 = "lusi";
            }
            else
            {
                word2 = "ERROR";
            }
        }
        if (digitA2 == 8)
        {
            if (digitB2 == 0)
            {
                word2 = "hafo";
            }
            else if (digitB2 == 1)
            {
                word2 = "viwo";
            }
            else if (digitB2 == 2)
            {
                word2 = "puav";
            }
            else if (digitB2 == 3)
            {
                word2 = "jeri";
            }
            else if (digitB2 == 4)
            {
                word2 = "acku";
            }
            else if (digitB2 == 5)
            {
                word2 = "abce";
            }
            else if (digitB2 == 6)
            {
                word2 = "anbi";
            }
            else if (digitB2 == 7)
            {
                word2 = "ofhi";
            }
            else if (digitB2 == 8)
            {
                word2 = "ojha";
            }
            else if (digitB2 == 9)
            {
                word2 = "yohe";
            }
            else
            {
                word2 = "ERROR";
            }
        }
        if (digitA2 == 9)
        {
            if (digitB2 == 0)
            {
                word2 = "qiex";
            }
            else if (digitB2 == 1)
            {
                word2 = "bixa";
            }
            else if (digitB2 == 2)
            {
                word2 = "ufne";
            }
            else if (digitB2 == 3)
            {
                word2 = "ruob";
            }
            else if (digitB2 == 4)
            {
                word2 = "tepo";
            }
            else if (digitB2 == 5)
            {
                word2 = "qeku";
            }
            else if (digitB2 == 6)
            {
                word2 = "rgea";
            }
            else if (digitB2 == 7)
            {
                word2 = "zone";
            }
            else if (digitB2 == 8)
            {
                word2 = "iwmo";
            }
            else if (digitB2 == 9)
            {
                word2 = "suhi";
            }
            else
            {
                word2 = "ERROR";
            }
        }
    }

    void getDigits()
    {
        foreach (int digit in Bomb.GetSerialNumberNumbers())
        {
            digitList.Add(digit);
        }
        digitA1 = digitList[0];
        digitB1 = digitList[1];
        digitA2 = digitList[digitList.Count - 1];
        digitB2 = digitList[digitList.Count - 2];
        loggedAnswers = "The numbers are " + digitA1.ToString() + digitB1.ToString() + " and " + digitA2.ToString() + digitB2.ToString() + ".";
        Debug.Log("[Keywords #" + moduleId + "] " + loggedAnswers);
    }

    void ButtonPress(KMSelectable button)
    {
        if (!moduleSolved)
        {
            button.AddInteractionPunch();
            GetComponent<KMAudio>().PlayGameSoundAtTransform(KMSoundOverride.SoundEffect.TypewriterKey, transform);
            button.AddInteractionPunch();
            GetComponent<KMAudio>().PlayGameSoundAtTransform(KMSoundOverride.SoundEffect.TypewriterKey, transform);
            button.AddInteractionPunch();
            displayTXTarray = displayTXT.text.ToCharArray();
            displayTXTarray[currentDigit] = button.GetComponentInChildren<TextMesh>().text.ToCharArray()[0];
            displayTXT.text = new string(displayTXTarray);
            if (currentDigit == 7)
            {
                if (displayTXT.text.ToLower() == answerA)
                {
                    striketrhough1.SetActive(true);
                    GetComponent<KMAudio>().PlayGameSoundAtTransform(KMSoundOverride.SoundEffect.WireSequenceMechanism, transform);
                    currentDigit = 0;
                    displayTXT.text = displayKey;
                    Debug.Log("[Keywords #" + moduleId + "] Correct! Both the user input and the top answer was " + answerA + ".");
                    if (striketrhough1.activeSelf && striketrhough2.activeSelf)
                    {
                        displayTXT.text = "--------";
                        moduleSolved = true;
                        GetComponent<KMBombModule>().HandlePass();
                        GetComponent<KMAudio>().PlayGameSoundAtTransform(KMSoundOverride.SoundEffect.CorrectChime, transform);
                        button.AddInteractionPunch(4f);
                        GetComponent<KMAudio>().PlayGameSoundAtTransform(KMSoundOverride.SoundEffect.CorrectChime, transform);
                        return;
                    }
                    return;
                }
                if (displayTXT.text.ToLower() == answerB)
                {
                    striketrhough2.SetActive(true);
                    GetComponent<KMAudio>().PlayGameSoundAtTransform(KMSoundOverride.SoundEffect.WireSequenceMechanism, transform);
                    currentDigit = 0;
                    displayTXT.text = displayKey;
                    Debug.Log("[Keywords #" + moduleId + "] Correct! Both the user input and the bottom answer was " + answerB + ".");
                    if (striketrhough1.activeSelf && striketrhough2.activeSelf)
                    {
                        displayTXT.text = "--------";
                        moduleSolved = true;
                        GetComponent<KMBombModule>().HandlePass();
                        GetComponent<KMAudio>().PlayGameSoundAtTransform(KMSoundOverride.SoundEffect.CorrectChime, transform);
                        button.AddInteractionPunch(4f);
                        GetComponent<KMAudio>().PlayGameSoundAtTransform(KMSoundOverride.SoundEffect.CorrectChime, transform);
                        return;
                    }
                    return;
                }
                if ((displayTXT.text.ToLower() != answerA) && (displayTXT.text.ToLower() != answerB))
                {
                    Debug.Log("[Keywords #" + moduleId + "] Wrong!!! The user input was " + displayTXT.text.ToLower() + " when the correct answers were " + answerA + " and " + answerB + ".");
                    button.AddInteractionPunch(4f);
                    GetComponent<KMBombModule>().HandleStrike();
                    button.AddInteractionPunch(4f);
                    currentDigit = 0;
                    displayTXT.text = displayKey;
                    return;
                }
            }
            if (striketrhough1.activeSelf && striketrhough2.activeSelf)
            {
                displayTXT.text = "--------";
                moduleSolved = true;
                GetComponent<KMBombModule>().HandlePass();
                GetComponent<KMAudio>().PlayGameSoundAtTransform(KMSoundOverride.SoundEffect.CorrectChime, transform);
                button.AddInteractionPunch(4f);
                GetComponent<KMAudio>().PlayGameSoundAtTransform(KMSoundOverride.SoundEffect.CorrectChime, transform);
                return;
            }
        }
        else
        {
            button.AddInteractionPunch(0.25f);
            GetComponent<KMAudio>().PlayGameSoundAtTransform(KMSoundOverride.SoundEffect.TypewriterKey, transform);
            button.AddInteractionPunch(0.25f);
        }
        currentDigit += 1;
    }
	
	//twitch plays
    #pragma warning disable 414
    private readonly string TwitchHelpMessage = @"Use the command !{0} type <text> to type the text in the keypad. Note: The text must be 8 characters long, and the letters must be capital";
    #pragma warning restore 414
	
	IEnumerator ProcessTwitchCommand(string command)
	{
		string[] parameters = command.Split(' ');
		if (Regex.IsMatch(parameters[0], @"^\s*type\s*$", RegexOptions.IgnoreCase | RegexOptions.CultureInvariant))
		{
			yield return null;
			if (parameters.Length != 2)
			{
				yield return "sendtochaterror The parameter length is invalid";
				yield break;
			}
			
			if (parameters[1].Length != 8)
			{
				yield return "sendtochaterror The text length being sent is invalid";
				yield break;
			}
			
			foreach (char c in parameters[1])
			{
				bool Interval = false;
				for (int x = 0; x < buttonTexts.Count(); x++)
				{
					if (c.ToString() == buttonTexts[x].text)
					{
						Interval = true;
						break;
					}
				}
				
				if (Interval == false)
				{
					yield return "sendtochaterror A character written in the text is not shown in the keypad";
					yield break;
				}
			}
			
			foreach (char d in parameters[1])
			{
				for (int a = 0; a < buttonTexts.Count(); a++)
				{
					if (d.ToString() == buttonTexts[a].text)
					{
						buttons[a].OnInteract();
						yield return new WaitForSecondsRealtime(0.2f);
						break;
					}
				}
			}
		}
	}
}
