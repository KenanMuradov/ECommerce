using ECommerce.Models.ViewModels;
using ECommerce.Models;
using AutoMapper;

namespace ECommerce.Mappers
{
    public class ProductProfile : Profile
    {
        public ProductProfile()
        {
            CreateMap<AddProductViewModel, Product>()
               .ForMember(viewModel => viewModel.Name, opt => opt.MapFrom(viewModel => viewModel.Name))
               .ForMember(viewModel => viewModel.Description, opt => opt.MapFrom(viewModel => viewModel.Description))
               .ForMember(viewModel => viewModel.CategoryId, opt => opt.MapFrom(viewModel => viewModel.CategoryId))
               .ReverseMap();
        }
    }
}
