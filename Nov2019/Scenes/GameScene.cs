using Nov2019.Devices;
using Nov2019.SceneDevices;
using Nov2019.ScenesDevice;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nov2019.Scenes
{
    class GameScene : SceneBase
    {
        public GameScene()
        {

        }

        public override void Initialize()
        {
            base.Initialize();
        }

        public override void Update()
        {
            base.Update();
        }

        public override void Draw(Renderer renderer)
        {
            base.Draw(renderer);
        }

        public override SceneEnum NextScene()
        {
            return SceneEnum.GameScene;
        }
    }
}
