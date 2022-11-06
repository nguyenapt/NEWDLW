using System;

using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Transactions;
using System.Xml.Serialization;
using Netafim.WebPlatform.Web.Features.SystemConfigurator.Repositories.Impl.DefaultSystemConfiguratorRepository;
using Netafim.WebPlatform.Web.Features.SystemConfigurator.Repositories.Impl.DefaultSystemConfiguratorRepository.Models;
using Netafim.WebPlatform.Web.Features.SystemConfigurator.Services.Impl.XmlSystemConfiguratorImporter.Mappers;
using Netafim.WebPlatform.Web.Features.SystemConfigurator.Services.Impl.XmlSystemConfiguratorImporter.Models;

namespace Netafim.WebPlatform.Web.Features.SystemConfigurator.Services.Impl.XmlSystemConfiguratorImporter
{
    internal class ImportContext
    {
        public CultureInfo Culture { get; set; }
        
        public List<Crop> Crops { get; set; }

        public List<Region> Regions { get; set; }

        public List<WaterSource> WaterSources { get; set; }

        public List<Connector> Connectors { get; set; }

        public List<DripperLine> DripperLines { get; set; }

        public List<Filtration> Filtrations { get; set; }

        public List<FiltrationType> FiltrationTypes { get; set; }

        public List<Pipe> Pipes { get; set; }

        public List<Valve> Valves { get; set; }

        public List<Contact> Contacts { get; set; }

        public List<Solution> Solutions { get; set; }

        public List<Dealer> Dealers { get; set; }

        public List<DigitalFarming> DigitalFarmings { get; set; }

        public List<ProductRelation> ProductRelations { get; set; }

        public List<ContentRelation> ContentRelations { get; set; }

        public List<ContactRelation> ContactRelations { get; set; }
    }

    public class XmlSystemConfiguratorImporter : ISystemConfiguratorImporter
    {
        private readonly DefaultSystemConfiguratorRepository _systemConfiguratorRepository;

        public XmlSystemConfiguratorImporter()
        {
            _systemConfiguratorRepository = new DefaultSystemConfiguratorRepository(new SystemConfiguratorDbContext());
        }

        public SystemConfiguratorImportResult Import(Stream stream)
        {
            var result = new SystemConfiguratorImportResult();

            try
            {
                DoImport(stream);

                result.Success = true;
            }
            catch (Exception ex)
            {
                result.Success = false;
                result.Message = ex.Message;
            }

            return result;
        }

        private void DoImport(Stream stream)
        {
            if (stream == null) throw new ArgumentNullException(nameof(stream));

            var context = ParseZipArchive(stream);

            // Map
            var regionEntities = context.Regions.Map(context.Culture).ToList();
            var cropEntities = context.Crops.Map(context.Culture).ToList();
            var content = new List<ContentEntity>();
            content.AddRange(context.Solutions.Map(context.Culture));
            content.AddRange(context.Dealers.Map(context.Culture));
            content.AddRange(context.DigitalFarmings.Map(context.Culture));
            var contactEntities = context.Contacts.Map(context.Culture).ToList();

            var waterSourceEntities = context.WaterSources.Map(context.Culture).ToList();
            var filtrationTypeEntities = context.FiltrationTypes.Map(context.Culture).ToList();

            // products
            var filtrationEntities = context.Filtrations.Map(filtrationTypeEntities, waterSourceEntities, context.Culture);
            var dripperLineEntities = context.DripperLines.Map(context.Culture).ToList();
            var connectorEntities = context.Connectors.Map(context.Culture).ToList();
            var pipeEntities = context.Pipes.Map(context.Culture).ToList();
            var valveEntities = context.Valves.Map(context.Culture).ToList();

            var productEntities = new List<ProductEntity>();
            productEntities.AddRange(filtrationEntities);
            productEntities.AddRange(dripperLineEntities);
            productEntities.AddRange(connectorEntities);
            productEntities.AddRange(pipeEntities);
            productEntities.AddRange(valveEntities);

            var decisionEntities = new List<DecisionTreeEntity>();
            decisionEntities.AddRange(context.ProductRelations.Map(productEntities, cropEntities, regionEntities, context.Culture));
            decisionEntities.AddRange(context.ContentRelations.Map(content, cropEntities, regionEntities, context.Culture));
            decisionEntities.AddRange(context.ContactRelations.Map(contactEntities, cropEntities, regionEntities, context.Culture));

            using (var scope = new TransactionScope())
            {
                // Remove existing data first.
                _systemConfiguratorRepository.Delete(context.Culture);

                // Store imported data.
                _systemConfiguratorRepository.Store(connectorEntities);
                _systemConfiguratorRepository.Store(contactEntities);
                _systemConfiguratorRepository.Store(waterSourceEntities);
                _systemConfiguratorRepository.Store(filtrationTypeEntities);
                _systemConfiguratorRepository.Store(regionEntities);
                _systemConfiguratorRepository.Store(filtrationEntities);
                _systemConfiguratorRepository.Store(dripperLineEntities);
                _systemConfiguratorRepository.Store(pipeEntities);
                _systemConfiguratorRepository.Store(valveEntities);
                _systemConfiguratorRepository.Store(cropEntities);
                _systemConfiguratorRepository.Store(decisionEntities);

                scope.Complete();
            }
        }

        private static ImportContext ParseZipArchive(Stream stream)
        {
            var archive = new ZipArchive(stream);

            var context = new ImportContext();

            // Basic validation on input.
            foreach (var entry in archive.Entries)
            {
                var parts = entry.Name.Split('.');
                if (parts.Length != 2 || !parts[1].Equals("xml", StringComparison.OrdinalIgnoreCase)) throw new Exception($"File name {entry.Name} does not match with the expected format");

                parts = parts[0].Split('_');
                if (parts.Length != 2) throw new Exception($"File name {entry.Name} does not match with the expected format");

                var culture = new CultureInfo(parts[1]);

                if (context.Culture == null) context.Culture = culture;
                else if (!context.Culture.Equals(culture))
                    throw new Exception(
                        $"Found a file with country extension {culture} while a previous file had extension {context.Culture}");

                switch (parts[0])
                {
                    case "Connectors":
                        context.Connectors = Parse<Connector>(entry.Open(), "Connectors");
                        break;
                    case "Contacts":
                        context.Contacts = Parse<Contact>(entry.Open(), "Contacts");
                        break;
                    case "ContactRelations":
                        context.ContactRelations = Parse<ContactRelation>(entry.Open(), "ContactRelations");
                        break;
                    case "Dealers":
                        context.Dealers = Parse<Dealer>(entry.Open(), "Dealers");
                        break;
                    case "Solutions":
                        context.Solutions = Parse<Solution>(entry.Open(), "Solutions");
                        break;
                    case "DigitalFarming":
                        context.DigitalFarmings = Parse<DigitalFarming>(entry.Open(), "DigitalFarmings");
                        break;
                    case "ContentRelations":
                        context.ContentRelations = Parse<ContentRelation>(entry.Open(), "ContentRelations");
                        break;
                    case "Crops":
                        context.Crops = Parse<Crop>(entry.Open(), "Crops");
                        break;
                    case "DripperLines":
                        context.DripperLines = Parse<DripperLine>(entry.Open(), "DripperLines");
                        break;
                    case "Filtrations":
                        context.Filtrations = Parse<Filtration>(entry.Open(), "Filtrations");
                        break;
                    case "FiltrationTypes":
                        context.FiltrationTypes = Parse<FiltrationType>(entry.Open(), "FiltrationTypes");
                        break;
                    case "Pipes":
                        context.Pipes = Parse<Pipe>(entry.Open(), "Pipes");
                        break;
                    case "ProductRelations":
                        context.ProductRelations = Parse<ProductRelation>(entry.Open(), "ProductRelations");
                        break;
                    case "Regions":
                        context.Regions = Parse<Region>(entry.Open(), "Regions");
                        break;
                    case "Valves":
                        context.Valves = Parse<Valve>(entry.Open(), "Valves");
                        break;
                    case "WaterSources":
                        context.WaterSources = Parse<WaterSource>(entry.Open(), "WaterSources");
                        break;
                    
                    default:
                        throw new Exception($"Unexpected file found {entry.Name}");
                }
            }

            if (context.Culture == null) throw new Exception("Culture could not be determined from input");
            if (context.Regions == null || !context.Regions.Any()) throw new Exception("No regions found or could not be parsed");
            if (context.Crops == null || !context.Crops.Any()) throw new Exception("No crops found or could not be parsed");
            if (context.Filtrations == null || !context.Filtrations.Any()) throw new Exception("No filtratiosn found or could not be parsed");
            if (context.FiltrationTypes == null || !context.FiltrationTypes.Any()) throw new Exception("No filtration types found or could not be parsed");
            if (context.WaterSources == null || !context.WaterSources.Any()) throw new Exception("No water sources found or could not be parsed");
            if (context.Contacts == null || !context.Contacts.Any()) throw new Exception("No contacts found or could not be parsed");

            return context;
        }

        private static List<T> Parse<T>(Stream stream, string rootElement)
        {
            var serializer = new XmlSerializer(typeof(List<T>), new XmlRootAttribute(rootElement));
            var entities = (List<T>)serializer.Deserialize(stream);

            return entities;
        }
    }
}