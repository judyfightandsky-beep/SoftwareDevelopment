# 軟體開發專案管理平台 - 第2階段系統設計規格

## 版本資訊
- **文檔版本**：1.0
- **建立日期**：2025-09-27
- **負責人**：系統設計師
- **審核狀態**：待審核
- **相關專案**：SoftwareDevelopment.API - Phase 2

---

## 1. 系統設計概覽

### 1.1 設計原則
基於第1階段的DDD架構基礎，第2階段系統設計遵循以下核心原則：

- **領域驅動**：以業務領域為中心的設計方法
- **分層架構**：清晰的責任分離和依賴管理
- **可擴展性**：支援未來功能模組的平滑擴展
- **整合性**：與第1階段身份管理系統的無縫整合
- **高內聚低耦合**：模組間的鬆散耦合設計

### 1.2 架構概覽
第2階段在第1階段的四層架構基礎上，新增四個核心業務領域：

```
┌─────────────────────────────────────────────────────────────┐
│                    Presentation Layer                       │
│  ┌─────────────┐ ┌─────────────┐ ┌─────────────┐ ┌────────┐ │
│  │ Template    │ │ Task        │ │ Quality     │ │ Workflow│ │
│  │ Controllers │ │ Controllers │ │ Controllers │ │ Controllers│ │
│  └─────────────┘ └─────────────┘ └─────────────┘ └────────┘ │
└─────────────────────────────────────────────────────────────┘
┌─────────────────────────────────────────────────────────────┐
│                   Application Layer                         │
│  ┌─────────────┐ ┌─────────────┐ ┌─────────────┐ ┌────────┐ │
│  │ Template    │ │ Task        │ │ Quality     │ │ Workflow│ │
│  │ Services    │ │ Services    │ │ Services    │ │ Services│ │
│  └─────────────┘ └─────────────┘ └─────────────┘ └────────┘ │
└─────────────────────────────────────────────────────────────┘
┌─────────────────────────────────────────────────────────────┐
│                     Domain Layer                            │
│  ┌─────────────┐ ┌─────────────┐ ┌─────────────┐ ┌────────┐ │
│  │ Template    │ │ Task        │ │ Quality     │ │ Workflow│ │
│  │ Domain      │ │ Domain      │ │ Domain      │ │ Domain  │ │
│  └─────────────┘ └─────────────┘ └─────────────┘ └────────┘ │
└─────────────────────────────────────────────────────────────┘
┌─────────────────────────────────────────────────────────────┐
│                Infrastructure Layer                         │
│  ┌─────────────┐ ┌─────────────┐ ┌─────────────┐ ┌────────┐ │
│  │ Template    │ │ Task        │ │ AI Service  │ │ Workflow│ │
│  │ Repository  │ │ Repository  │ │ Integration │ │ Engine  │ │
│  └─────────────┘ └─────────────┘ └─────────────┘ └────────┘ │
└─────────────────────────────────────────────────────────────┘
```

### 1.3 核心領域模組
1. **專案模板領域 (Project Template Domain)**
2. **任務管理領域 (Task Management Domain)**
3. **AI品質檢查領域 (AI Quality Check Domain)**
4. **工作流程領域 (Workflow Domain)**

---

## 2. 專案模板系統類別圖設計

### 2.1 模板領域核心類別

```mermaid
classDiagram
    class ProjectTemplate {
        <<AggregateRoot>>
        +TemplateId Id
        +string Name
        +string Description
        +TemplateType Type
        +TemplateConfiguration Configuration
        +TemplateMetadata Metadata
        +TemplateStatus Status
        +DateTime CreatedAt
        +UserId CreatedBy
        +DateTime UpdatedAt
        +UserId UpdatedBy

        +Create(name, description, type, configuration) ProjectTemplate
        +UpdateConfiguration(configuration) void
        +Activate() void
        +Deactivate() void
        +UpdateMetadata(downloads, rating) void
        +ValidateConfiguration() ValidationResult
    }

    class TemplateId {
        <<ValueObject>>
        +Guid Value
        +TemplateId(guid)
        +ToString() string
        +Equals(other) bool
    }

    class TemplateConfiguration {
        <<ValueObject>>
        +TechnologyStack TechStack
        +List~TemplateModule~ Modules
        +DatabaseSettings Database
        +AuthenticationSettings Authentication
        +Dictionary~string,object~ CustomSettings

        +AddModule(module) void
        +RemoveModule(moduleId) void
        +UpdateDatabaseSettings(settings) void
        +Validate() ValidationResult
    }

    class TemplateMetadata {
        <<ValueObject>>
        +int DownloadCount
        +decimal AverageRating
        +int ReviewCount
        +DateTime LastUsed
        +string Version
        +List~string~ Tags

        +IncrementDownloads() void
        +UpdateRating(rating) void
        +AddTag(tag) void
    }

    class TemplateModule {
        <<Entity>>
        +ModuleId Id
        +string Name
        +string Description
        +ModuleType Type
        +bool IsRequired
        +List~ModuleDependency~ Dependencies
        +Dictionary~string,object~ Settings

        +AddDependency(dependency) void
        +RemoveDependency(dependencyId) void
        +UpdateSettings(settings) void
    }

    class TechnologyStack {
        <<ValueObject>>
        +string Framework
        +string Version
        +string Database
        +string Runtime
        +List~string~ Libraries

        +AddLibrary(library) void
        +RemoveLibrary(library) void
        +IsCompatibleWith(other) bool
    }

    class TemplateType {
        <<Enumeration>>
        DDD
        CQRS
        CleanArchitecture
        Microservices
        MVC
        Custom
    }

    class TemplateStatus {
        <<Enumeration>>
        Draft
        Active
        Deprecated
        Archived
    }

    ProjectTemplate ||--|| TemplateId : has
    ProjectTemplate ||--|| TemplateConfiguration : contains
    ProjectTemplate ||--|| TemplateMetadata : has
    TemplateConfiguration ||--|| TechnologyStack : includes
    TemplateConfiguration ||--o{ TemplateModule : contains
    ProjectTemplate ||--|| TemplateType : has
    ProjectTemplate ||--|| TemplateStatus : has
```

### 2.2 模板服務和倉儲

```mermaid
classDiagram
    class IProjectTemplateRepository {
        <<Interface>>
        +GetByIdAsync(id) Task~ProjectTemplate~
        +GetAllActiveAsync() Task~List~ProjectTemplate~~
        +FindByTypeAsync(type) Task~List~ProjectTemplate~~
        +SearchAsync(criteria) Task~List~ProjectTemplate~~
        +AddAsync(template) Task
        +UpdateAsync(template) Task
        +DeleteAsync(id) Task
        +ExistsAsync(id) Task~bool~
    }

    class ProjectTemplateService {
        <<DomainService>>
        -IProjectTemplateRepository _repository
        -ITemplateValidator _validator
        -ITemplateGenerator _generator

        +CreateTemplateAsync(command) Task~ProjectTemplate~
        +UpdateTemplateAsync(id, command) Task~ProjectTemplate~
        +DeleteTemplateAsync(id) Task
        +GenerateProjectAsync(templateId, config) Task~ProjectGenerationResult~
        +ValidateTemplateAsync(template) Task~ValidationResult~
        +GetPopularTemplatesAsync() Task~List~ProjectTemplate~~
    }

    class TemplateGenerationService {
        <<DomainService>>
        -IProjectStructureGenerator _structureGenerator
        -ICodeGenerator _codeGenerator
        -IGitRepository _gitRepository

        +GenerateAsync(template, config) Task~ProjectGenerationResult~
        +CreateRepositoryAsync(projectName) Task~string~
        +GenerateCodeStructureAsync(template, config) Task~List~ProjectFile~~
        +ApplyConfigurationAsync(files, config) Task
    }

    class ProjectGenerationResult {
        <<ValueObject>>
        +string ProjectName
        +string RepositoryUrl
        +List~ProjectFile~ GeneratedFiles
        +TimeSpan GenerationTime
        +bool IsSuccess
        +List~string~ Errors
        +List~string~ Warnings
    }

    ProjectTemplateService ..> IProjectTemplateRepository : uses
    TemplateGenerationService ..> ProjectGenerationResult : creates
    ProjectTemplateService ..> TemplateGenerationService : uses
```

### 2.3 模板應用服務

```mermaid
classDiagram
    class TemplateApplicationService {
        <<ApplicationService>>
        -ProjectTemplateService _templateService
        -TemplateGenerationService _generationService
        -IEventPublisher _eventPublisher
        -ILogger _logger

        +CreateTemplateAsync(command) Task~TemplateDto~
        +UpdateTemplateAsync(command) Task~TemplateDto~
        +DeleteTemplateAsync(command) Task
        +GetTemplateAsync(query) Task~TemplateDto~
        +GetTemplatesAsync(query) Task~PagedResult~TemplateDto~~
        +GenerateProjectAsync(command) Task~ProjectGenerationDto~
        +GetTemplateUsageStatsAsync(query) Task~TemplateStatsDto~
    }

    class CreateTemplateCommand {
        <<Command>>
        +string Name
        +string Description
        +TemplateType Type
        +TechnologyStack TechStack
        +List~ModuleRequest~ Modules
        +UserId CreatedBy

        +Validate() ValidationResult
    }

    class GenerateProjectCommand {
        <<Command>>
        +TemplateId TemplateId
        +string ProjectName
        +string Namespace
        +DatabaseType DatabaseType
        +List~ModuleConfiguration~ ModuleConfigs
        +UserId RequestedBy

        +Validate() ValidationResult
    }

    class TemplateDto {
        <<DTO>>
        +Guid Id
        +string Name
        +string Description
        +string Type
        +TechnologyStackDto TechStack
        +List~ModuleDto~ Modules
        +TemplateMetadataDto Metadata
        +DateTime CreatedAt
        +string CreatedBy
    }

    TemplateApplicationService ..> CreateTemplateCommand : handles
    TemplateApplicationService ..> GenerateProjectCommand : handles
    TemplateApplicationService ..> TemplateDto : returns
```

---

## 3. 任務管理系統類別圖設計

### 3.1 任務領域核心類別

```mermaid
classDiagram
    class Task {
        <<AggregateRoot>>
        +TaskId Id
        +string Title
        +string Description
        +TaskType Type
        +Priority Priority
        +TaskStatus Status
        +ProjectId ProjectId
        +UserId AssignedTo
        +UserId CreatedBy
        +DateTime CreatedAt
        +DateTime? DueDate
        +EstimatedHours EstimatedHours
        +ActualHours ActualHours
        +Progress Progress
        +List~TaskComment~ Comments
        +List~TaskAttachment~ Attachments
        +List~TaskDependency~ Dependencies
        +GitLabIntegration GitLabInfo

        +Create(title, description, type, projectId) Task
        +AssignTo(userId) void
        +UpdateStatus(status) void
        +UpdateProgress(progress) void
        +AddComment(comment) void
        +AddAttachment(attachment) void
        +AddDependency(taskId) void
        +UpdateEstimation(hours) void
        +RecordActualHours(hours) void
        +SetDueDate(date) void
        +CanTransitionTo(status) bool
    }

    class TaskId {
        <<ValueObject>>
        +Guid Value
        +string TaskNumber
        +TaskId(guid, number)
        +ToString() string
        +GenerateTaskNumber(projectCode) string
    }

    class Priority {
        <<Enumeration>>
        Critical
        High
        Medium
        Low
    }

    class TaskStatus {
        <<Enumeration>>
        Todo
        InProgress
        Review
        Testing
        Done
        Closed
    }

    class Progress {
        <<ValueObject>>
        +int Percentage
        +DateTime LastUpdated
        +UserId UpdatedBy
        +string Notes

        +Update(percentage, notes, userId) void
        +IsComplete() bool
        +Validate() bool
    }

    class TaskComment {
        <<Entity>>
        +CommentId Id
        +string Content
        +UserId AuthorId
        +DateTime CreatedAt
        +DateTime? UpdatedAt
        +CommentType Type

        +Update(content) void
        +MarkAsEdited() void
    }

    class TaskAttachment {
        <<Entity>>
        +AttachmentId Id
        +string FileName
        +string FileType
        +long FileSize
        +string StoragePath
        +UserId UploadedBy
        +DateTime UploadedAt

        +GetDownloadUrl() string
        +ValidateFileType() bool
    }

    class TaskDependency {
        <<ValueObject>>
        +TaskId DependentTaskId
        +TaskId DependsOnTaskId
        +DependencyType Type
        +DateTime CreatedAt

        +IsBlocking() bool
        +CanBeRemoved() bool
    }

    class GitLabIntegration {
        <<ValueObject>>
        +int? IssueId
        +string IssueUrl
        +int? MergeRequestId
        +string BranchName
        +GitLabSyncStatus SyncStatus
        +DateTime? LastSyncAt

        +UpdateIssueId(issueId) void
        +LinkMergeRequest(mrId) void
        +UpdateSyncStatus(status) void
    }

    Task ||--|| TaskId : has
    Task ||--|| Priority : has
    Task ||--|| TaskStatus : has
    Task ||--|| Progress : has
    Task ||--o{ TaskComment : contains
    Task ||--o{ TaskAttachment : contains
    Task ||--o{ TaskDependency : has
    Task ||--|| GitLabIntegration : has
```

### 3.2 任務服務和倉儲

```mermaid
classDiagram
    class ITaskRepository {
        <<Interface>>
        +GetByIdAsync(id) Task~Task~
        +GetByProjectIdAsync(projectId) Task~List~Task~~
        +GetByAssigneeAsync(userId) Task~List~Task~~
        +GetByStatusAsync(status) Task~List~Task~~
        +GetOverdueTasksAsync() Task~List~Task~~
        +AddAsync(task) Task
        +UpdateAsync(task) Task
        +DeleteAsync(id) Task
        +SearchAsync(criteria) Task~PagedResult~Task~~
    }

    class TaskDomainService {
        <<DomainService>>
        -ITaskRepository _taskRepository
        -IUserRepository _userRepository
        -IProjectRepository _projectRepository
        -IDomainEventPublisher _eventPublisher

        +CreateTaskAsync(command) Task~Task~
        +AssignTaskAsync(taskId, userId) Task
        +UpdateTaskStatusAsync(taskId, status) Task
        +CalculateWorkloadAsync(userId) Task~WorkloadStats~
        +ValidateTaskDependenciesAsync(task) Task~ValidationResult~
        +GetTaskMetricsAsync(projectId) Task~TaskMetrics~
    }

    class GitLabIntegrationService {
        <<DomainService>>
        -IGitLabApiClient _gitLabClient
        -ITaskRepository _taskRepository
        -IConfiguration _configuration

        +SyncTaskToIssueAsync(task) Task~GitLabIssue~
        +SyncIssueToTaskAsync(issueId) Task~Task~
        +CreateMergeRequestAsync(taskId, branchName) Task~MergeRequest~
        +HandleWebhookAsync(payload) Task
        +GetCommitsByTaskAsync(taskId) Task~List~GitCommit~~
    }

    class WorkloadStats {
        <<ValueObject>>
        +UserId UserId
        +int TotalTasks
        +int InProgressTasks
        +decimal TotalEstimatedHours
        +decimal CompletionRate
        +Dictionary~Priority,int~ TasksByPriority
        +DateTime CalculatedAt
    }

    TaskDomainService ..> ITaskRepository : uses
    GitLabIntegrationService ..> ITaskRepository : uses
    TaskDomainService ..> WorkloadStats : creates
```

### 3.3 任務應用服務

```mermaid
classDiagram
    class TaskApplicationService {
        <<ApplicationService>>
        -TaskDomainService _taskService
        -GitLabIntegrationService _gitLabService
        -IEventPublisher _eventPublisher
        -INotificationService _notificationService

        +CreateTaskAsync(command) Task~TaskDto~
        +UpdateTaskAsync(command) Task~TaskDto~
        +AssignTaskAsync(command) Task
        +UpdateProgressAsync(command) Task
        +AddCommentAsync(command) Task~CommentDto~
        +GetTaskAsync(query) Task~TaskDto~
        +GetTasksAsync(query) Task~PagedResult~TaskDto~~
        +GetTaskStatisticsAsync(query) Task~TaskStatisticsDto~
        +BulkUpdateTasksAsync(command) Task~BulkUpdateResult~
    }

    class CreateTaskCommand {
        <<Command>>
        +string Title
        +string Description
        +TaskType Type
        +Priority Priority
        +ProjectId ProjectId
        +UserId? AssignedTo
        +DateTime? DueDate
        +int? EstimatedHours
        +UserId CreatedBy

        +Validate() ValidationResult
    }

    class UpdateTaskStatusCommand {
        <<Command>>
        +TaskId TaskId
        +TaskStatus NewStatus
        +string? Notes
        +UserId UpdatedBy

        +Validate() ValidationResult
    }

    class TaskDto {
        <<DTO>>
        +Guid Id
        +string TaskNumber
        +string Title
        +string Description
        +string Type
        +string Priority
        +string Status
        +Guid ProjectId
        +UserDto? AssignedTo
        +UserDto CreatedBy
        +DateTime CreatedAt
        +DateTime? DueDate
        +int? EstimatedHours
        +int? ActualHours
        +ProgressDto Progress
        +List~CommentDto~ Comments
        +GitLabInfoDto? GitLabInfo
    }

    TaskApplicationService ..> CreateTaskCommand : handles
    TaskApplicationService ..> UpdateTaskStatusCommand : handles
    TaskApplicationService ..> TaskDto : returns
```

---

## 4. AI程式碼檢查系統類別圖設計

### 4.1 AI品質檢查領域核心類別

```mermaid
classDiagram
    class CodeQualityCheck {
        <<AggregateRoot>>
        +CheckId Id
        +ProjectId ProjectId
        +string CommitHash
        +string BranchName
        +CheckTrigger TriggerType
        +CheckStatus Status
        +DateTime StartedAt
        +DateTime? CompletedAt
        +QualityScore OverallScore
        +List~QualityIssue~ Issues
        +List~CodeMetric~ Metrics
        +CheckConfiguration Configuration
        +UserId RequestedBy

        +Create(projectId, commitHash, trigger) CodeQualityCheck
        +Start() void
        +Complete(score, issues, metrics) void
        +Fail(error) void
        +AddIssue(issue) void
        +CalculateOverallScore() QualityScore
        +GetSummary() CheckSummary
    }

    class CheckId {
        <<ValueObject>>
        +Guid Value
        +CheckId(guid)
        +ToString() string
    }

    class QualityScore {
        <<ValueObject>>
        +decimal OverallScore
        +decimal CodeStyleScore
        +decimal ComplexityScore
        +decimal SecurityScore
        +decimal PerformanceScore
        +decimal TestCoverageScore
        +ScoreGrade Grade

        +CalculateOverall() decimal
        +GetGrade() ScoreGrade
        +CompareTo(other) int
    }

    class QualityIssue {
        <<Entity>>
        +IssueId Id
        +string RuleId
        +IssueSeverity Severity
        +IssueCategory Category
        +string Title
        +string Description
        +string FileName
        +int LineNumber
        +int? ColumnNumber
        +string CodeSnippet
        +string Suggestion
        +bool IsFixed
        +DateTime DetectedAt

        +MarkAsFixed() void
        +UpdateSuggestion(suggestion) void
        +GetLocation() CodeLocation
    }

    class CodeMetric {
        <<ValueObject>>
        +MetricType Type
        +string Name
        +decimal Value
        +string Unit
        +MetricTrend Trend
        +decimal? ThresholdValue
        +bool IsWithinThreshold

        +CheckThreshold() bool
        +GetTrendDirection() TrendDirection
    }

    class CheckConfiguration {
        <<ValueObject>>
        +List~string~ EnabledRules
        +List~string~ DisabledRules
        +Dictionary~string,object~ RuleParameters
        +QualityThresholds Thresholds
        +bool IncludeTestFiles
        +List~string~ ExcludedPaths

        +IsRuleEnabled(ruleId) bool
        +GetRuleParameter(ruleId, paramName) object
        +AddRule(ruleId) void
        +RemoveRule(ruleId) void
    }

    class CheckTrigger {
        <<Enumeration>>
        Manual
        CommitPush
        PullRequest
        Scheduled
        API
    }

    class CheckStatus {
        <<Enumeration>>
        Pending
        Running
        Completed
        Failed
        Cancelled
    }

    class IssueSeverity {
        <<Enumeration>>
        Critical
        High
        Medium
        Low
        Info
    }

    class IssueCategory {
        <<Enumeration>>
        CodeStyle
        Complexity
        Security
        Performance
        Maintainability
        Reliability
        TestCoverage
    }

    CodeQualityCheck ||--|| CheckId : has
    CodeQualityCheck ||--|| QualityScore : has
    CodeQualityCheck ||--o{ QualityIssue : contains
    CodeQualityCheck ||--o{ CodeMetric : contains
    CodeQualityCheck ||--|| CheckConfiguration : has
    CodeQualityCheck ||--|| CheckTrigger : has
    CodeQualityCheck ||--|| CheckStatus : has
    QualityIssue ||--|| IssueSeverity : has
    QualityIssue ||--|| IssueCategory : has
```

### 4.2 AI服務和規則引擎

```mermaid
classDiagram
    class ICodeQualityRepository {
        <<Interface>>
        +GetByIdAsync(id) Task~CodeQualityCheck~
        +GetByProjectIdAsync(projectId) Task~List~CodeQualityCheck~~
        +GetByCommitHashAsync(commitHash) Task~CodeQualityCheck~
        +GetLatestByProjectAsync(projectId) Task~CodeQualityCheck~
        +AddAsync(check) Task
        +UpdateAsync(check) Task
        +GetTrendsAsync(projectId, period) Task~List~QualityTrend~~
    }

    class AICodeAnalysisService {
        <<DomainService>>
        -IAIAnalysisEngine _aiEngine
        -ICodeParser _codeParser
        -IRuleEngine _ruleEngine
        -ICodeQualityRepository _repository

        +AnalyzeCodeAsync(check) Task~AnalysisResult~
        +DetectIssuesAsync(files, rules) Task~List~QualityIssue~~
        +CalculateMetricsAsync(files) Task~List~CodeMetric~~
        +GenerateSuggestionsAsync(issues) Task~List~Suggestion~~
        +GetRecommendationsAsync(projectId) Task~List~Recommendation~~
    }

    class RuleEngine {
        <<DomainService>>
        -IRuleRepository _ruleRepository
        -Dictionary~string,IQualityRule~ _loadedRules

        +LoadRulesAsync(configuration) Task
        +ExecuteRulesAsync(codeFiles) Task~List~QualityIssue~~
        +ValidateRuleConfigurationAsync(config) Task~ValidationResult~
        +GetAvailableRulesAsync() Task~List~QualityRule~~
        +UpdateRuleParametersAsync(ruleId, parameters) Task
    }

    class QualityRule {
        <<Entity>>
        +RuleId Id
        +string Name
        +string Description
        +IssueCategory Category
        +IssueSeverity DefaultSeverity
        +RuleType Type
        +string Pattern
        +Dictionary~string,object~ Parameters
        +bool IsEnabled
        +string Version

        +Execute(codeContext) RuleResult
        +ValidateParameters(parameters) bool
        +GetDocumentation() string
    }

    class AnalysisResult {
        <<ValueObject>>
        +CheckId CheckId
        +QualityScore Score
        +List~QualityIssue~ Issues
        +List~CodeMetric~ Metrics
        +TimeSpan AnalysisTime
        +int FilesAnalyzed
        +int LinesAnalyzed
        +AnalysisStatus Status

        +GetSummary() string
        +HasCriticalIssues() bool
    }

    AICodeAnalysisService ..> ICodeQualityRepository : uses
    AICodeAnalysisService ..> RuleEngine : uses
    RuleEngine ..> QualityRule : manages
    AICodeAnalysisService ..> AnalysisResult : creates
```

### 4.3 AI品質檢查應用服務

```mermaid
classDiagram
    class QualityCheckApplicationService {
        <<ApplicationService>>
        -AICodeAnalysisService _analysisService
        -ICodeQualityRepository _repository
        -INotificationService _notificationService
        -IEventPublisher _eventPublisher

        +StartCheckAsync(command) Task~CheckDto~
        +GetCheckAsync(query) Task~CheckDto~
        +GetCheckHistoryAsync(query) Task~PagedResult~CheckDto~~
        +GetQualityTrendsAsync(query) Task~QualityTrendsDto~
        +GetIssuesByProjectAsync(query) Task~List~IssueDto~~
        +UpdateRuleConfigurationAsync(command) Task
        +GetQualityReportAsync(query) Task~QualityReportDto~
        +RetryCheckAsync(command) Task~CheckDto~
    }

    class StartQualityCheckCommand {
        <<Command>>
        +ProjectId ProjectId
        +string CommitHash
        +string BranchName
        +CheckTrigger TriggerType
        +CheckConfiguration Configuration
        +UserId RequestedBy

        +Validate() ValidationResult
    }

    class QualityCheckQuery {
        <<Query>>
        +CheckId? CheckId
        +ProjectId? ProjectId
        +string? CommitHash
        +CheckStatus? Status
        +DateTime? FromDate
        +DateTime? ToDate
        +int Page
        +int PageSize
    }

    class CheckDto {
        <<DTO>>
        +Guid Id
        +Guid ProjectId
        +string CommitHash
        +string BranchName
        +string TriggerType
        +string Status
        +DateTime StartedAt
        +DateTime? CompletedAt
        +QualityScoreDto OverallScore
        +List~IssueDto~ Issues
        +List~MetricDto~ Metrics
        +string RequestedBy
    }

    class QualityReportDto {
        <<DTO>>
        +CheckDto Check
        +QualityTrendsDto Trends
        +List~RecommendationDto~ Recommendations
        +ComparisonDto Comparison
        +string ReportUrl
        +DateTime GeneratedAt
    }

    QualityCheckApplicationService ..> StartQualityCheckCommand : handles
    QualityCheckApplicationService ..> QualityCheckQuery : handles
    QualityCheckApplicationService ..> CheckDto : returns
    QualityCheckApplicationService ..> QualityReportDto : returns
```

---

## 5. 工作流程引擎系統類別圖設計

### 5.1 工作流程領域核心類別

```mermaid
classDiagram
    class WorkflowDefinition {
        <<AggregateRoot>>
        +WorkflowId Id
        +string Name
        +string Description
        +WorkflowVersion Version
        +WorkflowStatus Status
        +List~WorkflowNode~ Nodes
        +List~WorkflowTransition~ Transitions
        +WorkflowTrigger Trigger
        +WorkflowConfiguration Configuration
        +UserId CreatedBy
        +DateTime CreatedAt
        +DateTime? PublishedAt

        +Create(name, description, trigger) WorkflowDefinition
        +AddNode(node) void
        +RemoveNode(nodeId) void
        +AddTransition(transition) void
        +RemoveTransition(transitionId) void
        +Validate() ValidationResult
        +Publish() void
        +Archive() void
        +CreateInstance() WorkflowInstance
    }

    class WorkflowNode {
        <<Entity>>
        +NodeId Id
        +string Name
        +NodeType Type
        +NodeConfiguration Configuration
        +List~NodeId~ Inputs
        +List~NodeId~ Outputs
        +Position Position
        +bool IsRequired
        +TimeSpan? TimeLimit
        +List~string~ AssignedRoles

        +AddInput(nodeId) void
        +AddOutput(nodeId) void
        +UpdateConfiguration(config) void
        +CanExecute(context) bool
        +Execute(context) NodeResult
    }

    class WorkflowTransition {
        <<Entity>>
        +TransitionId Id
        +NodeId FromNodeId
        +NodeId ToNodeId
        +TransitionCondition Condition
        +string Name
        +TransitionType Type

        +Evaluate(context) bool
        +CanTransition(context) bool
    }

    class WorkflowInstance {
        <<AggregateRoot>>
        +InstanceId Id
        +WorkflowId WorkflowDefinitionId
        +string Title
        +WorkflowInstanceStatus Status
        +Dictionary~string,object~ Variables
        +List~NodeExecution~ NodeExecutions
        +DateTime StartedAt
        +DateTime? CompletedAt
        +UserId StartedBy
        +ProjectId? ProjectId
        +TaskId? TaskId

        +Start(variables) void
        +MoveToNext(nodeId) void
        +Complete() void
        +Cancel() void
        +Suspend() void
        +Resume() void
        +GetCurrentNodes() List~WorkflowNode~
        +CanMoveToNode(nodeId) bool
    }

    class NodeExecution {
        <<Entity>>
        +ExecutionId Id
        +NodeId NodeId
        +NodeExecutionStatus Status
        +DateTime StartedAt
        +DateTime? CompletedAt
        +UserId? AssignedTo
        +UserId? ExecutedBy
        +Dictionary~string,object~ InputData
        +Dictionary~string,object~ OutputData
        +string? Comments
        +List~ExecutionHistory~ History

        +Start(assignedTo) void
        +Complete(outputData, executedBy) void
        +Fail(error) void
        +Reassign(newAssignee) void
        +AddComment(comment) void
    }

    class NodeType {
        <<Enumeration>>
        Start
        End
        Task
        Decision
        Parallel
        Merge
        Timer
        Service
        Human
        Approval
    }

    class WorkflowInstanceStatus {
        <<Enumeration>>
        Created
        Running
        Suspended
        Completed
        Cancelled
        Failed
    }

    WorkflowDefinition ||--o{ WorkflowNode : contains
    WorkflowDefinition ||--o{ WorkflowTransition : contains
    WorkflowInstance ||--o{ NodeExecution : contains
    WorkflowNode ||--|| NodeType : has
    WorkflowInstance ||--|| WorkflowInstanceStatus : has
```

### 5.2 工作流程引擎服務

```mermaid
classDiagram
    class IWorkflowRepository {
        <<Interface>>
        +GetDefinitionByIdAsync(id) Task~WorkflowDefinition~
        +GetInstanceByIdAsync(id) Task~WorkflowInstance~
        +GetActiveInstancesAsync() Task~List~WorkflowInstance~~
        +GetInstancesByProjectAsync(projectId) Task~List~WorkflowInstance~~
        +SaveDefinitionAsync(definition) Task
        +SaveInstanceAsync(instance) Task
        +GetDefinitionsByTriggerAsync(trigger) Task~List~WorkflowDefinition~~
    }

    class WorkflowEngine {
        <<DomainService>>
        -IWorkflowRepository _repository
        -INodeExecutor _nodeExecutor
        -IConditionEvaluator _conditionEvaluator
        -IEventPublisher _eventPublisher

        +StartWorkflowAsync(definitionId, variables) Task~WorkflowInstance~
        +ExecuteNodeAsync(instanceId, nodeId) Task~NodeResult~
        +MoveToNextAsync(instanceId, nodeId) Task
        +CompleteWorkflowAsync(instanceId) Task
        +CancelWorkflowAsync(instanceId) Task
        +GetExecutableNodesAsync(instanceId) Task~List~WorkflowNode~~
        +EvaluateTransitionsAsync(instanceId, nodeId) Task~List~NodeId~~
    }

    class NodeExecutorFactory {
        <<Factory>>
        -Dictionary~NodeType,INodeExecutor~ _executors

        +CreateExecutor(nodeType) INodeExecutor
        +RegisterExecutor(nodeType, executor) void
    }

    class INodeExecutor {
        <<Interface>>
        +CanExecute(node, context) bool
        +ExecuteAsync(node, context) Task~NodeResult~
        +GetRequiredVariables(node) List~string~
        +ValidateConfiguration(config) ValidationResult
    }

    class HumanTaskExecutor {
        <<Service>>
        -ITaskRepository _taskRepository
        -INotificationService _notificationService

        +ExecuteAsync(node, context) Task~NodeResult~
        +CreateHumanTaskAsync(node, context) Task~Task~
        +NotifyAssigneeAsync(task) Task
    }

    class ServiceTaskExecutor {
        <<Service>>
        -IServiceProvider _serviceProvider
        -ILogger _logger

        +ExecuteAsync(node, context) Task~NodeResult~
        +InvokeServiceAsync(serviceName, parameters) Task~object~
        +HandleServiceErrorAsync(error) Task
    }

    class NodeResult {
        <<ValueObject>>
        +bool IsSuccess
        +Dictionary~string,object~ OutputData
        +string? ErrorMessage
        +NodeResultType ResultType
        +TimeSpan ExecutionTime

        +Success(outputData) NodeResult
        +Failure(errorMessage) NodeResult
        +Waiting() NodeResult
    }

    WorkflowEngine ..> IWorkflowRepository : uses
    WorkflowEngine ..> NodeExecutorFactory : uses
    NodeExecutorFactory ..> INodeExecutor : creates
    HumanTaskExecutor ..|> INodeExecutor : implements
    ServiceTaskExecutor ..|> INodeExecutor : implements
    INodeExecutor ..> NodeResult : returns
```

### 5.3 工作流程應用服務

```mermaid
classDiagram
    class WorkflowApplicationService {
        <<ApplicationService>>
        -WorkflowEngine _workflowEngine
        -IWorkflowRepository _repository
        -IEventPublisher _eventPublisher
        -INotificationService _notificationService

        +CreateWorkflowDefinitionAsync(command) Task~WorkflowDefinitionDto~
        +UpdateWorkflowDefinitionAsync(command) Task~WorkflowDefinitionDto~
        +PublishWorkflowAsync(command) Task
        +StartWorkflowInstanceAsync(command) Task~WorkflowInstanceDto~
        +ExecuteNodeAsync(command) Task~NodeExecutionDto~
        +GetWorkflowInstanceAsync(query) Task~WorkflowInstanceDto~
        +GetActiveWorkflowsAsync(query) Task~List~WorkflowInstanceDto~~
        +GetWorkflowHistoryAsync(query) Task~PagedResult~WorkflowInstanceDto~~
    }

    class CreateWorkflowDefinitionCommand {
        <<Command>>
        +string Name
        +string Description
        +WorkflowTrigger Trigger
        +List~NodeDefinition~ Nodes
        +List~TransitionDefinition~ Transitions
        +WorkflowConfiguration Configuration
        +UserId CreatedBy

        +Validate() ValidationResult
    }

    class StartWorkflowInstanceCommand {
        <<Command>>
        +WorkflowId WorkflowDefinitionId
        +string Title
        +Dictionary~string,object~ Variables
        +ProjectId? ProjectId
        +TaskId? TaskId
        +UserId StartedBy

        +Validate() ValidationResult
    }

    class ExecuteNodeCommand {
        <<Command>>
        +InstanceId InstanceId
        +NodeId NodeId
        +Dictionary~string,object~ InputData
        +UserId ExecutedBy
        +string? Comments

        +Validate() ValidationResult
    }

    class WorkflowDefinitionDto {
        <<DTO>>
        +Guid Id
        +string Name
        +string Description
        +string Version
        +string Status
        +List~NodeDto~ Nodes
        +List~TransitionDto~ Transitions
        +string TriggerType
        +DateTime CreatedAt
        +string CreatedBy
    }

    class WorkflowInstanceDto {
        <<DTO>>
        +Guid Id
        +Guid WorkflowDefinitionId
        +string Title
        +string Status
        +DateTime StartedAt
        +DateTime? CompletedAt
        +List~NodeExecutionDto~ NodeExecutions
        +string StartedBy
        +Guid? ProjectId
        +Guid? TaskId
    }

    WorkflowApplicationService ..> CreateWorkflowDefinitionCommand : handles
    WorkflowApplicationService ..> StartWorkflowInstanceCommand : handles
    WorkflowApplicationService ..> ExecuteNodeCommand : handles
    WorkflowApplicationService ..> WorkflowDefinitionDto : returns
    WorkflowApplicationService ..> WorkflowInstanceDto : returns
```

---

## 6. 關鍵序列圖設計

### 6.1 專案模板生成序列圖

```mermaid
sequenceDiagram
    participant User as 使用者
    participant Controller as TemplateController
    participant AppService as TemplateApplicationService
    participant DomainService as TemplateGenerationService
    participant Repository as IProjectTemplateRepository
    participant GitLab as GitLab API
    participant FileSystem as 檔案系統

    User->>Controller: GenerateProject(templateId, config)
    Controller->>AppService: GenerateProjectAsync(command)

    AppService->>Repository: GetByIdAsync(templateId)
    Repository-->>AppService: ProjectTemplate

    AppService->>DomainService: GenerateAsync(template, config)

    DomainService->>FileSystem: CreateProjectStructure()
    FileSystem-->>DomainService: FileStructure Created

    DomainService->>FileSystem: ApplyConfiguration(config)
    FileSystem-->>DomainService: Configuration Applied

    DomainService->>GitLab: CreateRepository(projectName)
    GitLab-->>DomainService: RepositoryUrl

    DomainService->>GitLab: PushInitialCommit(files)
    GitLab-->>DomainService: CommitHash

    DomainService-->>AppService: ProjectGenerationResult
    AppService-->>Controller: ProjectGenerationDto
    Controller-->>User: 201 Created + ProjectInfo
```

### 6.2 任務狀態更新與GitLab同步序列圖

```mermaid
sequenceDiagram
    participant User as 開發人員
    participant Controller as TaskController
    participant AppService as TaskApplicationService
    participant DomainService as TaskDomainService
    participant GitLabService as GitLabIntegrationService
    participant Repository as ITaskRepository
    participant GitLab as GitLab API
    participant EventBus as IEventPublisher

    User->>Controller: UpdateTaskStatus(taskId, newStatus)
    Controller->>AppService: UpdateTaskStatusAsync(command)

    AppService->>Repository: GetByIdAsync(taskId)
    Repository-->>AppService: Task

    AppService->>DomainService: UpdateStatusAsync(task, newStatus)

    alt Status changed to InProgress
        DomainService->>GitLabService: CreateBranchAsync(taskId)
        GitLabService->>GitLab: CreateBranch(branchName)
        GitLab-->>GitLabService: BranchCreated
    end

    alt Status changed to Review
        DomainService->>GitLabService: CreateMergeRequestAsync(taskId)
        GitLabService->>GitLab: CreateMergeRequest(branch)
        GitLab-->>GitLabService: MergeRequestCreated
    end

    DomainService->>Repository: UpdateAsync(task)
    Repository-->>DomainService: Updated

    DomainService->>EventBus: Publish(TaskStatusChangedEvent)
    EventBus-->>DomainService: Event Published

    DomainService-->>AppService: Success
    AppService-->>Controller: TaskDto
    Controller-->>User: 200 OK + UpdatedTask
```

### 6.3 AI程式碼品質檢查序列圖

```mermaid
sequenceDiagram
    participant Developer as 開發人員
    participant GitLab as GitLab
    participant Webhook as WebhookHandler
    participant AppService as QualityCheckApplicationService
    participant AIService as AICodeAnalysisService
    participant RuleEngine as RuleEngine
    participant Repository as ICodeQualityRepository
    participant Notification as INotificationService

    Developer->>GitLab: Push Commit
    GitLab->>Webhook: Webhook Event
    Webhook->>AppService: StartQualityCheckAsync(command)

    AppService->>Repository: CreateCheckAsync(check)
    Repository-->>AppService: Check Created

    AppService->>AIService: AnalyzeCodeAsync(check)

    AIService->>GitLab: FetchCodeFiles(commitHash)
    GitLab-->>AIService: Code Files

    AIService->>RuleEngine: ExecuteRulesAsync(files)
    RuleEngine-->>AIService: Quality Issues

    AIService->>AIService: CalculateMetrics(files)
    AIService->>AIService: GenerateSuggestions(issues)

    AIService-->>AppService: AnalysisResult

    AppService->>Repository: UpdateCheckAsync(check)
    Repository-->>AppService: Check Updated

    alt Quality Score < Threshold
        AppService->>Notification: SendQualityAlert(check)
        Notification-->>AppService: Alert Sent
    end

    AppService-->>Webhook: CheckDto
    Webhook-->>GitLab: Update Commit Status
```

### 6.4 工作流程執行序列圖

```mermaid
sequenceDiagram
    participant PM as 專案經理
    participant Controller as WorkflowController
    participant AppService as WorkflowApplicationService
    participant Engine as WorkflowEngine
    participant Repository as IWorkflowRepository
    participant Executor as INodeExecutor
    participant TaskService as TaskApplicationService
    participant EventBus as IEventPublisher

    PM->>Controller: StartWorkflow(definitionId, variables)
    Controller->>AppService: StartWorkflowInstanceAsync(command)

    AppService->>Repository: GetDefinitionByIdAsync(definitionId)
    Repository-->>AppService: WorkflowDefinition

    AppService->>Engine: StartWorkflowAsync(definition, variables)

    Engine->>Engine: CreateInstance(definition)
    Engine->>Repository: SaveInstanceAsync(instance)
    Repository-->>Engine: Instance Saved

    Engine->>Engine: GetExecutableNodes(instance)

    loop For each executable node
        Engine->>Executor: ExecuteAsync(node, context)

        alt Human Task Node
            Executor->>TaskService: CreateTaskAsync(nodeConfig)
            TaskService-->>Executor: Task Created
        end

        alt Service Task Node
            Executor->>Executor: InvokeService(config)
        end

        Executor-->>Engine: NodeResult

        alt Node Completed
            Engine->>Engine: EvaluateTransitions(nodeId)
            Engine->>Engine: MoveToNextNodes()
        end
    end

    Engine->>EventBus: Publish(WorkflowStartedEvent)
    EventBus-->>Engine: Event Published

    Engine-->>AppService: WorkflowInstance
    AppService-->>Controller: WorkflowInstanceDto
    Controller-->>PM: 201 Created + WorkflowInfo
```

---

## 7. 系統整體架構設計

### 7.1 微服務架構概覽

```mermaid
graph TB
    subgraph "API Gateway"
        Gateway[Ocelot API Gateway]
    end

    subgraph "Authentication Service"
        Auth[身份認證服務<br/>第1階段]
    end

    subgraph "Template Service"
        TemplateAPI[專案模板API]
        TemplateDB[(Template Database)]
        TemplateAPI --> TemplateDB
    end

    subgraph "Task Service"
        TaskAPI[任務管理API]
        TaskDB[(Task Database)]
        TaskAPI --> TaskDB
    end

    subgraph "Quality Service"
        QualityAPI[AI品質檢查API]
        QualityDB[(Quality Database)]
        QualityAPI --> QualityDB
    end

    subgraph "Workflow Service"
        WorkflowAPI[工作流程API]
        WorkflowDB[(Workflow Database)]
        WorkflowAPI --> WorkflowDB
    end

    subgraph "External Services"
        GitLab[GitLab API]
        AI[AI Analysis Service]
        Email[Email Service]
        Files[File Storage]
    end

    subgraph "Shared Infrastructure"
        Redis[(Redis Cache)]
        EventBus[Event Bus]
        Monitoring[監控與日誌]
    end

    Gateway --> Auth
    Gateway --> TemplateAPI
    Gateway --> TaskAPI
    Gateway --> QualityAPI
    Gateway --> WorkflowAPI

    TaskAPI --> GitLab
    QualityAPI --> AI
    QualityAPI --> GitLab
    WorkflowAPI --> Email

    TemplateAPI --> Files
    TaskAPI --> Files

    TemplateAPI --> Redis
    TaskAPI --> Redis
    QualityAPI --> Redis
    WorkflowAPI --> Redis

    TemplateAPI --> EventBus
    TaskAPI --> EventBus
    QualityAPI --> EventBus
    WorkflowAPI --> EventBus

    EventBus --> Monitoring
```

### 7.2 領域模組依賴關係

```mermaid
graph TD
    subgraph "Domain Layer"
        AuthDomain[Identity Domain<br/>第1階段]
        TemplateDomain[Template Domain]
        TaskDomain[Task Domain]
        QualityDomain[Quality Domain]
        WorkflowDomain[Workflow Domain]
    end

    subgraph "Shared Kernel"
        SharedKernel[共享核心<br/>- ValueObjects<br/>- DomainEvents<br/>- Specifications]
    end

    subgraph "Infrastructure"
        Database[Database Layer]
        External[External Services]
        Messaging[Messaging]
    end

    AuthDomain --> SharedKernel
    TemplateDomain --> SharedKernel
    TaskDomain --> SharedKernel
    QualityDomain --> SharedKernel
    WorkflowDomain --> SharedKernel

    TaskDomain --> AuthDomain
    TemplateDomain --> AuthDomain
    QualityDomain --> TaskDomain
    WorkflowDomain --> TaskDomain

    TemplateDomain --> Database
    TaskDomain --> Database
    QualityDomain --> Database
    WorkflowDomain --> Database

    TaskDomain --> External
    QualityDomain --> External
    WorkflowDomain --> Messaging
```

### 7.3 事件驅動架構設計

```mermaid
graph LR
    subgraph "Domain Events"
        TemplateCreated[TemplateCreated]
        ProjectGenerated[ProjectGenerated]
        TaskCreated[TaskCreated]
        TaskStatusChanged[TaskStatusChanged]
        QualityCheckCompleted[QualityCheckCompleted]
        WorkflowStarted[WorkflowStarted]
        WorkflowCompleted[WorkflowCompleted]
    end

    subgraph "Event Handlers"
        NotificationHandler[通知處理器]
        GitLabHandler[GitLab同步處理器]
        ReportHandler[報表更新處理器]
        WorkflowHandler[工作流程觸發器]
        CacheHandler[快取更新處理器]
    end

    subgraph "Integration Events"
        EmailNotification[Email通知]
        GitLabSync[GitLab同步]
        ReportUpdate[報表更新]
        CacheInvalidation[快取失效]
    end

    TaskCreated --> NotificationHandler
    TaskCreated --> GitLabHandler
    TaskStatusChanged --> NotificationHandler
    TaskStatusChanged --> GitLabHandler
    QualityCheckCompleted --> NotificationHandler
    QualityCheckCompleted --> ReportHandler
    ProjectGenerated --> GitLabHandler
    WorkflowStarted --> NotificationHandler

    NotificationHandler --> EmailNotification
    GitLabHandler --> GitLabSync
    ReportHandler --> ReportUpdate
    CacheHandler --> CacheInvalidation
```

---

## 8. 技術規格與實作建議

### 8.1 技術棧建議

**後端核心技術**：
- **.NET 8**: 主要開發框架
- **Entity Framework Core 8**: ORM映射
- **PostgreSQL 15**: 主要資料庫
- **Redis 7**: 快取和會話管理
- **MassTransit**: 訊息佇列和事件匯流排
- **Serilog**: 結構化日誌記錄

**AI和外部整合**：
- **OpenAI API**: AI程式碼分析
- **GitLab API**: 代碼管理整合
- **SendGrid**: 郵件發送服務
- **Azure Blob Storage**: 檔案存儲

**監控和部署**：
- **Prometheus + Grafana**: 監控和指標
- **ELK Stack**: 日誌分析
- **Docker + Kubernetes**: 容器化部署
- **Helm Charts**: Kubernetes應用管理

### 8.2 開發規範建議

**代碼組織結構**：
```
SoftwareDevelopment.API/
├── src/
│   ├── Core/
│   │   ├── SoftwareDevelopment.Domain/
│   │   │   ├── Templates/
│   │   │   ├── Tasks/
│   │   │   ├── Quality/
│   │   │   └── Workflows/
│   │   └── SoftwareDevelopment.Application/
│   │       ├── Templates/
│   │       ├── Tasks/
│   │       ├── Quality/
│   │       └── Workflows/
│   ├── Infrastructure/
│   │   ├── SoftwareDevelopment.Infrastructure/
│   │   └── SoftwareDevelopment.Persistence/
│   └── Presentation/
│       ├── SoftwareDevelopment.WebAPI/
│       └── SoftwareDevelopment.BackgroundServices/
├── tests/
└── docs/
```

**設計模式應用**：
- **Repository Pattern**: 資料存取抽象
- **Unit of Work**: 事務管理
- **CQRS**: 讀寫分離
- **Mediator Pattern**: 請求處理解耦
- **Strategy Pattern**: AI規則引擎
- **Observer Pattern**: 事件處理
- **Factory Pattern**: 物件創建

### 8.3 效能優化建議

**資料庫優化**：
- 適當的索引設計
- 查詢優化和分頁
- 連接池管理
- 讀寫分離配置

**快取策略**：
- Redis分散式快取
- 查詢結果快取
- 靜態資源CDN
- HTTP快取頭配置

**非同步處理**：
- 背景任務處理
- 訊息佇列應用
- 長時間操作分解
- 批次作業優化

---

## 9. 結論與下一步

### 9.1 設計總結

第2階段系統設計成功建立了四個核心業務領域的完整架構：

**設計成果**：
- **專案模板系統**: 支援多種架構模板的管理和生成
- **任務管理系統**: 完整的任務生命週期管理和GitLab整合
- **AI品質檢查系統**: 智能化的程式碼品質分析和建議
- **工作流程引擎**: 靈活的業務流程定義和執行

**設計特色**：
- **領域驅動**: 清晰的業務邊界和職責分離
- **事件驅動**: 鬆散耦合的模組間通信
- **可擴展性**: 支援未來功能的平滑擴展
- **整合性**: 與第1階段和外部系統的無縫整合

### 9.2 技術可行性評估

**架構優勢**：
- 基於成熟的DDD四層架構
- 使用企業級的技術棧
- 具備良好的擴展性和維護性
- 支援雲端原生部署

**實作複雜度**：
- 中高複雜度，需要經驗豐富的開發團隊
- AI整合部分需要特別關注
- GitLab整合需要仔細的錯誤處理
- 工作流程引擎需要詳細的測試

### 9.3 風險評估與緩解

**技術風險**：
- **AI服務依賴**: 建立後備的靜態分析機制
- **外部API穩定性**: 實作重試和降級機制
- **效能瓶頸**: 建立完善的監控和優化策略

**業務風險**：
- **使用者接受度**: 提供漸進式功能導入
- **資料遷移**: 建立完整的備份和測試程序
- **學習成本**: 準備充分的文件和培訓

### 9.4 下一步工作規劃

**立即執行**：
1. 更新第2階段資料庫架構設計
2. 建立詳細的API規格文件
3. 準備開發團隊技術交接文件

**短期規劃**：
1. 建立開發環境和CI/CD流程
2. 實作核心領域模型
3. 開發關鍵用例功能

**長期規劃**：
1. 完整功能開發和測試
2. 效能優化和安全加固
3. 使用者驗收和功能調整

這個系統設計為第2階段的開發提供了堅實的技術基礎，確保能夠交付一個高品質、可擴展的專案管理平台核心功能。

---

*此系統設計規格文件將作為第2階段開發的核心技術指引，並隨開發進展和需求變化持續更新完善。*