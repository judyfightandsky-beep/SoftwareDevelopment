using System;
using System.Collections.Generic;
using System.Linq;

namespace SoftwareDevelopment.Domain.Templates.ValueObjects
{
    /// <summary>
    /// 技術堆疊值物件，代表專案的技術選擇
    /// </summary>
    public class TechnologyStack
    {
        public string Framework { get; private set; }
        public string Version { get; private set; }
        public string Database { get; private set; }
        public string Runtime { get; private set; }
        public List<string> Libraries { get; private set; }

        public TechnologyStack(
            string framework,
            string version,
            string database,
            string runtime,
            List<string> libraries = null)
        {
            Framework = framework ?? throw new ArgumentNullException(nameof(framework));
            Version = version ?? throw new ArgumentNullException(nameof(version));
            Database = database ?? throw new ArgumentNullException(nameof(database));
            Runtime = runtime ?? throw new ArgumentNullException(nameof(runtime));
            Libraries = libraries ?? new List<string>();
        }

        /// <summary>
        /// 新增函式庫
        /// </summary>
        public void AddLibrary(string library)
        {
            if (string.IsNullOrWhiteSpace(library))
                throw new ArgumentException("函式庫名稱不能為空", nameof(library));

            if (!Libraries.Contains(library))
                Libraries.Add(library);
        }

        /// <summary>
        /// 移除函式庫
        /// </summary>
        public void RemoveLibrary(string library)
        {
            Libraries.Remove(library);
        }

        /// <summary>
        /// 檢查技術堆疊相容性
        /// </summary>
        public bool IsCompatibleWith(string databaseType)
        {
            // 定義相容性規則
            var compatibilityMap = new Dictionary<string, List<string>>
            {
                { ".NET Core", new List<string> { "PostgreSQL", "SQLServer", "MySQL" } },
                { "Spring Boot", new List<string> { "PostgreSQL", "MySQL", "Oracle" } }
                // 可根據實際情況擴展
            };

            return compatibilityMap.TryGetValue(Framework, out var compatibleDatabases)
                   && compatibleDatabases.Contains(databaseType);
        }

        public override bool Equals(object obj)
        {
            return obj is TechnologyStack other &&
                   Framework == other.Framework &&
                   Version == other.Version &&
                   Database == other.Database &&
                   Runtime == other.Runtime &&
                   Libraries.SequenceEqual(other.Libraries);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Framework, Version, Database, Runtime, Libraries);
        }
    }
}