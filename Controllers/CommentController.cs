using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Concurrent;

namespace WebApplication1.Controllers
{
    [Route("api/comments")]
    [ApiController]
    public class CommentController : ControllerBase
    {
        private static ConcurrentDictionary<string, DateTime> RequestIds = new ConcurrentDictionary<string, DateTime>();

        [HttpPost]
        public async Task<IActionResult> CreateComment([FromBody] string commentText)
        {
            string requestId = Guid.NewGuid().ToString();

            // Эмулируем задержку перед записью в базу данных
            await Sleep();

            RequestIds.TryAdd(requestId, DateTime.UtcNow.AddSeconds(120));

            return Accepted(requestId);
        }

        private static async Task Sleep()
        {
            Random random = new Random();
            int delaySeconds = random.Next(10, 16);
            await Task.Delay(delaySeconds * 1000);
        }

        [HttpGet("{requestId}")]
        public IActionResult GetCommentStatus(string requestId)
        {
            if (RequestIds.TryGetValue(requestId, out DateTime expirationTime) && DateTime.UtcNow <= expirationTime)
            {
                return Accepted();
            }
            else
            {
                return NotFound();
            }
        }
    }
}
