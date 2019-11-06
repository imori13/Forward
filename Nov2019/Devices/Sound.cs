using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Media;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace Nov2019.Devices
{
    public class Sound
    {
        #region フィールドとコンストラクタ
        // コンテンツ管理者
        private ContentManager contentManager;
        // MP3管理用
        private Dictionary<string, Song> bgms;
        // WAV管理用
        private Dictionary<string, SoundEffect> soundEffects;
        // WAVインスタンス管理用(WAVの高度な利用)
        private Dictionary<string, SoundEffectInstance> seInstances;
        // WAVインスタンスの再生管理用ディクショナリ
        private Dictionary<string, SoundEffectInstance> sePlayDict;
        // 現在再生中のMP3のアセット名
        private string currentBGM;

        // コンストラクタ
        public Sound(ContentManager content)
        {
            // Game1クラスのコンテンツ管理者と紐づけ
            contentManager = content;
            // BGMは繰り返し再生
            MediaPlayer.IsRepeating = true;

            // 各Dictionaryの実体生成
            bgms = new Dictionary<string, Song>();
            soundEffects = new Dictionary<string, SoundEffect>();
            seInstances = new Dictionary<string, SoundEffectInstance>();
            // 再生Listの実体生成
            sePlayDict = new Dictionary<string, SoundEffectInstance>();

            // 何も再生していないのでnullで初期化
            currentBGM = null;
        }

        public void Unload()
        {
            // ディクショナリをクリア
            bgms.Clear();
            soundEffects.Clear();
            seInstances.Clear();
            sePlayDict.Clear();
        }

        #endregion

        // Assert用エラーメッセージ
        private string ErrorMessage(string name)
        {
            return "再生する音データのアセット名(" + name + ")がありません" +
                "アセット名の確認、Dictionaryに登録しているか確認してください";
        }

        #region BGM(MP3:MediaPlayer)関連

        // BGM(MP3)の読み込み
        public void LoadBGM(string name, string filepath = "./")
        {
            // 既に登録されているか?
            if (bgms.ContainsKey(name))
            {
                return;
            }
            // MP3の読み込みとDictionaryへ登録
            bgms.Add(name, contentManager.Load<Song>(filepath + name));
        }

        // BGMが停止中か?
        public bool IsStoppedBGM()
        {
            return MediaPlayer.State == MediaState.Stopped;
        }

        // BGMが再生中か?
        public bool IsPlayingBGM()
        {
            return MediaPlayer.State == MediaState.Playing;
        }

        // BGMが一時停止中か?
        public bool IsPausedBGM()
        {
            return MediaPlayer.State == MediaState.Paused;
        }

        // BGMを停止
        public void StopBGM()
        {
            MediaPlayer.Stop();
            currentBGM = null;
        }

        // BGMを再生
        public void PlayBGM(string name)
        {
            // アセット名がディクショナリに登録されているか?
            Debug.Assert(bgms.ContainsKey(name), ErrorMessage(name));

            // 同じ曲か?
            if (currentBGM == name)
            {
                // 同じ曲だったら何もしない
                return;
            }

            // BGMは再生中か?
            if (IsPlayingBGM())
            {
                // 再生中なら、停止処理
                StopBGM();
            }

            // ボリューム設定
            MediaPlayer.Volume = 0.25f;

            // 現在のBGM名を設定
            currentBGM = name;

            // 再生開始
            MediaPlayer.Play(bgms[currentBGM]);
        }

        // BGMの一時停止
        public void PauseBGM()
        {
            if (IsPlayingBGM())
            {
                MediaPlayer.Pause();
            }
        }

        // 一時停止からの再生
        public void ResumeBGM()
        {
            if (IsPausedBGM())
            {
                MediaPlayer.Resume();
            }
        }

        // BGMループフラグを変更
        public void ChangeBGMLoopFlag(bool loopFlag)
        {
            MediaPlayer.IsRepeating = loopFlag;
        }

        #endregion

        #region WAV(SE.SoundEffect)関連

        public void LoadSE(string name, string filepath = "./")
        {
            // 既に登録されていたら
            if (soundEffects.ContainsKey(name))
            {
                // 何もしない
                return;
            }

            soundEffects.Add(name, contentManager.Load<SoundEffect>(filepath + name));
        }

        public void PlaySE(string name)
        {
            // アセット名が登録されているか?
            Debug.Assert(soundEffects.ContainsKey(name), ErrorMessage(name));

            // 再生
            soundEffects[name].Play();
        }
        #endregion

        #region WAVインスタンス関連

        public void CreateSEInstance(string name)
        {
            // 既に登録されいたら何もしない
            if (seInstances.ContainsKey(name))
            {
                return;
            }

            // WAV用ディクショナリに登録されていないと無理
            Debug.Assert(soundEffects.ContainsKey(name),
                "先に" + name + "の読み込み処理を行ってください");

            // WAVデータのインスタンスを生成し、登録
            seInstances.Add(name, soundEffects[name].CreateInstance());
        }

        public void PlaySEInstances(string name, int no, bool loopFlag = false)
        {
            Debug.Assert(seInstances.ContainsKey(name), ErrorMessage(name));

            // 再生管理用ディクショナリ登録されてたら何もしない
            if (sePlayDict.ContainsKey(name + no))
            {
                return;
            }

            // 音データから取り出して、再生管理用ディクショナリに入れる
            var data = seInstances[name];
            data.IsLooped = loopFlag;
            data.Play();
            sePlayDict.Add(name + no, data);
        }

        // 指定SEの停止
        public void StoppedSE(string name, int no)
        {
            // 再生管理用ディクショナリになければ何もしない
            if (sePlayDict.ContainsKey(name + no))
            {
                return;
            }
            // 再生中なら停止
            if (sePlayDict[name + no].State == SoundState.Playing)
            {
                sePlayDict[name + no].Stop();
            }
        }

        // 再生中のSEをすべて停止
        public void StoppedSE()
        {
            foreach (var se in sePlayDict)
            {
                if (se.Value.State == SoundState.Playing)
                {
                    se.Value.Stop();
                }
            }
        }

        // 指定したSEを削除
        public void RemoveSE(string name, int no)
        {
            // 再生管理用ディクショナリになければ何もしない
            if (sePlayDict.ContainsKey(name + no) == false)
            {
                return;
            }
            sePlayDict.Remove(name + no);
        }

        // すべてのSEを削除
        public void RemoveSE()
        {
            sePlayDict.Clear();
        }

        // 指定したSEを一時停止
        public void PauseSE(string name, int no)
        {
            // 再生管理用ディクショナリになければ何もしない
            if (sePlayDict.ContainsKey(name + no) == false)
            {
                return;
            }
            // 再生中なら一時停止
            if (sePlayDict[name + no].State == SoundState.Playing)
            {
                sePlayDict[name + no].Pause();
            }
        }

        // すべてのSEを一時停止
        public void PauseSE()
        {
            foreach (var se in sePlayDict)
            {
                if (se.Value.State == SoundState.Playing)
                {
                    se.Value.Pause();
                }
            }
        }

        // 指定したSEを一時停止から復帰
        public void ResumeSE(string name, int no)
        {
            // 再生管理用ディクショナリになければ何もしない
            if (sePlayDict.ContainsKey(name + no) == false)
            {
                return;
            }

            if (sePlayDict[name + no].State == SoundState.Paused)
            {
                sePlayDict[name + no].Resume();
            }
        }

        // 一時停止中のすべてのSEを復帰
        public void ResumeSE()
        {
            foreach (var se in sePlayDict)
            {
                if (se.Value.State == SoundState.Paused)
                {
                    se.Value.Resume();
                }
            }
        }

        // SEインスタンスが再生中か?
        public bool IsPlayingSEInstance(string name, int no)
        {
            return sePlayDict[name + no].State == SoundState.Playing;
        }

        // SEインスタンスが停止中か?
        public bool IsStoppedSEInstance(string name, int no)
        {
            return sePlayDict[name + no].State == SoundState.Stopped;
        }

        // SEインスタンスが一時停止中か?
        public bool IsPausedSEInstance(string name, int no)
        {
            return sePlayDict[name + no].State == SoundState.Paused;
        }

        #endregion
    }
}
