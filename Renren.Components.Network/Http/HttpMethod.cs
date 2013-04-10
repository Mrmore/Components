using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Renren.Components.Network.Http
{
    /// <summary>
    /// The Http method's definition
    /// </summary>
    public enum HttpMethod
    {
        /// <summary>
        /// Post method
        /// </summary>
        POST,

        /// <summary>
        /// Get method
        /// </summary>
        GET,

        /// <summary>
        /// Pull method
        /// </summary>
        PULL,

        /// <summary>
        /// Post method with formdata
        /// </summary>
        POST_FORMDATA,

        /// <summary>
        /// delete method
        /// </summary>
        DELETE
    }
}
