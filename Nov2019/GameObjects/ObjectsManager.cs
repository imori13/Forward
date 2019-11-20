using Microsoft.Xna.Framework;
using Nov2019.Devices;
using Nov2019.Devices.Particles;
using System;
using System.Collections.Generic;

namespace Nov2019.GameObjects
{
    // 複数Managerをひとつにまとめて、1つの参照で複数のマネージャーにアクセスしたい
    // 保守性などない
    class ObjectsManager
    {
        public readonly Vector3 OffsetPosition = new Vector3(MapLength / 2f, 0, MapLength / 2f);

        // 当たり判定だけ行う
        public List<GameObject>[] Array = new List<GameObject>[85];
        // オブジェクトを更新する
        private List<GameObject> gameobjects = new List<GameObject>();
        private List<Particle> particles = new List<Particle>();

        private List<GameObject> addGameObjects = new List<GameObject>();
        private List<Particle> addParticles = new List<Particle>();

        public Camera Camera { get; private set; }
        public Player Player { get; private set; }
        public BossEnemy BossEnemy { get; private set; }

        public static readonly float MapLength = 5000f;
        // 分割数 / ルート空間の長さ
        public float unitLength { get; private set; } = (2 * 2 * 2) / MapLength;

        public float objectCount { get { return gameobjects.Count; } }
        public float particleCount { get { return particles.Count; } }

        public ObjectsManager(Camera camera)
        {
            this.Camera = camera;

            for (int i = 0; i < Array.Length; i++)
            {
                Array[i] = new List<GameObject>();
            }
        }

        public void Initialize()
        {
            foreach (var aa in Array)
            {
                aa.Clear();
            }

            gameobjects.Clear();
            particles.Clear();
            addParticles.Clear();
        }

        public void AddGameObject(GameObject gameobject, bool offsetFlag)
        {
            if (gameobject == null) { return; }

            if (offsetFlag)
            {
                gameobject.Position += OffsetPosition;
            }

            gameobject.ObjectsManager = this;
            gameobject.Camera = Camera;
            gameobject.UpdateListPos();
            addGameObjects.Add(gameobject);

            gameobject.Initialize();

            if (gameobject is Player)
            {
                Player = gameobject as Player;
            }

            if (gameobject is BossEnemy)
            {
                BossEnemy = gameobject as BossEnemy;
            }
        }

        public void AddParticle(Particle particle)
        {
            if (particle == null) { return; }

            particle.Initialize();
            particles.Add(particle);
        }

        public void Update()
        {
            gameobjects.AddRange(addGameObjects);

            addGameObjects.Clear();

            gameobjects.ForEach(g => g.Update());

            particles.ForEach(p => p.Update());

            CollisionCheck();

            foreach (var a in Array)
            {
                a.RemoveAll(aaa => aaa.IsDead);
            }

            gameobjects.RemoveAll(g => g.IsDead);
            particles.RemoveAll(p => p.IsDead);
        }

        public void Draw(Renderer renderer)
        {
            particles.ForEach(p => p.Draw(renderer, Camera));
            gameobjects.ForEach(g =>
            {
                if (Vector3.DistanceSquared(Player.Position, g.Position) <= 1500f * 1500f)
                {
                    g.Draw(renderer);
                }
            });

            foreach (var g in gameobjects)
            {
                if (g.Collider != null)
                {
                    g.Collider.Draw(renderer);
                }
            }
        }

        public void DrawUI(Renderer renderer)
        {
            gameobjects.ForEach(g => g.DrawUI(renderer));
        }

        public void RemoveEnemyBullet()
        {
            Random rand = GameDevice.Instance().Random;
            gameobjects.ForEach(g =>
            {
                if (g.GameObjectTag == GameObjectTag.EnemyBullet)
                {
                    g.IsDead = true;

                    for(int i = 0; i < 10; i++)
                    {
                        AddParticle(new SmokeParticle3D(g.Position, rand));
                    }
                }
            });
        }

        public void CollisionCheck()
        {
            for (int array = 0; array < Array.Length; array++)
            {
                // 同じリスト内で当たり判定
                for (int i = 0; i < Array[array].Count; i++)
                {
                    // 同じリスト内のオブジェクトと当たり判定
                    for (int j = 0; j < Array[array].Count; j++)
                    {
                        // 一度当たり判定を行っているならreturn
                        if (i <= j) { continue; }
                        if (Array[array][i].IsDead) { continue; }
                        if (Array[array][j].IsDead) { continue; }

                        // 衝突判定
                        if (Array[array][i].Collider.IsCollision(Array[array][j].Collider))
                        {
                            Array[array][i].HitAction(Array[array][j]);
                            Array[array][j].HitAction(Array[array][i]);
                        }
                    }
                }

                // 孫→子、親、ルート
                if (20 < array && array < 85)
                {
                    int num;

                    // 子
                    num = (array - 21) >> 2;
                    num += 5;
                    for (int i = 0; i < Array[array].Count; i++)
                    {
                        for (int j = 0; j < Array[num].Count; j++)
                        {
                            if (Array[array][i].IsDead) { continue; }
                            if (Array[num][j].IsDead) { continue; }

                            // 衝突判定
                            if (Array[array][i].Collider.IsCollision(Array[num][j].Collider))
                            {
                                Array[array][i].HitAction(Array[num][j]);
                                Array[num][j].HitAction(Array[array][i]);
                            }
                        }
                    }

                    // 親
                    num = (array - 21) >> 4;
                    num += 1;

                    for (int i = 0; i < Array[array].Count; i++)
                    {
                        for (int j = 0; j < Array[num].Count; j++)
                        {
                            if (Array[array][i].IsDead) { continue; }
                            if (Array[num][j].IsDead) { continue; }

                            // 衝突判定
                            if (Array[array][i].Collider.IsCollision(Array[num][j].Collider))
                            {
                                Array[array][i].HitAction(Array[num][j]);
                                Array[num][j].HitAction(Array[array][i]);
                            }
                        }
                    }

                    // ルート
                    for (int i = 0; i < Array[array].Count; i++)
                    {
                        for (int j = 0; j < Array[0].Count; j++)
                        {
                            if (Array[array][i].IsDead) { continue; }
                            if (Array[0][j].IsDead) { continue; }

                            // 衝突判定
                            if (Array[array][i].Collider.IsCollision(Array[0][j].Collider))
                            {
                                Array[array][i].HitAction(Array[0][j]);
                                Array[0][j].HitAction(Array[array][i]);
                            }
                        }
                    }
                }

                // 子→親、ルート
                if (4 < array && array < 21)
                {
                    int num;

                    // 親
                    num = (array - 5) >> 2;
                    num += 1;
                    for (int i = 0; i < Array[array].Count; i++)
                    {
                        for (int j = 0; j < Array[num].Count; j++)
                        {
                            if (Array[array][i].IsDead) { continue; }
                            if (Array[num][j].IsDead) { continue; }

                            // 衝突判定
                            if (Array[array][i].Collider.IsCollision(Array[num][j].Collider))
                            {
                                Array[array][i].HitAction(Array[num][j]);
                                Array[num][j].HitAction(Array[array][i]);
                            }
                        }
                    }

                    // ルート
                    for (int i = 0; i < Array[array].Count; i++)
                    {
                        for (int j = 0; j < Array[0].Count; j++)
                        {
                            if (Array[array][i].IsDead) { continue; }
                            if (Array[0][j].IsDead) { continue; }

                            // 衝突判定
                            if (Array[array][i].Collider.IsCollision(Array[0][j].Collider))
                            {
                                Array[array][i].HitAction(Array[0][j]);
                                Array[0][j].HitAction(Array[array][i]);
                            }
                        }
                    }
                }

                // 親→ルート  
                if (0 < array && array < 5)
                {
                    // ルート
                    for (int i = 0; i < Array[array].Count; i++)
                    {
                        for (int j = 0; j < Array[0].Count; j++)
                        {
                            if (Array[array][i].IsDead) { continue; }
                            if (Array[0][j].IsDead) { continue; }

                            // 衝突判定
                            if (Array[array][i].Collider.IsCollision(Array[0][j].Collider))
                            {
                                Array[array][i].HitAction(Array[0][j]);
                                Array[0][j].HitAction(Array[array][i]);
                            }
                        }
                    }
                }
            }
        }

        uint XYtoMotton(ushort x, ushort y)
        {
            return (BitSeparate32(x) | (BitSeparate32(y) << 1));
        }

        uint BitSeparate32(ushort n)
        {
            uint i = (uint)n;
            i = (i | (i << 8)) & 0x00ff00ff;
            i = (i | (i << 4)) & 0x0f0f0f0f;
            i = (i | (i << 2)) & 0x33333333;
            return (i | (i << 1)) & 0x55555555;
        }

        int ToLinearSpace(int mortonNumber, int level)
        {
            int _divisionNumber = 4;
            int denom = _divisionNumber - 1;
            int additveNum = (int)((Math.Pow(_divisionNumber, level) - 1) / denom);
            return mortonNumber + additveNum;
        }
    }
}
