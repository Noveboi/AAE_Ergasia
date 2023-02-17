using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Authentication.ExtendedProtection.Configuration;
using System.Text;
using System.Threading.Tasks;

namespace battleships_official2
{
    internal class Ship
    {
        public string Name;
        /// <summary>
        /// List of size-3 integer arrays.        
        /// index 0 - x coordinate of the ship,
        /// index 1 - y coordinate of the ship,
        /// index 2 - ship state ( 0 - ALIVE / 1 - DESTROYED )
        /// </summary>
        public List<int[]> States;
        public int Length;

        public Ship(int length, string name)
        {
            Length = length;
            Name = name;
            States = new List<int[]>();
        }

        /// <summary>
        /// Return true if EVERY ship state is 1
        /// </summary>
        /// <returns></returns>
        public bool IsDestroyed()
        {
            int destroyedBlocks = 0;
            foreach(var state in States)
            {
                if (state[2] == 1) destroyedBlocks++;
            }
            if (destroyedBlocks == Length) return true;
            return false;
        }
    }
}
