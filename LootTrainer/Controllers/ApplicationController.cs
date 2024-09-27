using AutoMapper;
using Core.DTOs.ApplicationForm;
using Core.Services.AppFormService;
using Core.Utilities;
using Infrastructure.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace LootTrainer.Controllers
{
    [Route("applications")]
    [Authorize(AuthenticationSchemes = "Blizzard")]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public class ApplicationController : BaseController
    {
        private readonly UserManager<BlizzUser> _userManager;
        private readonly IMapper _mapper;
        private readonly IAppFormService _formService;

        public ApplicationController(UserManager<BlizzUser> userManager, IMapper mapper, IAppFormService formService)
        {
            _userManager = userManager;
            _mapper = mapper;
            _formService = formService;
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

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<AppFormViewModel>> Get()
        {
            var applications = await _formService.GetAll();            

            return _mapper.Map<AppFormViewModel>(applications);
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
            int battleNetId = int.Parse(User.Claims.FirstOrDefault(c => c.Type == Claims.BattleNetId)!.Value);

            applicationModel.ApplicantId = user.Id;
            applicationModel.BattleNetId = battleNetId;

            var createdApp = await _formService.Create(applicationModel);

            return _mapper.Map<AppFormViewModel>(createdApp);
        }        
    }
}
