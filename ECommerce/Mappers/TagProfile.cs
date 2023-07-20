using AutoMapper;
using ECommerce.Models.ViewModels;
using ECommerce.Models;

namespace ECommerce.Mappers
{
    public class TagProfile : Profile
    {
        public TagProfile()
        {
            CreateMap<AddTagViewModel, Tag>()
               .ForMember(viewModel => viewModel.Name, opt => opt.MapFrom(viewModel => viewModel.Name))
               .ReverseMap();
        }
    }
}
