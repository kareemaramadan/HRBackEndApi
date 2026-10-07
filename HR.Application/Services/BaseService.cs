using HR.Application.Interfaces;
using HR.Application.Response;
using Microsoft.EntityFrameworkCore.Query;
using System.Linq.Expressions;

namespace HR.Application.Services
{
    public class BaseService<T>(IBaseRepository<T> _repository, ILocalizationService localization) : IBaseService<T>
        where T : class
    {

        //Main CRUD Operations
        //=====================
        //1) GetAllAsync
        //=================

        public async Task<ApiResponse<IEnumerable<T>>> GetAllAsync()
        {
            ApiResponse<IEnumerable<T>> response = new()
            {
                Success = false,
                Data = null,
                Language = localization.GetLanguage()
            };
            var items = await _repository.GetAllAsync();
            if (!items.Any())
            {
                response.Message = localization.Get("itemsNotFound");
                return response;
            }
            response.Success = true;
            response.Message = localization.Get("itemsRetrieved");
            response.Data = [.. items];
            return response;
        }
        public async Task<ApiResponse<IEnumerable<T>>> GetAllAsync(Expression<Func<T, bool>> criteria)
        {
            ApiResponse<IEnumerable<T>> response = new()
            {
                Success = false,
                Data = null,
                Language = localization.GetLanguage()
            };
            var items = await _repository.GetAllAsync(criteria);
            if (!items.Any())
            {
                response.Message = localization.Get("itemsNotFound");
                return response;
            }
            response.Success = true;
            response.Message = localization.Get("itemsRetrieved");
            response.Data = [.. items];
            return response;
        }
        public async Task<ApiResponse<IEnumerable<T>>> GetAllAsync(Expression<Func<T, bool>>? filter,
              Func<IQueryable<T>, IIncludableQueryable<T, object>>? include)
        {
            ApiResponse<IEnumerable<T>> response = new()
            {
                Success = false,
                Data = null,
                Language = localization.GetLanguage()
            };
            var items = await _repository.GetAllAsync(filter, include);
            if (!items.Any())
            {
                response.Message = localization.Get("itemsNotFound");
                response.Data = [.. items];
                return response;
            }
            response.Success = true;
            response.Message = localization.Get("itemsRetrieved");
            response.Data = [.. items];
            return response;

        }

        //2) CreateAsync
        //==================

        public async Task<ApiResponse<T>> CreateAsync(T entity)
        {
            ApiResponse<T> response = new()
            {
                Success = false,
                Data = null,
                Language = localization.GetLanguage()
            };
            if (entity is null)
            {
                response.Message = localization.Get("missingfields");
                return response;
            }
            var createdEntity = await _repository.CreateAsync(entity);
            if (createdEntity != null)
            {
                response.Success = true;
                response.Data = createdEntity;
                response.Message = localization.Get("itemcreated");
                return response;
            }
            response.Message = localization.Get("creationfailed");
            return response;
        }
        public async Task<ApiResponse<IEnumerable<T>>> CreateAsyncAndGetAll(T entity)
        {
            ApiResponse<IEnumerable<T>> response = new()
            {
                Success = false,
                Data = null,
                Language = localization.GetLanguage()
            };
            if (entity == null)
            {
                response.Message = localization.Get("missingfields");
                return response;
            }
            var createdEntity = await _repository.CreateAsync(entity);
            if (createdEntity != null)
            {
                response.Success = true;
                var allItems = await GetAllAsync();
                response.Data = [.. allItems.Data!];
                response.Message = localization.Get("itemcreated");
                return response;
            }
            response.Message = localization.Get("creationfailed");
            return response;
        }
        public async Task<ApiResponse<T>> CreateAsync(T entity, Expression<Func<T, bool>> checkCriteria)
        {
            ApiResponse<bool> IsExist = await IsExistAsync(checkCriteria);
            if (IsExist.Success)
            {
                return new ApiResponse<T>
                {
                    Success = false,
                    Message = localization.Get("exists"),
                    Data = null,
                    Language = localization.GetLanguage()
                };
            }
            return new ApiResponse<T>
            {
                Success = true,
                Message = localization.Get("itemcreated"),
                Data = await _repository.CreateAsync(entity),
                Language = localization.GetLanguage()
            };
        }
        public async Task<ApiResponse<IEnumerable<T>>> CreateAsyncAndGetAll(T entity, Expression<Func<T, bool>> checkCriteria, Expression<Func<T, bool>>? getAllCriteria)
        {
            ApiResponse<IEnumerable<T>> response = new()
            {
                Success = false,
                Data = null,
                Language = localization.GetLanguage()
            };
            ApiResponse<bool> IsExist = await IsExistAsync(checkCriteria);
            if (IsExist.Success)
            {

                response.Message = localization.Get("exists");
                return response;
            }
            var createdEntity = await _repository.CreateAsync(entity);
            if (createdEntity != null)
            {
                response.Success = true;
                var allItems = (getAllCriteria is null) ? await GetAllAsync() : await GetAllAsync(getAllCriteria);
                response.Data = [.. allItems.Data!];
                response.Message = localization.Get("itemcreated");
                return response;
            }
            response.Message = localization.Get("creationfailed");
            return response;
        }
        public async Task<ApiResponse<IEnumerable<T>>> CreateAsyncAndGetAll(T entity, Expression<Func<T, bool>> checkCriteria, 
            Expression<Func<T, bool>>? getAllCriteria, Func<IQueryable<T>, IIncludableQueryable<T, object>>? include)
        {
            ApiResponse<IEnumerable<T>> response = new()
            {
                Success = false,
                Data = null,
                Language = localization.GetLanguage()
            };
            ApiResponse<bool> IsExist = await IsExistAsync(checkCriteria);
            if (IsExist.Success)
            {

                response.Message = localization.Get("exists");
                return response;
            }
            var createdEntity = await _repository.CreateAsync(entity);
            if (createdEntity != null)
            {
                response.Success = true;
                var allItems = (getAllCriteria is null) ? await GetAllAsync() : await GetAllAsync(getAllCriteria,include);
                response.Data = [.. allItems.Data!];
                response.Message = localization.Get("itemcreated");
                return response;
            }
            response.Message = localization.Get("creationfailed");
            return response;
        }
        //3) UpdateAsync
        //==================

        public async Task<ApiResponse<T>> UpdateAsync(T entity)
        {
            ApiResponse<T> response = new()
            {
                Success = false,
                Data = null,
                Language = localization.GetLanguage()
            };
            if (entity == null)
            {
                response.Message = localization.Get("missingfields");
                return response;
            }
            var updatedItem = await _repository.UpdateAsync(entity);
            if (updatedItem != null)
            {
                response.Success = true;
                response.Data = updatedItem;
                response.Message = localization.Get("itemupdated");
                return response;
            }
            response.Message = localization.Get("itemupdatedfailed");
            return response;
        }
        public async Task<ApiResponse<T>> UpdateAsync(T entity, Expression<Func<T, bool>> criteria)
        {
            ApiResponse<T> response = new()
            {
                Success = false,
                Data = null,
                Language = localization.GetLanguage()
            };
            if (entity == null)
            {
                response.Message = localization.Get("missingfields");
                return response;
            }
            var updatedItem = await _repository.UpdateAsync(entity, criteria);
            if (updatedItem != null)
            {
                response.Success = true;
                response.Data = updatedItem;
                response.Message = localization.Get("itemupdated");
                return response;
            }
            response.Message = localization.Get("itemupdatedfailed");
            return response;

        }
        public async Task<ApiResponse<IEnumerable<T>>> UpdateAsyncAndGetAll(T entity, Expression<Func<T, bool>> checkCriteria, Expression<Func<T, bool>>? getAllCriteria)
        {
            ApiResponse<IEnumerable<T>> response = new()
            {
                Success = false,
                Data = null,
                Language = localization.GetLanguage()
            };
            ApiResponse<bool> IsExist = await IsExistAsync(checkCriteria);
            if (IsExist.Success)
            {
                response.Message = localization.Get("exists");
                return response;
            }
            var createdEntity = await _repository.UpdateAsync(entity, checkCriteria);
            if (createdEntity != null)
            {
                response.Success = true;
                var allItems = (getAllCriteria is null) ? await GetAllAsync() : await GetAllAsync(getAllCriteria);
                response.Data = [.. allItems.Data!];
                response.Message = localization.Get("itemcreated");
                return response;
            }
            response.Message = localization.Get("creationfailed");
            return response;
        }

        //4) DeleteAsync
        //==================

        public async Task<ApiResponse<int>> DeleteAsync(T entity)
        {
            ApiResponse<int> response = new()
            {
                Success = false,
                Data = 0,
                Language = localization.GetLanguage()
            };
            if (entity is null)
            {
                response.Message = localization.Get("missingfields");
                return response;
            }
            var deletedItem = await _repository.DeleteAsync(entity);
            if (deletedItem > 0)
            {
                response.Success = true;
                response.Data = deletedItem;
                response.Message = localization.Get("deleted");
                return response;
            }
            response.Message = localization.Get("deletionfailed");
            return response;
        }
        public async Task<ApiResponse<int>> DeleteAsync(Expression<Func<T, bool>> criteria)
        {
            int affectedrows = await _repository.DeleteAsync(criteria);
            if (affectedrows == 0)
            {
                return new ApiResponse<int>
                {
                    Success = false,
                    Message = localization.Get("deletionfailed"),
                    Data = affectedrows,
                    Language = localization.GetLanguage()
                };
            }
            return new ApiResponse<int>
            {
                Success = true,
                Message = localization.Get("deleted"),
                Data = affectedrows,
                Language = localization.GetLanguage()
            };

        }
        public async Task<ApiResponse<IEnumerable<T>>> DeleteAsyncAndGetAll(Expression<Func<T, bool>> Criteria, Expression<Func<T, bool>>? GetAllCriteria)
        {
            ApiResponse<IEnumerable<T>> response = new()
            {
                Success = false,
                Data = null,
                Language = localization.GetLanguage()
            };
            int affectedrows = await _repository.DeleteAsync(Criteria);
            if (affectedrows == 0)
            {
                response.Message = localization.Get("deletionfailed");
                response.Data = null;
                return response;
            }
            return new ApiResponse<IEnumerable<T>>
            {
                Success = true,
                Message = localization.Get("deleted"),
                Data = [.. (GetAllCriteria is null ? await GetAllAsync() : await GetAllAsync(GetAllCriteria)).Data!],
                Language = localization.GetLanguage()
            };
        }
        public async Task<ApiResponse<IEnumerable<T>>> DeleteAsyncAndGetAll(Expression<Func<T, bool>>? filter,
        Expression<Func<T, bool>>? GetAllCriteria, Func<IQueryable<T>, IIncludableQueryable<T, object>>? include)
        {
            ApiResponse<IEnumerable<T>> response = new()
            {
                Success = false,
                Data = null,
                Language = localization.GetLanguage()
            };
            int affectedrows = await _repository.DeleteAsync(filter!);
            if (affectedrows == 0)
            {
                response.Message = localization.Get("deletionfailed");
                response.Data = null;
                return response;
            }
            return new ApiResponse<IEnumerable<T>>
            {
                Success = true,
                Message = localization.Get("deleted"),
                Data = [.. (GetAllCriteria is null ? await GetAllAsync() : await GetAllAsync(GetAllCriteria, include)).Data!],
                Language = localization.GetLanguage()
            };
        }
        //Extra Functions for count and checking
        //=======================================

        public async Task<ApiResponse<bool>> IsExistAsync(Expression<Func<T, bool>> criteria)
        {
            ApiResponse<bool> response = new()
            {
                Success = false,
                Data = false,
                Language = localization.GetLanguage()
            };

            ApiResponse<int> count = await CountAsync(criteria);

            if (count.Data > 0)
            {
                response.Success = true;
                response.Data = true;
                response.Message = localization.Get("exists");
                return response;
            }
            response.Message = localization.Get("itemNotFound");
            return response;
        }
        public async Task<ApiResponse<int>> CountAsync()
        {
            return new ApiResponse<int>
            {
                Success = true,
                Message = localization.Get("itemscount"),
                Data = await _repository.CountAsync(),
                Language = localization.GetLanguage()
            };
        }
        public async Task<ApiResponse<int>> CountAsync(Expression<Func<T, bool>> criteria)
        {
            return new ApiResponse<int>
            {
                Success = true,
                Message = localization.Get("itemscount"),
                Data = await _repository.CountAsync(criteria),
                Language = localization.GetLanguage()
            };
        }
        public async Task<ApiResponse<IEnumerable<T>>> FindAsync(Expression<Func<T, bool>> criteria)
        {
            var items = await _repository.FindAsync(criteria);
            if (!items.Any())
            {
                return new ApiResponse<IEnumerable<T>>
                {
                    Success = false,
                    Message = localization.Get("itemNotFound"),
                    Data = [.. items],
                    Language = localization.GetLanguage()

                };
            }
            return new ApiResponse<IEnumerable<T>>
            {
                Success = true,
                Message = localization.Get("itemRetrieved"),
                Data = [.. items],
                Language = localization.GetLanguage()
            };
        }
        public async Task<ApiResponse<T>> FindItemAsync(Expression<Func<T, bool>> criteria)
        {
            T item = await _repository.FindItemAsync(criteria);
            if (item is null)
            {
                return new ApiResponse<T>
                {
                    Success = false,
                    Message = localization.Get("itemNotFound"),
                    Data = null,
                    Language = localization.GetLanguage()

                };
            }
            return new ApiResponse<T>
            {
                Success = true,
                Message = localization.Get("itemRetrieved"),
                Data = item,
                Language = localization.GetLanguage()
            };
        }

        //Using StoredProcedures
        //=======================
        public async Task<ApiResponse<IEnumerable<T>>> GetUsingStoredProcedureAsync(string spName, Dictionary<string, object> parameters)
        {
            if (string.IsNullOrWhiteSpace(spName) || parameters == null)
            {
                return new ApiResponse<IEnumerable<T>>
                {
                    Success = false,
                    Message = localization.Get("missingfields"),
                    Data = null,
                    Language = localization.GetLanguage()
                };
            }
            return new ApiResponse<IEnumerable<T>>
            {
                Success = true,
                Message = localization.Get("itemsRetrieved"),
                Data = await _repository.GetUsingStoredProcedureAsync(spName, parameters),
                Language = localization.GetLanguage()
            };
        }
        public async Task<ApiResponse<int>> CUDUsingStoredProcedureAsync(string spName, Dictionary<string, object> parameters, Expression<Func<T, bool>> checkCriteria)
        {
            ApiResponse<int> response = new()
            {
                Success = false,
                Data = 0,
                Language = localization.GetLanguage()
            };
            if (string.IsNullOrWhiteSpace(spName) || parameters == null)
            {
                response.Message = localization.Get("missingfields");
                return response;
            }
         ;
            ApiResponse<bool> IsExist = await IsExistAsync(checkCriteria);
            if (!IsExist.Success)
            {
                response.Success = false;
                response.Message = localization.Get("itemapplyfailed");
                response.Data = 0;
                return response;
            }
            response.Success = true;
            response.Message = localization.Get("itemapply");
            response.Data = await _repository.CUDUsingStoredProcedureAsync(spName, parameters);
            return response;
        }

    }
}
