Model: GPT-4

> 1 / 1
> 
> C# ScottPlot 开发, 给出散点图Demo, 造100万行数据, 左侧,右侧都要有Y轴刻度

ChatGPTChatGPT

1 / 1

使用 ScottPlot 库，我们可以轻松地创建一个具有双 Y 轴刻度的散点图。在这个示例中，我们将生成 100 万个数据点。请确保您已通过 NuGet 安装了 ScottPlot。

首先，我们需要创建一个控制台应用程序并引入 ScottPlot：

csharp

```csharp
using System;
using ScottPlot;
```

接下来，我们将创建一个散点图示例，其中包含 100 万个随机数据点：

csharp

```csharp
namespace ScottPlotDemo
{
    class Program
    {
        static void Main(string[] args)
        {
            // 生成随机数据点
            int pointCount = 1_000_000;
            Random rand = new Random(0);
            double[] xs = new double[pointCount];
            double[] ys1 = new double[pointCount];
            double[] ys2 = new double[pointCount];

            for (int i = 0; i < pointCount; i++)
            {
                xs[i] = i;
                ys1[i] = rand.NextDouble() * 10;
                ys2[i] = rand.NextDouble() * 10 + 10;
            }

            // 创建一个新的 ScottPlot 图表
            var plt = new ScottPlot.Plot(800, 600);

            // 添加左侧 Y 轴的散点图
            plt.AddScatter(xs, ys1, Color.Blue, lineWidth: 0, markerSize: 5);

            // 添加右侧 Y 轴的散点图
            var sp = plt.AddScatter(xs, ys2, Color.Red, lineWidth: 0, markerSize: 5);
            sp.YAxisIndex = 1;

            // 配置右侧 Y 轴
            plt.YAxis.Label("Left Y Axis");
            plt.YAxis.Color(Color.Blue);
            plt.AddYAxis("Right Y Axis", Color.Red);

            // 保存图表为图片文件
            plt.SaveFig("scatterplot.png");

            Console.WriteLine("Scatter plot saved as scatterplot.png");
        }
    }
}
```

运行该程序后，您将在应用程序的目录中找到名为 "scatterplot.png" 的文件，其中包含一个具有双 Y 轴刻度的散点图。左侧 Y 轴显示蓝色数据点，右侧 Y 轴显示红色数据点。

> 1 / 1
> 
> 在此基础上， X轴文字，垂直显示

ChatGPTChatGPT

1 / 1

在 ScottPlot 中，要将 X 轴标签设置为垂直显示，您需要修改 X 轴的标签旋转。这是修改后的代码：

csharp

```csharp
namespace ScottPlotDemo
{
    class Program
    {
        static void Main(string[] args)
        {
            // 生成随机数据点
            int pointCount = 1_000_000;
            Random rand = new Random(0);
            double[] xs = new double[pointCount];
            double[] ys1 = new double[pointCount];
            double[] ys2 = new double[pointCount];

            for (int i = 0; i < pointCount; i++)
            {
                xs[i] = i;
                ys1[i] = rand.NextDouble() * 10;
                ys2[i] = rand.NextDouble() * 10 + 10;
            }

            // 创建一个新的 ScottPlot 图表
            var plt = new ScottPlot.Plot(800, 600);

            // 添加左侧 Y 轴的散点图
            plt.AddScatter(xs, ys1, Color.Blue, lineWidth: 0, markerSize: 5);

            // 添加右侧 Y 轴的散点图
            var sp = plt.AddScatter(xs, ys2, Color.Red, lineWidth: 0, markerSize: 5);
            sp.YAxisIndex = 1;

            // 配置右侧 Y 轴
            plt.YAxis.Label("Left Y Axis");
            plt.YAxis.Color(Color.Blue);
            plt.AddYAxis("Right Y Axis", Color.Red);

            // 设置 X 轴标签为垂直显示
            plt.XAxis.TickLabelStyle(rotation: 90);

            // 保存图表为图片文件
            plt.SaveFig("scatterplot.png");

            Console.WriteLine("Scatter plot saved as scatterplot.png");
        }
    }
}
```

在这个修改后的代码中，我们添加了以下行来设置 X 轴标签的旋转：

csharp

```csharp
plt.XAxis.TickLabelStyle(rotation: 90);
```

运行此代码后，您将在应用程序的目录中找到名为 "scatterplot.png" 的文件，其中包含一个具有双 Y 轴刻度和垂直 X 轴标签的散点图。

> 1 / 1
> 
> 在此基础上，如何设置不同点的颜色，和形状

ChatGPTChatGPT

1 / 1

要设置不同点的颜色和形状，您可以为每个点单独添加散点图。在这个示例中，我们将减少数据点的数量以提高性能。以下是如何实现不同颜色和形状的散点图的代码：

csharp

```csharp
using System;
using System.Drawing;
using ScottPlot;
using ScottPlot.Drawing;

namespace ScottPlotDemo
{
    class Program
    {
        static void Main(string[] args)
        {
            // 生成随机数据点
            int pointCount = 1000;
            Random rand = new Random(0);
            double[] xs = new double[pointCount];
            double[] ys1 = new double[pointCount];
            double[] ys2 = new double[pointCount];

            for (int i = 0; i < pointCount; i++)
            {
                xs[i] = i;
                ys1[i] = rand.NextDouble() * 10;
                ys2[i] = rand.NextDouble() * 10 + 10;
            }

            // 创建一个新的 ScottPlot 图表
            var plt = new ScottPlot.Plot(800, 600);

            // 添加具有不同颜色和形状的数据点
            for (int i = 0; i < pointCount; i++)
            {
                var color = Color.FromArgb(rand.Next(256), rand.Next(256), rand.Next(256));
                var shape = (MarkerShape)rand.Next(Enum.GetNames(typeof(MarkerShape)).Length);
                plt.AddScatter(new double[] { xs[i] }, new double[] { ys1[i] }, color, markerShape: shape, markerSize: 5);
                plt.AddScatter(new double[] { xs[i] }, new double[] { ys2[i] }, color, markerShape: shape, markerSize: 5, yAxisIndex: 1);
            }

            // 配置右侧 Y 轴
            plt.YAxis.Label("Left Y Axis");
            plt.YAxis.Color(Color.Blue);
            plt.AddYAxis("Right Y Axis", Color.Red);

            // 设置 X 轴标签为垂直显示
            plt.XAxis.TickLabelStyle(rotation: 90);

            // 保存图表为图片文件
            plt.SaveFig("scatterplot.png");

            Console.WriteLine("Scatter plot saved as scatterplot.png");
        }
    }
}
```

在这个示例中，我们首先导入了 `System.Drawing` 和 `ScottPlot.Drawing` 命名空间。然后，我们减少了数据点的数量以提高性能。接下来，在循环中，我们为每个点生成了一个随机颜色和形状。我们使用 `plt.AddScatter()` 函数分别添加了每个左侧和右侧 Y 轴上的点。

请注意，使用这种方法可能会降低性能，尤其是当有大量的数据点时。如果可能的话，尝试将具有相同颜色和形状的点分组，并为每个组添加一个散点图。这将显著提高性能。