using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FP_Greenfall.Interfaces
{
    public interface IDamageable
    {
        public bool IsDead();
        public int TakeDamage(int damage);
    }
}
