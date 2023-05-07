using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    private string pi;
    private int cursor;
    private string[] panels_names = { "Menu", "PiDigits", "Setting", "Highscore" };
    private string[] setting_names = { "Guide", "Timer", "Speedrun" };
    private string[,] setting_options = { { "On", "Off", "/" }, { "30s", "60s", "\u221E"}, { "100th", "150th", "200th" } };
    private int[] setting_values = { 0, 0, 0 };
    private int[] setting_values_max = { 1, 2, 2 };
    public Button[] setting_buttons;
    public TextMeshProUGUI[] setting_buttons_texts;
    public TextMeshProUGUI user_input;
    public GameObject[] panels;
    public Button[] keypad_buttons;
    private void Start()
    {
        GameObject.Find("Menu_" + panels_names[1] + "_Toggle").GetComponent<Button>().onClick.AddListener(delegate { open_panel(1); });
        GameObject.Find("Menu_" + panels_names[2] + "_Toggle").GetComponent<Button>().onClick.AddListener(delegate { open_panel(2); });
        GameObject.Find("Menu_" + panels_names[3] + "_Toggle").GetComponent<Button>().onClick.AddListener(delegate { open_panel(3); });
        panels = new GameObject[4];
        keypad_buttons = new Button[12];
        setting_buttons = new Button[3];
        setting_buttons_texts = new TextMeshProUGUI[3];
        pi = "3141592653589793238462643383279502884197169399375105820974944592307816406286208998628034825342117067982148086513282306647093844609550582231725359408128481117450284102701938521105559644622948954930381964428810975665933446128475648233786783165271201909145648566923460348610454326648213393607260249141273724587006606315588174881520920962829254091715364367892590360011330530548820466521384146951941511609433057270365759591953092186117381932611793105118548074462379962749567351885752724891227938183011949129833673362440656643086021394946395224737190702179860943702770539217176293176752384674818467669405132000568127145263560827785771342757789609173637178721468440901224953430146549585371050792279689258923542019956112129021960864034418159813629774771309960518707211349999998372978049951059731732816096318595024459455346908302642522308253344685035261931188171010003137838752886587533208381420617177669147303598253490428755468731159562863882353787593751957781857780532171226806613001927876611195909216420198";
        cursor = 0;
        for (int i = 0; i < 12; i++)
        {
            int copy = i;
            if (i < 4)
            {
                if (i < 3)
                {
                    setting_buttons[i] = GameObject.Find("Setting_" + setting_names[i] + "_Toggle").GetComponent<Button>();
                    setting_buttons_texts[i] = GameObject.Find("Setting_" + setting_names[i] + "_Toggle_Text").GetComponent<TextMeshProUGUI>();
                    setting_buttons[i].onClick.AddListener(delegate { setting_toggle(copy); });
                }
                panels[i] = GameObject.Find(panels_names[i]);
                GameObject.Find(panels_names[i] + "_Exit_Toggle").GetComponent<Button>().onClick.AddListener(delegate { exit_panel(copy); });
                //if (i != 0)
                //{
                //    GameObject.Find("Menu_" + panels_names[i] + "_Toggle").GetComponent<Button>().onClick.AddListener(delegate { open_panel(copy); });
                //}
                panels[i].SetActive(false);
            }
            keypad_buttons[i] = GameObject.Find("Button_" + i).GetComponent<Button>();
            keypad_buttons[i].onClick.AddListener(delegate { keypad_clicked(copy); });
        }
    }

    private void keypad_clicked(int index)
    {
        if (9 <= index && index <= 11)
        {
            check_pidigit(index-8);
        }
        else if (6 <= index && index <= 8)
        {
            check_pidigit(index - 2);
        }
        else if (3 <= index && index <= 5)
        {
            check_pidigit(index + 4);
        }
        else if (index == 2)
        {
            panels[0].SetActive(true);
        }
        else if (index == 1)
        {
            check_pidigit(0);
        }
    }

    private void exit_panel(int index)
    {
        panels[index].SetActive(false);
    }

    private void open_panel(int index)
    {
        panels[index].SetActive(true);
    }

    private void setting_toggle(int index)
    {
        if (setting_values[index] != setting_values_max[index])
        {
            setting_values[index] += 1;
        } else
        {
             setting_values[index] = 0;
        }
        setting_buttons_texts[index].text = setting_options[index, setting_values[index]];
    }

    private void check_pidigit(int input)
    {
        input += 48;
        if (input == pi[cursor])
        {
            user_input.text += pi[cursor];
            if (cursor == 0)
            {
                user_input.text += ".";
            }
            cursor += 1;
        }
        else
        {
            GameOver();
        }
    }

    private void GameOver()
    {
        cursor = 0;
        user_input.text = "";
    }
}
