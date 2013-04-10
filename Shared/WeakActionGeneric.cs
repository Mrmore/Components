﻿
using System;

// Thanks for mvvm light:
// http://mvvmlight.codeplex.com/

namespace Renren.Components.Tools
{
    /// <summary>
    /// Stores an Action without causing a hard reference
    /// to be created to the Action's owner. The owner can be garbage collected at any time.
    /// </summary>
    /// <typeparam name="T">The type of the Action's parameter.</typeparam>
    ////[ClassInfo(typeof(Messenger))]
    public class WeakAction<T> : WeakAction
    {
        private readonly Action<T> _action;

        public new bool IsStatic
        {
            get
            {
                return _action != null && _action.Target == null;
            }
        }

        /// <summary>
        /// Initializes a new instance of the WeakAction class.
        /// </summary>
        /// <param name="target">The action's owner.</param>
        /// <param name="action">The action that will be associated to this instance.</param>
        public WeakAction(object target, Action<T> action)
            : base(target, null)
        {
            _action = action;
        }

        public WeakAction(Action<T> action)
            : this(action.Target, action)
        {
        }

        /// <summary>
        /// Gets the Action associated to this instance.
        /// </summary>
        public new Action<T> Action
        {
            get
            {
                return _action;
            }
        }

        /// <summary>
        /// Executes the action. This only happens if the action's owner
        /// is still alive. The action's parameter is set to default(T).
        /// </summary>
        public new void Execute()
        {
            if (_action != null
                && IsAlive)
            {
                _action(default(T));
            }
        }

        /// <summary>
        /// Executes the action. This only happens if the action's owner
        /// is still alive.
        /// </summary>
        /// <param name="parameter">A parameter to be passed to the action.</param>
        public void Execute(T parameter)
        {
            if (_action != null
                && (IsStatic || IsAlive))
            {
                _action(parameter);
            }
        }
    }
}