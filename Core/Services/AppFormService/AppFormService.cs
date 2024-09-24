using AutoMapper;
using Core.BaseService;
using Core.DTOs.ApplicationForm;
using Core.Repositories.GuildApplicationRepo;
using Infrastructure.Models;

namespace Core.Services.AppFormService
{
    public class AppFormService : SimpleService<GuildApplication, AppFormModel, AppFormAddModel, AppFormEditModel>, IAppFormService
    {
        public AppFormService(IApplicationRepository repository, IMapper mapper) : base(repository, mapper)
        {
        }
    }
}
