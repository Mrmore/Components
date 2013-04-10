// Thanks for Microsoft patterns & practices Enterprise Library Caching Application Block
// http://entlib.codeplex.com/

using System;
namespace Renren.Components.Caching
{
	/// <summary>
	/// Represents a cache scavenger.
	/// </summary>
    public interface ICacheScavenger
    {
		/// <summary>
		/// Starts the scavenging process.
		/// </summary>
        void StartScavenging();
    }
}
