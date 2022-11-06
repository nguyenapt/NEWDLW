using System;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using System.Xml.Serialization;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Netafim.WebPlatform.Web.Features.SystemConfigurator.Services.Impl.XmlSystemConfiguratorImporter.Models;

namespace Netafim.WebPlatform.UnitTest.Web.Features.SystemConfigurator.Import
{
    [TestClass]
    public class SystemConfiguratorImportTests
    {
        #region Serialization

        [TestMethod]
        public void Serialize_connectors()
        {
            var connectors = new[]
            {
                new Connector { Key = "fastring", CatalogNumber = "70640-######", Name = "Fast Ring" }
            };

            SerializeAndOutputToConsole(connectors, "Connectors");
        }

        [TestMethod]
        public void Serialize_contacts()
        {
            var contacts = new[]
            {
                new Contact
                {
                    PhoneNumber = "123456789",
                    FirstName = "John",
                    LastName = "Doe",
                    Title = "CEO",
                    Email = "john.doe@gmail.com"
                },
                new Contact
                {
                    PhoneNumber = "987654321",
                    FirstName = "Jane",
                    LastName = "Doe",
                    Title = "CFO",
                    Email = "jane.doe@gmail.com"
                }
            };

            SerializeAndOutputToConsole(contacts, "Contacts");
        }

        [TestMethod]
        public void Serialize_content_relations()
        {
            var relations = new[]
            {
                new ContentRelation() { CropKey = "soia", RegionKey = "abruzzo", ContentKey = "dorot" }
            };

            SerializeAndOutputToConsole(relations, "ContentRelations");
        }

        [TestMethod]
        public void Serialize_crops()
        {
            var crops = new[]
            {
                new Crop { Key = "mandorlo", Group = "Deciduous Plantations", Name ="Mandorlo", CropFactor = 0.75m},
                new Crop { Key = "soia", Group = "Row Crops", Name ="Soia", CropFactor = 1.1m}
            };

            SerializeAndOutputToConsole(crops, "Crops");
        }

        [TestMethod]
        public void Serialize_dealer_relations()
        {
            var relations = new[]
            {
                new DealerRelation { CropKey = "mandorlo", RegionKey = "bari", DealerKey = "1" },
                new DealerRelation { CropKey = "soia", RegionKey = "abruzzo", DealerKey = "2" }
            };

            SerializeAndOutputToConsole(relations, "DealerRelations");
        }

        [TestMethod]
        public void Serialize_dripperlines()
        {
            var dripperLines = new[]
            {
                new DripperLine { Key = "dripnet-pc-16010", CatalogNumber = "70640-######", Name = "DRIPNET PC 16010", FlowRate  = 1.6m, EmiterSpacing = 0.8m, NumberOfLaterals = 1, FlowVariation = 0 }
            };

            SerializeAndOutputToConsole(dripperLines, "DripperLines");
        }

        [TestMethod]
        public void Serialize_filtrations()
        {
            var filtrations = new[]
            {
                new Filtration { Key = "akal-1.5-filter-120-mesh", FlowRate = 8m, TypeOfFiltration = "Disc", FiltrationType = "manual", FamilyName = "Arkal", CatalogNumber = "70640-002720", Name = "ARKAL 1.5\" FILTER 120MESH (SHORT)"},
                new Filtration { Key = "akal-1.5-super-filter-120-mesh", FlowRate = 12.5m, TypeOfFiltration = "Disc", FiltrationType = "automatic", FamilyName = "Arkal", CatalogNumber = "70640-003400", Name = "ARKAL 1.5\" SUPER FILTER 120 MESH"}
            };

            SerializeAndOutputToConsole(filtrations, "Filtrations");
        }

        [TestMethod]
        public void Serialize_filtration_types()
        {
            var filtrationTypes = new[]
            {
                new FiltrationType {Key = "automatic", Name = "Automatic"},
                new FiltrationType {Key = "manual", Name = "Manual"}
            };

            SerializeAndOutputToConsole(filtrationTypes, "FiltrationTypes");
        }

        [TestMethod]
        public void Serialize_pipes()
        {
            var pipes = new[]
            {
                new Pipe { Key = "flexnet", CatalogNumber = "70640-######", Name = "FlexNet" }
            };

            SerializeAndOutputToConsole(pipes, "Pipes");
        }

        [TestMethod]
        public void Serialize_product_relations()
        {
            var relations = new[]
            {
                new ProductRelation { CropKey = "mandorlo", RegionKey = "bari", ProductKey = "dripnet-pc-16010" },
                new ProductRelation { CropKey = "soia", RegionKey = "abruzzo", ProductKey = "dripnet-pc-17050" }
            };

            SerializeAndOutputToConsole(relations, "ProductRelations");
        }

        [TestMethod]
        public void Serialize_regions()
        {
            var regions = new[]
            {
                new Region { Key = "abruzzo", Name = "ABRUZZO", Eto = 6, Dealer = "G", DealerPhone = "888888", NetafimSales = "TARANTINO", NetafimPhone = "339 4937403" },
                new Region { Key = "bari", Name = "BARI", Eto = 9, Dealer = "P", DealerPhone = "888888", NetafimSales = "POLLICORO", NetafimPhone = "339 4937403" }
            };

            SerializeAndOutputToConsole(regions, "Regions");
        }

        [TestMethod]
        public void Serialize_valves()
        {
            var valves = new[]
            {
                new Valve { Key = "dorot", CatalogNumber = "70640-######", Name = "Dorot valve" }
            };

            SerializeAndOutputToConsole(valves, "Valves");
        }

        [TestMethod]
        public void Serialize_watersources()
        {
            var waterSources = new[]
            {
                new WaterSource { Key = "surface", Name = "Surface"},
                new WaterSource { Key = "well", Name = "Well"}
            };

            SerializeAndOutputToConsole(waterSources, "WaterSources");
        }

        #endregion

        #region Deserialize

        [TestMethod]
        public void Can_deserialize_connectors()
        {
            var stream = GetEmbeddedFileAsStream<SystemConfiguratorImportTests>("Files.Connectors_en.xml");
            Assert.IsNotNull(stream);

            var serializer = new XmlSerializer(typeof(List<Connector>), new XmlRootAttribute("Connectors"));

            var connectors = (List<Connector>)serializer.Deserialize(stream);

            Assert.AreEqual(2, connectors.Count);
        }

        [TestMethod]
        public void Can_deserialize_contacts()
        {
            var stream = GetEmbeddedFileAsStream<SystemConfiguratorImportTests>("Files.Contacts_en.xml");
            Assert.IsNotNull(stream);

            var serializer = new XmlSerializer(typeof(List<Contact>), new XmlRootAttribute("Contacts"));

            var contacts = (List<Contact>)serializer.Deserialize(stream);

            Assert.AreEqual(2, contacts.Count);
        }

        [TestMethod]
        public void Can_deserialize_content_relations()
        {
            var stream = GetEmbeddedFileAsStream<SystemConfiguratorImportTests>("Files.ContentRelations_en.xml");
            Assert.IsNotNull(stream);

            var serializer = new XmlSerializer(typeof(List<ContentRelation>), new XmlRootAttribute("ContentRelations"));

            var relations = (List<ContentRelation>)serializer.Deserialize(stream);

            Assert.AreEqual(3, relations.Count);
        }

        [TestMethod]
        public void Can_deserialize_crops()
        {
            var stream = GetEmbeddedFileAsStream<SystemConfiguratorImportTests>("Files.Crops_en.xml");
            Assert.IsNotNull(stream);

            var serializer = new XmlSerializer(typeof(List<Crop>), new XmlRootAttribute("Crops"));

            var crops = (List<Crop>)serializer.Deserialize(stream);

            Assert.AreEqual(32, crops.Count);
        }

        [TestMethod]
        public void Can_deserialize_dripperlines()
        {
            var stream = GetEmbeddedFileAsStream<SystemConfiguratorImportTests>("Files.DripperLines_en.xml");
            Assert.IsNotNull(stream);

            var serializer = new XmlSerializer(typeof(List<DripperLine>), new XmlRootAttribute("DripperLines"));

            var dripperLines = (List<DripperLine>)serializer.Deserialize(stream);

            Assert.AreEqual(7, dripperLines.Count);
        }

        [TestMethod]
        public void Can_deserialize_filtrations()
        {
            var stream = GetEmbeddedFileAsStream<SystemConfiguratorImportTests>("Files.Filtrations_en.xml");
            Assert.IsNotNull(stream);

            var serializer = new XmlSerializer(typeof(List<Filtration>), new XmlRootAttribute("Filtrations"));

            var filtrations = (List<Filtration>)serializer.Deserialize(stream);

            Assert.AreEqual(2, filtrations.Count);
        }

        [TestMethod]
        public void Can_deserialize_filtration_types()
        {
            var stream = GetEmbeddedFileAsStream<SystemConfiguratorImportTests>("Files.FiltrationTypes_en.xml");
            Assert.IsNotNull(stream);

            var serializer = new XmlSerializer(typeof(List<FiltrationType>), new XmlRootAttribute("FiltrationTypes"));

            var filtrationTypes = (List<FiltrationType>)serializer.Deserialize(stream);

            Assert.AreEqual(2, filtrationTypes.Count);
        }

        [TestMethod]
        public void Can_deserialize_pipes()
        {
            var stream = GetEmbeddedFileAsStream<SystemConfiguratorImportTests>("Files.Pipes_en.xml");
            Assert.IsNotNull(stream);

            var serializer = new XmlSerializer(typeof(List<Pipe>), new XmlRootAttribute("Pipes"));

            var pipes = (List<Pipe>)serializer.Deserialize(stream);

            Assert.AreEqual(1, pipes.Count);
        }

        [TestMethod]
        public void Can_deserialize_product_relations()
        {
            var stream = GetEmbeddedFileAsStream<SystemConfiguratorImportTests>("Files.ProductRelations_en.xml");
            Assert.IsNotNull(stream);

            var serializer = new XmlSerializer(typeof(List<ProductRelation>), new XmlRootAttribute("ProductRelations"));

            var relations = (List<ProductRelation>)serializer.Deserialize(stream);

            Assert.AreEqual(9, relations.Count);
        }
        
        [TestMethod]
        public void Can_deserialize_regions()
        {
            var stream = GetEmbeddedFileAsStream<SystemConfiguratorImportTests>("Files.Regions_en.xml");
            Assert.IsNotNull(stream);

            var serializer = new XmlSerializer(typeof(List<Region>), new XmlRootAttribute("Regions"));

            var regions = (List<Region>) serializer.Deserialize(stream);

            Assert.AreEqual(25, regions.Count);
        }

        [TestMethod]
        public void Can_deserialize_valves()
        {
            var stream = GetEmbeddedFileAsStream<SystemConfiguratorImportTests>("Files.Valves_en.xml");
            Assert.IsNotNull(stream);

            var serializer = new XmlSerializer(typeof(List<Valve>), new XmlRootAttribute("Valves"));

            var valves = (List<Valve>)serializer.Deserialize(stream);

            Assert.AreEqual(1, valves.Count);
        }

        [TestMethod]
        public void Can_deserialize_watersources()
        {
            var stream = GetEmbeddedFileAsStream<SystemConfiguratorImportTests>("Files.WaterSources_en.xml");
            Assert.IsNotNull(stream);

            var serializer = new XmlSerializer(typeof(List<WaterSource>), new XmlRootAttribute("WaterSources"));

            var waterSources = (List<WaterSource>)serializer.Deserialize(stream);

            Assert.AreEqual(2, waterSources.Count);
        }

        #endregion

        #region Helpers

        private static void SerializeAndOutputToConsole<T>(IEnumerable<T> filtrations, string elementName)
        {
            var serializer = new XmlSerializer(filtrations.GetType(), new XmlRootAttribute(elementName));
            string xml;

            using (var sw = new StringWriter())
            {
                var settings = new XmlWriterSettings { Indent = true };
                using (var writer = XmlWriter.Create(sw, settings))
                {
                    serializer.Serialize(writer, filtrations);
                    xml = sw.ToString();
                }
            }

            Console.Write(xml);
        }

        private static Stream GetEmbeddedFileAsStream<T>(string relativePath)
        {
            var type = typeof(T);
            var str = type.Assembly.GetManifestResourceStream(type, relativePath);
            if (str == null)
                throw new Exception(string.Format("Could not locate embedded resource '{0}.{1}' in '{2}'.",
                    type.Namespace, relativePath, type.Assembly));
            return str;
        }

        #endregion
    }
}