var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

app.MapGet("/", (HttpContext httpContext) =>
    {
        string http_request_path = httpContext.Request.Path;
        string htt_request_method = httpContext.Request.Method;

        // Modification of the response is possible
        // httpContext.Response.StatusCode = 404;
        return "The Request Path of the HTTP Context is : " + http_request_path + "\nMethod : " + htt_request_method;
    }
);

app.Run();
