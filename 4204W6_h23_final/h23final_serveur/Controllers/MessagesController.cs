using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using h23final_serveur.Data;
using h23final_serveur.Models;
using h23final_serveur.DTOs;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using static System.Collections.Specialized.BitVector32;

namespace h23final_serveur.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class MessagesController : ControllerBase
    {
        private readonly h23final_serveurContext _context;

        public MessagesController(h23final_serveurContext context)
        {
            _context = context;
        }

        [HttpGet("{channelId}")]
        public async Task<IActionResult> GetChannelMessages(int channelId)
        {
            if(_context.Message == null || _context.Channel == null)
            {
                return Problem("Entity set 'h23final_serveurContext.Message' is null.");
            }

            Channel? channel = await _context.Channel.FindAsync(channelId);
            if(channel == null || channel.Messages == null)
            {
                return Ok(new List<Message>());
            }
            else
            {
                // Les messages du channel sont transformés en MessageDTOs, pour ajouter le nom de l'utilisateur pour chaque message.
                return Ok(channel.Messages.Select(x => new MessageDTO() { Id = x.Id, Text = x.Text, SentAt = x.SentAt, UserName = x.User.UserName, Reactions = x.Reactions }));
            }
        }

        // POST: api/Messages
        [HttpPost]
        [Authorize]
        public async Task<ActionResult> PostMessage(PostMessageDTO messageDTO)
        {
            if (_context.Message == null || _context.Channel == null)
            {
                return Problem("Entity set 'h23final_serveurContext.Message' is null.");
            }

            User? user = await _context.Users.FindAsync(User.FindFirstValue(ClaimTypes.NameIdentifier));
            Channel? channel = await _context.Channel.FindAsync(messageDTO.ChannelId);

            if(channel == null || user == null)
            {
                return NotFound(new { Message = "Ce channel n'existe pas." });
            }

            Message message = new Message() { Id = 0, Text = messageDTO.Text, Channel = channel, User = user, SentAt = DateTime.Now };

            _context.Message.Add(message);
            await _context.SaveChangesAsync();

            MessageDTO returnMessage = new MessageDTO() { Id = message.Id, Text = message.Text, SentAt = message.SentAt, UserName = message.User.UserName, Reactions = message.Reactions };

            return Ok(returnMessage);
        }

        // DELETE: api/Messages/5
        [HttpDelete("{id}")]
        [Authorize]
        public async Task<IActionResult> DeleteMessage(int id)
        {
            if (_context.Message == null)
            {
                return NotFound();
            }

            // ███ Ajouter du code ici ███
            Message? message = null; // Ce message vide devra être remplacé par un vrai message

            // Code pour supprimer toutes les réactions du message. Ne pas toucher.
            if (message.Reactions != null)
            {
                for(int i = message.Reactions.Count - 1; i >= 0; i--)
                {
                    Reaction reaction = message.Reactions[i];
                    System.IO.File.Delete(Directory.GetCurrentDirectory() + "/images/reactions/" + reaction.FileName);
                    _context.Reaction.Remove(reaction);
                }
            }

            // ███ Ajouter du code ici ███

            return NoContent();
        }
    }
}
