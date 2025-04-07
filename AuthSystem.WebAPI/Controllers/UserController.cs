using AuthSystem.Data;
using AuthSystem.DataModel;
using AuthSystem.DataModel.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AuthSystem.Controllers
{
    /// <summary>
    /// Controller handling operations on users (eg. CRUD operations).
    /// </summary>
    [Authorize]
    [Route("[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly UserManager<User> _userManager;
        private readonly AppDbContext _dbContext;
        
        public UserController(
            UserManager<User> userManager,
            AppDbContext dbContext)
        {
            _userManager = userManager;
            _dbContext = dbContext;           
        }

        /// <summary>
        /// Registers new user and adds to database.
        /// </summary>
        /// <param name="request">Request model body containing informations about user.</param>
        /// <returns><see cref="Task{IActionResult}"/> response.</returns>
        [AllowAnonymous]
        [HttpPost("register")]
        public async Task<IActionResult> PostRegister(RegisterRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            User user = new User
            {
                FirstName = request.FirstName,
                LastName = request.LastName,
                UserName = request.UserName,
                Email = request.Email              
            };

            IdentityResult result = await _userManager.CreateAsync(user, request.Password!);

            if (!result.Succeeded)
                return BadRequest(result);

            return Ok(result);
        }

        /// <summary>
        /// Deletes user from database
        /// </summary>
        /// <returns><see cref="IActionResult"/></returns>
        [HttpDelete("user")]
        public async Task<IActionResult> DeleteUser()
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            User user = (await _userManager.GetUserAsync(User))!;

            _dbContext.RefreshTokens.RemoveRange(
                _dbContext.RefreshTokens.Where(rt => rt.UserId == user.Id));

            if (user.Notes is not null)
                _dbContext.Notes.RemoveRange(user.Notes);

            await _dbContext.SaveChangesAsync();  

            await _userManager.DeleteAsync(user);

            return Ok();
        }

        /// <summary>
        /// Gets user data transfer object filled with <see cref="User"/>'s data.
        /// </summary>
        /// <returns><see cref="UserDto"/> with <see cref="User"/>'s data.</returns>
        [HttpGet("user")]
        public async Task<ActionResult<UserDto>> GetUser()
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            User user = (await _userManager.GetUserAsync(User))!;

            UserDto dto = new UserDto
            {
                Email = user.Email,
                UserName = user.UserName,
                FirstName = user.FirstName,
                LastName = user.LastName
            };

            return Ok(dto);
        }

        //[HttpDelete("note")]
        //public async Task<IActionResult> DeleteNote(Guid id)
        //{
        //    if (!ModelState.IsValid)
        //        return BadRequest(ModelState);

        //    string userId = _userManager.GetUserId(User)!;

        //    _dbContext.RemoveRange(
        //        _dbContext.Notes.Where(n => n.UserId == userId && n.Id == id));

        //    await _dbContext.SaveChangesAsync();

        //    return Ok();
        //}

        //[HttpGet("notes")]
        //public ActionResult<IEnumerable<Note>?> GetNotes()
        //{
        //    if (!ModelState.IsValid)
        //        return BadRequest(ModelState);

        //    string id = _userManager.GetUserId(User)!;
        //    IEnumerable<Note>? notes = _dbContext.Notes.Where(n => n.UserId == id);

        //    return Ok(notes);
        //}

        //[HttpPatch("note")]
        //public async Task<IActionResult> PatchNote(NoteDto noteDto)
        //{
        //    if (!ModelState.IsValid)
        //        return BadRequest(ModelState);

        //    string userId = _userManager.GetUserId(User)!;
        //    Note? note = await _dbContext.Notes.FindAsync(noteDto.Id);

        //    if (note is null)
        //        return NotFound();

        //    note.Title = noteDto.Title;
        //    note.Content = noteDto.Content;

        //    await _dbContext.SaveChangesAsync();

        //    return Ok();
        //}

        //[HttpPost("note")]
        //public async Task<IActionResult> PostNote(AddNoteRequest request)
        //{
        //    if (!ModelState.IsValid)
        //        return BadRequest(ModelState);

        //    string? title = request.Title;
        //    string? content = request.Content;

        //    if (string.IsNullOrEmpty(title))
        //        title = "Untitled";

        //    Note note = new Note
        //    {
        //        Title = title,
        //        Content = content,
        //        CreatedAt = DateTime.Now,
        //    };

        //    string id = _userManager.GetUserId(User)!;
        //    User user = (await _dbContext.Users.Where(u => u.Id == id)
        //        .Include(u => u.Notes)
        //        .FirstOrDefaultAsync())!;

        //    if (user.Notes is null)
        //        user.Notes = new List<Note>();

        //    user.Notes.Add(note);

        //    await _userManager.UpdateAsync(user);

        //    return Ok();
        //}
    }
}
