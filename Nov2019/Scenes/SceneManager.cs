using Nov2019.Devices;
using Nov2019.SceneDevices;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nov2019.ScenesDevice
{
    class SceneManager
    {
        private Dictionary<SceneEnum, SceneBase> scenes;
        private SceneBase currentScene;

        public SceneManager()
        {
            scenes = new Dictionary<SceneEnum, SceneBase>();
        }

        public void AddScene(SceneEnum sceneEnum, SceneBase scene)
        {
            Debug.Assert(scene != null, "追加されたシーン[ " + sceneEnum + " ]がnullです");
            Debug.Assert(!scenes.ContainsKey(sceneEnum), "すでに同じシーンEnumが使われています[ " + sceneEnum + " ]");

            scenes.Add(sceneEnum, scene);
        }

        public void ChangeScene(SceneEnum sceneEnum)
        {
            Debug.Assert(scenes.ContainsKey(sceneEnum), "変更指定したシーンがまだ登録されていません[ " + sceneEnum + " ]");

            currentScene = scenes[sceneEnum];
            currentScene.Initialize();
        }

        public void Update()
        {
            if (currentScene == null) { return; }
            currentScene.Update();

            if (currentScene.IsEndFlag) { ChangeScene(currentScene.NextScene()); }
        }

        public void Draw(Renderer renderer)
        {
            if (currentScene == null) { return; }
            currentScene.Draw(renderer);
        }
    }
}
