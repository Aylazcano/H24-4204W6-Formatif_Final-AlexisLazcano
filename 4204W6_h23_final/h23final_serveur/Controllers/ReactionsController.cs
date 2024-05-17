using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using h23final_serveur.Data;
using h23final_serveur.Models;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;

namespace h23final_serveur.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class ReactionsController : ControllerBase
    {
        private readonly h23final_serveurContext _context;

        public ReactionsController(h23final_serveurContext context)
        {
            _context = context;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetReactionPicture(int id)
        {
            if(_context.Reaction == null)
            {
                return Problem("Entity set 'h23final_serveurContext.Reaction' is null.");
            }

            Reaction? reaction = await _context.Reaction.FindAsync(id);
            if(reaction == null)
            {
                return NotFound(new { Message = "Cette réaction n'existe pas." });
            }

            byte[] bytes = System.IO.File.ReadAllBytes(Directory.GetCurrentDirectory() + "/images/reactions/" + reaction.FileName);
            return File(bytes, reaction.MimeType);
        }

        [HttpPut("{id}")]
        [Authorize]
        public async Task<IActionResult> ToggleReaction(int id)
        {
            if (_context.Reaction == null || _context.Users == null)
            {
                return Problem("Entity set 'h23final_serveurContext.Reaction' is null.");
            }

            // ███ À compléter ███
			
            Reaction? reaction = null;

            /*if (reaction == null || user == null)
            {
                return NotFound("Réaction non trouvée ou utilisateur non trouvé !");
            }*/

            // ███ À compléter ███

            return Ok(reaction);
        }

        [HttpPost("{messageId}")]
        [Authorize]
        public async Task<ActionResult<Reaction>> PostReaction(int messageId)
        {
            if (_context.Reaction == null || _context.Message == null)
            {
                return Problem("Entity set 'h23final_serveurContext.Reaction'  is null.");
            }

            User? user = await _context.Users.FindAsync(User.FindFirstValue(ClaimTypes.NameIdentifier));
            Message? message = await _context.Message.FindAsync(messageId);

            if(user == null || message == null)
            {
                return NotFound(new { Message = "Le message n'existe pas." });
            }

            try
            {
                IFormCollection formCollection = await Request.ReadFormAsync();
                IFormFile? file = formCollection.Files.GetFile("imageReaction");
                if(file != null)
                {
                    Image image = Image.Load(file.OpenReadStream());

                    Reaction reaction = new Reaction()
                    {
                        Id = 0,
                        Quantity = 1,
                        MimeType = file.ContentType,
                        FileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName),
                        Message = message,
                        Users = new List<User>() { user }
                    };

                    image.Mutate(i => i.Resize(new ResizeOptions()
                    {
                        Mode = ResizeMode.Min,
                        Size = new Size() { Height = 30 }
                    }));
                    image.Save(Directory.GetCurrentDirectory() + "/images/reactions/" + reaction.FileName);

                    _context.Reaction.Add(reaction);
                    await _context.SaveChangesAsync();
                    return Ok(reaction);
                }
                else
                {
                    return NotFound(new { Message = "Aucune image fournie." });
                }
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
