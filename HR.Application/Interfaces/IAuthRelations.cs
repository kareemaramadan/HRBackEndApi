using HR.Application.Response;

namespace HR.Application.Interfaces
{
 public interface IAuthRelations
 {
  Task<ApiResponse<string?>> AddUserToRole ( string username,string roleId );
  Task<ApiResponse<string?>> RemoveUserFromRole ( string username,string roleId );
  Task<ApiResponse<string?>> AddSubModuleToRole ( string roleId,int ModuleId,int SubModuleId );
  Task<ApiResponse<string?>> RemoveSubModuleFromRole ( string roleName,int ModuleId,int SubModuleId );
  Task<ApiResponse<string>> AddPageToSubModule ( int SubmoduleId,int PageId );
  Task<ApiResponse<string>> RemovePageFromSubModule ( int SubmoduleId,int PageId );
 }
}
