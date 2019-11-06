using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nov2019.Devices
{
    // 入力状態を扱うクラス
    public static class Input
    {
        // 現在の入力状態
        private static KeyboardState currentKey;    // キーボード
        private static MouseState currentMouse;     // マウス
        private static GamePadState[] currentPad = new GamePadState[4];

        // 1フレーム前の入力状態
        private static KeyboardState prevKey;
        private static MouseState prevMouse;
        private static GamePadState[] prevPad = new GamePadState[4];

        // 更新処理
        public static void Update()
        {
            // 1フレーム前の入力状態を更新
            prevKey = currentKey;
            prevMouse = currentMouse;

            for (int i = 0; i < currentPad.Length; i++)
            {
                prevPad[i] = currentPad[i];
            }

            // 現在のキーボードの状態を取得
            currentKey = Keyboard.GetState();
            currentMouse = Mouse.GetState();
            for (int i = 0; i < currentPad.Length; i++)
            {
                currentPad[i] = GamePad.GetState(i);
            }
        }

        #region キーボード関連

        // 指定のキーを押した瞬間
        public static bool GetKeyDown(Keys key)
        {
            return currentKey.IsKeyDown(key) && prevKey.IsKeyUp(key);
        }

        // 指定のキーを離した瞬間
        public static bool GetKeyUp(Keys key)
        {
            return currentKey.IsKeyUp(key) && prevKey.IsKeyDown(key);
        }

        // 指定のキーを長押している状態
        public static bool GetKey(Keys key)
        {
            return currentKey.IsKeyDown(key);
        }

        // 押しているキーボードを受け取る
        public static Keys[] GetPressedKey()
        {
            List<Keys> current = currentKey.GetPressedKeys().ToList();

            for (int i = 0; i < current.Count; i++)
            {
                if (((int)current[i] >= 48 && (int)current[i] <= 57) ||
                    ((int)current[i] >= 65 && (int)current[i] <= 90) ||
                    current[i] == Keys.OemBackslash)
                {

                }
                else
                {
                    current[i] = Keys.None;
                }
            }

            current.RemoveAll(c => c == Keys.None);

            List<Keys> list = new List<Keys>();
            foreach (var c in current)
            {
                if (currentKey.IsKeyDown(c) && prevKey.IsKeyUp(c))
                {
                    list.Add(c);
                }
            }

            return list.ToArray();
        }

        #endregion キーボード関連

        #region マウス関連

        // マウス左ボタンを押した瞬間
        public static bool IsLeftMouseDown()
        {
            return currentMouse.LeftButton == ButtonState.Pressed
                && prevMouse.LeftButton == ButtonState.Released;
        }

        // マウス左ボタンを離した瞬間
        public static bool IsLeftMouseUp()
        {
            return currentMouse.LeftButton == ButtonState.Released
                && prevMouse.LeftButton == ButtonState.Pressed;
        }

        // マウス左ボタンを長押している状態
        public static bool IsLeftMouseHold()
        {
            return currentMouse.LeftButton == ButtonState.Pressed;
        }

        // マウス右ボタンを押した瞬間
        public static bool IsRightMouseDown()
        {
            return currentMouse.RightButton == ButtonState.Pressed
                && prevMouse.RightButton == ButtonState.Released;
        }

        // マウス右ボタンを離した瞬間
        public static bool IsRightMouseUp()
        {
            return currentMouse.RightButton == ButtonState.Released
                && prevMouse.RightButton == ButtonState.Released;
        }

        // マウス右ボタンを長押ししている状態
        public static bool IsRightMouseHold()
        {
            return currentMouse.RightButton == ButtonState.Pressed;
        }

        // マウスの位置を変更する
        public static void SetMousePosition(int x, int y)
        {
            Mouse.SetPosition(x, y);
        }

        // マウスの位置を返す
        public static Vector2 GetMousePosition()
        {
            return new Vector2(currentMouse.X, currentMouse.Y);
        }

        //マウスのスクロールホイールの変化量
        public static int GetMouseWheel()
        {
            return prevMouse.ScrollWheelValue - currentMouse.ScrollWheelValue;
        }

        #endregion マウス関連

        #region コントローラー関連

        // 指定したGamePadボタンを押した瞬間
        public static bool IsPadButtonDown(Buttons button, int playerIndex)
        {
            return currentPad[playerIndex].IsButtonDown(button) && prevPad[playerIndex].IsButtonUp(button);
        }

        // 指定したGamePadボタンを離した瞬間
        public static bool IsPadButtonUp(Buttons button, int playerIndex)
        {
            return currentPad[playerIndex].IsButtonUp(button) && prevPad[playerIndex].IsButtonDown(button);
        }

        // 指定したGamePadボタンを押している状態
        public static bool IsPadButtonHold(Buttons button, int playerIndex)
        {
            return currentPad[playerIndex].IsButtonDown(button);
        }

        // 左スティックを倒した向きを取得
        public static Vector2 GetLeftStickState(int playerIndex)
        {
            return currentPad[playerIndex].ThumbSticks.Left;
        }

        // 右スティックを倒したときの向きを取得
        public static Vector2 GetRightStickState(int playerIndex)
        {
            return currentPad[playerIndex].ThumbSticks.Right;
        }

        // LeftTrigger(L2)ボタンの押されている状態を取得
        public static float GetLeftTriggerButton(int playerIndex)
        {
            return currentPad[playerIndex].Triggers.Left;
        }

        // RightTrigger(R2)ボタンの押されている状態を取得
        public static float GetRightTriggerButton(int playerIndex)
        {
            return currentPad[playerIndex].Triggers.Right;
        }

        // コントローラーを振動させる
        public static void SetVibration(int playerIndex, float motor)
        {
            GamePad.SetVibration((PlayerIndex)playerIndex, motor, motor);
        }

        public static bool IsPadConnect(int playerIndex)
        {
            return currentPad[playerIndex].IsConnected;
        }

        #endregion コントローラー関連

    }
}
