using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Renren.Components.Network
{
    /// <summary>
    ///	Specifies the network item priority levels.
    /// </summary>
    public enum NetworkPriority
    {
        /// <summary>
        /// It's a low priority token
        /// </summary>
        Low,

        /// <summary>
        /// It's a normal priority token
        /// </summary>
        Normal,

        /// <summary>
        /// It's a high priority token
        /// </summary>
        High,

        /// <summary>
        /// It's a urgent token means shoudl be taken right now
        /// </summary>
        Urgent,

        /// <summary>
        /// Should never use it
        /// </summary>
        Unkown
    }

    /// <summary>
    ///	Specifies the Transition item priority levels.
    /// </summary>
    public class TransitionPriority
    {
        /// <summary>
        /// Provides the default priority transition item
        /// </summary>
        public static TransitionPriority Default 
        { 
            get 
            { 
                return new TransitionPriority() 
                { Priority = NetworkPriority.Normal, Grayness = 0 };
            }
        }

        /// <summary>
        /// Getter and Setter of priority
        /// </summary>
        public NetworkPriority Priority { get; set; }

        /// <summary>
        /// Getter and Setter of grayness of same level priority
        /// Note: it's a sub-level arbitration term for secheduler scheduling
        /// </summary>
        public int Grayness { get; set; }

        /// <summary>
        /// Try to tell whether they are the duplicated tokens
        /// </summary>
        /// <param name="left">The specified token needding compare</param>
        /// <returns>True: The same one, False: not</returns>
        public bool Equals(TransitionPriority left)
        {
            if (left != null &&
                this.Grayness == left.Grayness &&
                this.Priority == left.Priority)
            {
                return true;
            }

            return false;
        }
    }
}
