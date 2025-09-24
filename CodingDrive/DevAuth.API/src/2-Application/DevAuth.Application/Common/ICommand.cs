using MediatR;

namespace DevAuth.Application.Common;

/// <summary>
/// 命令介面標記
/// 表示不返回結果的命令
/// </summary>
public interface ICommand : IRequest
{
}

/// <summary>
/// 命令介面標記
/// 表示返回指定類型結果的命令
/// </summary>
/// <typeparam name="TResponse">回應類型</typeparam>
public interface ICommand<out TResponse> : IRequest<TResponse>
{
}

/// <summary>
/// 命令處理器介面
/// </summary>
/// <typeparam name="TCommand">命令類型</typeparam>
public interface ICommandHandler<in TCommand> : IRequestHandler<TCommand>
    where TCommand : ICommand
{
}

/// <summary>
/// 命令處理器介面
/// </summary>
/// <typeparam name="TCommand">命令類型</typeparam>
/// <typeparam name="TResponse">回應類型</typeparam>
public interface ICommandHandler<in TCommand, TResponse> : IRequestHandler<TCommand, TResponse>
    where TCommand : ICommand<TResponse>
{
}