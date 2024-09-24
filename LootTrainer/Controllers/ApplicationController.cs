using AutoMapper;
using Core.DTOs.ApplicationForm;
using Core.Services.AppFormService;
using Core.Services.BattleNet;
using Infrastructure.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace LootTrainer.Controllers
{
    [Route("applications")]
    [Authorize(AuthenticationSchemes = "Blizzard")]
    public class ApplicationController : BaseController
    {
        private readonly UserManager<BlizzUser> _userManager;
        private readonly IMapper _mapper;
        private readonly IAppFormService _formService;
        private readonly BattleNetService _bNetService;

        public ApplicationController(UserManager<BlizzUser> userManager, IMapper mapper, IAppFormService formService, BattleNetService bNetService)
        {
            _userManager = userManager;
            _mapper = mapper;
            _formService = formService;
            _bNetService = bNetService;
        }

        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<AppFormViewModel>> Get(int id)
        {
            var application = await _formService.GetById(id);

            if (application == null)
            {
                return NotFound();
            }

            return _mapper.Map<AppFormViewModel>(application);
        }

        [HttpPost]
        public async Task<ActionResult<AppFormViewModel>> Create(AppFormAddViewModel? application)
        {
            if (application == null)
            {
                return new AppFormViewModel();
            }

            var applicationModel = _mapper.Map<AppFormAddModel>(application);

            var user = await _userManager.GetUserAsync(User);

            applicationModel.ApplicantId = user.Id;

            var createdApp = await _formService.Create(applicationModel);

            return _mapper.Map<AppFormViewModel>(createdApp);
        }

        [HttpGet]
        public async Task<IActionResult> GetProfile()
        {
            var userToken = User.Claims.FirstOrDefault(c => c.Type == "Token").Value;

            return Ok(await _bNetService.Test(userToken));
        }
    }
}
