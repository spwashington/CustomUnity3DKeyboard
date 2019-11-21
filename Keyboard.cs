//Created by Washington Oliveira da Silva

using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using TMPro;
using UnityEngine;
using UnityEditor;
using System;

public class Keyboard : MonoBehaviour
{
    [Header("--Keyboard Setup--")]
    [SerializeField] private Sprite[] m_CapsKeySprite;
    [SerializeField] private Color m_ActiveSpecialButtons;
    [SerializeField] private Color m_InactiveSpecialButtons;
    [SerializeField] private Color m_BackgroundColor;
    private TMP_InputField m_InputFieldObject;
    private Button m_CurrentInput;
    private bool m_Caps;
    private bool m_Acute;
    private bool m_Tilde;
    private bool m_Circumflex;

    [Header("--Instances--")]
    [SerializeField] private GameObject m_KeyboardKeys;
    [SerializeField] private GameObject m_KeyboardEmailsKeys;
    [SerializeField] private Image[] m_KeyboardBackground;
    [SerializeField] private List<string> m_Character;

    [Header("--Register Fields--")]
    [SerializeField] private TMP_InputField[] m_FieldsToUserFill;


    void Start()
    {
        m_Character = new List<string>();
        m_Caps = false;
        m_Acute = false;
        m_Tilde = false;
        m_Circumflex = false;
        m_KeyboardBackground[0].color = m_BackgroundColor;
        m_KeyboardBackground[1].color = m_BackgroundColor;
    }

    #region Fix Functions
    public void GetTextField(TMP_InputField _TextObject)
    {
        if (!m_KeyboardKeys.activeInHierarchy)
            OpenKeyboard();

        char[] temp = _TextObject.text.ToCharArray();
        m_Character.Clear();

        for (int i = 0; i < temp.Length; i++)
        {
            m_Character.Add(temp[i].ToString());
        }

        m_InputFieldObject = _TextObject;
    }

    public void AddLetter(string _char)
    {
        string _letter = _char;

        switch (_letter.ToLower())
        {
            case "a":
                if (m_Tilde)
                    _letter = "ã";
                else if (m_Acute)
                    _letter = "á";
                else if (m_Circumflex)
                    _letter = "â";
                break;

            case "e":
                if (m_Acute)
                    _letter = "é";
                else if (m_Circumflex)
                    _letter = "ê";
                break;

            case "i":
                if (m_Acute)
                    _letter = "í";
                else if (m_Circumflex)
                    _letter = "î";
                break;

            case "o":
                if (m_Tilde)
                    _letter = "õ";
                else if (m_Acute)
                    _letter = "ó";
                else if (m_Circumflex)
                    _letter = "ô";
                break;

            case "u":
                if (m_Acute)
                    _letter = "ú";
                else if (m_Circumflex)
                    _letter = "û";
                break;

            default:
                if (m_Tilde)
                    _letter = "~" + _letter;
                else if (m_Acute)
                    _letter = "´" + _letter;
                else if (m_Circumflex)
                    _letter = "^" + _letter;
                break;

        }

        if (m_Caps)
            m_Character.Add(_letter.ToUpper());
        else
            m_Character.Add(_letter.ToLower());

        m_Tilde = false;
        m_Circumflex = false;
        m_Acute = false;

        CheckIfSpecialButtonsIsOn();
        UpdatedText();
    }

    private void CheckIfSpecialButtonsIsOn()
    {
        GameObject button;

        if (m_Tilde)
        {
            button = GameObject.Find("Key:Tilde");
            button.GetComponent<Image>().color = m_ActiveSpecialButtons;
        }
        else
        {
            button = GameObject.Find("Key:Tilde");
            button.GetComponent<Image>().color = m_InactiveSpecialButtons;
        }

        if (m_Acute)
        {
            button = GameObject.Find("Key:Acute");
            button.GetComponent<Image>().color = m_ActiveSpecialButtons;
        }
        else
        {
            button = GameObject.Find("Key:Acute");
            button.GetComponent<Image>().color = m_InactiveSpecialButtons;
        }

        if (m_Circumflex)
        {
            button = GameObject.Find("Key:Circumflex");
            button.GetComponent<Image>().color = m_ActiveSpecialButtons;
        }
        else
        {
            button = GameObject.Find("Key:Circumflex");
            button.GetComponent<Image>().color = m_InactiveSpecialButtons;
        }

        if (m_Caps)
        {
            button = GameObject.Find("Key:Caps");
            button.GetComponent<Image>().color = m_ActiveSpecialButtons;
            button.transform.GetChild(0).GetComponent<Image>().sprite = m_CapsKeySprite[1];
        }
        else
        {
            button = GameObject.Find("Key:Caps");
            button.GetComponent<Image>().color = m_InactiveSpecialButtons;
            button.transform.GetChild(0).GetComponent<Image>().sprite = m_CapsKeySprite[0];
        }
    }

    private void UpdatedText()
    {
        string tempText = "";

        for (int i = 0; i < m_Character.Count; i++)
        {
            tempText = string.Concat(tempText, m_Character[i]);
        }

        if (m_InputFieldObject != null)
        {
            m_InputFieldObject.text = tempText.Replace("\\n", "\n");
        }
    }

    public void BackSpaceKey()
    {
        if (m_Character.Count > 0)
        {
            m_Character.RemoveAt(m_Character.Count - 1);
            UpdatedText();
        }
    }

    public void AddSpecialCharacter(string _Acent)
    {
        switch (_Acent)
        {
            case "~":
                if (m_Tilde)
                {
                    AddLetter("");
                    m_Tilde = false;
                }
                else
                    m_Tilde = true;
                break;

            case "´":
                if (m_Acute)
                {
                    AddLetter("");
                    m_Acute = false;
                }
                else
                    m_Acute = true;
                break;

            case "^":
                if (m_Circumflex)
                {
                    AddLetter("");
                    m_Circumflex = false;
                }
                else
                    m_Circumflex = true;
                break;
        }

        CheckIfSpecialButtonsIsOn();
    }

    public void SpaceKey()
    {
        m_Character.Add(" ");
        UpdatedText();
    }

    public void EnterKey()
    {
        for (int i = 0; i < m_FieldsToUserFill.Length; i++)
        {
            if (m_FieldsToUserFill[i] == m_InputFieldObject && i < m_FieldsToUserFill.Length - 1)
                GetTextField(m_FieldsToUserFill[i + 1]);
        }

        m_InputFieldObject.Select();
    }

    public void CapsKey(GameObject _Keys)
    {
        m_Caps = !m_Caps;
        IsCaps(_Keys);
        CheckIfSpecialButtonsIsOn();
    }

    public void OpenKeyboard()
    {
        m_KeyboardKeys.SetActive(true);
    }

    public void OpenKeyboardWithEmailKeys()
    {
        m_KeyboardKeys.SetActive(true);
        m_KeyboardEmailsKeys.SetActive(true);
    }

    public void CloseKeyboard()
    {
        m_KeyboardKeys.SetActive(false);
    }

    public void CallEmailsKey()
    {
        m_KeyboardEmailsKeys.SetActive(true);
    }

    public void CloseEmailsKeys()
    {
        m_KeyboardEmailsKeys.SetActive(false);
    }

    private void IsCaps(GameObject _Keys)
    {
        for (int i = 22; i < _Keys.transform.childCount; i++)
        {
            if (m_Caps)
                _Keys.transform.GetChild(i).transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = _Keys.transform.GetChild(i).transform.GetChild(0).GetComponent<TextMeshProUGUI>().text.ToUpper();
            else
                _Keys.transform.GetChild(i).transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = _Keys.transform.GetChild(i).transform.GetChild(0).GetComponent<TextMeshProUGUI>().text.ToLower();
        }
    }
    #endregion
}