using Crdc.Agro.Plataforma.Application.Interfaces;
using Crdc.Agro.Plataforma.Application.ViewModels.Requests.Log;
using Crdc.Agro.Plataforma.Application.ViewModels.Responses.Images;
using Crdc.Agro.Plataforma.Application.ViewModels.Responses.Invoices;
using Crdc.Agro.Plataforma.Domain.Extensions;
using Crdc.Agro.Plataforma.Domain.Interfaces.Repository;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Crdc.Agro.Plataforma.Worker
{
    public class Image3DCameraExecution
    {
        private readonly ILogger<Image3DCameraExecution> _logger;
        private readonly IImage3DService _image3DService;
        private readonly ILogService _logService;
        private readonly ITranslateService _translateService;
        private readonly IZipService _zipService;
        private readonly IConfiguration _configuration;

        private string logMessage = String.Empty;
        public Image3DCameraExecution(
            ILogger<Image3DCameraExecution> logger,
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

                await CreateCollectionCarColor();
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
        private async Task<IEnumerable<CarColorCollectionResponse>> SelectCarColorCollection()
        {
            return await _image3DService.SelectCarColorCollection();
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
        private async Task CreateCollectionCarColor()
        {
            var list = await SelectCarColorCollection();

            if (list != null && list.Count() > 0)
            {
                foreach (var item in list)
                {
                    var iconData = await _image3DService.SelectCarIconById(item.IconId);

                    if (iconData != null ) 
                    {
                        if ( !string.IsNullOrWhiteSpace(iconData.FileJBL)) 
                        {
                            var listIconCamera = _image3DService.SelectDataFileDaeCamera(item.IconId, iconData.FileNameDAE, item.CarColor);

                            if (listIconCamera != null)
                            {
                                foreach (var coord in listIconCamera)
                                {
                                    await _image3DService.InsertIconCamera(coord);
                                }
                            }
                        }
                    }
                }
            }
            logMessage = $"Atualização de Imagens executada em : {DateTimeOffset.Now.ToString("dd/MM/yyyy")} finalizado com sucesso em {DateTimeOffset.Now}.";

            _logger.LogInformation(logMessage);

            await _logService.CreateLogInformation(logMessage, Guid.Empty, string.Empty);
        }
    }
}