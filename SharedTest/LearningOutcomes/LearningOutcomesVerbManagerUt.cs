using System.Globalization;
using NUnit.Framework;
using Shared.LearningOutcomes;

namespace SharedTest.LearningOutcomes;

[TestFixture]
public class LearningOutcomesVerbManagerUt
{
    [SetUp]
    public void SetUp()
    {
        _verbManager = new LearningOutcomesVerbManager();
        _cultureDe = new CultureInfo("de-DE");
        _cultureEn = new CultureInfo("en-DE");
    }

    private LearningOutcomesVerbManager _verbManager;
    private CultureInfo _cultureDe;
    private CultureInfo _cultureEn;

    [Test]
    public void GetVerbsOfVisibility_ReturnsCorrectVerbs_T0De()
    {
        var verbsT0De = _verbManager.GetVerbsOfVisibility(TaxonomyLevel.None, _cultureDe);
        Assert.Multiple(() =>
        {
            Assert.That(verbsT0De[0], Is.EqualTo("abgrenzen"));
            Assert.That(verbsT0De[1], Is.EqualTo("ableiten"));
            Assert.That(verbsT0De.Last(), Is.EqualTo("zusammenstellen"));
        });
    }

    [Test]
    public void GetVerbsOfVisibility_ReturnsCorrectVerbs_T1De()
    {
        var verbsT1De = _verbManager.GetVerbsOfVisibility(TaxonomyLevel.Level1, _cultureDe);
        Assert.Multiple(() =>
        {
            Assert.That(verbsT1De[0], Is.EqualTo("anführen"));
            Assert.That(verbsT1De[1], Is.EqualTo("angeben"));
            Assert.That(verbsT1De.Last(), Is.EqualTo("zusammenstellen"));
        });
    }

    [Test]
    public void GetVerbsOfVisibility_ReturnsCorrectVerbs_T2De()
    {
        var verbsT2De = _verbManager.GetVerbsOfVisibility(TaxonomyLevel.Level2, _cultureDe);
        Assert.Multiple(() =>
        {
            Assert.That(verbsT2De[0], Is.EqualTo("abgrenzen"));
            Assert.That(verbsT2De[1], Is.EqualTo("ableiten"));
            Assert.That(verbsT2De.Last(), Is.EqualTo("zusammenfassen"));
        });
    }

    [Test]
    public void GetVerbsOfVisibility_ReturnsCorrectVerbs_T3De()
    {
        var verbsT3De = _verbManager.GetVerbsOfVisibility(TaxonomyLevel.Level3, _cultureDe);
        Assert.Multiple(() =>
        {
            Assert.That(verbsT3De[0], Is.EqualTo("abschätzen"));
            Assert.That(verbsT3De[1], Is.EqualTo("ändern"));
            Assert.That(verbsT3De.Last(), Is.EqualTo("zeigen"));
        });
    }

    [Test]
    public void GetVerbsOfVisibility_ReturnsCorrectVerbs_T4De()
    {
        var verbsT4De = _verbManager.GetVerbsOfVisibility(TaxonomyLevel.Level4, _cultureDe);
        Assert.Multiple(() =>
        {
            Assert.That(verbsT4De[0], Is.EqualTo("abgrenzen"));
            Assert.That(verbsT4De[1], Is.EqualTo("ableiten"));
            Assert.That(verbsT4De.Last(), Is.EqualTo("zuordnen"));
        });
    }

    [Test]
    public void GetVerbsOfVisibility_ReturnsCorrectVerbs_T5De()
    {
        var verbsT5De = _verbManager.GetVerbsOfVisibility(TaxonomyLevel.Level5, _cultureDe);
        Assert.Multiple(() =>
        {
            Assert.That(verbsT5De[0], Is.EqualTo("abschätzen"));
            Assert.That(verbsT5De[1], Is.EqualTo("abwägen"));
            Assert.That(verbsT5De.Last(), Is.EqualTo("widerlegen"));
        });
    }

    [Test]
    public void GetVerbsOfVisibility_ReturnsCorrectVerbs_T6De()
    {
        var verbsT6De = _verbManager.GetVerbsOfVisibility(TaxonomyLevel.Level6, _cultureDe);
        Assert.Multiple(() =>
        {
            Assert.That(verbsT6De[0], Is.EqualTo("ableiten"));
            Assert.That(verbsT6De[1], Is.EqualTo("anknüpfen"));
            Assert.That(verbsT6De.Last(), Is.EqualTo("zusammenstellen"));
        });
    }

    [Test]
    public void GetVerbsOfVisibility_ReturnsCorrectVerbs_T0En()
    {
        var verbsT0En = _verbManager.GetVerbsOfVisibility(TaxonomyLevel.None, _cultureEn);
        Assert.Multiple(() =>
        {
            Assert.That(verbsT0En[0], Is.EqualTo("abstract"));
            Assert.That(verbsT0En[1], Is.EqualTo("accumulate"));
            Assert.That(verbsT0En.Last(), Is.EqualTo("write down"));
        });
    }

    [Test]
    public void GetVerbsOfVisibility_ReturnsCorrectVerbs_T1En()
    {
        var verbsT1En = _verbManager.GetVerbsOfVisibility(TaxonomyLevel.Level1, _cultureEn);
        Assert.Multiple(() =>
        {
            Assert.That(verbsT1En[0], Is.EqualTo("allocate"));
            Assert.That(verbsT1En[1], Is.EqualTo("assign"));
            Assert.That(verbsT1En.Last(), Is.EqualTo("write down"));
        });
    }

    [Test]
    public void GetVerbsOfVisibility_ReturnsCorrectVerbs_T2En()
    {
        var verbsT2En = _verbManager.GetVerbsOfVisibility(TaxonomyLevel.Level2, _cultureEn);
        Assert.Multiple(() =>
        {
            Assert.That(verbsT2En[0], Is.EqualTo("abstract"));
            Assert.That(verbsT2En[1], Is.EqualTo("allocate"));
            Assert.That(verbsT2En.Last(), Is.EqualTo("visualize"));
        });
    }

    [Test]
    public void GetVerbsOfVisibility_ReturnsCorrectVerbs_T3En()
    {
        var verbsT3En = _verbManager.GetVerbsOfVisibility(TaxonomyLevel.Level3, _cultureEn);
        Assert.Multiple(() =>
        {
            Assert.That(verbsT3En[0], Is.EqualTo("acquire"));
            Assert.That(verbsT3En[1], Is.EqualTo("actualize"));
            Assert.That(verbsT3En.Last(), Is.EqualTo("work on"));
        });
    }

    [Test]
    public void GetVerbsOfVisibility_ReturnsCorrectVerbs_T4En()
    {
        var verbsT4En = _verbManager.GetVerbsOfVisibility(TaxonomyLevel.Level4, _cultureEn);
        Assert.Multiple(() =>
        {
            Assert.That(verbsT4En[0], Is.EqualTo("allocate"));
            Assert.That(verbsT4En[1], Is.EqualTo("analyze"));
            Assert.That(verbsT4En.Last(), Is.EqualTo("vindicate"));
        });
    }

    [Test]
    public void GetVerbsOfVisibility_ReturnsCorrectVerbs_T5En()
    {
        var verbsT5En = _verbManager.GetVerbsOfVisibility(TaxonomyLevel.Level5, _cultureEn);
        Assert.Multiple(() =>
        {
            Assert.That(verbsT5En[0], Is.EqualTo("analyze"));
            Assert.That(verbsT5En[1], Is.EqualTo("appraise"));
            Assert.That(verbsT5En.Last(), Is.EqualTo("weight"));
        });
    }

    [Test]
    public void GetVerbsOfVisibility_ReturnsCorrectVerbs_T6En()
    {
        var verbsT6En = _verbManager.GetVerbsOfVisibility(TaxonomyLevel.Level6, _cultureEn);
        Assert.Multiple(() =>
        {
            Assert.That(verbsT6En[0], Is.EqualTo("abstract"));
            Assert.That(verbsT6En[1], Is.EqualTo("accumulate"));
            Assert.That(verbsT6En.Last(), Is.EqualTo("write"));
        });
    }

    [Test]
    public void GetTaxonomyLevelNames_ReturnsCorrectNames_De()
    {
        var namesDe = _verbManager.GetTaxonomyLevelNames(_cultureDe);
        Assert.Multiple(() =>
        {
            Assert.That(namesDe.Keys, Has.Count.EqualTo(7));
            Assert.That(namesDe.ElementAt(0).Key, Is.EqualTo(TaxonomyLevel.None));
            Assert.That(namesDe.ElementAt(0).Value, Is.EqualTo(""));
            Assert.That(namesDe.ElementAt(1).Key, Is.EqualTo(TaxonomyLevel.Level1));
            Assert.That(namesDe.ElementAt(1).Value, Is.EqualTo("Erinnern"));
            Assert.That(namesDe.ElementAt(2).Key, Is.EqualTo(TaxonomyLevel.Level2));
            Assert.That(namesDe.ElementAt(2).Value, Is.EqualTo("Verstehen"));
            Assert.That(namesDe.ElementAt(3).Key, Is.EqualTo(TaxonomyLevel.Level3));
            Assert.That(namesDe.ElementAt(3).Value, Is.EqualTo("Anwenden"));
            Assert.That(namesDe.ElementAt(4).Key, Is.EqualTo(TaxonomyLevel.Level4));
            Assert.That(namesDe.ElementAt(4).Value, Is.EqualTo("Analysieren"));
            Assert.That(namesDe.ElementAt(5).Key, Is.EqualTo(TaxonomyLevel.Level5));
            Assert.That(namesDe.ElementAt(5).Value, Is.EqualTo("Beurteilen"));
            Assert.That(namesDe.ElementAt(6).Key, Is.EqualTo(TaxonomyLevel.Level6));
            Assert.That(namesDe.ElementAt(6).Value, Is.EqualTo("Erschaffen"));
        });
    }

    [Test]
    public void GetTaxonomyLevelNames_ReturnsCorrectNames_En()
    {
        var namesEn = _verbManager.GetTaxonomyLevelNames(_cultureEn);
        Assert.Multiple(() =>
        {
            Assert.That(namesEn.Keys, Has.Count.EqualTo(7));
            Assert.That(namesEn.ElementAt(0).Key, Is.EqualTo(TaxonomyLevel.None));
            Assert.That(namesEn.ElementAt(0).Value, Is.EqualTo(""));
            Assert.That(namesEn.ElementAt(1).Key, Is.EqualTo(TaxonomyLevel.Level1));
            Assert.That(namesEn.ElementAt(1).Value, Is.EqualTo("Remember"));
            Assert.That(namesEn.ElementAt(2).Key, Is.EqualTo(TaxonomyLevel.Level2));
            Assert.That(namesEn.ElementAt(2).Value, Is.EqualTo("Understand"));
            Assert.That(namesEn.ElementAt(3).Key, Is.EqualTo(TaxonomyLevel.Level3));
            Assert.That(namesEn.ElementAt(3).Value, Is.EqualTo("Apply"));
            Assert.That(namesEn.ElementAt(4).Key, Is.EqualTo(TaxonomyLevel.Level4));
            Assert.That(namesEn.ElementAt(4).Value, Is.EqualTo("Analyse"));
            Assert.That(namesEn.ElementAt(5).Key, Is.EqualTo(TaxonomyLevel.Level5));
            Assert.That(namesEn.ElementAt(5).Value, Is.EqualTo("Evaluate"));
            Assert.That(namesEn.ElementAt(6).Key, Is.EqualTo(TaxonomyLevel.Level6));
            Assert.That(namesEn.ElementAt(6).Value, Is.EqualTo("Create"));
        });
    }
}