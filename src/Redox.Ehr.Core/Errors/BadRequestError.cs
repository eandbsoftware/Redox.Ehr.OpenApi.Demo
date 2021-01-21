using System.Diagnostics.CodeAnalysis;
using System.Net;
using Redox.Ehr.Core.Errors.Base;
using Microsoft.AspNetCore.Http;

namespace Redox.Ehr.Core.Errors
{
	[ExcludeFromCodeCoverage]
	public class BadRequestError : ApiError
	{
		public BadRequestError()
			: base(StatusCodes.Status400BadRequest, HttpStatusCode.BadRequest.ToString())
		{
		}

		public BadRequestError(string message)
			: base(StatusCodes.Status400BadRequest, HttpStatusCode.BadRequest.ToString(), message)
		{
		}
	}
}
