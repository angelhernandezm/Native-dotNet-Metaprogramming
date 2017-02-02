using System;
using System.Runtime.Remoting.Contexts;
using System.Runtime.Remoting.Messaging;

namespace Metadata.Core {
	public class ContextProperty : IContextProperty, IContributeObjectSink {

		/// <summary>
		/// Called when the context is frozen.
		/// </summary>
		/// <param name="newContext">The context to freeze.</param>
		/// <PermissionSet>
		///   <IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="Infrastructure" />
		///   </PermissionSet>
		/// <exception cref="System.NotImplementedException"></exception>
		public void Freeze(Context newContext) {
			
		}

		/// <summary>
		/// Returns a Boolean value indicating whether the context property is compatible with the new context.
		/// </summary>
		/// <param name="newCtx">The new context in which the <see cref="T:System.Runtime.Remoting.Contexts.ContextProperty" /> has been created.</param>
		/// <returns>
		/// true if the context property can coexist with the other context properties in the given context; otherwise, false.
		/// </returns>
		/// <exception cref="System.NotImplementedException"></exception>
		public bool IsNewContextOK(Context newCtx) {
			return (newCtx.GetProperty(Consts.ContextName) as ContextProperty != null);
		}

		/// <summary>
		/// Gets the name of the property under which it will be added to the context.
		/// </summary>
		/// <returns>The name of the property.</returns>
		///   <PermissionSet>
		///   <IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="Infrastructure" />
		///   </PermissionSet>
		public string Name {
			get {
				return Consts.ContextName;
			}
		}

		/// <summary>
		/// Chains the message sink of the provided server object in front of the given sink chain.
		/// </summary>
		/// <param name="obj">The server object which provides the message sink that is to be chained in front of the given chain.</param>
		/// <param name="nextSink">The chain of sinks composed so far.</param>
		/// <returns>
		/// The composite sink chain.
		/// </returns>
		/// <exception cref="System.NotImplementedException"></exception>
		public IMessageSink GetObjectSink(MarshalByRefObject obj, IMessageSink nextSink) {
			return (new MessageSink(nextSink));
		}
	}
}
