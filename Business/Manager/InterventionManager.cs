using AutoMapper;
using Business.Abstraction.Exceptions;
using Business.Abstraction.Manager;
using Business.Abstraction.Models;
using Business.Extensions;
using Business.Validators;
using DataAccess.Abstraction;
using DataAccess.Abstraction.Entities;
using DataAccess.Abstraction.Entity;
using DataAccess.Abstraction.Repositories;
using FluentValidation;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Caching.Memory;

namespace Business.Manager
{
    public class InterventionManager : IInterventionManager
    {
        private readonly IInterventionRepository _interventionRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IMemoryCache _memoryCache;
        private readonly IValidator<InterventionModel> _interventionValidator;
        private readonly UserManager<UserEntity> _userManager;

        public InterventionManager(
            IInterventionRepository interventionRepository,
            IUnitOfWork unitOfWork,
            IMapper mapper,
            IMemoryCache memoryCache,
            IValidator<InterventionModel> interventionValidator,
            UserManager<UserEntity> userManager)
        {
            _interventionRepository = interventionRepository;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _memoryCache = memoryCache;
            _interventionValidator = interventionValidator;
            _userManager = userManager;

        }

        public async Task<GetInterventionModel> GetByIdAsync(long interventionId)
        {
            string cacheKey = $"intervention_{interventionId}";

            if (_memoryCache.TryGetValue(cacheKey, out GetInterventionModel cachedArticle))
            {
                return cachedArticle;
            }

            InterventionEntity interventionEntity = await _interventionRepository.GetInterventionWithClientAndTechniciansAsync(interventionId)
                ?? throw new ResourceNotFoundException<GetInterventionModel>(interventionId);

            GetInterventionModel result = _mapper.Map<GetInterventionModel>(interventionEntity);

            var cacheEntryOptions = new MemoryCacheEntryOptions()
                .SetSlidingExpiration(TimeSpan.FromMinutes(5))
                .SetAbsoluteExpiration(TimeSpan.FromMinutes(30));

            _memoryCache.Set(cacheKey, result, cacheEntryOptions);

            return result;
        }

        public async Task<long> CreateAsync(InterventionModel intervention, string username)
        {

            await _interventionValidator.ValidateAndThrowAsync(intervention, ValidationContextType.CREATE);
           
            UserEntity client = (await _userManager.FindByNameAsync(intervention.ClientName))!;

            List<UserEntity> technicians = new List<UserEntity>();

            foreach ( string TechniciansName in intervention.TechniciansNames )
            {
                UserEntity technician = (await _userManager.FindByNameAsync(TechniciansName))!;
                technicians.Add(technician);
            }

            InterventionEntity interventionEntity = new()
            {
                Name = intervention.Name,
                ServiceType = intervention.ServiceType,
                MaterialType = intervention.MaterialType,
                Technician = technicians,
                Client = client,
                CreatedBy = username,
                CreatedOn = DateTime.UtcNow,
                UpdatedBy = username
            };

            await _interventionRepository.AddAsync(interventionEntity);
            await _unitOfWork.CompleteAsync();

            return interventionEntity.Id;
        }

        public async Task<long> UpdateAsync(long interventionId, InterventionModel intervention, string username)
        {

            await _interventionValidator.ValidateAndThrowAsync(intervention, ValidationContextType.UPDATE);

            InterventionEntity interventionEntity = await _interventionRepository.GetByIdAsync(interventionId);

            if (interventionEntity is null)
                throw new ResourceNotFoundException<InterventionEntity>(interventionId);

            UserEntity client = (await _userManager.FindByNameAsync(intervention.ClientName))!;

            List<UserEntity> technicians = new List<UserEntity>();

            foreach (string TechniciansName in intervention.TechniciansNames)
            {
                UserEntity technician = (await _userManager.FindByNameAsync(TechniciansName))!;
                technicians.Add(technician);
            }

            interventionEntity.Name = intervention.Name;
            interventionEntity.ServiceType = intervention.ServiceType;
            interventionEntity.MaterialType = intervention.MaterialType;
            interventionEntity.Client = client;
            interventionEntity.Technician = technicians;
            interventionEntity.UpdatedBy = username;
            interventionEntity.UpdatedOn = DateTime.UtcNow;

            _interventionRepository.Update(interventionEntity);
            await _unitOfWork.CompleteAsync();

            // Invalide le cache
            _memoryCache.Remove($"intervention_{interventionId}");

            return interventionEntity.Id;
        }

        public async Task DeleteAsync(long interventionId)
        {
            var interventionEntity = await _interventionRepository.GetByIdAsync(interventionId);

            if (interventionEntity is null)
                throw new ResourceNotFoundException<InterventionEntity>(interventionId);

            interventionEntity.Client = null;

            _interventionRepository.Delete(interventionEntity);
            await _unitOfWork.CompleteAsync();

            // Invalide le cache
            _memoryCache.Remove($"intervention_{interventionId}");
        }


        public async Task<List<InterventionModel>> SearchAsync(bool isAdmin,string? username)
        {
            if(isAdmin)
            {
                var interventions = await _interventionRepository.GetAsync();
                var interventionModels = _mapper.Map<List<InterventionModel>>(interventions);

                return interventionModels;
            }
            else
            {
                var interventions = await _interventionRepository.GetAsync(i => i.Technician.Any(t => t.UserName.ToLower() == username.ToLower()));
                var interventionModels = _mapper.Map<List<InterventionModel>>(interventions);

                return interventionModels;
            }
        }
    }
}
