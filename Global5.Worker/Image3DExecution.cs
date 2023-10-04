using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Crdc.Agro.Plataforma.Application.Interfaces;
using Crdc.Agro.Plataforma.Application.ViewModels.Requests.Log;
using Crdc.Agro.Plataforma.Application.ViewModels.Responses.Invoices;
using Crdc.Agro.Plataforma.Domain.Extensions;
using Crdc.Agro.Plataforma.Domain.Interfaces.Repository;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Crdc.Agro.Plataforma.Worker
{
    public class Image3DExecution
    {
        private readonly ILogger<Image3DExecution> _logger;
        private readonly IImage3DService _image3DService;
        private readonly ILogService _logService;
        private readonly ITranslateService _translateService;
        private readonly IZipService _zipService;
        private readonly IConfiguration _configuration;

        private string logMessage = String.Empty;
        public Image3DExecution(
            ILogger<Image3DExecution> logger,
            IImage3DService image3DService,
            IZipService zipService,
            ILogService logService,
            ITranslateService translateService,
            IImage3DRepository image3DRepository,
            IConfiguration configuration
        )
        {
            _logger = logger;
            _image3DService = image3DService;
            _zipService = zipService;
            _logService = logService;
            _translateService = translateService;
            _configuration = configuration;
        }
        public async Task ExecuteAsync()
        {
            logMessage = $"Worker Icon iniciado em: {DateTimeOffset.Now}";

            _logger.LogInformation(logMessage);

            try
            {
                await _logService.CreateLogInformation(logMessage, Guid.Empty, string.Empty);

                await UpdateIcon();
            }
            catch (Exception ex)
            {
                logMessage = $"Worker Icon com Problemas na execução. Motivo: {ex.Message}";

                _logger.LogInformation(logMessage);

                await _logService.CreateLogError(logMessage, ex.ToString(), ex.StackTrace, Guid.Empty, string.Empty);
            }
            GC.Collect();
            GC.WaitForPendingFinalizers();
            GC.Collect();

            _logger.LogInformation("Worker Bank Slip Document Processado. Aguardar próximo Ciclo de execução.", DateTimeOffset.Now);
        }
        private async Task<IEnumerable<IconResponse>> SelectCarIcon()
        {
            return await _image3DService.SelectCarIcon();
        }
        private async Task<List<Image3DUpdateRequest>> ProcessFile(IEnumerable<IconResponse> listIcon)
        {
            List<Image3DUpdateRequest> listImage3DUpdateRequest = new();
            Image3DUpdateRequest image3DUpdateRequest = null;

            //Parallel.ForEach(listIcon, async line =>
            foreach (var item in listIcon)
            {
                try
                {
                    if (!string.IsNullOrWhiteSpace(item.FileNameDAE))
                    {
                        try
                        {
                            var response = await GetFileDae(item.FileNameDAE);

                            if ( response != null )
                            {
                                var dataFile =  _image3DService.SelectDataFileDae(item.FileNameDAE);

                                if ( dataFile != null )
                                {
                                    image3DUpdateRequest = new Image3DUpdateRequest()
                                    {
                                        IconId = item.IconId,
                                        IconName = item.FileNameDAE,
                                        CenterX = dataFile.CenterX,
                                        CenterY = dataFile.CenterY,
                                        CenterZ = dataFile.CenterZ,
                                        Ray = dataFile.Ray,
                                        FileJBL = Convert.ToBase64String(response)
                                    };
                                    listImage3DUpdateRequest.Add(image3DUpdateRequest);
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            logMessage = $"Process File com problemas. Json {JsonMapExtensions.Bind("")} Motivo: {ex.Message}";

                            _logger.LogInformation(logMessage);

                            await _logService.CreateLogError(logMessage, ex.ToString(), ex.StackTrace, Guid.Empty, string.Empty);
                        }
                    }
                }
                catch (Exception ex)
                {
                    logMessage = $"Process File com problemas. Json {JsonMapExtensions.Bind("")} Motivo: {ex.Message}";

                    _logger.LogInformation(logMessage);

                    await _logService.CreateLogError(logMessage, ex.ToString(), ex.StackTrace, Guid.Empty, string.Empty);
                }
            };
            return listImage3DUpdateRequest;
        }

        private async Task<byte[]> GetFileDae(string fileName, bool compress = true)
        {
            var directoryFile = _configuration.GetSection("DAESettings").GetSection("Directory").Value;

            var file = await File.ReadAllBytesAsync(directoryFile + "\\" + fileName);

            if (compress)
            {
                return _zipService.Compress(fileName, file);
            }
            return file;
        }
        private async Task UpdateIcon()
        {
            var list = await SelectCarIcon();

            if (list != null && list.Count() > 0)
            {
                var listImageFile = await ProcessFile(list);

                if (listImageFile != null && listImageFile.Count() > 0)
                {
                    foreach (var item in listImageFile)
                    {
                        await _image3DService.UpdateIcon(item);
                    }
                }
            }
            logMessage = $"Atualização de Imagens executada em : {DateTimeOffset.Now.ToString("dd/MM/yyyy")} finalizado com sucesso em {DateTimeOffset.Now}.";

            _logger.LogInformation(logMessage);

            await _logService.CreateLogInformation(logMessage, Guid.Empty, string.Empty);
        }
    }
}