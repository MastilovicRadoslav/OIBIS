using System.ServiceModel;

namespace SecurityManager
{
	public class CustomAuthorizationManager : ServiceAuthorizationManager
	{
		protected override bool CheckAccessCore(OperationContext operationContext)
		{
			CustomPrincipal principal = operationContext.ServiceSecurityContext.
				 AuthorizationContext.Properties["Principal"] as CustomPrincipal;
			return principal.IsInRole("Read");
		}
	}
}