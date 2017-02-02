using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Activation;
using System.Runtime.Remoting.Contexts;
using System.Text;
using System.Threading.Tasks;

namespace Metadata.Core {
	[AttributeUsage(AttributeTargets.Class)]
	public class Enhanceable : ContextAttribute {
		/// <summary>
		/// Initializes a new instance of the <see cref="Boostable"/> class.
		/// </summary>
		public Enhanceable()
			: base(Consts.ContextName) {

		}

		/// <summary>
		/// Called when the context is frozen.
		/// </summary>
		/// <param name="newContext">The context to freeze.</param>
		/// <PermissionSet>
		///   <IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="Infrastructure" />
		///   </PermissionSet>
		public override void Freeze(Context newContext) {
			throw new NotImplementedException();
		}

		/// <summary>
		/// Adds the current context property to the given message.
		/// </summary>
		/// <param name="ctorMsg">The <see cref="T:System.Runtime.Remoting.Activation.IConstructionCallMessage" /> to which to add the context property.</param>
		/// <PermissionSet>
		///   <IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="Infrastructure" />
		///   </PermissionSet>
		public override void GetPropertiesForNewContext(IConstructionCallMessage ctorMsg) {
			ctorMsg.ContextProperties.Add(new ContextProperty());
		}

		/// <summary>
		/// Returns a Boolean value indicating whether the context parameter meets the context attribute's requirements.
		/// </summary>
		/// <param name="ctx">The context in which to check.</param>
		/// <param name="ctorMsg">The <see cref="T:System.Runtime.Remoting.Activation.IConstructionCallMessage" /> to which to add the context property.</param>
		/// <returns>
		/// true if the passed in context is okay; otherwise, false.
		/// </returns>
		/// <PermissionSet>
		///   <IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="Infrastructure" />
		///   </PermissionSet>
		public override bool IsContextOK(Context ctx, System.Runtime.Remoting.Activation.IConstructionCallMessage ctorMsg) {
			return (ctx.GetProperty(Consts.ContextName) as ContextProperty != null);
		}

		/// <summary>
		/// Returns a Boolean value indicating whether the context property is compatible with the new context.
		/// </summary>
		/// <param name="newCtx">The new context in which the property has been created.</param>
		/// <returns>
		/// true if the context property is okay with the new context; otherwise, false.
		/// </returns>
		/// <PermissionSet>
		///   <IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="Infrastructure" />
		///   </PermissionSet>
		public override bool IsNewContextOK(Context newCtx) {
			return (newCtx.GetProperty(Consts.ContextName) as ContextProperty != null);
		}
	}
}
