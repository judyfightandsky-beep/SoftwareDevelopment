using MediatR;

namespace DevAuth.Application.Common;

/// <summary>
/// 查詢介面標記
/// </summary>
/// <typeparam name="TResponse">回應類型</typeparam>
public interface IQuery<out TResponse> : IRequest<TResponse>
{
}

/// <summary>
/// 查詢處理器介面
/// </summary>
/// <typeparam name="TQuery">查詢類型</typeparam>
/// <typeparam name="TResponse">回應類型</typeparam>
public interface IQueryHandler<in TQuery, TResponse> : IRequestHandler<TQuery, TResponse>
    where TQuery : IQuery<TResponse>
{
}