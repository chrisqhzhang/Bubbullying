using System;
using UnityEditor;
using UnityEngine;

namespace MKColorManager
{
    /// <summary>
    /// You can make any class inherit this interface and implement it any way you need
    /// </summary>
    [Serializable]
    public abstract class IColorable : MonoBehaviour
    {

        /// <summary>
        /// Which category does this Colorable belong to?
        /// </summary>
        [SerializeField]
        [Tooltip("Which category does this Colorable belong to?")]
        protected string _category;
        public string Category => _category;

        /// <summary>
        /// If your project uses color in a creative way that this package doesn't already handle, 
        /// you can implement SetColor to do what you need to do.
        /// </summary>
        /// <param name="color"></param>
        public abstract void SetColor(Color color);
    }
}