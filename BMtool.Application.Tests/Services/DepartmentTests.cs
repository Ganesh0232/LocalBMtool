using AutoMapper;
using BMtool.Application.Interfaces;
using BMtool.Application.Services;
using BMtool.Core.Repository;
using Telerik.JustMock;

namespace BMtool.Application.Tests.Services
{
    [TestClass]
    public class DepartmentTests
    {
        private readonly IDepartmentService _departmentService;
        private readonly IDepartmentRepository _mockDepartmentRepository;
        private readonly IMapper _mapper;

        public DepartmentTests(IMapper mapper)
        {
            _mockDepartmentRepository = Mock.Create<IDepartmentRepository>();
            _mapper = mapper;
            _departmentService = new DepartmentService(_mockDepartmentRepository, _mapper);
        }

        [TestMethod]
        [TestCategory("Services")]
        public void GetDepartmentList_ShouldCreateInstance_WhenMethodCalled()
        {
            IDepartmentService departmentService = new DepartmentService(_mockDepartmentRepository, _mapper);
            Assert.IsInstanceOfType(departmentService,typeof(DepartmentService));
        }
    }
}