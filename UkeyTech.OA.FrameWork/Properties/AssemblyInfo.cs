using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

// 有关程序集的常规信息通过以下
// 特性集控制。更改这些特性值可修改
// 与程序集关联的信息。
[assembly: AssemblyTitle("UkeyTech.OA.Base.BLL")]
[assembly: AssemblyDescription("")]
[assembly: AssemblyConfiguration("")]
[assembly: AssemblyCompany("")]
[assembly: AssemblyProduct("UkeyTech.OA.Base.BLL")]
[assembly: AssemblyCopyright("Copyright ©  2014-2016")]
[assembly: AssemblyTrademark("")]
[assembly: AssemblyCulture("")]

// 将 ComVisible 设置为 false 使此程序集中的类型
// 对 COM 组件不可见。如果需要从 COM 访问此程序集中的类型，
// 则将该类型上的 ComVisible 特性设置为 true。
[assembly: ComVisible(false)]

// 如果此项目向 COM 公开，则下列 GUID 用于类型库的 ID
[assembly: Guid("36115e4e-030b-44ca-a413-b40c07c9956c")]

// 程序集的版本信息由下面四个值组成:
//
//      主版本
//      次版本 
//      内部版本号
//      修订号
//
// 可以指定所有这些值，也可以使用“内部版本号”和“修订号”的默认值，
// 方法是按如下所示使用“*”:
// [assembly: AssemblyVersion("1.0.*")]
[assembly: AssemblyVersion("1.1.0.5")]
[assembly: AssemblyFileVersion("1.1.0.5")]


//1.0.0.1 2016-2-02 增加对默认角色的支持
//1.0.0.2 2016-3-10 删除用户部门岗位角色时，关联删除用户部门信息表
//1.0.0.3 2016-4-17 增加引用附件方法
//1.0.0.4 2016-6-12 GetFullInfoByAdminId方法增加对无角色数据获取的参数
//1.0.0.5 2016-6-15 所有构造函数存在树初始化的对象必须使用double check对象