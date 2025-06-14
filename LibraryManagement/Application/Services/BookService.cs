﻿using AutoMapper;
using LibraryManagement.API.Middleware;
using LibraryManagement.Application.DTOs;
using LibraryManagement.Application.DTOs.Request;
using LibraryManagement.Application.Interfaces.Repositories;
using LibraryManagement.Application.Interfaces.Services;
using LibraryManagement.Domain.Entities;
using System.Linq;

namespace LibraryManagement.Application.Services
{
    public class BookService : IBookService
    {
        private readonly IBookRepository _bookRepository;
        private readonly ICategoryRepository _categoryRepository;
        private readonly IMapper _mapper;

        public BookService(IBookRepository bookRepository, ICategoryRepository categoryRepository, IMapper mapper)
        {
            _bookRepository = bookRepository;
            _categoryRepository = categoryRepository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<BookDto>> GetAllAsync()
        {
            var books = await _bookRepository.GetAllAsync(null);
            return _mapper.Map<IEnumerable<BookDto>>(books);
        }

        public async Task<IEnumerable<BookDto>> SearchAsync(int? categoryId, string? searchTerm)
        {
            var books = await _bookRepository.GetAllAsync(categoryId);
            
            if (!string.IsNullOrWhiteSpace(searchTerm))
            {
                searchTerm = searchTerm.ToLower();
                books = books.Where(b => 
                    b.Title.ToLower().Contains(searchTerm) || 
                    b.Author.ToLower().Contains(searchTerm));
            }

            return _mapper.Map<IEnumerable<BookDto>>(books);
        }

        public async Task<BookDto> GetByIdAsync(int id)
        {
            var book = await _bookRepository.GetByIdAsync(id);
            if (book == null)
                throw new AppException(ErrorCodes.BOOK_ID_NOT_FOUND);

            return _mapper.Map<BookDto>(book);
        }

        public async Task<EbookInfoDto?> GetEbookInfoAsync(int id)
        {
            var book = await _bookRepository.GetByIdAsync(id);
            if (book == null || string.IsNullOrEmpty(book.EbookUrl))
                return null;

            return new EbookInfoDto
            {
                BookId = book.Id,
                Title = book.Title,
                Author = book.Author,
                DownloadUrl = book.EbookUrl,
                Format = book.EbookFormat ?? "Unknown",
                Size = book.EbookSize ?? 0
            };
        }

        public async Task<BookDto> CreateAsync(CreateBookRequest request)
        {
            var category = await _categoryRepository.GetByIdAsync(request.CategoryId);
            if (category == null)
                throw new AppException(ErrorCodes.CATEGORY_ID_NOT_FOUND);

            var book = _mapper.Map<Book>(request);

            await _bookRepository.AddAsync(book);
            return _mapper.Map<BookDto>(book);
        }

        public async Task<BookDto> UpdateAsync(int id, UpdateBookRequest request)
        {
            var book = await _bookRepository.GetByIdAsync(id);
            if (book == null)
                throw new AppException(ErrorCodes.BOOK_ID_NOT_FOUND);

            var category = await _categoryRepository.GetByIdAsync(request.CategoryId);
            if (category == null)
                throw new AppException(ErrorCodes.CATEGORY_ID_NOT_FOUND);

            _mapper.Map(request, book);
            await _bookRepository.UpdateAsync(book);
            return _mapper.Map<BookDto>(book);
        }

        public async Task DeleteAsync(int id)
        {
            var book = await _bookRepository.GetByIdAsync(id);
            if (book == null)
                throw new AppException(ErrorCodes.BOOK_ID_NOT_FOUND);

            book.IsActive = false;
            await _bookRepository.UpdateAsync(book);
        }
    }
}
