using CommonBoilerPlateEight.Application.Contracts.Services;
using CommonBoilerPlateEight.Domain.Interfaces;
using CommonBoilerPlateEight.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommonBoilerPlateEight.Domain.Services
{
    public class ApplicationSettingService : IApplicationSettingService
    {
        private readonly ISettingService _settingService;
        public ApplicationSettingService(ISettingService settingService)
        {
            _settingService = settingService;
        }
        public async Task SetEmailSettings(EmailSetupViewModel model)
        {
            var settingModels = new List<SettingViewModel>();
            AddSettingModel(SettingKeyConstants.KeyEmailSetupHostServer, model.Host ?? string.Empty, settingModels);
            AddSettingModel(SettingKeyConstants.KeyEmailSetupFromEmail, model.FromEmail ?? string.Empty, settingModels);
            AddSettingModel(SettingKeyConstants.KeyEmailSetupPort, model.Port ?? string.Empty, settingModels);
            AddSettingModel(SettingKeyConstants.KeyEmailSetupFromName, model.FromName ?? string.Empty, settingModels);
            AddSettingModel(SettingKeyConstants.KeyEmailSetupUserName, model.UserName ?? string.Empty, settingModels);
            AddSettingModel(SettingKeyConstants.KeyEmailSetupPassword, model.Password ?? string.Empty, settingModels);
            await _settingService.SetInBulk(settingModels).ConfigureAwait(false);
        }

        public async Task<EmailSetupViewModel> GetEmailSettings()
        {
            var emailSetupViewModel = new EmailSetupViewModel()
            {
                Host = await _settingService.Get(SettingKeyConstants.KeyEmailSetupHostServer),
                FromEmail = await _settingService.Get(SettingKeyConstants.KeyEmailSetupFromEmail),
                Port = await _settingService.Get(SettingKeyConstants.KeyEmailSetupPort),
                UserName = await _settingService.Get(SettingKeyConstants.KeyEmailSetupUserName),
                Password = await _settingService.Get(SettingKeyConstants.KeyEmailSetupPassword),
                FromName = await _settingService.Get(SettingKeyConstants.KeyEmailSetupFromName)
            };
            return emailSetupViewModel;
        }

        private static void AddSettingModel(string key, string value, List<SettingViewModel> settingModels)
        {
            settingModels.Add(new SettingViewModel
            {
                Key = key,
                Value = value
            });
        }
    }
}
