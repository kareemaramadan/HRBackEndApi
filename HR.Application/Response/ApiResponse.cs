using System;
using System.Collections.Generic;
using System.Text;

namespace HR.Application.Response
{
 public class ApiResponse<T>
 {
  public bool Success { get; set; }
  public string Message { get; set; } = string.Empty;
  public T? Data { get; set; }
  public string Language { get; set; } = string.Empty;
 }
}
