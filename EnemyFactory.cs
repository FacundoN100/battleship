using System;
using System.Collections.Generic;

namespace MyGame
{
    public class EnemyFactory
    {
        private readonly List<Enemy> _enemies;
        private readonly List<IColisionable> _collidables;


        public EnemyFactory(List<Enemy> enemies, List<IColisionable> collidables)
        {
            _enemies = enemies ?? throw new ArgumentNullException(nameof(enemies));
            _collidables = collidables ?? throw new ArgumentNullException(nameof(collidables));

        }

        public Enemy Create(float x, float y)
        {
            return new Enemy(x, y);
        }

        public void SpawnEnemy(float x, float y)
        {
            var enemy = new Enemy(x, y);
            _enemies.Add(enemy);
            _collidables.Add(enemy);
            //return enemy;
            //enemies.Add(new Enemy(x, y));
        }

        //public Enemy Create(float x, float y)
        //{
        //    var enemy = new Enemy(x, y);   
        //    colisionables.Add(enemy);      
        //    return enemy;
        //}
    }
}
