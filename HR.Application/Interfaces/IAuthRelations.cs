using HR.Application.Response;

namespace HR.Application.Interfaces
{
 public interface IAuthRelations
 {
  Task<ApiResponse<string?>> Addpagestorole ( string username );
 }
}
