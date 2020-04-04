using UnityEngine;

[CreateAssetMenu(fileName = "New TextConfig", menuName = "Config/TextConfig", order = 0)]
public class TextConfig : ScriptableObject
{
    [Header("Combat - General")]
    public string fadeInText = "COMBAT!";
    public string fadeOutText = "FINISH";

    [Header("Combat - UI")]
    public string gameWinTxt = "WIN";
    public string gameLoseTxt = "LOSE";
    public string playActionTxt = "EXECUTE";
    public string informationOneText = "{0}: <size=175>{1}</size>";
    public string informationTwoText = "{0}: <size=175>{1}-{2}</size>";
    public string formatStats = "STR {0}\nDEX {1}\nDEF {2}\nAGI {3}\nLCK {4}\nRCT {5}";

    [Header("Combat - Actions")]
    public string actionTypeWeapon = "DAMAGE";
    public string actionTypeItemPlayer = "";
    public string actionTypeItemEnemy = "";
    public string actionTypeDefense = "DEFENSE";

    [Header("Message")]
    public Color colorMsgHeal;
    public Color colorMsgDamage;
    public Color colorMsgDefense;

}