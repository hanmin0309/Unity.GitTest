using UnityEngine;
using UnityEngine.TextCore.Text;
using UnityEngine.UI;

[System.Serializable]
public class CharacterUI
{
    public Text playerHealthTxt;
    public Text playerAmmoTxt;
    public Text playerCoinTxt;

    public Image weapon1Img;
    public Image weapon2Img;
    public Image weapon3Img;
    public Image weapon4Img;
    public Image weapon5Img;
    public Image weaponRImg;

    public Character character; // Character script reference, if you have one
}
