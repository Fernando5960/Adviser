

using Microsoft.AspNetCore.Mvc;

namespace Adviser.Controllers
{
    public class HomeController : Controller
    {
        private ILogger _logger;
        private IConfiguration _configuration;
        public HomeController(ILogger<HomeController> logger, IConfiguration configuration)
        {
            _logger = logger;
            _configuration = configuration;
        }


        [Route("/")]
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        [Route("submitForm")]
        [RequestSizeLimit(10485760)]
        public async Task<IActionResult> SubmitForm(IFormFile file)
        {
            if (ModelState.IsValid)
            {

                if (file == null || file.Length == 0)
                {
                    _logger.Log(LogLevel.Information, "Nenhum arquivo enviado.");
                    return BadRequest("Nenhum arquivo enviado.");
                }

                try
                {
                    string uploadFilePath = _configuration["UploadFilePath"]!;
                    string filePath = Path.Combine(uploadFilePath, file.FileName);
                    using (Stream fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        await file.CopyToAsync(fileStream);
                    }
                }
                catch (Exception ex)
                {
                    _logger.Log(LogLevel.Error, $"Ocorreu um erro ao processar a requisição: {ex.Message}");
                    return new StatusCodeResult(StatusCodes.Status500InternalServerError);
                }
                _logger.Log(LogLevel.Information, $"Arquivo enviado com sucesso: {file.FileName}");
                TempData["Success"] = true;
                return RedirectToAction("Index");
            }
            TempData["Success"] = null;
            return RedirectToAction("Index");
        }
    }
}