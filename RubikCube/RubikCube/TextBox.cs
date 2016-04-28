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
        public bool FocusTextBox;
        public Rectangle TextBoxRect;
        public Texture2D GraphicBox;
        int alpha;
        Cube cube;
        Camera camera;
        string algOrder;
        bool startedRot;
        string realVectorBox = "";
        int orderLength;
        bool didStart;
        MouseState oldMouseState;

        public TextBox(Cube _cube, ContentManager content)
        {
            GraphicBox = content.Load<Texture2D>("pics/box2");
            TextBoxRect = new Rectangle(300, 375, 266, 20);
            cube = _cube;
        }
        public void Update(KeyboardState state, KeyboardState oldState, GameTime gameTime, SpriteFont mono, string algorder, Camera _camera, Cube _cube)
        {
            //check
            cube = _cube;
            MouseState mouse = Mouse.GetState();
            Point mousePos = new Point(mouse.X, mouse.Y);
            if (mouse.LeftButton == ButtonState.Pressed &&
                oldMouseState.LeftButton == ButtonState.Released)
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
            if (state.IsKeyDown(Keys.D1) && oldState.IsKeyUp(Keys.D1))
                Debug.WriteLine("");
            if (DrawBox.Count() == 0)
                startedRot = false;
            if ((algOrder.Length != orderLength) && (EnterPressed || didStart))
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
                orderLength = algOrder.Length;
            }
            //34
            CheckForDeviation(mono);
            tabTimer += gameTime.ElapsedGameTime.Milliseconds % 1000;
            //(Keys)(Enum.Parse(typeof(Keys), "A"));
            string usedKeys = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
            if (state.IsKeyDown(Keys.P) && oldState.IsKeyUp(Keys.P))
            {
                StartAlgo();
            }

            if (FocusTextBox)
            {
                for (int i = 0; i < usedKeys.Length; i++)
                {
                    CheckForClick(ref state, ref oldState, gameTime, (Keys)(Enum.Parse(typeof(Keys), usedKeys.Substring(i, 1)))); //converts string letter from usedKeys to Keys, and send to cheak.
                }
                CheckForClick(ref state, ref oldState, gameTime, Keys.Space);
                CheckForClick(ref state, ref oldState, gameTime, Keys.OemSemicolon); //nope


                if (state.IsKeyDown(Keys.Enter) && oldState.IsKeyUp(Keys.Enter))
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

            if ((state.IsKeyDown(Keys.Back)) && (Textbox.Length > 0))
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
                    else
                    {
                        Console.Beep();
                    }

                }
            }
            else if (state.IsKeyDown(Keys.Right))
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
                    else
                    {
                        Console.Beep();
                    }
                }
            }
            else if (state.IsKeyDown(Keys.Left))
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
                    else
                    {
                        Console.Beep();
                    }

                }
            }
            if (Textbox.Length > BoxSize && (MovedTo + BoxSize + MovedToRight()) == Textbox.Length)
            {
                DrawBox = Textbox.Substring(MovedTo, BoxSize);
            }
            else
            {
                DrawBox = Textbox;
            }

            //}
            //else if (drawBox.Length < (boxSize - 1))
            //{
            //    if (mono.MeasureString(textbox.Substring(movedTo, drawBox.Length + 1)).X <= realBoxSize)
            //    {
            //        boxSize++;
            //    }

            //}
            CheckForDeviation(mono);

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
            oldState = state;

            //END OF UPDATE
        }
        private void StartAlgo()
        {
            didStart = false;
            orderLength = algOrder.Length;
            realVectorBox = "";
            string nono = " ACEGHJKMNOPQSTVWXYZ";
            TabPlace = 0;
            for (int i = 0; i < nono.Length; i++)
            {
                Textbox = Textbox.Replace(nono[i].ToString(), "");
            }
            MovedTo = 0;
            MovedToRight();
            TabPlace = 0;
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

            if (algOrder.Length == 0)
            {
                EnterPressed = true;
                startedRot = true;
            }
            Debug.WriteLine("Somthing's in the way");
        }
        public string GetRealVectorBox { get; set; }
        public bool EnterPressed { get; set; }
        private void CheckForDeviation(SpriteFont mono)
        {
            Vector2 boxVector = (mono.MeasureString(DrawBox));
            if (boxVector.X >= RealBoxSize)
            {
                if (DrawBox.Length > 0)
                {
                    if (DrawBox.Last() == 'M')
                        Debug.WriteLine("");
                }
                int check = 1;
                Debug.WriteLine((mono.MeasureString(DrawBox.Substring(0, DrawBox.Length - check)).X));
                while ((mono.MeasureString(DrawBox.Substring(0, DrawBox.Length - check)).X) >= RealBoxSize)
                {
                    Debug.WriteLine("size of string" + mono.MeasureString(DrawBox.Substring(0, DrawBox.Length - check)).X);
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
        private int MovedToRight()
        {
            if ((Textbox.Length - BoxSize - MovedTo) <= 0)
            {
                return 0;
            }
            Debug.Write("");
            return Textbox.Length - BoxSize - MovedTo;
        }
        private void CheckForClick(ref KeyboardState keyboardState, ref KeyboardState oldKeyboardState, GameTime gameTime, Keys key)
        {
            if (keyboardState.IsKeyDown(key) && (keyboardState.IsKeyUp(Keys.LeftControl)) && (keyboardState.IsKeyUp(Keys.RightControl)))
            {
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
                    Textbox = Textbox.Insert(MovedTo + TabPlace, KeyToChar(key, keyboardState, oldKeyboardState));
                    if (TabPlace < BoxSize)
                    {
                        if (TabPlace >= 52)
                            Debug.Write("");
                        TabPlace++;
                    }
                    else
                    {
                        if (TabPlace >= 52)
                            Debug.Write("");
                        TabPlace = BoxSize;
                        MovedTo++;
                    }
                }
            }
        }
        private bool CheakForKeyChange(KeyboardState keyboardState, KeyboardState oldKeyboardState, Keys key)
        {
            if (((keyboardState.IsKeyUp(key)) && (oldKeyboardState.IsKeyDown(key))) || (keyboardState.IsKeyDown(key) && oldKeyboardState.IsKeyUp(key)))
            {
                return true;
            }
            return false;
        }

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
            Debug.WriteLine("VectorToChar returned null");
            return "";

        }
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
            Debug.WriteLine("CharToVector returned null");
            return Vector3.Zero;
        }

        public void Draw(SpriteBatch spriteBatch, SpriteFont mono, SpriteFont font, Texture2D tex)
        {
            //265
            // should it get to the if before this error
            Vector2 tabVector = (mono.MeasureString(DrawBox.Substring(0, TabPlace)));
            tabVector = new Vector2((tabVector.X + 295), (375));
            spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend);
            if (startedRot)
            {
                bool shouldMakeBothGreen = false;
                for (int i = 0; i < DrawBox.Count(); i++)
                {
                    if (i == 0)
                    {
                        if (DrawBox.Length > 1)
                        {
                            if ((DrawBox[1] == 'I' || DrawBox[1] == '\'') && (DrawBox[0] != 'I' && DrawBox[0] != '\''))
                            {
                                shouldMakeBothGreen = true;
                                spriteBatch.Draw(tex, new Rectangle(300 + 11, 375, 11, (int)mono.MeasureString(DrawBox[1].ToString()).Y), null,
                               Color.LimeGreen, 0f, new Vector2(0, 0), SpriteEffects.None, 0.3f);
                            }
                        }
                        spriteBatch.Draw(tex, new Rectangle(300, 375, 11, (int)mono.MeasureString(DrawBox[i].ToString()).Y), null,
                           Color.LimeGreen, 0f, new Vector2(0, 0), SpriteEffects.None, 0.3f);
                    }
                    else if (!(i == 1 && shouldMakeBothGreen))
                    {
                        spriteBatch.Draw(tex, new Rectangle(300 + (i) * 11, 375, 11, (int)mono.MeasureString(DrawBox[i].ToString()).Y), null,
                            Color.LightGray, 0f, new Vector2(0, 0), SpriteEffects.None, 0.3f);
                    }
                }
            }
            if (FocusTextBox)
                alpha = 250;
            else
                alpha = 225;
            spriteBatch.DrawString(font, "Enter your algorithm here:", new Vector2(300, 350), Color.Black);
            spriteBatch.DrawString(font, ("|"), new Vector2(300 + RealBoxSize, 375), Color.Red);
            spriteBatch.DrawString(mono, (DrawBox), new Vector2(300, 375), Color.Black, 0f, new Vector2(0, 0), 1f, SpriteEffects.None, 0.5f);
            spriteBatch.DrawString(font, (physTab), tabVector, Color.Black);
            spriteBatch.Draw(GraphicBox, new Rectangle(300 - 12, 375, 266 + 30, 30), new Color(255, 255, 255, alpha));

            spriteBatch.End();
        }

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
