namespace SoftwareDevelopment.Domain.Templates.ValueObjects;

/// <summary>
/// 專案模板類型
/// </summary>
public enum TemplateType
{
    /// <summary>
    /// Web 應用程式
    /// </summary>
    WebApplication = 1,

    /// <summary>
    /// 桌面應用程式
    /// </summary>
    DesktopApplication = 2,

    /// <summary>
    /// 移動應用程式
    /// </summary>
    MobileApplication = 3,

    /// <summary>
    /// API 服務
    /// </summary>
    ApiService = 4,

    /// <summary>
    /// 微服務
    /// </summary>
    Microservice = 5,

    /// <summary>
    /// 函式庫
    /// </summary>
    Library = 6,

    /// <summary>
    /// 框架
    /// </summary>
    Framework = 7,

    /// <summary>
    /// 工具
    /// </summary>
    Tool = 8,

    /// <summary>
    /// 其他
    /// </summary>
    Other = 99
}