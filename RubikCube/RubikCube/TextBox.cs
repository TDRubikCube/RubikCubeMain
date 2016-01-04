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
        public int movedTo = 0;
        public int tabPlace = 0;
        public TextBox()
        {
        }

        public void Update(KeyboardState state, KeyboardState oldState)
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
            CheckForClick(ref state, ref oldState, Keys.OemSemicolon);
            if (((state.IsKeyDown(Keys.Back)) && (oldState.IsKeyUp(Keys.Back))) && (textbox.Length > 0))
            {
                textbox = textbox.Substring(0, (textbox.Length - 1));
                if (textbox.Length == 34)
                {
                    movedTo++;
                }
            }
            if ((state.IsKeyDown(Keys.Right)) && (oldState.IsKeyUp(Keys.Right)))
            {
                if ((tabPlace >= 35) && ((textbox.Length - movedTo) > 35))
                {
                    movedTo++;
                }
                if (tabPlace < 35)
                {
                    tabPlace++;
                }
            }
            if ((state.IsKeyDown(Keys.Left)) && (oldState.IsKeyUp(Keys.Left)))
            {
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
            if (textbox.Length > 34)
            {
                drawBox = textbox.Substring(movedTo, movedTo + 34);
            }
            else
            {
                drawBox = textbox;
            }
            oldState = state;
        }
        private void CheckForClick(ref KeyboardState keyboardState, ref KeyboardState oldKeyboardState, Keys key)
        {
            if (keyboardState.IsKeyDown(key) && oldKeyboardState.IsKeyUp(key))
            {
                if (((keyboardState.IsKeyDown(Keys.RightShift)) || (keyboardState.IsKeyDown(Keys.LeftShift))) && ((oldKeyboardState.IsKeyUp(Keys.RightShift)) || (oldKeyboardState.IsKeyUp(Keys.LeftShift))))
                {
                    textbox += KeyToChar(key, keyboardState, oldKeyboardState);
                    textbox += "i";
                }
                else
                    textbox += KeyToChar(key, keyboardState, oldKeyboardState);
            }
        }

        public void Draw(SpriteBatch spriteBatch, SpriteFont font)
        {
            spriteBatch.Begin();
            spriteBatch.DrawString(font, ("Text: " + drawBox), new Vector2(300, 400), Color.Black);
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
