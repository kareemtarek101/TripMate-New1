using Microsoft.AspNetCore.Mvc;
using TripMate.Application;
using TripMate.Application.Services;
using TripMate.Application.Interface;

namespace TripMate.Api.Controllers
{
    [ApiController]
    [Route("api/destinations")]
    public class DestinationsController : ControllerBase
    {
        private readonly IDestinationService _destinationService;
        private readonly IRecommendationService _recommendationService;
        private readonly IInteractionService _interactionService;

        public DestinationsController(
            IDestinationService destinationService,
            IRecommendationService recommendationService,
            IInteractionService interactionService)
        {
            _destinationService = destinationService;
            _recommendationService = recommendationService;
            _interactionService = interactionService;
        }

        // ➕ Create
        [HttpPost]
        public async Task<IActionResult> Create(CreateDestinationRequest request)
        {
            await _destinationService.CreateAsync(request);
            return Ok("Destination created");
        }

        // 📋 Get All
        [HttpGet]
        public async Task<IActionResult> GetAll(int page = 1, int pageSize = 10)
        {
            var data = await _destinationService.GetAllAsync(page, pageSize);
            return Ok(data);
        }

        // 🔍 Search
        [HttpGet("search")]
        public async Task<IActionResult> Search(string query)
        {
            var result = await _destinationService.SearchAsync(query);
            return Ok(result);
        }

        // 🎯 Filter
        [HttpGet("filter")]
        public async Task<IActionResult> Filter(string? country, string? category, decimal? budget)
        {
            var result = await _destinationService.FilterAsync(country, category, budget);

            return Ok(result);
        }

        // 🧠 Smart Destinations
        [HttpGet("smart-recommendations")]
        public async Task<IActionResult> GetSmart(int userId, decimal budget)
        {
            var result = await _recommendationService.GetSmartRecommendations(userId, budget);
            return Ok(result);
        }

        // 🔥 Smart Packages
        [HttpGet("smart-packages")]
        public async Task<IActionResult> GetSmartPackages(int userId, decimal budget, string from)
        {
            var result = await _recommendationService.GetSmartPackages(userId, budget, from);
            return Ok(result);
        }

        // 📄 Get By Id + Interaction
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id, int userId)
        {
            var destination = await _destinationService.GetByIdAsync(id);

            if (destination == null)
                return NotFound();

            await _interactionService.AddInteraction(userId, id, "View");

            return Ok(destination);
        }
    }
}