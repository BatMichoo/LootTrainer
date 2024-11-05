using AutoMapper;
using Core.DTOs.ApplicationForm;
using Infrastructure.Models.GuildApplications;

namespace Core.AutoMapper
{
    public class AppFormProfile : Profile
    {
        public AppFormProfile()
        {
            CreateMap<GuildApplication, AppFormModel>();

            CreateMap<AppFormModel, AppFormViewModel>();

            CreateMap<AppFormAddViewModel, AppFormAddModel>();

            CreateMap<AppFormAddModel, GuildApplication>();

            CreateMap<AppFormModel, GuildApplication>();
        }
    }
}
