using System.ComponentModel.DataAnnotations;

namespace AuthenticationJWT.DTO
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="UserName"></param>
    /// <param name="Password"></param>
    public record UserValidationRequestModel([Required] string UserName, [Required] string Password);
}
