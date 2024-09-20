using System;
using System.Collections.Generic;
using TECNM.Residencias.Data.Entities;

namespace TECNM.Residencias
{
    internal sealed class AppSettings
    {
        private static readonly TimeSpan DefaultBackupFrequency = TimeSpan.FromDays(30);

        private static AppSettings? s_instance;
        private IDictionary<string, Setting> _settings;

        public static AppSettings Default
        {
            get
            {
                if (s_instance == null)
                {
                    using var context = new AppDbContext();
                    var settings = new Dictionary<string, Setting>();

                    foreach (Setting setting in context.Settings.EnumerateSettings())
                    {
                        settings[setting.Name] = setting;
                    }

                    s_instance = new AppSettings(settings);
                }

                return s_instance;
            }
        }

        private AppSettings(IDictionary<string, Setting> settings)
        {
            _settings = settings;
        }

        public int DefaultAdvisorType
        {
            get => int.Parse(GetSetting(nameof(DefaultAdvisorType), -1).Value);
            set => SetSetting(nameof(DefaultAdvisorType), value);
        }

        public int DefaultCompanyType
        {
            get => int.Parse(GetSetting(nameof(DefaultCompanyType), -1).Value);
            set => SetSetting(nameof(DefaultCompanyType), value);
        }

        public long DefaultStudentCareer
        {
            get => long.Parse(GetSetting(nameof(DefaultStudentCareer), -1).Value);
            set => SetSetting(nameof(DefaultStudentCareer), value);
        }

        public DateTime LastBackupDate
        {
            get => DateTime.Parse(GetSetting(nameof(LastBackupDate), "2024-09-01 13:00:00").Value);
            set => SetSetting(nameof(LastBackupDate), value.ToString("yyyy-MM-dd HH:mm:ss"));
        }

        public TimeSpan BackupFrequency
        {
            get => TimeSpan.Parse(GetSetting(nameof(BackupFrequency), DefaultBackupFrequency).Value);
            set => SetSetting(nameof(BackupFrequency), value);
        }

        /// <summary>
        /// Gets a value indicating whether a backup is required.
        /// </summary>
        /// <remarks>
        /// The property evaluates to <c>true</c> if the current date and time
        /// is greater than the sum of the last backup date and the backup frequency.
        /// This property is not stored in the database.
        /// </remarks>
        /// <value>
        /// <c>true</c> if a backup is required; otherwise, <c>false</c>.
        /// </value>
        public bool IsBackupRequired => DateTime.Now > (LastBackupDate + BackupFrequency);

        public void Save()
        {
            using var context = new AppDbContext();

            foreach (Setting setting in _settings.Values)
            {
                context.Settings.InsertOrUpdate(setting);
            }

            context.Commit();
        }

        private Setting GetSetting(string name, object defaultValue)
        {
            Setting? setting;
            if (!_settings.TryGetValue(name, out setting))
            {
                setting = new Setting
                {
                    Name = name,
                    Value = defaultValue.ToString()!,
                };

                _settings[name] = setting;
            }

            return setting;
        }

        private void SetSetting(string name, object value)
        {
            Setting setting = GetSetting(name, "");
            setting.Value = value.ToString()!;
        }
    }
}
