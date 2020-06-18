using BattleForSpaceResources.Entitys;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BattleForSpaceResources
{
    public class InputManager
    {
        public KeyboardState keyboardState;
        public KeyboardState lastKeyboardState;
        public MouseState lastMouseState;
        public MouseState mouseState;
        public MouseState originalMouseState;
        public GameObject cursor;
        public float cursorAddX, cursorAddY;
        public Vector2 cursorAdvancedPosition, mousePressedVector;
        private Core core = Core.GetCore();
        public InputManager()
        {
            CreateMouse();
        }
        public void CreateMouse()
        {
            SetOriginalPos();
            cursor = new GameObject(Textures.guiCursor, new Vector2(core.screenWidth / 2, core.screenHeight / 2));
            cursor.Origin = Vector2.Zero;
            cursor.Size = 0.75F;
            cursor.Update();
        }
        public void SetOriginalPos()
        {
            Mouse.SetPosition(core.screenWidth / 2, core.screenHeight / 2);
            originalMouseState = Mouse.GetState();
        }
        public bool IsCapsLocked()
        {
            return System.Windows.Forms.Control.IsKeyLocked(System.Windows.Forms.Keys.CapsLock);
        }
        public bool IsLeftButtonClicking()
        {
            return lastMouseState.LeftButton == ButtonState.Released && mouseState.LeftButton == ButtonState.Pressed;
        }
        public float GetMouseScroll()
        {
            if (mouseState.ScrollWheelValue - lastMouseState.ScrollWheelValue >= 1)
            {
                return  0.1F;
            }
            else if (mouseState.ScrollWheelValue - lastMouseState.ScrollWheelValue <= -1)
            {
                return -0.1F;
            }
            else return 0;
        }
        public bool IsKeyDown(Keys keyToTest)
        {
            return keyboardState.IsKeyDown(keyToTest);
        }
        public bool IsKeyPressed(Keys keyToTest)
        {
            return keyboardState.IsKeyUp(keyToTest) && lastKeyboardState.IsKeyDown(keyToTest);
        }
        public bool IsKeyReleased(Keys keyToTest)
        {
            return keyboardState.IsKeyDown(keyToTest) && lastKeyboardState.IsKeyUp(keyToTest);
        }
        public bool IsLeftButtonClicked()
        {
            return mouseState.LeftButton == ButtonState.Released
                   && lastMouseState.LeftButton == ButtonState.Pressed;
        }
        public bool IsRightButtonClicked()
        {
            return mouseState.RightButton == ButtonState.Released
                   && lastMouseState.RightButton == ButtonState.Pressed;
        }
        public Keys[] GetPressedKeys()
        {
            return keyboardState.GetPressedKeys();
        }
        public bool IsLeftButtonDown()
        {
            return mouseState.LeftButton == ButtonState.Pressed;
        }
        public bool IsRightButtonDown()
        {
            return mouseState.RightButton == ButtonState.Pressed;
        }
        public void Flush()
        {
            lastKeyboardState = keyboardState;
            lastMouseState = mouseState;
        }
        public void Update()
        {
            if (originalMouseState.X != core.screenWidth / 2 || originalMouseState.Y != core.screenHeight / 2)
            {
                SetOriginalPos();
            }

            Flush();

            keyboardState = Keyboard.GetState();
            mouseState = Mouse.GetState();

            UpdateCursor();
        }
        public void UpdateCursor()
        {
            if (!Settings.isUseSystemCursor)
                if (mouseState != originalMouseState)
                {
                    cursorAddX += (mouseState.X - originalMouseState.X) * Settings.mouseSpeed;
                    cursorAddY += (mouseState.Y - originalMouseState.Y) * Settings.mouseSpeed;
                    if (cursorAddX > core.screenWidth)
                        cursorAddX = core.screenWidth;
                    else if (cursorAddX < 0)
                        cursorAddX = 0;
                    if (cursorAddY > core.screenHeight)
                        cursorAddY = core.screenHeight;
                    else if (cursorAddY < 0)
                        cursorAddY = 0;
                    Mouse.SetPosition(core.screenWidth / 2, core.screenHeight / 2);
                    if (Settings.mouseSpeed <= 0.1f)
                        Settings.mouseSpeed = 0.1f;
                }
            //if (cursor != null)
            {
                if (!Settings.isUseSystemCursor)
                    cursor.Position = new Vector2(core.cam.screenCenter.X - core.screenWidth / 2 + cursorAddX, core.cam.screenCenter.Y - core.screenHeight / 2 + cursorAddY);
                else
                    cursor.Position = new Vector2(core.cam.screenCenter.X - core.screenWidth / 2 + mouseState.X, core.cam.screenCenter.Y - core.screenHeight / 2 + mouseState.Y);
            }
            cursorAdvancedPosition = Vector2.Transform(new Vector2(cursor.Position.X - core.cam.screenCenter.X + core.screenWidth / 2, cursor.Position.Y - core.cam.screenCenter.Y + core.screenHeight / 2), Matrix.Invert(core.cam.defaultScreen));
        }
    }
}
