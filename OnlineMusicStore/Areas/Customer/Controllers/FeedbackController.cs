using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using OnlineMusic.Models.ViewModels;
using System.Text;

namespace OnlineMusicStore.Areas.Customer.Controllers
{
    [Area("Customer")]
    [Authorize]
    public class FeedbackController : Controller
    {
        private readonly HttpClient _httpClient;
        Uri baseAddress = new Uri("https://localhost:7194/api");
        public FeedbackController()
        {
            _httpClient = new HttpClient();
            _httpClient.BaseAddress = baseAddress;
        }
        [HttpGet]
        public IActionResult Index()
        {
            List<FeedbackVM> feedbackList = new List<FeedbackVM>();
            HttpResponseMessage response = _httpClient.GetAsync(_httpClient.BaseAddress + "/Feedback/GetFeedback").Result;

            if (response.IsSuccessStatusCode)
            {
                string data = response.Content.ReadAsStringAsync().Result;
                feedbackList = JsonConvert.DeserializeObject<List<FeedbackVM>>(data);
            }
            return View(feedbackList);
        }
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Create(FeedbackVM model)
        {
            try
            {
                string data = JsonConvert.SerializeObject(model);
                StringContent content = new StringContent(data, Encoding.UTF8, "application/json");
                HttpResponseMessage response = _httpClient.PostAsync(_httpClient.BaseAddress + "/Feedback/AddFeedback", content).Result;
                if (response.IsSuccessStatusCode)
                {
                    TempData["successMessage"] = "Feedback Added";
                    return RedirectToAction("Index");
                }
            }
            catch (Exception ex)
            {
                TempData["Error"] = "Invalid Id";
                TempData["errorMessage"] = ex.Message;
                return View();
            }
            return View();


        }   
    }
}
