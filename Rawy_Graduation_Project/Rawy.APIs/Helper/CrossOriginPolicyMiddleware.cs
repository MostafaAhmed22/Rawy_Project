namespace Rawy.APIs.Helper
{
	public class CrossOriginPolicyMiddleware
	{
		private readonly RequestDelegate _next;

		public CrossOriginPolicyMiddleware(RequestDelegate next)
		{
			_next = next;
		}

		public async Task InvokeAsync(HttpContext context)
		{
			// Apply only for specific endpoint, e.g. /api/accounts/google-login
			var path = context.Request.Path.Value?.ToLower();
			if (path != null && (path.Contains("google-login")))
			{
				context.Response.OnStarting(() =>
				{
					context.Response.Headers["Cross-Origin-Opener-Policy"] = "unsafe-none";
					context.Response.Headers["Cross-Origin-Embedder-Policy"] = "unsafe-none";
					return Task.CompletedTask;
				});
			}

			await _next(context);
		}
	}
}
