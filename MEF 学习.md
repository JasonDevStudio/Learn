# MEF 学习

标签（空格分隔）： MEF C#

---

**

C# MEF DLL文件非常多的时候，如何快速加载？
--------------------------
C#中的MEF（Managed Extensibility Framework）是一种用于在应用程序中动态加载组件的框架。当需要加载的DLL文件非常多时，可以采取以下措施以提高加载速度：

使用合适的目录结构：将DLL文件分散到多个目录下，每个目录下包含一组相关的DLL文件，可以减少单个目录下文件的数量，从而加快搜索速度。

使用MEF的组合方式：将多个DLL文件组合成一个包，以减少文件的数量和搜索时间。

使用MEF的缓存功能：MEF有一个默认的缓存机制，用于缓存已加载的组件。这样，当第二次加载同一组件时，MEF会从缓存中加载，而不是重新搜索并加载。

预热MEF容器：在应用程序启动时，预先加载所有需要的组件，以便在运行时快速访问。可以使用MEF的ComposeParts方法来实现预热。

懒加载：只有在需要使用组件时才加载它们，而不是在应用程序启动时就加载所有组件。这可以通过将组件标记为[Import(AllowDefault = true)]和[ImportMany(AllowDefault = true)]来实现。

以上方法可以结合使用，以获得最佳的加载速度和性能。
**
    
## 使用MEF的组合方式 的代码 ##
使用MEF的组合方式，可以将多个DLL文件组合成一个包，以便更容易地管理和加载组件。以下是一个示例代码，演示如何使用MEF的组合方式：

首先，我们需要创建一个包含所有需要组合的DLL文件的目录。在这个例子中，我们将使用Components目录作为组件的根目录。在这个目录下，我们将创建两个子目录，分别命名为Core和Extensions。Core目录包含应用程序的核心组件，而Extensions目录包含可选的扩展组件。

然后，我们需要创建一个包含所有需要组合的组件的类。这个类需要使用MEF的[Export]和[ExportMetadata]属性来标记组件，以便MEF能够识别并加载它们。在这个例子中，我们创建了一个Component类，其中包含一个Name和一个Version属性：


    using System.ComponentModel.Composition;
    
    namespace MyApplication.Components
    {
        public class Component
        {
            public string Name { get; set; }
            public string Version { get; set; }
        }
    
        [Export(typeof(Component))]
        [ExportMetadata("Category", "Core")]
        public class CoreComponent : Component
        {
            public CoreComponent()
            {
                Name = "Core Component";
                Version = "1.0";
            }
        }
    
        [Export(typeof(Component))]
        [ExportMetadata("Category", "Extension")]
        public class ExtensionComponent : Component
        {
            public ExtensionComponent()
            {
                Name = "Extension Component";
                Version = "1.0";
            }
        }
    }

在这个例子中，我们定义了两个组件：CoreComponent和ExtensionComponent。这些组件都继承自Component类，并使用MEF的[Export]属性将它们标记为可以导出的组件。我们还使用[ExportMetadata]属性指定了每个组件的类别，以便我们可以根据需要选择加载哪些组件。

接下来，我们需要创建一个MEF容器，并使用MEF的DirectoryCatalog类来将Components目录下的DLL文件组合成一个包。我们还需要使用MEF的AssemblyCatalog类将应用程序的主程序集加入到容器中。以下是一个示例代码：

    using System.ComponentModel.Composition.Hosting;
    using System.Reflection;
    
    namespace MyApplication
    {
        public class App
        {
            private CompositionContainer container;
    
            public void Run()
            {
                var catalog = new AggregateCatalog();
                catalog.Catalogs.Add(new DirectoryCatalog("Components/Core"));
                catalog.Catalogs.Add(new DirectoryCatalog("Components/Extensions"));
                catalog.Catalogs.Add(new AssemblyCatalog(Assembly.GetExecutingAssembly()));
    
                container = new CompositionContainer(catalog);
    
                var coreComponents = container.GetExports<Component>()
                    .Where(c => c.Metadata["Category"].ToString() == "Core")
                    .Select(c => c.Value);
    
                var extensionComponents = container.GetExports<Component>()
                    .Where(c => c.Metadata["Category"].ToString() == "Extension")
                    .Select(c => c.Value);
    
                // TODO: Use coreComponents and extensionComponents as needed
            }
        }
    }
    
在这个示示代码中，我们首先创建了一个AggregateCatalog对象，并使用它来组合所有需要加载的DLL文件。我们使用DirectoryCatalog类来加载位于Components/Core和Components/Extensions目录下的DLL文件，以及使用AssemblyCatalog类加载应用程序的主程序集。

然后，我们创建了一个MEF容器，并将AggregateCatalog对象传递给容器的构造函数。这样，MEF将能够自动搜索并加载所有包含在这些目录和程序集中的组件。

最后，我们使用容器的GetExports方法来获取所有标记为Component类型的组件。我们使用Where方法和Metadata属性来筛选出Category元数据等于Core和Extension的组件，并使用Select方法来选择它们的值。这样，我们就可以将这些组件保存到变量中，并在需要使用它们的地方进行处理。

需要注意的是，我们在使用MEF的组合方式时，需要保证每个组件都有唯一的名称和版本号，以便MEF能够正确地加载和组合它们。此外，我们还需要注意组件之间的依赖关系，以确保组件能够正确地协作.

## 详细说明MEF 使用MEF的缓存功能 ##
MEF（Managed Extensibility Framework）提供了缓存功能来优化组件的加载速度，特别是在大型应用程序中，当需要加载大量组件时，缓存功能能够有效地提高应用程序的性能。

在MEF中，缓存功能是通过CompositionScopeDefinition和CompositionScope类来实现的。CompositionScopeDefinition表示一个作用域定义，定义了一组组件的生命周期和缓存行为。CompositionScope表示一个作用域实例，表示一个具体的组件生命周期和缓存实例。

缓存分为两种类型：作用域缓存和共享缓存。

作用域缓存：每个作用域都有自己的缓存实例，用于存储在该作用域中加载的组件。当作用域被销毁时，该作用域的缓存实例也会被销毁。作用域缓存适用于那些在不同作用域中具有不同生命周期的组件，例如在不同请求中处理的Web应用程序中的组件。
共享缓存：所有作用域共享同一个缓存实例，用于存储在所有作用域中加载的组件。当应用程序关闭时，共享缓存实例也会被销毁。共享缓存适用于那些在整个应用程序生命周期内都具有相同生命周期的组件。
默认情况下，MEF会在每个作用域中使用作用域缓存，并在所有作用域之间使用共享缓存。我们可以通过以下代码来配置缓存选项：

    var catalog = new AggregateCatalog();
    var container = new CompositionContainer(catalog, true);
    
    var scope = container.CreateScoped();
    scope.ConfigureCompositionScope(new CompositionScopeDefinition()
    {
        IsolationLevel = System.ComponentModel.Composition.Hosting.IsolationLevel.Any,
        ShouldAddToCache = true,
        ShouldExport = (definition) => true,
        ShouldParticipateInComposition = true
    });
在上面的代码中，我们创建了一个CompositionContainer对象，并将true传递给构造函数，以启用缓存功能。然后，我们使用CreateScoped方法创建一个作用域实例，并使用ConfigureCompositionScope方法来配置作用域的缓存选项。

在CompositionScopeDefinition对象中，我们可以配置以下选项：

IsolationLevel：指定作用域的隔离级别，可以是None、Any或Isolated。默认值是Any，表示使用共享缓存。
ShouldAddToCache：指定是否将组件添加到缓存中。默认值是true，表示将组件添加到缓存中。
ShouldExport：指定应如何处理导出定义。默认值是null，表示应将所有导出定义添加到缓存中。
ShouldParticipateInComposition：指定是否应该参与组件的组合。默认值是true，表示应该参与组件的组合。

当作用域被创建时，MEF将自动为作用域创建一个缓存实例，并根据缓存选项来决定缓存的行为。当在作用域中请求组件时，MEF会首先检查作用域的缓存实例中是否已经有该组件的实例。如果有，MEF将直接返回该实例；如果没有，MEF将按照正常的方式来加载和组合组件，并将结果保存到缓存实例中。

需要注意的是，缓存功能虽然能够提高组件的加载速度，但也会增加应用程序的内存消耗。因此，在使用缓存功能时，我们需要根据实际情况来平衡性能和内存消耗，并进行必要的优化和调整。

## 详细说明MEF预热MEF容器的方式 ##
在使用MEF时，有时我们需要在应用程序启动时预热MEF容器，以便提高应用程序的启动速度和性能。预热容器的方式是让MEF在应用程序启动时预加载和组合所有需要的组件，这样在后续的使用中，就可以直接从缓存中获取这些组件，而不需要重新加载和组合。

以下是预热MEF容器的方式：

创建一个MEF组合目录（ComposablePartCatalog），该目录包含所有需要预加载和组合的组件。例如：

    var catalog = new AggregateCatalog();
    catalog.Catalogs.Add(new AssemblyCatalog(typeof(Program).Assembly)); // 预加载当前程序集中的组件
    catalog.Catalogs.Add(new DirectoryCatalog("Extensions")); // 预加载“Extensions”目录中的组件
创建一个MEF容器（CompositionContainer），并将组合目录传递给它。同时，将true作为第二个参数传递给构造函数，以启用缓存功能：

    var container = new CompositionContainer(catalog, true);
调用ComposeParts方法，让MEF预加载和组合所有需要的组件。这个方法会阻塞线程直到所有组件都被加载和组合完成：

    container.ComposeParts();
完成预热后，MEF将会自动缓存所有预加载的组件，以便在后续的使用中快速获取。当需要使用组件时，只需要从MEF容器中获取即可，不需要重新加载和组合。

需要注意的是，预热MEF容器可能会影响应用程序的启动时间，特别是当需要加载大量组件时。因此，我们需要根据实际情况来平衡性能和启动时间，并进行必要的优化和调整。

## 详细说明MEF懒加载的方式 ##
在使用MEF时，有时我们需要延迟加载组件，以便在需要使用组件时才进行加载和组合，从而提高应用程序的启动速度和性能。这种方式称为懒加载（Lazy Loading）。

以下是懒加载MEF组件的方式：

创建一个MEF组合目录（ComposablePartCatalog），该目录包含所有需要懒加载的组件。例如：

    var catalog = new AggregateCatalog();
    catalog.Catalogs.Add(new AssemblyCatalog(typeof(Program).Assembly)); // 预加载当前程序集中的组件
    catalog.Catalogs.Add(new DirectoryCatalog("Extensions", "*.dll")); // 懒加载“Extensions”目录中的组件
创建一个MEF容器（CompositionContainer），并将组合目录传递给它。同时，将false作为第二个参数传递给构造函数，以禁用缓存功能：

    var container = new CompositionContainer(catalog, false);
调用ComposeParts方法，让MEF加载和组合所有预加载的组件。这个方法不会加载和组合懒加载的组件，而是等到需要使用这些组件时才进行加载和组合：

    container.ComposeParts();
在需要使用懒加载组件的地方，使用MEF容器的GetExport或GetExports方法获取组件。这些方法会在需要时自动加载和组合懒加载的组件：

    var lazyComponent = container.GetExport<ILazyComponent>();
需要注意的是，懒加载MEF组件可能会对应用程序的性能产生一定的影响，因为每次获取懒加载组件时，MEF都会重新加载和组合组件。因此，我们需要根据实际情况来平衡性能和懒加载的需求，并进行必要的优化和调整。



    
    
    
    
    







