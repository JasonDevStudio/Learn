// 生成随机数据点
using System.Diagnostics;
using System.Drawing;
using ScottPlot;
using SkiaSharp;
using Color = ScottPlot.Color;

Console.WriteLine($"[{DateTime.Now}] 开始准备数据...");

int pointCount = 10000;
Random rand = new Random(0);
double[] xs = new double[pointCount];
double[] ys1 = new double[pointCount];
double[] ys2 = new double[pointCount];

for (int i = 0; i < pointCount; i++)
{
    xs[i] = i;
    ys1[i] = rand.NextDouble() * 6;
    ys2[i] = rand.NextDouble() * 7 + 11;
}

var file = Path.Combine(AppContext.BaseDirectory, "scatterplot.png");

Console.WriteLine($"[{DateTime.Now}] 开始渲染绘图...");
var st = Stopwatch.StartNew();
// 创建一个新的 ScottPlot 图表
var plt = new Plot();

// 添加具有不同颜色和形状的数据点

var shape = (MarkerShape)rand.Next(Enum.GetNames(typeof(MarkerShape)).Length);

var scatter1 = plt.Add.Scatter(xs, ys1, Colors.Blue);
scatter1.MarkerStyle = new MarkerStyle((MarkerShape)rand.Next(0, 5), 15);
scatter1.MarkerStyle.Fill.Color = Colors.Blue; 
scatter1.MarkerStyle.Outline.Color = Colors.Red; // 边框颜色
scatter1.MarkerStyle.Outline.Width = 0.5f; // 边框宽度

var scatter2 = plt.Add.Scatter(xs, ys2, Colors.Green);
scatter2.MarkerStyle = new MarkerStyle((MarkerShape)rand.Next(0, 5), 10);

// 配置右侧 Y 轴 
plt.LeftAxis.Label.Text = "Left Y Axis";
plt.LeftAxis.Label.BackgroundColor = Colors.Blue;
plt.LeftAxis.Label.PointColor = Colors.Blue;
plt.LeftAxis.MinorTickColor = Colors.Blue;
plt.LeftAxis.MajorTickLength = 5;
plt.RightAxis.TickGenerator.Regenerate(new CoordinateRange(ys2.Min(), ys2.Max()), Edge.Right, 30);
plt.RightAxis.Label.Text = "Right Y Axis";
plt.RightAxis.Label.BackgroundColor = Colors.Green;
plt.RightAxis.Label.PointColor = Colors.Green; 

// 设置 X 轴标签为垂直显示
plt.XAxis.Label.Rotation = 90;
plt.XAxis.Label.PointColor = Colors.Green;
plt.RightAxis.IsVisible = true; 

// 保存图表为图片文件
plt.Save(file, 1800, 1400, ImageFormat.Png, 100);

st.Stop();
Console.WriteLine($"[{DateTime.Now}] 绘图完成，耗时【{st.Elapsed.TotalMilliseconds}】ms...");
