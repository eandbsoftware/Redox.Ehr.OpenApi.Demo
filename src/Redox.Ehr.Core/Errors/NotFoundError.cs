using System.Diagnostics.CodeAnalysis;
using System.Net;
using Redox.Ehr.Core.Errors.Base;
using Microsoft.AspNetCore.Http;

namespace Redox.Ehr.Core.Errors
{
	[ExcludeFromCodeCoverage]
	public class NotFoundError : ApiError
	{
		public NotFoundError()
			: base(StatusCodes.Status404NotFound, HttpStatusCode.NotFound.ToString())
		{
		}
		
		public NotFoundError(string message)
			: base(StatusCodes.Status404NotFound, HttpStatusCode.NotFound.ToString(), message)
		{
		}
	}
}
