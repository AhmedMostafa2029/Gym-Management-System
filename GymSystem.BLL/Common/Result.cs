using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymSystem.BLL.Common
{
    public enum RuseltKind
    {
        Ok,
        NotFound,
        Conflict,
        ValidationFailed,
        Forbidden
    }

    public record Result(bool IsSuccess, string? Error = null, RuseltKind Kind = RuseltKind.Ok)
    {
        public static Result Ok() => new Result(true);
        public static Result Fail(string errorMessage, RuseltKind kind = RuseltKind.Conflict) => new Result(false, errorMessage, kind);
        public static Result NotFound(string errorMessage = "Not Found") => new Result(false, errorMessage, RuseltKind.NotFound);
        public static Result Validation(string errorMessage) => new Result(false, errorMessage, RuseltKind.ValidationFailed);

    }


    public record Result<T>(bool IsSuccess, T? Value = default, string? Error = null, RuseltKind Kind = RuseltKind.Ok)
    {
        public static Result<T> Ok(T value) => new Result<T>(true, value);
        public static Result<T> Fail(string errorMessage, RuseltKind kind = RuseltKind.Conflict) => new Result<T>(false, default, errorMessage, kind);
        public static Result<T> NotFound(string errorMessage = "Not Found") => new Result<T>(false, default, errorMessage, RuseltKind.NotFound);
        public static Result<T> Validation(string errorMessage) => new Result<T>(false, default, errorMessage, RuseltKind.ValidationFailed);

    }

}
