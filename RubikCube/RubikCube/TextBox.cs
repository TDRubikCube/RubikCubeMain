using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using System.Diagnostics;
using System.Threading;
using System.Windows.Forms;
using Keys = Microsoft.Xna.Framework.Input.Keys;                      /*
using System.Of[A].Down;                                            */

namespace RubikCube
{
    class TextBox
    {
        public string textbox = ""; // the entirety of what u wrote
        public string drawBox = ""; // what u see on screen
        int drawTab = 0; // physical place of tab in vector.x
        string physTab = ""; // the flashing tab
        public int movedTo = 0; // how much u moved to the right
        int timeSincePress = 0; // time passed since last click
        int timeSinceLetterPress = 0;//time passed since you started pressing a letter
        int tabTimer = 0; //timer of the tab
        Keys oldKey = Keys.F24; //old key last pressed
        public int boxSize = 20; //the boxsize in letters, not in vector.x!!! this size changes depending on the string vector.x size!
        public const int realBoxSize = 266; //the real box size in vectors.x, const!
        public int tabPlace = 0; //the logical place of the tab in the string
        public bool FocusTextBox = false;
        public Rectangle textBoxRect;
        public Texture2D graphicBox;
        int alpha;
        Cube cube;
        Camera camera;
        string algOrder;
        bool startedRot;
        string realVectorBox = "";
        int orderLength;
        bool didStart = false;
        MouseState oldMouseState;

        public TextBox(Cube _cube, ContentManager content)
        {
            graphicBox = content.Load<Texture2D>("pics/box2");
            textBoxRect = new Rectangle(300, 375, 266, 20);
            cube = _cube;
        }
        public void Update(KeyboardState state, KeyboardState oldState, GameTime gameTime, SpriteFont mono, string _algorder, Camera _camera, Cube _cube)
        {
            //check
            cube = _cube;
            MouseState mouse = Mouse.GetState();
            Point mousePos = new Point(mouse.X, mouse.Y);
            if (mouse.LeftButton == Microsoft.Xna.Framework.Input.ButtonState.Pressed &&
                oldMouseState.LeftButton == Microsoft.Xna.Framework.Input.ButtonState.Released)
            {
                if (textBoxRect.Contains(mousePos))
                {
                    FocusTextBox = true;
                }
                else
                    FocusTextBox = false;
            }
            camera = _camera;
            algOrder = _algorder;
            if (state.IsKeyDown(Keys.D1) && oldState.IsKeyUp(Keys.D1))
                Debug.WriteLine("");
            if (drawBox.Count() == 0)
                startedRot = false;
            if ((algOrder.Length != orderLength) && (EnterPressed || didStart))
            {
                didStart = true;
                if (textbox.Length == 1)
                {
                    textbox = "";
                    tabPlace = 0;
                }
                else if (textbox.Length > 0)
                {
                        textbox = textbox.Substring(1, textbox.Length - 2);
                    tabPlace = 0;
                }
                if (textbox.Length == 0)
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

            if ((state.IsKeyDown(Keys.Back)) && (textbox.Length > 0))
            {
                timeSincePress += gameTime.ElapsedGameTime.Milliseconds;
                if ((oldState.IsKeyUp(Keys.Back) || (timeSincePress > 250)))
                {
                    if (timeSincePress > 250)
                    {
                        timeSincePress = 225;
                    }
                    if ((tabPlace + movedTo) > 0)
                    {
                        if (textbox.Length <= boxSize)
                        {
                            tabPlace--;
                        }
                        else if ((MovedToRight() > 0 && (movedTo > 0)))
                        {
                            if (tabPlace > 0)
                            {
                                tabPlace--;
                            }
                            else
                            {
                                movedTo--;
                            }
                        }
                        else if (MovedToRight() > 0 && (movedTo == 0))
                        {
                            tabPlace--;
                        }
                        else if ((MovedToRight() == 0) && (movedTo > 0))
                        {
                            movedTo--;
                        }
                        textbox = textbox.Remove(movedTo + tabPlace, 1);
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
                    if (!((tabPlace == boxSize) && (MovedToRight() == 0)))
                    {
                        if (timeSincePress > 250)
                        {
                            timeSincePress = 225;
                        }
                        if ((tabPlace == boxSize) && (MovedToRight() > 0))
                        {
                            movedTo++;
                        }
                        else if ((tabPlace < boxSize) && (textbox.Length - tabPlace > 0))
                        {
                            tabPlace++;
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
                    if (movedTo + tabPlace > 0)
                    {
                        if (timeSincePress > 250)
                        {
                            timeSincePress = 225;
                        }
                        if (tabPlace > 0)
                        {
                            tabPlace--;
                        }
                        if ((tabPlace == 0) && (movedTo > 0))
                        {
                            movedTo--;
                        }
                        tabTimer = 0;
                    }
                    else
                    {
                        Console.Beep();
                    }

                }
            }
            if (textbox.Length > boxSize && (movedTo + boxSize + MovedToRight()) == textbox.Length)
            {
                drawBox = textbox.Substring(movedTo, boxSize);
            }
            else
            {
                drawBox = textbox;
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

            if (textbox.Length > boxSize && (movedTo + boxSize + MovedToRight()) == textbox.Length)
            {
                drawBox = textbox.Substring(movedTo, boxSize);
            }
            else
            {
                drawBox = textbox;
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
            tabPlace = 0;
            for (int i = 0; i < nono.Length; i++)
            {
                textbox = textbox.Replace(nono[i].ToString(), "");
            }
            movedTo = 0;
            MovedToRight();
            tabPlace = 0;
            foreach (var s in textbox)
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
            Vector2 boxVector = (mono.MeasureString(drawBox));
            if (boxVector.X >= realBoxSize)
            {
                if (drawBox.Length > 0)
                {
                    if (drawBox.Last<char>() == 'M')
                        Debug.WriteLine("");
                }
                int check = 1;
                Debug.WriteLine((mono.MeasureString(drawBox.Substring(0, drawBox.Length - check)).X));
                while ((mono.MeasureString(drawBox.Substring(0, drawBox.Length - check)).X) >= realBoxSize)
                {
                    Debug.WriteLine("size of string" + mono.MeasureString(drawBox.Substring(0, drawBox.Length - check)).X);
                    if (tabPlace >= boxSize)
                    {
                        tabPlace--;
                    }
                    boxSize--;
                    movedTo++;
                    check++;
                }
            }
            else if ((drawBox.Length >= boxSize))
            {
                boxSize++;
            }
        }
        private int MovedToRight()
        {
            if ((textbox.Length - boxSize - movedTo) <= 0)
            {
                return 0;
            }
            else
                Debug.Write("");
            return textbox.Length - boxSize - movedTo;
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
                    textbox = textbox.Insert(movedTo + tabPlace, KeyToChar(key, keyboardState, oldKeyboardState));
                    if (tabPlace < boxSize)
                    {
                        if (tabPlace >= 52)
                            Debug.Write("");
                        tabPlace++;
                    }
                    else
                    {
                        if (tabPlace >= 52)
                            Debug.Write("");
                        tabPlace = boxSize;
                        movedTo++;
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
                return "l";
            }
            if (real == Vector3.Right)
            {
                return "r";
            }
            if (real == Vector3.Backward)
            {
                return "b";
            }
            if (real == Vector3.Forward)
            {
                return "f";
            }
            if (real == Vector3.Up)
            {
                return "u";
            }
            if (real == Vector3.Down)
            {
                return "d";
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
            Vector2 tabVector = (mono.MeasureString(drawBox.Substring(0, tabPlace)));
            tabVector = new Vector2((tabVector.X + 295), (375));
            spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend);
            if (startedRot)
            {
                bool shouldMakeBothGreen = false;
                for (int i = 0; i < drawBox.Count(); i++)
                {
                    if (i == 0)
                    {
                        if (drawBox.Length > 1)
                        {
                            if ((drawBox[1] == 'I' || drawBox[1] == '\'') && (drawBox[0] != 'I' && drawBox[0] != '\''))
                            {
                                shouldMakeBothGreen = true;
                                spriteBatch.Draw(tex, new Rectangle(300 + 11, 375, 11, (int)mono.MeasureString(drawBox[1].ToString()).Y), null,
                               Color.LimeGreen, 0f, new Vector2(0, 0), SpriteEffects.None, 0.3f);
                            }
                        }
                        spriteBatch.Draw(tex, new Rectangle(300, 375, 11, (int)mono.MeasureString(drawBox[i].ToString()).Y), null,
                           Color.LimeGreen, 0f, new Vector2(0, 0), SpriteEffects.None, 0.3f);
                    }
                    else if (!(i == 1 && shouldMakeBothGreen))
                    {
                        spriteBatch.Draw(tex, new Rectangle(300 + (i) * 11, 375, 11, (int)mono.MeasureString(drawBox[i].ToString()).Y), null,
                            Color.LightGray, 0f, new Vector2(0, 0), SpriteEffects.None, 0.3f);
                    }
                }
            }
            if (FocusTextBox)
                alpha = 250;
            else
                alpha = 225;
            spriteBatch.DrawString(font, "Enter your algorithm here:", new Vector2(300, 350), Color.Black);
            spriteBatch.DrawString(font, ("|"), new Vector2(300 + realBoxSize, 375), Color.Red);
            spriteBatch.DrawString(mono, (drawBox), new Vector2(300, 375), Color.Black, 0f, new Vector2(0, 0), 1f, SpriteEffects.None, 0.5f);
            spriteBatch.DrawString(font, (physTab), tabVector, Color.Black);
            spriteBatch.Draw(graphicBox, new Rectangle(300 - 12, 375, 266 + 30, 30), new Color(255, 255, 255, alpha));

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
