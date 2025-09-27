using System;
using System.Collections.Generic;
using System.Linq;
using SoftwareDevelopment.Domain.Shared.ValueObjects;

namespace SoftwareDevelopment.Domain.Templates.ValueObjects
{
    /// <summary>
    /// 專案模板配置值物件，表示模板的技術堆疊和模組配置
    /// </summary>
    public class TemplateConfiguration
    {
        public TechnologyStack TechStack { get; private set; }
        public List<TemplateModule> Modules { get; private set; }
        public DatabaseSettings Database { get; private set; }
        public AuthenticationSettings Authentication { get; private set; }
        public Dictionary<string, object> CustomSettings { get; private set; }

        public TemplateConfiguration(
            TechnologyStack techStack,
            List<TemplateModule> modules,
            DatabaseSettings database,
            AuthenticationSettings authentication,
            Dictionary<string, object> customSettings = null)
        {
            TechStack = techStack ?? throw new ArgumentNullException(nameof(techStack));
            Modules = modules ?? new List<TemplateModule>();
            Database = database ?? throw new ArgumentNullException(nameof(database));
            Authentication = authentication ?? throw new ArgumentNullException(nameof(authentication));
            CustomSettings = customSettings ?? new Dictionary<string, object>();
        }

        /// <summary>
        /// 新增模組
        /// </summary>
        public void AddModule(TemplateModule module)
        {
            if (module == null)
                throw new ArgumentNullException(nameof(module));

            if (Modules.Any(m => m.Id == module.Id))
                throw new InvalidOperationException("已存在相同ID的模組");

            Modules.Add(module);
        }

        /// <summary>
        /// 移除模組
        /// </summary>
        public void RemoveModule(ModuleId moduleId)
        {
            var moduleToRemove = Modules.FirstOrDefault(m => m.Id == moduleId);
            if (moduleToRemove != null)
                Modules.Remove(moduleToRemove);
        }

        /// <summary>
        /// 更新資料庫設定
        /// </summary>
        public void UpdateDatabaseSettings(DatabaseSettings settings)
        {
            Database = settings ?? throw new ArgumentNullException(nameof(settings));
        }

        /// <summary>
        /// 驗證配置
        /// </summary>
        public ValidationResult Validate()
        {
            var validationResult = new ValidationResult();

            // 驗證技術堆疊
            if (!TechStack.IsCompatibleWith(Database.Type))
                validationResult.AddError("技術堆疊與資料庫類型不相容");

            // 驗證模組依賴
            foreach (var module in Modules)
            {
                var moduleDependencyErrors = module.ValidateDependencies(Modules);
                validationResult.AddErrors(moduleDependencyErrors);
            }

            // 驗證必要模組
            var requiredModules = Modules.Where(m => m.IsRequired);
            if (requiredModules.Any(m => !Modules.Contains(m)))
                validationResult.AddError("缺少必要模組");

            return validationResult;
        }
    }
}