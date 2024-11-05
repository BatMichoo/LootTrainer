using Core.BaseService;
using Core.DTOs.ApplicationForm;
using Infrastructure.Models.GuildApplications;

namespace Core.Services.AppFormService
{
    public interface IAppFormService : ISimpleService<GuildApplication, AppFormModel, AppFormAddModel, AppFormEditModel>
    {
    }
}
