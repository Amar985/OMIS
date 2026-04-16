using FeedbackAPI.Data;
using FeedbackAPI.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FeedbackAPI.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class FeedbackController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public FeedbackController(ApplicationDbContext context)
        {
            _context = context;
        }
        [HttpGet]
        public IActionResult GetFeedback()
        {
            try
            {
                var feedbacks = _context.feedbackServices.ToList();
                if (feedbacks.Count == 0)
                {
                    return NotFound("Feedbacks not available.");

                }
                return Ok(feedbacks);
            }
            catch (Exception ex)
            {

                return BadRequest(ex.Message);
            }
        }
        [HttpGet("{Id}")]
        public IActionResult SearchFeedback(int Id)
        {
            try
            {
                var feedback = _context.feedbackServices.Find(Id);
                if (feedback == null)
                {
                    return NotFound($"Feedback details not found with Id {Id}");

                }
                return Ok(feedback);
            }


            catch (Exception ex)
            {

                return BadRequest(ex.Message);
            }
        }
        
        [HttpPost]
        public IActionResult AddFeedback(FeedbackServices model)
        {
            try
            {
                _context.feedbackServices.Add(model);
                _context.SaveChanges();
                return Ok("Feedback Created");
            }
            catch (Exception ex)
            {

                return BadRequest(ex.Message);
            }

        }

        [HttpPut]
        public IActionResult UpdateFeedback(FeedbackServices model)
        {
            try
            {
                if (model == null || model.Id == 0)
                {
                    if (model == null)
                    {
                        return BadRequest("data is invalid");
                    }
                    else if (model.Id == 0)
                    {
                        return BadRequest($"Id {model.Id} is invalid");
                    }
                }
                var feedback = _context.feedbackServices.Find(model.Id);
                if (feedback == null)
                {
                    return NotFound($"Feedback not found with Id {model.Id}");
                }
                feedback.Name = model.Name;
                feedback.Rating = model.Rating;
                feedback.Feedback = model.Feedback;
                _context.SaveChanges();
                return Ok("feedback Updated");
            }
            catch (Exception ex)
            {

                return BadRequest(ex.Message);
            }
        }
        [HttpDelete]

        public IActionResult DeleteFeedback(int Id)
        {
            try
            {
                var feedback = _context.feedbackServices.Find(Id);
                if (feedback == null)
                {
                    return NotFound($"Feedback not found with Id {Id}");

                }
                _context.feedbackServices.Remove(feedback);
                _context.SaveChanges();
                return Ok("Feedback details deleted");
            }
            catch (Exception ex)
            {

                return BadRequest(ex.Message);
            }

        }
    }
}
