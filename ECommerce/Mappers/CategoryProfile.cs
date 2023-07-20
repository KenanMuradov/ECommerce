using AutoMapper;
using ECommerce.Models;
using ECommerce.Models.ViewModels;

namespace ECommerce.Mappers
{
    public class CategoryProfile : Profile
    {
        public CategoryProfile()
        {
            CreateMap<AddCategoryViewModel, Category>()
                .ForMember(viewModel => viewModel.Name, opt => opt.MapFrom(viewModel => viewModel.Name))
                .ReverseMap();
        }
    }
}
