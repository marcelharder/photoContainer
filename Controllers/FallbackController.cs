namespace api.Controllers;

public class FallbackController : Controller
{
    public IActionResult Index()
    {
        string contentType = "text/HTML";
        return PhysicalFile(
            Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "index.html"),
            contentType
        );
    }
}
