using System;
using System.Diagnostics;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

/*
using System.Of[A].Down;                                            */

namespace RubikCube
{
    class TextBox
    {
        public string Textbox = ""; // the entirety of what u wrote
        public string DrawBox = ""; // what u see on screen
        int drawTab = 0; // physical place of tab in vector.x
        string physTab = ""; // the flashing tab
        public int MovedTo; // how much u moved to the right
        int timeSincePress; // time passed since last click
        int timeSinceLetterPress;//time passed since you started pressing a letter
        int tabTimer; //timer of the tab
        Keys oldKey = Keys.F24; //old key last pressed
        public int BoxSize = 20; //the boxsize in letters, not in vector.x!!! this size changes depending on the string vector.x size!
        public const int RealBoxSize = 266; //the real box size in vectors.x, const!
        public int TabPlace; //the logical place of the tab in the string
        public bool FocusTextBox; //Should the game focus on the textBox, and type in it.
        public Rectangle TextBoxRect; //Rectangle around the textBox to find out when the user clicks on it
        public Texture2D GraphicBox; //The image of the bordors of the textBox
        int alpha; //The brightness of the bordors of the textbox
        Cube cube;
        Camera camera;
        string algOrder; //The orders of the algoritems
        bool startedRot; //Has the cube started to apply the algoritems from the textBox. 
        string realVectorBox = ""; //The textbox with vector-algo
        int oldAlgoLength; //the old length of algOrder
        bool didStart; //Has the textBox started to send it's algo
        MouseState oldMouseState;

        /// <summary>
        /// A Text Box for writing algorithems in it, and applying them on the cube.
        /// </summary>
        /// <param name="_cube">The Cube</param>
        /// <param name="content">The texture of the border of the TextBox</param>
        public TextBox(Cube _cube, ContentManager content)
        {
            //loading the image and stuff of the textBox
            GraphicBox = content.Load<Texture2D>("pics/box2");
            TextBoxRect = new Rectangle(300, 375, 266, 20);
            cube = _cube;
            //calling cube to use it
        }
        /// <summary>
        /// Updates and does all the logical calculations of the TextBox, decides what to draw, and where to put the Tab |
        /// </summary>
        /// <param name="state">The state of the Keyboard</param>
        /// <param name="oldState">The old state of the Keyboard</param>
        /// <param name="gameTime">Time since the Game started running</param>
        /// <param name="mono">The Mono-Space font</param>
        /// <param name="algorder">The order of the algorithem</param>
        /// <param name="_camera">The Camera</param>
        /// <param name="_cube">The Cube</param>
        public void Update(KeyboardState state, KeyboardState oldState, GameTime gameTime, SpriteFont mono, string algorder, Camera _camera, Cube _cube)
        {
            //check
            cube = _cube;
            MouseState mouse = Mouse.GetState();
            Point mousePos = new Point(mouse.X, mouse.Y);
            if (mouse.LeftButton == ButtonState.Pressed &&
                oldMouseState.LeftButton == ButtonState.Released) //If the user click on the textBox, focus on it
            {
                if (TextBoxRect.Contains(mousePos))
                {
                    FocusTextBox = true;
                }
                else
                    FocusTextBox = false;
            }
            camera = _camera;
            algOrder = algorder;
            if (Textbox.Length == 0) //Stop rotating, if it's rotating
                startedRot = false;
            if ((algOrder.Length != oldAlgoLength) && (EnterPressed || didStart)) //Should it delete from the algo if the cube is rotating by the textBox
            {
                didStart = true;
                if (Textbox.Length == 1)
                {
                    Textbox = "";
                    TabPlace = 0;
                }
                else if (Textbox.Length > 0)
                {
                    Textbox = Textbox.Substring(1, Textbox.Length - 2);
                    TabPlace = 0;
                }
                if (Textbox.Length == 0)
                    didStart = false;
                oldAlgoLength = algOrder.Length;
            }
            //34
            CheckForDeviation(mono); //checks length of the text, considering the font
            tabTimer += gameTime.ElapsedGameTime.Milliseconds % 1000; //Timer of the Tab, decides if it should flash or not.
            //(Keys)(Enum.Parse(typeof(Keys), "A"));
            string usedKeys = "ABCDEFGHIJKLMNOPQRSTUVWXYZ"; //All the keys that you can type to the textBox

            if (FocusTextBox)
            {
                for (int i = 0; i < usedKeys.Length; i++) ///
                {
                    CheckForClick(ref state, ref oldState, gameTime, (Keys)(Enum.Parse(typeof(Keys), usedKeys.Substring(i, 1)))); //converts string letter from usedKeys to Keys, and send to cheak.
                }
                CheckForClick(ref state, ref oldState, gameTime, Keys.Space); //checks if the Space bar is pressed 
                CheckForClick(ref state, ref oldState, gameTime, Keys.OemSemicolon); //This is not the right char 


                if (state.IsKeyDown(Keys.Enter) && oldState.IsKeyUp(Keys.Enter)) //If enter has been pressed, call StartAlgo to send the command 
                {
                    StartAlgo();
                    //textbox = textbox.Insert(movedTo + tabPlace, "ABCDEFGHIJKLMNOPQRSTUVWXYZ");
                }
                else
                {
                    EnterPressed = false;
                }
            }
            GetRealVectorBox = realVectorBox;
            //checks if one of the keys that count how much time has passed since you pressed them have been...um...Un-pressed?
            if ((CheakForKeyChange(state, oldState, Keys.Right)) || (CheakForKeyChange(state, oldState, Keys.Left)) || (CheakForKeyChange(state, oldState, Keys.Back)) || (CheakForKeyChange(state, oldState, Keys.Space)))
            {
                timeSincePress = 0;
                oldKey = Keys.F24;
                tabTimer = 0;
            }
            if (CheakForKeyChange(state, oldState, oldKey))
            {
                timeSinceLetterPress = 0;
                oldKey = Keys.F24;
                tabTimer = 0;
            }

            if ((state.IsKeyDown(Keys.Back)) && (Textbox.Length > 0)) //Code regarding the backspace button, and deleting text
            {
                timeSincePress += gameTime.ElapsedGameTime.Milliseconds;
                if ((oldState.IsKeyUp(Keys.Back) || (timeSincePress > 250)))
                {
                    if (timeSincePress > 250)
                    {
                        timeSincePress = 225;
                    }
                    if ((TabPlace + MovedTo) > 0)
                    {
                        if (Textbox.Length <= BoxSize)
                        {
                            TabPlace--;
                        }
                        else if ((MovedToRight() > 0 && (MovedTo > 0)))
                        {
                            if (TabPlace > 0)
                            {
                                TabPlace--;
                            }
                            else
                            {
                                MovedTo--;
                            }
                        }
                        else if (MovedToRight() > 0 && (MovedTo == 0))
                        {
                            TabPlace--;
                        }
                        else if ((MovedToRight() == 0) && (MovedTo > 0))
                        {
                            MovedTo--;
                        }
                        Textbox = Textbox.Remove(MovedTo + TabPlace, 1);
                        tabTimer = 0;
                    }

                }
            }
            else if (state.IsKeyDown(Keys.Right))//Code regarding the right arrow button, and moving the tab cursor
            {
                timeSincePress += gameTime.ElapsedGameTime.Milliseconds;
                if ((oldState.IsKeyUp(Keys.Right) || (timeSincePress > 250)))
                {
                    if (!((TabPlace == BoxSize) && (MovedToRight() == 0)))
                    {
                        if (timeSincePress > 250)
                        {
                            timeSincePress = 225;
                        }
                        if ((TabPlace == BoxSize) && (MovedToRight() > 0))
                        {
                            MovedTo++;
                        }
                        else if ((TabPlace < BoxSize) && (Textbox.Length - TabPlace > 0))
                        {
                            TabPlace++;
                        }
                        tabTimer = 0;
                    }
                }
            }
            else if (state.IsKeyDown(Keys.Left))//Code regarding the left arrow button, and moving the tab cursor
            {
                timeSincePress += gameTime.ElapsedGameTime.Milliseconds;
                if ((oldState.IsKeyUp(Keys.Left) || (timeSincePress > 250)))
                {
                    if (MovedTo + TabPlace > 0)
                    {
                        if (timeSincePress > 250)
                        {
                            timeSincePress = 225;
                        }
                        if (TabPlace > 0)
                        {
                            TabPlace--;
                        }
                        if ((TabPlace == 0) && (MovedTo > 0))
                        {
                            MovedTo--;
                        }
                        tabTimer = 0;
                    }
                }
            }
            //Decides what part of the text box should be visible on screen
            if (Textbox.Length > BoxSize && (MovedTo + BoxSize + MovedToRight()) == Textbox.Length)
            {
                DrawBox = Textbox.Substring(MovedTo, BoxSize);
            }
            else
            {
                DrawBox = Textbox;
            }

            CheckForDeviation(mono); //Checks how to crop the box depending on the font.
            //Re-defies what part of the text box should be visible on screen, after CheckForDeviation
            if (Textbox.Length > BoxSize && (MovedTo + BoxSize + MovedToRight()) == Textbox.Length) 
            {
                DrawBox = Textbox.Substring(MovedTo, BoxSize);
            }
            else
            {
                DrawBox = Textbox;
            }
            if (FocusTextBox)
            {
                //Decides if the tab cursor should flash or not
                if (tabTimer < 500)
                {
                    physTab = "|";
                }
                else
                {
                    physTab = "";
                }
            }
            else
            {
                physTab = "";
            }
            if (tabTimer >= 1000)
            {
                tabTimer = 0;
            }
            oldState = state; //Defines the old state of the keyboard

            //END OF UPDATE
        }
        /// <summary>
        /// When called, clears all non-algorithm related chars from textbox, and sends the right command of the algorith
        /// </summary>
        private void StartAlgo()
        {
            oldAlgoLength = algOrder.Length;
            realVectorBox = "";
            string nono = " ACEGHJKMNOPQSTVWXYZ";
            TabPlace = 0; //sets the place of the the tab cursor to 0 to avoid problems 
            for (int i = 0; i < nono.Length; i++) //clears all non-algorithm related chars
            {
                Textbox = Textbox.Replace(nono[i].ToString(), "");
            }
            MovedTo = 0;
            MovedToRight();
            TabPlace = 0;
            //Adds the right letter based on the angle of the camera to realVectorBox
            foreach (var s in Textbox) 
            {

                if (s == 'F')
                {
                    realVectorBox += VectorToChar(camera.RealForward);
                }
                else if (s == 'U')
                {
                    realVectorBox += "U";
                }
                else if (s == 'D')
                {
                    realVectorBox += "D";
                }
                else if (s == 'B')
                {
                    realVectorBox += VectorToChar(camera.RealBackward);
                }
                else if (s == 'L')
                {
                    realVectorBox += VectorToChar(camera.RealLeft);
                }
                else if (s == 'R')
                {
                    realVectorBox += VectorToChar(camera.RealRight);
                }
                else if (s == 'I')
                {
                    realVectorBox += "I";
                }
            }
<<<<<<< HEAD
            int limit = realVectorBox.Length;
            //replace any double ii
            for (int i = 0; i < limit; i++)
            {
                realVectorBox = realVectorBox.Replace("II", "I");
            }

            //removes any useless i
            if (realVectorBox.Length > 0)
            {
                if (realVectorBox[0] == 'I')
                {
                    realVectorBox = realVectorBox.Substring(1, realVectorBox.Length - 1);
                }
            }

=======
            //If the length of algOrder is 0, change the state of the next bools.
>>>>>>> origin/Tamir's-branch
            if (algOrder.Length == 0)
            {
                EnterPressed = true;
                startedRot = true;
            }
        }
        
        /// <summary>
        /// Gets and Sets the state of GetRealVectorBox so it could be used outisde of the class TextBox
        /// </summary>
        public string GetRealVectorBox { get; set; }

        /// <summary>
        /// Gets and Sets the state of EnterPressed so it could be used outisde of the class TextBox
        /// </summary>
        public bool EnterPressed { get; set; }
        /// <summary>
        /// checks length of the text, considering the font
        /// </summary>
        /// <param name="mono"></param>
        private void CheckForDeviation(SpriteFont mono)
        {
            //Measures the vector length of the etxt in DrawBox using the mono-space font
            Vector2 boxVector = (mono.MeasureString(DrawBox));
            if (boxVector.X >= RealBoxSize)
            {
                int check = 1;
                //While the vector2 size of DrawBox using the mono-space font is not smaller then RealBoxSize
                while ((mono.MeasureString(DrawBox.Substring(0, DrawBox.Length - check)).X) >= RealBoxSize)
                {
                    if (TabPlace >= BoxSize)
                    {
                        TabPlace--;
                    }
                    BoxSize--;
                    MovedTo++;
                    check++;
                }
            }
            else if ((DrawBox.Length >= BoxSize))
            {
                BoxSize++;
            }
        }
        /// <summary>
        /// Acts as a sort of Var, updates depending on the state of MovedTo, when called
        /// </summary>
        /// <returns></returns>
        private int MovedToRight()
        {
            //The amount of chars outside from the right are depended on the amount of chars outisde from the left, and on the size of the box
            if ((Textbox.Length - BoxSize - MovedTo) <= 0)
            {
                return 0;
            }
            return Textbox.Length - BoxSize - MovedTo;
        }

        /// <summary>
        /// Checks for clicks of char keys, adds chars to the textbox depending on what was pressed
        /// </summary>
        /// <param name="keyboardState"></param>
        /// <param name="oldKeyboardState"></param>
        /// <param name="gameTime"></param>
        /// <param name="key"></param>
        private void CheckForClick(ref KeyboardState keyboardState, ref KeyboardState oldKeyboardState, GameTime gameTime, Keys key)
        {
            if (keyboardState.IsKeyDown(key) && (keyboardState.IsKeyUp(Keys.LeftControl)) && (keyboardState.IsKeyUp(Keys.RightControl)))
            {
                //This code is for when you press and hold a key
                timeSinceLetterPress += gameTime.ElapsedGameTime.Milliseconds;
                if (key != oldKey)
                {
                    timeSinceLetterPress = 0;
                }
                oldKey = key;
                if ((oldKeyboardState.IsKeyUp(key)) || (timeSinceLetterPress > 250))
                {
                    tabTimer = 0;
                    if (timeSinceLetterPress > 250)
                    {
                        timeSinceLetterPress = 225;
                    }
                    //Adds the chars pressed to the textbox
                    Textbox = Textbox.Insert(MovedTo + TabPlace, KeyToChar(key, keyboardState, oldKeyboardState));
                    //Decides if it should change the value of Tab place and MovedTo
                    if (TabPlace < BoxSize)
                    {
                        TabPlace++;
                    }
                    else
                    {
                        TabPlace = BoxSize;
                        MovedTo++;
                    }
                }
            }
        }
        /// <summary>
        /// Checks for change between char keys, used for adding the press-and-hold affect for pressing char keys
        /// </summary>
        /// <param name="keyboardState"></param>
        /// <param name="oldKeyboardState"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        private bool CheakForKeyChange(KeyboardState keyboardState, KeyboardState oldKeyboardState, Keys key)
        {
            if (((keyboardState.IsKeyUp(key)) && (oldKeyboardState.IsKeyDown(key))) || (keyboardState.IsKeyDown(key) && oldKeyboardState.IsKeyUp(key)))
            {
                return true;
            }
            return false;
        }
        /// <summary>
        /// Returns a char depending on the Vector3, to be used in the algorithm
        /// </summary>
        /// <param name="real"></param>
        /// <returns></returns>
        public string VectorToChar(Vector3 real)
        {
            if (real == Vector3.Left)
            {
                return "L";
            }
            if (real == Vector3.Right)
            {
                return "R";
            }
            if (real == Vector3.Backward)
            {
                return "B";
            }
            if (real == Vector3.Forward)
            {
                return "F";
            }
            if (real == Vector3.Up)
            {
                return "U";
            }
            if (real == Vector3.Down)
            {
                return "D";
            }
            return "";

        }
        /// <summary>
        /// Returns a vector3 depending on the char, to be used when working with the algorithm
        /// </summary>
        /// <param name="real"></param>
        /// <returns></returns>
        public Vector3 CharToVector(string real)
        {
            if ((real == "l") || (real == "L"))
            {
                return Vector3.Left;
            }
            if ((real == "r") || (real == "R"))
            {
                return Vector3.Right;
            }
            if ((real == "b") || (real == "B"))
            {
                return Vector3.Backward;
            }
            if ((real == "f") || (real == "F"))
            {
                return Vector3.Forward;
            }
            if ((real == "u") || (real == "U"))
            {
                return Vector3.Up;
            }
            if ((real == "d") || (real == "D"))
            {
                return Vector3.Down;
            }
            return Vector3.Zero;
        }
        /// <summary>
        /// Draws the text box on screen, as well as the tab cursor, and the image of the borders of the text box
        /// </summary>
        /// <param name="spriteBatch">The spriteBatch</param>
        /// <param name="mono">Mono-space font</param>
        /// <param name="font">Regular font</param>
        /// <param name="tex">The textre of the borders of the text box</param>
        public void Draw(SpriteBatch spriteBatch, SpriteFont mono, SpriteFont font, Texture2D tex)
        {
            //265
            //Measures the vector size of DrawBox from start to the place of the tab cursor
            Vector2 tabVector = (mono.MeasureString(DrawBox.Substring(0, TabPlace)));
            //takes only the X size of the vector, and changes the X size a bit
            tabVector = new Vector2((tabVector.X + 295), (375));

            spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend);
            //If the cube is currently rotating from the textbox, draws rectangles behind the text, in colors depending on what part of the algorithm is currently at use
            if (startedRot)
            {
                bool shouldMakeBothGreen = false;
                for (int i = 0; i < DrawBox.Count(); i++)
                {
                    if (i == 0)
                    {
                        if (DrawBox.Length > 1)
                        {
                            //If the second char is ' or I, it should be made green as well
                            if ((DrawBox[1] == 'I' || DrawBox[1] == '\'') && (DrawBox[0] != 'I' && DrawBox[0] != '\''))
                            {
                                shouldMakeBothGreen = true;
                                spriteBatch.Draw(tex, new Rectangle(300 + 11, 375, 11, (int)mono.MeasureString(DrawBox[1].ToString()).Y), null,
                                    //Draws the algorithm currently at use in green
                               Color.LimeGreen, 0f, new Vector2(0, 0), SpriteEffects.None, 0.3f);
                            }
                        }
                        spriteBatch.Draw(tex, new Rectangle(300, 375, 11, (int)mono.MeasureString(DrawBox[i].ToString()).Y), null,
                            //Draws the algorithm currently at use in green
                           Color.LimeGreen, 0f, new Vector2(0, 0), SpriteEffects.None, 0.3f);
                    }
                    else if (!(i == 1 && shouldMakeBothGreen))
                    {
                        spriteBatch.Draw(tex, new Rectangle(300 + (i) * 11, 375, 11, (int)mono.MeasureString(DrawBox[i].ToString()).Y), null,
                            //Draws the algorithm that's not currently at use in gray
                            Color.LightGray, 0f, new Vector2(0, 0), SpriteEffects.None, 0.3f);
                    }
                }
            }
            //If the textbox is on focus, meaning the user is using it, change the Alpha of the borders of the textbox
            if (FocusTextBox)
                alpha = 250;
            else
                alpha = 225;
            //Draws the text inside the textBox, the tab cursor, and other text-related string
            spriteBatch.DrawString(font, "Enter your algorithm here:", new Vector2(300, 350), Color.Black);
            spriteBatch.DrawString(font, ("|"), new Vector2(300 + RealBoxSize, 375), Color.Red);
            spriteBatch.DrawString(mono, (DrawBox), new Vector2(300, 375), Color.Black, 0f, new Vector2(0, 0), 1f, SpriteEffects.None, 0.5f);
            spriteBatch.DrawString(font, (physTab), tabVector, Color.Black);
            spriteBatch.Draw(GraphicBox, new Rectangle(300 - 12, 375, 266 + 30, 30), new Color(255, 255, 255, alpha));

            spriteBatch.End();
        }
        /// <summary>
        /// Returns a char based on the key sent, to be used when typing inside the textbox from keys
        /// </summary>
        /// <param name="key"></param>
        /// <param name="state"></param>
        /// <param name="oldstate"></param>
        /// <returns></returns>
        public string KeyToChar(Keys key, KeyboardState state, KeyboardState oldstate)
        {

            if (key == Keys.Space)
            {
                return " ";
            }
            if (key == Keys.OemSemicolon)
            {
                return "\'";
            }
            if (key.ToString().Length == 1)
            {
                return (key.ToString());
            }

            return "";
        }
    }
}
