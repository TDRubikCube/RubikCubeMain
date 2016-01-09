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
using Keys = Microsoft.Xna.Framework.Input.Keys; /*
using System.Of[A].Down; */

namespace RubikCube
{
    class TextBox
    {
        public string textbox = "";
        public string drawBox = "";
        public string tabString = "";
        public int movedTo = 0;
        int timeSincePress = 0;
        public const int boxSize = 20;
        public int tabPlace = 0;
        public TextBox()
        {
        }

        public void Update(KeyboardState state, KeyboardState oldState, GameTime gameTime)
        {
            //34
            //(Keys)(Enum.Parse(typeof(Keys), "A"));
            CheckForClick(ref state, ref oldState, Keys.A);
            CheckForClick(ref state, ref oldState, Keys.B);
            CheckForClick(ref state, ref oldState, Keys.C);
            CheckForClick(ref state, ref oldState, Keys.D);
            CheckForClick(ref state, ref oldState, Keys.E);
            CheckForClick(ref state, ref oldState, Keys.F);
            CheckForClick(ref state, ref oldState, Keys.G);
            CheckForClick(ref state, ref oldState, Keys.H);
            CheckForClick(ref state, ref oldState, Keys.I);
            CheckForClick(ref state, ref oldState, Keys.J);
            CheckForClick(ref state, ref oldState, Keys.K);
            CheckForClick(ref state, ref oldState, Keys.L);
            CheckForClick(ref state, ref oldState, Keys.M);
            CheckForClick(ref state, ref oldState, Keys.N);
            CheckForClick(ref state, ref oldState, Keys.O);
            CheckForClick(ref state, ref oldState, Keys.P);
            CheckForClick(ref state, ref oldState, Keys.Q);
            CheckForClick(ref state, ref oldState, Keys.R);
            CheckForClick(ref state, ref oldState, Keys.S);
            CheckForClick(ref state, ref oldState, Keys.T);
            CheckForClick(ref state, ref oldState, Keys.U);
            CheckForClick(ref state, ref oldState, Keys.V);
            CheckForClick(ref state, ref oldState, Keys.W);
            CheckForClick(ref state, ref oldState, Keys.X);
            CheckForClick(ref state, ref oldState, Keys.Y);
            CheckForClick(ref state, ref oldState, Keys.Z);
            CheckForClick(ref state, ref oldState, Keys.Space);
            CheckForClick(ref state, ref oldState, Keys.OemSemicolon); //nope
            if ((state.IsKeyDown(Keys.Enter)) && (oldState.IsKeyUp(Keys.Enter)))
            {
                textbox += "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
            }
            //cheaks if one of the keys that count how much time has passed since you pressed them have been...um...Un-pressed?
            if (((state.IsKeyUp(Keys.Right)) && (oldState.IsKeyDown(Keys.Right)))||((state.IsKeyUp(Keys.Left))&&(oldState.IsKeyDown(Keys.Left)))||((state.IsKeyUp(Keys.Back))&&(oldState.IsKeyDown(Keys.Back))))
            {
                timeSincePress = 0;
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
                        if ((textbox.Length -boxSize -movedTo >0)&&)
                        {
                            tabPlace--;
                        }
                        else if (tabPlace<boxSize)
                        {
                            tabPlace--;
                        }
                        if (movedTo > 0)
                        {
                            if (textbox.Length - boxSize - movedTo ==0)
                            {
                                movedTo--;
                            }
                           
                        }
                        textbox = textbox.Remove(movedTo + tabPlace);
                    }

                }
            }
            if (state.IsKeyDown(Keys.Right))
            {
                timeSincePress += gameTime.ElapsedGameTime.Milliseconds;
                if ((oldState.IsKeyUp(Keys.Right) || (timeSincePress > 250)))
                {
                    if (timeSincePress > 250)
                    {
                        timeSincePress = 225;
                    }
                    if ((tabPlace == boxSize) && (textbox.Length - boxSize - movedTo > 0))
                    {
                        movedTo++;
                    }
                    else if ((tabPlace < boxSize) && (textbox.Length - tabPlace > 0))
                    {
                        tabPlace++;
                    }
                    else
                        Console.Beep();
                }
            }
            if (state.IsKeyDown(Keys.Left))
            {
                timeSincePress += gameTime.ElapsedGameTime.Milliseconds;
                if ((oldState.IsKeyUp(Keys.Left) || (timeSincePress > 250)))
                {
                    if (timeSincePress > 250)
                    {
                        timeSincePress = 225;
                    }
                    if (tabPlace > 0)
                    {
                        tabPlace--;
                    }
                    if (tabPlace == 0)
                    {
                        if (movedTo > 0)
                        {
                            movedTo--;
                        }
                        else
                        {
                            Console.Beep();
                        }
                    }

                }
            }

            if (textbox.Length > boxSize)
            {
                drawBox = textbox.Substring(movedTo, boxSize);
            }
            else
            {
                drawBox = textbox;
            }
            tabString = "";
            for (int i = 0; i < tabPlace; i++)
            {
                tabString += "  ";
            }
            oldState = state;
        }
        private void CheckForClick(ref KeyboardState keyboardState, ref KeyboardState oldKeyboardState, Keys key)
        {
            if (keyboardState.IsKeyDown(key) && oldKeyboardState.IsKeyUp(key))
            {
                //   if (((keyboardState.IsKeyDown(Keys.RightShift)) || (keyboardState.IsKeyDown(Keys.LeftShift))) && ((oldKeyboardState.IsKeyUp(Keys.RightShift)) || (oldKeyboardState.IsKeyUp(Keys.LeftShift))))
                // {
                //   textbox += KeyToChar(key, keyboardState, oldKeyboardState);
                // textbox += "i";
                //}
                //else
                //{
                textbox = textbox.Insert(movedTo + tabPlace,KeyToChar(key, keyboardState, oldKeyboardState));
                if (tabPlace < boxSize)
                {
                    tabPlace++;
                }
                if ((tabPlace >= boxSize) && (textbox.Length - boxSize - movedTo > 0))
                {
                    tabPlace = boxSize;
                    movedTo++;
                }
                //}
            }
        }

        public void Draw(SpriteBatch spriteBatch, SpriteFont font)
        {
            spriteBatch.Begin();
            spriteBatch.DrawString(font, ("TimeSincePress: " + timeSincePress), new Vector2(300, 350), Color.Black);
            spriteBatch.DrawString(font, ("Text: " + tabString + ","), new Vector2(300, 375), Color.Black);
            spriteBatch.DrawString(font, ("Text: " + drawBox), new Vector2(300, 375), Color.Black);
            spriteBatch.DrawString(font, ("Length: " + textbox.Length), new Vector2(300, 400), Color.Black);
            spriteBatch.DrawString(font, ("MovedTo: " + movedTo), new Vector2(300, 425), Color.Black);
            spriteBatch.DrawString(font, ("TabPlace: " + tabPlace), new Vector2(300, 450), Color.Black);
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
