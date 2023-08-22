using Microsoft.AspNetCore.Mvc;

namespace WebApplication1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CommentController : ControllerBase
    {
        private static readonly Dictionary<Guid, Comment> commentStorage = new Dictionary<Guid, Comment>();

        [HttpPost]
        public ActionResult<Guid> CreateComment([FromBody] Comment comment)
        {
            Guid requestId = Guid.NewGuid();
            Task.Run(async () =>
            {
                await Task.Delay(new Random().Next(10, 16) * 1000);
                commentStorage.Add(requestId, comment);
                comment.DateAdded = DateTime.Now;
            });

            return Accepted(requestId);
        }

        [HttpGet("{id}")]
        public ActionResult<Comment> GetComment(Guid id)
        {
            if (commentStorage.TryGetValue(id, out Comment comment))
            {
                return comment;
            }
            else
            {
                return NotFound();
            }
        }
    }

    public class Comment
    {
        public string Text { get; set; }
        public DateTime DatePost { get; set; } = DateTime.Now;
        public DateTime DateAdded { get; set; }
    }
}
