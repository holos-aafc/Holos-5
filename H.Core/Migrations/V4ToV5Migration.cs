using Newtonsoft.Json.Linq;

namespace H.Core.Migrations;

/// <summary>
/// Migrates farm JSON from v4.0 to v5.0 schema.
///
/// <para>Currently fixes one v4 data-shape gap that bit us in production:</para>
///
/// <para>
/// In v4, the convention was that <c>CropViewItem</c> instances stored in
/// <c>FieldSystemComponent.CropViewItems</c> were main crops and instances stored in
/// <c>FieldSystemComponent.CoverCrops</c> were secondary crops — distinguished by which
/// collection they lived in, not by an <c>IsSecondaryCrop</c> flag. v4 added the
/// <c>IsSecondaryCrop</c> bool late in its lifecycle, and some farms saved before that
/// addition do not have the flag set correctly on existing items. When such a farm is
/// loaded into v5 the carbon pipeline reads <c>IsSecondaryCrop</c> directly (e.g.
/// <c>AssignPerennialStandIds</c> filters on <c>IsSecondaryCrop == false</c>) and crashed
/// before defensive fallbacks were added in four sibling methods.
/// </para>
///
/// <para>
/// This migration normalizes the flag at the JObject level — before deserialization —
/// based on collection membership, which is the authoritative signal:
/// </para>
/// <list type="bullet">
///   <item>Items in <c>FieldSystemComponent.CropViewItems</c> → <c>IsSecondaryCrop = false</c></item>
///   <item>Items in <c>FieldSystemComponent.CoverCrops</c> → <c>IsSecondaryCrop = true</c></item>
/// </list>
///
/// <para>
/// The defensive fallbacks in <c>FieldResultsService.Perennials.cs:AssignPerennialStandIds</c>,
/// <c>FieldResultsService.DetailViewItems.cs:GetMainCropForYear</c>,
/// <c>FieldComponentHelper.GetMainCropForYear</c>, and
/// <c>IFieldComponentHelper.GetMainCropForYear</c> are kept in place as belt-and-suspenders
/// — this migration fixes the root cause, but the fallbacks catch any future variants we
/// haven't seen yet.
/// </para>
/// </summary>
public class V4ToV5Migration : IJsonMigration
{
    public Version FromVersion => new("4.0");
    public Version ToVersion => new("5.0");

    public void MigrateApplicationData(JObject root)
    {
        if (root["Farms"] is JArray farms)
        {
            MigrateFarms(farms);
        }
    }

    public void MigrateFarmExport(JArray farms)
    {
        MigrateFarms(farms);
    }

    private void MigrateFarms(JArray farms)
    {
        foreach (var farm in farms)
        {
            MigrateComponents(farm);
        }
    }

    private void MigrateComponents(JToken farm)
    {
        if (farm["Components"] is not JArray components)
        {
            return;
        }

        foreach (var component in components)
        {
            var typeName = component.Value<string>("$type");
            if (typeName == null)
            {
                continue;
            }

            // Patch FieldSystemComponent crop-item IsSecondaryCrop flags so they match
            // collection membership. See the class-level docstring for rationale.
            if (typeName.Contains("FieldSystemComponent"))
            {
                NormalizeIsSecondaryCropFlags(component);
            }

            // Future: patch other component types here.
            // Example (when NumberOfFields is added to RotationComponent):
            //
            // if (typeName.Contains("RotationComponent"))
            // {
            //     if (component["NumberOfFields"] == null)
            //     {
            //         component["NumberOfFields"] = 1;
            //     }
            // }
        }
    }

    /// <summary>
    /// Walks a <c>FieldSystemComponent</c>'s crop collections and forces
    /// <c>IsSecondaryCrop</c> on each contained <c>CropViewItem</c> to match the
    /// authoritative source — the collection the item lives in. This overwrites
    /// whatever value was on the JSON (true or false) because collection membership
    /// IS the truth: v4 farms saved before the <c>IsSecondaryCrop</c> field existed have
    /// it unset on legacy items, and even where it exists it may not have been kept in
    /// sync with collection moves.
    /// </summary>
    private static void NormalizeIsSecondaryCropFlags(JToken fieldComponent)
    {
        // Main-crop collection. By definition every item here is the main crop for its year.
        SetIsSecondaryCropOnAll(fieldComponent["CropViewItems"], value: false);

        // Cover-crop collection. By definition every item here is a secondary crop.
        SetIsSecondaryCropOnAll(fieldComponent["CoverCrops"], value: true);
    }

    /// <summary>
    /// Sets <c>IsSecondaryCrop</c> on every <c>CropViewItem</c> in the given collection
    /// token. Handles both shapes Newtonsoft can produce when serializing a list with
    /// <c>TypeNameHandling.Auto</c>: a plain <c>JArray</c>, or a <c>JObject</c> envelope
    /// of the form <c>{ "$type": "...", "$values": [...] }</c>.
    /// </summary>
    private static void SetIsSecondaryCropOnAll(JToken? collection, bool value)
    {
        if (collection is null)
        {
            return;
        }

        JArray? items = collection as JArray
                        ?? (collection is JObject envelope ? envelope["$values"] as JArray : null);
        if (items is null)
        {
            return;
        }

        foreach (var item in items)
        {
            if (item is JObject crop)
            {
                crop["IsSecondaryCrop"] = value;
            }
        }
    }
}
