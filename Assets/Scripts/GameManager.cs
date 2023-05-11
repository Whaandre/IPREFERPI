using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System;

public class GameManager : MonoBehaviour
{
    private float timeValue;
    private bool gameOn;
    private string pi;
    private int cursor;
    private float seconds;
    private float[] highscore_values = { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
    private string[] panels_names = { "Menu", "PiDigits", "Setting", "Highscore" };
    private string[] setting_names = {"Keypad","Guide", "Timer", "Speedrun" };
    private string[,] setting_options = { { "Flip", "Flip", "/", "/" }, { "Off", "On", "/", "/" }, { "X", "30s", "60s", "/" }, { "X", "100", "150", "200" } };
    private int[] setting_values = { 0, 0, 0, 0 };
    private int[] setting_values_max = { 1, 1, 2, 3 };
    private float[] timer_values = { 0.0f, 30.0f, 60.0f };
    private int[] keypad_values = { -1, 0, -1, 7, 8, 9, 4, 5, 6, 1, 2, 3 };
    private Button[] setting_buttons;
    private TextMeshProUGUI[] setting_buttons_texts;
    private TextMeshProUGUI[] keypad_buttons_texts;
    private TextMeshProUGUI[] highscore_texts;
    public TextMeshProUGUI user_input_text;
    public TextMeshProUGUI timer_text;
    public TextMeshProUGUI result_text;
    private GameObject[] panels;
    private Button[] keypad_buttons;
    private void Start()
    {
        gameOn = true;
        highscore_texts = new TextMeshProUGUI[12];
        panels = new GameObject[4];
        keypad_buttons = new Button[12];
        keypad_buttons_texts = new TextMeshProUGUI[12];
        setting_buttons = new Button[4];
        setting_buttons_texts = new TextMeshProUGUI[4];
        timer_text.gameObject.SetActive(false);
        pi = "3141592653589793238462643383279502884197169399375105820974944592307816406286208998628034825342117067982148086513282306647093844609550582231725359408128481117450284102701938521105559644622948954930381964428810975665933446128475648233786783165271201909145648566923460348610454326648213393607260249141273724587006606315588174881520920962829254091715364367892590360011330530548820466521384146951941511609433057270365759591953092186117381932611793105118548074462379962749567351885752724891227938183011949129833673362440656643086021394946395224737190702179860943702770539217176293176752384674818467669405132000568127145263560827785771342757789609173637178721468440901224953430146549585371050792279689258923542019956112129021960864034418159813629774771309960518707211349999998372978049951059731732816096318595024459455346908302642522308253344685035261931188171010003137838752886587533208381420617177669147303598253490428755468731159562863882353787593751957781857780532171226806613001927876611195909216420198";
        cursor = 0;
        for (int i = 0; i < 12; i++)
        {
            int copy = i;
            if (i < 4)
            {
                setting_buttons[i] = GameObject.Find("Setting_" + setting_names[i] + "_Toggle").GetComponent<Button>();
                setting_buttons_texts[i] = GameObject.Find("Setting_" + setting_names[i] + "_Toggle_Text").GetComponent<TextMeshProUGUI>();
                setting_buttons[i].onClick.AddListener(delegate { setting_toggle(copy); });
                panels[i] = GameObject.Find(panels_names[i]);
                GameObject.Find(panels_names[i] + "_Exit_Toggle").GetComponent<Button>().onClick.AddListener(delegate { exit_panel(copy); });
                if (i != 0)
                {
                    GameObject.Find("Menu_" + panels_names[i] + "_Toggle").GetComponent<Button>().onClick.AddListener(delegate { open_panel(copy); });
                }
            }
            keypad_buttons[i] = GameObject.Find("Button_" + i).GetComponent<Button>();
            keypad_buttons[i].onClick.AddListener(delegate { keypad_clicked(copy); });
            keypad_buttons_texts[i] = GameObject.Find("Text_" + i).GetComponent<TextMeshProUGUI>();
            highscore_texts[i] = GameObject.Find("Highscore_Value_" + i).GetComponent<TextMeshProUGUI>();
            if (PlayerPrefs.HasKey(i.ToString()))
            {
                highscore_values[i] = PlayerPrefs.GetFloat(i.ToString());
            }
        }
        for (int i = 0; i < 4; i++)
        {
            panels[i].SetActive(false);
        }
    }

    private void Update()
    {
        if (gameOn)
        {
            if (setting_values[2] != 0)
            {
                if (timeValue > 0)
                {
                    timeValue -= Time.deltaTime;
                    seconds = MathF.Floor(timeValue * 10) / 10;
                    timer_text.text = seconds.ToString();
                }
                else
                {
                    GameOver();
                    timeValue = timer_values[setting_values[2]];
                    seconds = MathF.Floor(timeValue * 10) / 10;
                    timer_text.text = seconds.ToString();
                }
            } else if (setting_values[3] != 0)
            {
                timeValue += Time.deltaTime;
                seconds = MathF.Floor(timeValue * 10) / 10;
                timer_text.text = seconds.ToString();
            }
        }
    }
    private void keypad_clicked(int index)
    {
        if (keypad_values[index] != -1)
        {
            check_pidigit(keypad_values[index]);
        }
    }

    private void exit_panel(int index)
    {
        if (index == 0)
        {
            gameOn = true;
        }
        panels[index].SetActive(false);
    }

    private void open_panel(int index)
    {
        gameOn = false;
        panels[index].SetActive(true);
        if (index == 3)
        {
            for (int i = 0; i < 12; i++)
            {
                highscore_texts[i].text = highscore_values[i].ToString();
                if (i % 6 >= 3)
                {
                    highscore_texts[i].text += "s";
                } else
                {
                    highscore_texts[i].text += " digits";
                }
            }
        }
    }

    private void setting_toggle(int index)
    {
        if (setting_values[index] != setting_values_max[index])
        {
            if (index == 2)
            {
                setting_values[3] = 0;
                setting_buttons_texts[3].text = setting_options[3, setting_values[3]];
            } else if (index == 3)
            {
                setting_values[2] = 0;
                setting_buttons_texts[2].text = setting_options[2, setting_values[2]];
            }
            setting_values[index] += 1;
        } else
        {
             setting_values[index] = 0;
        }
        setting_buttons_texts[index].text = setting_options[index, setting_values[index]];
        if (index == 0)
        {
            if (setting_values[1] == 1)
            {
                ColorBlock colors = keypad_buttons[0].colors;
                colors.normalColor = Color.white;
                colors.highlightedColor = new Color32(245, 245, 245, 255);
                keypad_buttons[Array.IndexOf(keypad_values, pi[cursor] - 48)].colors = colors;
            }
            if (setting_values[0] == 0)
            {
                keypad_values = new int[12] { -1, 0, -1, 7, 8, 9, 4, 5, 6, 1, 2, 3 };
            }
            else
            {
                keypad_values = new int[12] { -1, 0, -1, 1, 2, 3, 4, 5, 6, 7, 8, 9 };
            }
            for (int i = 0; i < 12; i++)
            {
                keypad_buttons_texts[i].text = keypad_values[i].ToString();
            }
            if (setting_values[1] == 1)
            {
                ColorBlock colors = keypad_buttons[0].colors;
                colors.normalColor = Color.red;
                colors.highlightedColor = new Color32(255, 100, 100, 255);
                keypad_buttons[Array.IndexOf(keypad_values, pi[cursor] - 48)].colors = colors;
            }
        } else if (index == 1)
        {
            if (setting_values[1] == 0)
            {
                ColorBlock colors = keypad_buttons[0].colors;
                colors.normalColor = Color.white;
                colors.highlightedColor = new Color32(245, 245, 245, 255);
                keypad_buttons[Array.IndexOf(keypad_values, pi[cursor] - 48)].colors = colors;
            } else
            {
                ColorBlock colors = keypad_buttons[0].colors;
                colors.normalColor = Color.red;
                colors.highlightedColor = new Color32(255, 100, 100, 255);
                keypad_buttons[Array.IndexOf(keypad_values, pi[cursor] - 48)].colors = colors;
            }
        }
        else if (index == 2)
        {
            if (setting_values[2] == 0)
            {
                timer_text.gameObject.SetActive(false);
            }
            else
            {
                timer_text.gameObject.SetActive(true);
            }
            timeValue = timer_values[setting_values[2]];
            seconds = MathF.Floor(timeValue * 10) / 10;
            timer_text.text = seconds.ToString();
        }
        else if (index == 3)
        {
            if (setting_values[3] == 0)
            {
                timer_text.gameObject.SetActive(false);
            }
            else
            {
                timer_text.gameObject.SetActive(true);
                timeValue = 0;
                seconds = MathF.Floor(timeValue * 10) / 10;
                timer_text.text = seconds.ToString();
            }
        }
    }

    private void check_pidigit(int input)
    {
        input += 48;
        if (input == pi[cursor])
        {
            if (setting_values[1] == 1)
            {
                ColorBlock colors = keypad_buttons[0].colors;
                colors.normalColor = Color.white;
                colors.highlightedColor = new Color32(245, 245, 245, 255);
                keypad_buttons[Array.IndexOf(keypad_values, pi[cursor] - 48)].colors = colors;
                colors.normalColor = Color.red;
                colors.highlightedColor = new Color32(255, 100, 100, 255);
                keypad_buttons[Array.IndexOf(keypad_values, pi[cursor + 1] - 48)].colors = colors;
            }
            if (setting_values[3] != 0)
            {
                if (int.Parse(setting_options[3, setting_values[3]]) == cursor + 1)
                {
                    GameOver();
                }
            }
            user_input_text.text += pi[cursor];
            if (cursor == 0)
            {
                user_input_text.text += ".";
            }
            cursor += 1;
        }
        else
        {
            if (setting_values[2] == 0 && setting_values[3] == 0)
            {
                GameOver();
            }
        }
    }
    private void GameOver()
    {
        if (setting_values[1] == 1)
        {
            ColorBlock colors = keypad_buttons[0].colors;
            colors.normalColor = Color.white;
            colors.highlightedColor = new Color32(245, 245, 245, 255);
            keypad_buttons[Array.IndexOf(keypad_values, pi[cursor] - 48)].colors = colors;
        }
        gameOn = false;
        panels[0].SetActive(true);
        result_text.text = "Your Score:\n";
        if (setting_values[2] == 0 && setting_values[3] != 0)
        {
            if (highscore_values[setting_values[1] * 6 + setting_values[3] + 2] > seconds)
            {
                highscore_values[setting_values[1] * 6 + setting_values[3] + 2] = seconds;
                PlayerPrefs.SetFloat((setting_values[1] * 6 + setting_values[3] + 2).ToString(), seconds);
            }
            result_text.text += seconds.ToString() + "s";
        }
        else
        {
            if (highscore_values[setting_values[1] * 6 + setting_values[2]] < cursor)
            {
                highscore_values[setting_values[1] * 6 + setting_values[2]] = cursor;
                PlayerPrefs.SetFloat((setting_values[1] * 6 + setting_values[2]).ToString(), cursor);
            }
            result_text.text += cursor.ToString() + " digits";
        }
        cursor = 0;
        user_input_text.text = "";
        timeValue = timer_values[setting_values[2]];
        seconds = MathF.Floor(timeValue * 10) / 10;
        timer_text.text = seconds.ToString();
        if (setting_values[1] == 1)
        {
            ColorBlock colors = keypad_buttons[0].colors;
            colors.normalColor = Color.red;
            colors.highlightedColor = new Color32(255, 100, 100, 255);
            keypad_buttons[Array.IndexOf(keypad_values, pi[cursor] - 48)].colors = colors;
        }
    }
}
