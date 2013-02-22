using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Artemis;
using Artemis.Interface;

namespace ArtemisTest.Components
{
    public class Health : IComponent
    {
        private float health = 0;
        private float maximumHealth = 0;
		
		public Health()
        {
        }
		
        public Health(float health)
        {
            this.health = this.maximumHealth = health;
        }

        public float HP
        {
            get { return health;}
			set { health = value;}
        }
		
        public float MaximumHealth
        {
            get { return maximumHealth;}
        }

        public double HealthPercentage
        {
            get { return Math.Round(health / maximumHealth * 100f);}
        }

        public void AddDamage(int damage)
        {
            health -= damage;
            if (health < 0)
                health = 0;
        }

        public bool IsAlive
        {
            get { return health > 0;}
        }
    }
}
