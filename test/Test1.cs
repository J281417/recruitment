
using recruitment;

namespace test
{
    [TestClass]
    public sealed class Test1
    {

        [TestMethod]
        public void TestAddContractor()
        {
            RecruitmentSystem recruitmentSystem = new RecruitmentSystem();

            var contractor = new Contractor("Peter", "Parker", DateTime.Now.AddMonths(-12), 45);
            recruitmentSystem.AddContractor(contractor);

            Assert.AreEqual(recruitmentSystem.GetContractors().Count, 1);
            Assert.IsTrue(recruitmentSystem.GetContractors().Contains(contractor));
        }

    }
}