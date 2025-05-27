using AutoMapper;
using LibraryManagement.DTOs;
using LibraryManagement.DTOs.Request;
using LibraryManagement.Models;

namespace LibraryManagement.MappingProfiles
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<Book, BookDto>()
                .ForMember(dest => dest.CategoryName, opt => opt.MapFrom(src => src.Category.Name));

            CreateMap<CreateBookRequest, Book>();
            CreateMap<UpdateBookRequest, Book>();

            CreateMap<Borrower, BorrowerDto>();
            CreateMap<BorrowBookRequest, Borrower>();

            CreateMap<BorrowRecord, BorrowRecordDto>()
                .ForMember(dest => dest.BookTitle, opt => opt.MapFrom(src => src.Book.Title))
                .ForMember(dest => dest.BorrowerName, opt => opt.MapFrom(src => src.Borrower.FullName));

            CreateMap<Category, CategoryDto>();
            CreateMap<CreateCategoryRequest, Category>();

            CreateMap<User, UserDto>().ReverseMap();
            CreateMap<CreateUserRequest, User>();
            CreateMap<UpdateUserRequest, User>();
        }
    }
}
