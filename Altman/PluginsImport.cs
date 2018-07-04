using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;

using PluginFramework;

namespace Altman
{
    /// <summary>
    /// MEF框架导入部分的相关代码，包括Import语句和申请的变量
    /// </summary>
    public class PluginsImport
    {
        //MEF可扩展框架编程，多部件导入语句(需要在项目中添加System.ComponentModel.Composition的引用)。
        [ImportMany(typeof(IPlugin), RequiredCreationPolicy = CreationPolicy.NonShared, AllowRecomposition = true)]//AllowRecomposition = true表示导出内容发生变化时，自动进行重新组合部件
        public IEnumerable<IPlugin> Plugins = null;//存储导入的插件集合（ImportMany表示导入多个对象，需要使用集合来存储），组合部件后该变量才指向导出的对象

        //RequiredCreationPolicy = CreationPolicy.NonShared表示创建策略，其实就是组合容器决定如何创建部件。
        //在组合容器组合部件时，如果导入和导出匹配成功，则组合容器会将导入成员的值设置成为导出的实例。因此，导出部件的创建策略决定了部件来源于何处：是现有实例还是新实例。
        //MEF的创建策略有：Shared（共享）和NonShared(非共享)。
        //使用Shared创建策略的部件将在每一个导入部件中共享实例。仅当容器中没有该部件的实例时才会创建新实例。使用共享策略创建的部件可以是提供服务的部件，以及较占用内存的部件。他们的内部状态应该尽可能少得受外界影响。
        //使用NonShared策略的部件在匹配每一个导入时都会有新的实例。这些部件的内部状态都是相互独立的，当某些部件需要保持特定的状态时可以使用这种策略。
    }
}
