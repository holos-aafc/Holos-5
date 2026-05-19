using H.Core.Migrations;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json.Linq;

namespace H.Core.Test.Migrations;

[TestClass]
public class V4ToV5MigrationTests
{
    private V4ToV5Migration _migration = null!;

    [TestInitialize]
    public void Setup()
    {
        _migration = new V4ToV5Migration();
    }

    [TestMethod]
    public void FromVersion_ReturnsV4()
    {
        Assert.AreEqual(new Version("4.0"), _migration.FromVersion);
    }

    [TestMethod]
    public void ToVersion_ReturnsV5()
    {
        Assert.AreEqual(new Version("5.0"), _migration.ToVersion);
    }

    [TestMethod]
    public void MigrateApplicationData_WithFarms_NoErrors()
    {
        var root = new JObject
        {
            ["Farms"] = new JArray
            {
                new JObject
                {
                    ["Name"] = "Test Farm",
                    ["Components"] = new JArray
                    {
                        new JObject
                        {
                            ["$type"] = "H.Core.Models.LandManagement.Rotation.RotationComponent, H.Core",
                            ["ShiftLeft"] = true
                        }
                    }
                }
            }
        };

        // Should not throw
        _migration.MigrateApplicationData(root);

        // Farm data should still be intact
        var farms = root["Farms"] as JArray;
        Assert.IsNotNull(farms);
        Assert.AreEqual(1, farms!.Count);
    }

    [TestMethod]
    public void MigrateApplicationData_NoFarmsProperty_NoErrors()
    {
        var root = new JObject
        {
            ["GlobalSettings"] = new JObject()
        };

        // Should not throw even when Farms is missing
        _migration.MigrateApplicationData(root);
    }

    [TestMethod]
    public void MigrateFarmExport_WithFarms_NoErrors()
    {
        var farms = new JArray
        {
            new JObject
            {
                ["Name"] = "Farm 1",
                ["Components"] = new JArray
                {
                    new JObject
                    {
                        ["$type"] = "H.Core.Models.Animals.Beef.CowCalfComponent, H.Core",
                        ["Name"] = "Beef Herd"
                    }
                }
            },
            new JObject
            {
                ["Name"] = "Farm 2",
                ["Components"] = new JArray()
            }
        };

        // Should not throw
        _migration.MigrateFarmExport(farms);

        Assert.AreEqual(2, farms.Count);

        Assert.AreEqual(1, (farms[0]["Components"] as JArray)!.Count);
        Assert.AreEqual(0, (farms[1]["Components"] as JArray)!.Count);
    }

    [TestMethod]
    public void MigrateFarmExport_EmptyArray_NoErrors()
    {
        var farms = new JArray();

        // Should not throw
        _migration.MigrateFarmExport(farms);

        Assert.AreEqual(0, farms.Count);
    }

    [TestMethod]
    public void MigrateApplicationData_WithNullComponents_NoErrors()
    {
        var root = new JObject
        {
            ["Farms"] = new JArray
            {
                new JObject
                {
                    ["Name"] = "Test Farm",
                    ["Components"] = null
                }
            }
        };

        // Should not throw
        _migration.MigrateApplicationData(root);

        var farms = root["Farms"] as JArray;
        Assert.IsNotNull(farms);
        Assert.AreEqual(1, farms.Count);
    }

    [TestMethod]
    public void MigrateApplicationData_WithMultipleFarmsAndComponents_PreservesStructure()
    {
        var root = new JObject
        {
            ["Farms"] = new JArray
            {
                new JObject
                {
                    ["Name"] = "Farm 1",
                    ["Components"] = new JArray
                    {
                        new JObject
                        {
                            ["$type"] = "H.Core.Models.LandManagement.Rotation.RotationComponent, H.Core",
                            ["Name"] = "Rotation 1"
                        },
                        new JObject
                        {
                            ["$type"] = "H.Core.Models.Animals.Beef.CowCalfComponent, H.Core",
                            ["Name"] = "Beef Herd"
                        }
                    }
                },
                new JObject
                {
                    ["Name"] = "Farm 2",
                    ["Components"] = new JArray
                    {
                        new JObject
                        {
                            ["$type"] = "H.Core.Models.Animals.Swine.SwineComponent, H.Core"
                        }
                    }
                }
            }
        };

        _migration.MigrateApplicationData(root);

        var farms = root["Farms"] as JArray;
        Assert.IsNotNull(farms);
        Assert.AreEqual(2, farms.Count);
        Assert.AreEqual(2, (farms[0]["Components"] as JArray)!.Count);
        Assert.AreEqual(1, (farms[1]["Components"] as JArray)!.Count);
    }

    [TestMethod]
    public void MigrateFarmExport_WithVariousComponentTypes_PreserveAll()
    {
        var farms = new JArray
        {
            new JObject
            {
                ["Name"] = "Mixed Farm",
                ["Components"] = new JArray
                {
                    new JObject { ["$type"] = "H.Core.Models.LandManagement.Rotation.RotationComponent, H.Core" },
                    new JObject { ["$type"] = "H.Core.Models.Animals.Beef.CowCalfComponent, H.Core" },
                    new JObject { ["$type"] = "H.Core.Models.Animals.Poultry.LayerComponent, H.Core" },
                }
            }
        };

        _migration.MigrateFarmExport(farms);

        var components = (farms[0]["Components"] as JArray)!;
        Assert.AreEqual(3, components.Count);
    }

    [TestMethod]
    public void MigrateApplicationData_WithEmptyFarmsArray_NoErrors()
    {
        var root = new JObject
        {
            ["Farms"] = new JArray()
        };

        _migration.MigrateApplicationData(root);

        var farms = root["Farms"] as JArray;
        Assert.IsNotNull(farms);
        Assert.AreEqual(0, farms.Count);
    }

    // ----------------------------------------------------------------------------------
    // IsSecondaryCrop normalization (v4 farms saved before the flag existed need
    // collection-membership-based fix-up before deserialization). See the class-level
    // docstring on V4ToV5Migration for the full rationale.
    // ----------------------------------------------------------------------------------

    [TestMethod]
    public void Migrate_FieldSystemComponent_SetsIsSecondaryCropFalseOnCropViewItems()
    {
        var farms = BuildFarmExportWithFieldComponent(
            mainCropMissingFlag: true,
            coverCropMissingFlag: true);

        _migration.MigrateFarmExport(farms);

        var fieldComponent = (farms[0]["Components"] as JArray)![0];
        var mainCrops = (fieldComponent["CropViewItems"] as JArray)!;
        Assert.AreEqual(2, mainCrops.Count);
        Assert.AreEqual(false, mainCrops[0]["IsSecondaryCrop"]!.Value<bool>());
        Assert.AreEqual(false, mainCrops[1]["IsSecondaryCrop"]!.Value<bool>());
    }

    [TestMethod]
    public void Migrate_FieldSystemComponent_SetsIsSecondaryCropTrueOnCoverCrops()
    {
        var farms = BuildFarmExportWithFieldComponent(
            mainCropMissingFlag: true,
            coverCropMissingFlag: true);

        _migration.MigrateFarmExport(farms);

        var fieldComponent = (farms[0]["Components"] as JArray)![0];
        var coverCrops = (fieldComponent["CoverCrops"] as JArray)!;
        Assert.AreEqual(1, coverCrops.Count);
        Assert.AreEqual(true, coverCrops[0]["IsSecondaryCrop"]!.Value<bool>());
    }

    [TestMethod]
    public void Migrate_FieldSystemComponent_OverwritesStaleExistingFlag()
    {
        // A main crop with IsSecondaryCrop=true is the corruption pattern we keep seeing
        // on v4 imports. Migration must overwrite it to false because collection
        // membership is the authoritative signal — not whatever the JSON happens to say.
        var farms = new JArray
        {
            new JObject
            {
                ["Components"] = new JArray
                {
                    new JObject
                    {
                        ["$type"] = "H.Core.Models.LandManagement.Fields.FieldSystemComponent, H.Core",
                        ["CropViewItems"] = new JArray
                        {
                            new JObject { ["IsSecondaryCrop"] = true, ["Year"] = 1980 }, // wrong; must flip to false
                        },
                        ["CoverCrops"] = new JArray
                        {
                            new JObject { ["IsSecondaryCrop"] = false, ["Year"] = 1980 }, // wrong; must flip to true
                        },
                    },
                },
            },
        };

        _migration.MigrateFarmExport(farms);

        var fieldComponent = (farms[0]["Components"] as JArray)![0];
        Assert.AreEqual(false, fieldComponent["CropViewItems"]![0]!["IsSecondaryCrop"]!.Value<bool>());
        Assert.AreEqual(true,  fieldComponent["CoverCrops"]![0]!["IsSecondaryCrop"]!.Value<bool>());
    }

    [TestMethod]
    public void Migrate_FieldSystemComponent_HandlesValuesEnvelopeShape()
    {
        // Newtonsoft.Json with TypeNameHandling.Auto sometimes wraps collections in
        // `{ "$type": "...", "$values": [...] }` instead of emitting a plain JArray.
        // The migration must handle both shapes.
        var farms = new JArray
        {
            new JObject
            {
                ["Components"] = new JArray
                {
                    new JObject
                    {
                        ["$type"] = "H.Core.Models.LandManagement.Fields.FieldSystemComponent, H.Core",
                        ["CropViewItems"] = new JObject
                        {
                            ["$type"] = "System.Collections.ObjectModel.ObservableCollection`1[[...]], System",
                            ["$values"] = new JArray
                            {
                                new JObject { ["Year"] = 1980 },
                                new JObject { ["Year"] = 1981 },
                            },
                        },
                    },
                },
            },
        };

        _migration.MigrateFarmExport(farms);

        var inner = farms[0]!["Components"]![0]!["CropViewItems"]!["$values"] as JArray;
        Assert.IsNotNull(inner);
        Assert.AreEqual(false, inner[0]["IsSecondaryCrop"]!.Value<bool>());
        Assert.AreEqual(false, inner[1]["IsSecondaryCrop"]!.Value<bool>());
    }

    [TestMethod]
    public void Migrate_NonFieldComponent_IsLeftAlone()
    {
        // Animal / shelterbelt / AD components shouldn't get touched.
        var farms = new JArray
        {
            new JObject
            {
                ["Components"] = new JArray
                {
                    new JObject
                    {
                        ["$type"] = "H.Core.Models.Animals.Beef.BeefCattleComponent, H.Core",
                        ["Groups"] = new JArray { new JObject { ["Name"] = "Cow-Calf" } },
                    },
                },
            },
        };

        _migration.MigrateFarmExport(farms);

        var component = (farms[0]["Components"] as JArray)![0];
        Assert.IsNull(component["IsSecondaryCrop"]);
        Assert.IsNotNull(component["Groups"]);
    }

    /// <summary>
    /// Helper that builds a minimal v4-shape farm export with one field component
    /// containing two main crops and one cover crop. Toggle whether the
    /// <c>IsSecondaryCrop</c> property is omitted (legacy farms) or present.
    /// </summary>
    private static JArray BuildFarmExportWithFieldComponent(
        bool mainCropMissingFlag,
        bool coverCropMissingFlag)
    {
        JObject Crop(int year, bool? flag)
        {
            var o = new JObject { ["Year"] = year };
            if (flag.HasValue)
            {
                o["IsSecondaryCrop"] = flag.Value;
            }
            return o;
        }

        return new JArray
        {
            new JObject
            {
                ["Components"] = new JArray
                {
                    new JObject
                    {
                        ["$type"] = "H.Core.Models.LandManagement.Fields.FieldSystemComponent, H.Core",
                        ["CropViewItems"] = new JArray
                        {
                            Crop(1980, mainCropMissingFlag ? null : false),
                            Crop(1981, mainCropMissingFlag ? null : false),
                        },
                        ["CoverCrops"] = new JArray
                        {
                            Crop(1980, coverCropMissingFlag ? null : true),
                        },
                    },
                },
            },
        };
    }
}
