using UnityEngine;
using System.Collections;

// Class for storing the rainbow colors to be used system wide
public class RainbowColors : MonoBehaviour
{

    public static RainbowColors singleton;

    //Singleton
    void Awake() {
        if (singleton != null) {
            Object.Destroy(singleton);
        } else {
            singleton = this;
        }
    }

    public Color32 RainbowRed;
    public Color32 RainbowOrange;
    public Color32 RainbowYellow;
    public Color32 RainbowGreen;
    public Color32 RainbowBlue;
    public Color32 RainbowViolet;
    public Color32 RainbowBlack;
    public Color32 RainbowWhite;

	public Color32 SkyboxLightMode;
	public Color32 SkyboxDarkMode;

	public Color32 GroundSegmentLight;
	public Color32 GroundSegmentDark;

	public Color32 MainMenuButtonsColorLight;
	public Color32 MainMenuButtonsColorDark;

	public Color32 MainMenuDarkerGrayLight;
	public Color32 MainMenuDarkerGrayDark;

	public Color32 PolyKromosTitleTextLight;
	public Color32 PolyKromosTitleTextDark;

	public Color32 MainMenuTextDark;
    public Color32 MainMenuTextLight;

    public Color32 getRed() { return RainbowRed; }
    public Color32 getOrange() { return RainbowOrange; }
    public Color32 getYellow() { return RainbowYellow; }
    public Color32 getGreen() { return RainbowGreen; }
    public Color32 getBlue() { return RainbowBlue; }
    public Color32 getViolet() { return RainbowViolet; }
    public Color32 getBlack() { return RainbowBlack; }
    public Color32 getWhite() { return RainbowWhite; }

	public Color32 getSkyboxLight() { return SkyboxLightMode; }
	public Color32 getSkyboxDark() { return SkyboxDarkMode; }

	public Color32 getGroundSegmentLight() { return GroundSegmentLight; }
	public Color32 getGroundSegmentDark() { return GroundSegmentDark; }

	public Color32 getMainMenuButtonsColorLight() { return MainMenuButtonsColorLight; }
	public Color32 getMainMenuButtonsColorDark() { return MainMenuButtonsColorDark; }

	public Color32 getPolyKromosTitleTextLight() { return PolyKromosTitleTextLight; }
	public Color32 getPolyKromosTitleTextDark() { return PolyKromosTitleTextDark; }

	public Color32 getMainMenuDarkerGrayLight() { return MainMenuDarkerGrayLight; }
	public Color32 getMainMenuDarkerGrayDark() { return MainMenuDarkerGrayDark; }

	public Color32 getMainMenuTextDark() {	return MainMenuTextDark; }
    public Color32 getMainMenuTextLight() { return MainMenuTextLight; }

    public Color32 getColorFromString(string color) {
        if (color == "Red") return getRed();
        if (color == "Orange") return getOrange();
        if (color == "Yellow") return getYellow();
        if (color == "Green") return getGreen();
        if (color == "Blue") return getBlue();
        if (color == "Violet") return getViolet();
        if (color == "White") return getWhite();
        if (color == "Black") return getBlack();
        return getBlack();
    }


}
