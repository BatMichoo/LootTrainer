using Core.BaseService;
using Core.DTOs.ApplicationForm;
using Infrastructure.Models;

namespace Core.Services.AppFormService
{
    public interface IAppFormService : ISimpleService<GuildApplication, AppFormModel, AppFormAddModel, AppFormEditModel>
    {
    }
}
