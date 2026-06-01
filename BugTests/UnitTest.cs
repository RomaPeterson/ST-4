using BugPro;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BugTests;

[TestClass]
public class BugWorkflowTests
{
    private Bug? _bug;

    [TestInitialize]
    public void BeforeEach()
    {
        _bug = new Bug();
    }

    [TestMethod]
    public void Initial_IsCreated()
    {
        Assert.AreEqual(Bug.State.Created, _bug!.GetState());
    }

    [TestMethod]
    public void Created_Accept_BecomesAccepted()
    {
        _bug!.Accept();
        Assert.AreEqual(Bug.State.Accepted, _bug.GetState());
    }

    [TestMethod]
    public void Accepted_StartWork_BecomesWorking()
    {
        _bug!.Accept();
        _bug.StartWork();
        Assert.AreEqual(Bug.State.Working, _bug.GetState());
    }

    [TestMethod]
    public void Accepted_Finish_BecomesDone()
    {
        _bug!.Accept();
        _bug.Finish();
        Assert.AreEqual(Bug.State.Done, _bug.GetState());
    }

    [TestMethod]
    public void Working_CompleteFix_BecomesFixed()
    {
        _bug!.Accept();
        _bug.StartWork();
        _bug.CompleteFix();
        Assert.AreEqual(Bug.State.Fixed, _bug.GetState());
    }

    [TestMethod]
    public void Working_Finish_BecomesDone()
    {
        _bug!.Accept();
        _bug.StartWork();
        _bug.Finish();
        Assert.AreEqual(Bug.State.Done, _bug.GetState());
    }

    [TestMethod]
    public void Fixed_Finish_BecomesDone()
    {
        _bug!.Accept();
        _bug.StartWork();
        _bug.CompleteFix();
        _bug.Finish();
        Assert.AreEqual(Bug.State.Done, _bug.GetState());
    }

    [TestMethod]
    public void Fixed_Return_BecomesReturned()
    {
        _bug!.Accept();
        _bug.StartWork();
        _bug.CompleteFix();
        _bug.Return();
        Assert.AreEqual(Bug.State.Returned, _bug.GetState());
    }

    [TestMethod]
    public void Done_Return_BecomesReturned()
    {
        _bug!.Accept();
        _bug.Finish();
        _bug.Return();
        Assert.AreEqual(Bug.State.Returned, _bug.GetState());
    }

    [TestMethod]
    public void Returned_Accept_BecomesAccepted()
    {
        _bug!.Accept();
        _bug.Finish();
        _bug.Return();
        _bug.Accept();
        Assert.AreEqual(Bug.State.Accepted, _bug.GetState());
    }

    [TestMethod]
    public void Returned_Finish_BecomesDone()
    {
        _bug!.Accept();
        _bug.Finish();
        _bug.Return();
        _bug.Finish();
        Assert.AreEqual(Bug.State.Done, _bug.GetState());
    }

    [TestMethod]
    public void FullWorkflow_AllSteps()
    {
        _bug!.Accept();
        _bug.StartWork();
        _bug.CompleteFix();
        _bug.Finish();
        _bug.Return();
        Assert.AreEqual(Bug.State.Returned, _bug.GetState());
    }

    [TestMethod]
    public void Return_OnReturned_Ignored()
    {
        _bug!.Accept();
        _bug.Finish();
        _bug.Return();
        _bug.Return();
        Assert.AreEqual(Bug.State.Returned, _bug.GetState());
    }

    [TestMethod]
    public void Finish_OnDone_Ignored()
    {
        _bug!.Accept();
        _bug.Finish();
        _bug.Finish();
        Assert.AreEqual(Bug.State.Done, _bug.GetState());
    }

    [TestMethod]
    public void StartWork_FromCreated_Throws()
    {
        var ex = Assert.ThrowsException<InvalidOperationException>(() => _bug!.StartWork());
        Assert.IsNotNull(ex);
    }

    [TestMethod]
    public void CompleteFix_FromCreated_Throws()
    {
        Assert.ThrowsException<InvalidOperationException>(() => _bug!.CompleteFix());
    }

    [TestMethod]
    public void Finish_FromCreated_Throws()
    {
        Assert.ThrowsException<InvalidOperationException>(() => _bug!.Finish());
    }

    [TestMethod]
    public void Return_FromCreated_Throws()
    {
        Assert.ThrowsException<InvalidOperationException>(() => _bug!.Return());
    }

    [TestMethod]
    public void Accept_FromAccepted_Throws()
    {
        _bug!.Accept();
        Assert.ThrowsException<InvalidOperationException>(() => _bug.Accept());
    }

    [TestMethod]
    public void Accept_FromWorking_Throws()
    {
        _bug!.Accept();
        _bug.StartWork();
        Assert.ThrowsException<InvalidOperationException>(() => _bug.Accept());
    }

    [TestMethod]
    public void CompleteFix_FromAccepted_Throws()
    {
        _bug!.Accept();
        Assert.ThrowsException<InvalidOperationException>(() => _bug.CompleteFix());
    }

    [TestMethod]
    public void Return_FromWorking_Throws()
    {
        _bug!.Accept();
        _bug.StartWork();
        Assert.ThrowsException<InvalidOperationException>(() => _bug.Return());
    }

    [TestMethod]
    public void StartWork_FromDone_Throws()
    {
        _bug!.Accept();
        _bug.Finish();
        Assert.ThrowsException<InvalidOperationException>(() => _bug.StartWork());
    }

    [TestMethod]
    public void CompleteFix_FromDone_Throws()
    {
        _bug!.Accept();
        _bug.Finish();
        Assert.ThrowsException<InvalidOperationException>(() => _bug.CompleteFix());
    }

    [TestMethod]
    public void Accept_FromDone_Throws()
    {
        _bug!.Accept();
        _bug.Finish();
        Assert.ThrowsException<InvalidOperationException>(() => _bug.Accept());
    }
}
