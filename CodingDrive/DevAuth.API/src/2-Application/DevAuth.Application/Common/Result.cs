namespace DevAuth.Application.Common;

/// <summary>
/// 結果物件，用於封裝操作結果
/// </summary>
public sealed class Result
{
    private Result(bool isSuccess, Error error)
    {
        IsSuccess = isSuccess;
        Error = error;
    }

    /// <summary>
    /// 是否成功
    /// </summary>
    public bool IsSuccess { get; }

    /// <summary>
    /// 是否失敗
    /// </summary>
    public bool IsFailure => !IsSuccess;

    /// <summary>
    /// 錯誤資訊
    /// </summary>
    public Error Error { get; }

    /// <summary>
    /// 建立成功結果
    /// </summary>
    /// <returns>成功結果</returns>
    public static Result Success() => new(true, Error.None);

    /// <summary>
    /// 建立失敗結果
    /// </summary>
    /// <param name="error">錯誤資訊</param>
    /// <returns>失敗結果</returns>
    public static Result Failure(Error error) => new(false, error);

}

/// <summary>
/// 帶值的結果物件
/// </summary>
/// <typeparam name="T">值類型</typeparam>
public sealed class Result<T>
{
    private Result(bool isSuccess, T? value, Error error)
    {
        IsSuccess = isSuccess;
        Value = value;
        Error = error;
    }

    /// <summary>
    /// 是否成功
    /// </summary>
    public bool IsSuccess { get; }

    /// <summary>
    /// 是否失敗
    /// </summary>
    public bool IsFailure => !IsSuccess;

    /// <summary>
    /// 結果值
    /// </summary>
    public T? Value { get; }

    /// <summary>
    /// 錯誤資訊
    /// </summary>
    public Error Error { get; }

    /// <summary>
    /// 建立成功結果
    /// </summary>
    /// <param name="value">結果值</param>
    /// <returns>成功結果</returns>
    public static Result<T> Success(T value) => new(true, value, Error.None);

    /// <summary>
    /// 建立失敗結果
    /// </summary>
    /// <param name="error">錯誤資訊</param>
    /// <returns>失敗結果</returns>
    public static Result<T> Failure(Error error) => new(false, default, error);

    /// <summary>
    /// 隱式轉換為 Result
    /// </summary>
    public static implicit operator Result(Result<T> result) => result.IsSuccess
        ? Result.Success()
        : Result.Failure(result.Error);
}

/// <summary>
/// 錯誤資訊
/// </summary>
public sealed record Error
{
    /// <summary>
    /// 無錯誤的預設值
    /// </summary>
    public static readonly Error None = new(string.Empty, string.Empty);

    /// <summary>
    /// 初始化錯誤資訊
    /// </summary>
    /// <param name="code">錯誤代碼</param>
    /// <param name="message">錯誤訊息</param>
    public Error(string code, string message)
    {
        Code = code;
        Message = message;
    }

    /// <summary>
    /// 錯誤代碼
    /// </summary>
    public string Code { get; }

    /// <summary>
    /// 錯誤訊息
    /// </summary>
    public string Message { get; }

    /// <summary>
    /// 隱式轉換為字串
    /// </summary>
    public static implicit operator string(Error error) => error.Code;
}