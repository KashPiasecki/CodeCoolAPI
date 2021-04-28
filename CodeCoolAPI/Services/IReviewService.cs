﻿using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using CodeCoolAPI.CustomExceptions;
using CodeCoolAPI.DAL.Models;
using CodeCoolAPI.DAL.UnitOfWork;
using CodeCoolAPI.Dtos;

namespace CodeCoolAPI.Services
{
    public interface IReviewService
    {
        Task<ReviewReadDto> ReadReviewById(int id);
        Task<IEnumerable<ReviewReadDto>> ReadAllReviews();
        Task<ReviewReadDto> CreateReviewReadDto(ReviewUpsertDto reviewUpsertDto, string userId);
        Task UpdateReview(int id, ReviewUpsertDto reviewUpsertDto);
        Task DeleteReview(int id);
        Task UserOwnsPost(int id, string userId);
    }

    class ReviewService : IReviewService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public ReviewService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        
        public async Task<ReviewReadDto> ReadReviewById(int id)
        {
            var review = await FindReview(id);
            var reviewReadDto = _mapper.Map<ReviewReadDto>(review);
            return reviewReadDto;
        }

        public async Task<IEnumerable<ReviewReadDto>> ReadAllReviews()
        {
            var reviewList = await _unitOfWork.Reviews.FindAll();
            if (!reviewList.Any())
                throw new NotFoundException("Review list is empty");

            var reviewReadDtoList = _mapper.Map<IEnumerable<ReviewReadDto>>(reviewList);
            return reviewReadDtoList;
        }

        public async Task<ReviewReadDto> CreateReviewReadDto(ReviewUpsertDto reviewUpsertDto, string userId)
        {
            var review = _mapper.Map<Review>(reviewUpsertDto);
            review.UserId = userId;
            await _unitOfWork.Reviews.Create(review);
            await _unitOfWork.Save();
            return _mapper.Map<ReviewReadDto>(review);
        }

        public async Task UpdateReview(int id, ReviewUpsertDto reviewUpsertDto)
        {
            var review = await FindReview(id);
            _mapper.Map(reviewUpsertDto, review);
            await _unitOfWork.Reviews.Update(review);
            await _unitOfWork.Save();
        }

        public async  Task DeleteReview(int id)
        {
            var review = await FindReview(id);
            await _unitOfWork.Reviews.Delete(review);
            await _unitOfWork.Save();
        }

        public async Task UserOwnsPost(int id, string userId)
        {
            var review = await FindReview(id);
            if (review is null || review.UserId != userId)
                throw new BadRequestException("This review does not belong to you.");
        }

        private async Task<Review> FindReview(int id)
        {
            var review = await _unitOfWork.Reviews.Find(id);
            if (review is null)
                throw new NotFoundException("Review not found");
            return review;
        }
    }
}