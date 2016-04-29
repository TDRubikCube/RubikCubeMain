namespace RubikCube
{
    /// <summary>
    /// Responsible for all text shown in the program, and showing the text in the right language
    /// </summary>
    class Text
    {
        //List of diffrent strings for diffrent lines of text in the game
        public string MainTitle;
        public string OptionsTitle;
        public string OptionsAddMusic;
        public string FreePlayTitle;
        public string TutorialTitle;
        public string TutorialFreeText;
        public string TutorialFreeText2;
        public string FreePlayScramble;
        public string FreePlaySolve;
        public string FreePlayStopperShow;
        public string FreePlayStopperPause;
        public string FreePlayStopperResume;
        public string FreePlayStopperReset;

        /// <summary>
        /// Constracts the Text
        /// </summary>
        public Text()
        {
            //English is the default language
            English();
        }

        /// <summary>
        /// List of all the diffrent text shown in the game, in the English language, and puts it in its matching string
        /// </summary>
        public void English()
        {
            MainTitle = "Rubik's Cube -  Main Menu";
            FreePlayTitle = "Rubik's Cube -  Free Play";
            OptionsTitle = "Rubik's Cube -  Settings";
            TutorialTitle = "Rubik's Cube -  Tutorial";
            OptionsAddMusic = "Add Music";
            TutorialFreeText = "Use the L,R,U,D,F,B keys to turn the cube";
            TutorialFreeText2 = "Hold Shift to turn the cube counter-clockwise";
            FreePlayScramble = "Scramble";
            FreePlaySolve = "Reset!";
            FreePlayStopperShow = "Show/Hide Stopper";
            FreePlayStopperPause = "Pause Stopper";
            FreePlayStopperResume = "Resume Stopper";
            FreePlayStopperReset = "Reset Stopper";
        }
        /// <summary>
        /// List of all the diffrent text shown in the game, in the Hebrew language, and puts it in its matching string
        /// </summary>
        public void Hebrew()
        {
            //The text is written in reverse, and with spaces between each char, in order for it to show properly in the game
            MainTitle = "י ש א ר  ט י ר פ ת - ת י ר ג נ ו ה  ה י ב ו ק";
            FreePlayTitle = "י ש פ ו ח  ק ח ש מ - ת י ר ג נ ו ה  ה י ב ו ק";
            OptionsTitle = "ת ו ר ד ג ה - ת י ר ג נ ו ה  ה י ב ו ק";
            TutorialTitle = "ה כ ר ד ה - ת י ר ג נ ו ה  ה י ב ו ק";
            OptionsAddMusic = "ה ק י ס ו מ  ף ס ו ה";
            TutorialFreeText = "ה י ב ו ק ה  ת א  ז י ז ה ל  י ד כ  L,R,U,D,F,B  ם י ש ק מ ה  ל ע  ץ ח ל";
            TutorialFreeText2 = "ן ו ע ש ה  ן ו ו כ  ד ג נ  ה י ב ו ק ה  ת א  ב ב ו ס ל  י ד כ  Shift ת א  ק ז ח ה";
            FreePlayScramble = "ב ב ר ע";
            FreePlaySolve = "! ס פ א";
            FreePlayStopperShow = "ר פ ו ט ס  ה א ר ה / ר ת ס ה";
            FreePlayStopperPause = "ר פ ו ט ס  ק ס פ ה";
            FreePlayStopperResume = "ר פ ו ט ס  ך ש מ ה";
            FreePlayStopperReset = "ר פ ו ט ס  ס פ א";
        }
        /// <summary>
        /// List of all the diffrent text shown in the game, in the Russian language, and puts it in its matching string
        /// </summary>
        public void Russian()
        {
            //The text is written with spaces between each char, in order for it to show properly in the game
            MainTitle = "К у б и к  Р у б и к а   -   Г л а в н о е  M е н ю";
            FreePlayTitle = "К у б и к  Р у б и к а  -   С в о б о д н а я  И г р а";
            OptionsTitle = "К у б и к  Р у б и к а  -   Н а с т р о й к и";
            TutorialTitle = "К у б и к  Р у б и к а  -   У ч е б н и к";
            OptionsAddMusic = "Д о б а в и т ь  м у з ы к у";
            TutorialFreeText = "И с п о л ь з о в а т ь   L,R,U,D,F,B к л а в и ш и  ч т о б ы  п о в е р н у т ь  к у б и к а";
            TutorialFreeText2 = "Д е р ж а т ь  Shift  ч т о б ы  п о в е р н у т ь  к у б и к а  п р о т и в  ч а с о в о й  с т р е л к и";
            FreePlayScramble = "М и ш а т ь";
            FreePlaySolve = "О ч и с т и т ь!";
            FreePlayStopperShow = "П о к а з а т ь / у б р а т ь  ч а с ы";
            FreePlayStopperPause = "О с т а н о в и т ь  ч а с ы";
            FreePlayStopperResume = "В к л ю ч и т ь  ч а с ы";
            FreePlayStopperReset = "А н у л и р о в а т ь  ч а с ы";
        }
    }
}
